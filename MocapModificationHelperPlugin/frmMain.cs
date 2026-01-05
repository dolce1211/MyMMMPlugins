using DxMath;
using Linearstar.Keystone.IO.MikuMikuDance;
using MikuMikuPlugin;
using MMDUtil;
using MoCapModificationHelperPlugin.offsetAdder;
using MoCapModificationHelperPlugin.service;
using MyUtility;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static MoCapModificationHelperPlugin.offsetAdder.OffsetAdderUtil;
using static System.Net.Mime.MediaTypeNames;

namespace MoCapModificationHelperPlugin
{
    public partial class frmMain : Form
    {
        private OffsetAdderService _offsetAdder = null;
        private Color _defBackColor = Color.Empty;
        private int _prevModelId = -1;
        private Form _parentForm;
        private Scene _scene;

        private Configs _configs;

        public frmMain(Form parentForm, Scene scene, Configs configs)
        {
            InitializeComponent();
            this.panel.Location = this.dataGridView1.Location;
            _parentForm = parentForm;
            _scene = scene;
            _configs = configs;

            DateTime lastPressed = DateTime.MinValue;

            _defBackColor = this.BackColor;
            this.cboGapSelector.Tag = ServiceType.GapSelectorService;
            this.btnGapSelector.Tag = ServiceType.GapSelectorService;

            this.cboLayerBoneSelector.Tag = ServiceType.LayerBoneSelectorService;
            this.btnLayerBoneSelector.Tag = ServiceType.LayerBoneSelectorService;
            this.chkLayerBoneSelector.Tag = ServiceType.LayerBoneSelectorService;

            this.cboModifiedLayerSelector.Tag = ServiceType.ModifiedLayerSelectorService;
            this.btnModifiedLayerSelector.Tag = ServiceType.ModifiedLayerSelectorService;
            this.chkModifiedLayerSelector.Tag = ServiceType.ModifiedLayerSelectorService;

            this.cboSelectedKeySaver.Tag = ServiceType.SelectedKeysSaverService;
            this.btnSelectedKeySaver.Tag = ServiceType.SelectedKeysSaverService;

            this.cboSelectedKeyLoader.Tag = ServiceType.SelectedKeysLoaderService;
            this.btnSelectedKeyLoader.Tag = ServiceType.SelectedKeysLoaderService;

            this.cboFillDisplayFrame.Tag = ServiceType.FillDisplayFramesService;
            this.btnFillDisplayFrame.Tag = ServiceType.FillDisplayFramesService;

            this.cboBlinkCanceller.Tag = ServiceType.BlinkCancellerService;
            this.btnBlinkCanceller.Tag = ServiceType.BlinkCancellerService;
            this.chkClickOffsetBtnByShiftEnter.Checked = configs.ClickOffsetBtnByShiftEnter;
            CreateControls();
            this.timer1.Interval = 500;
            this.timer1.Enabled = false;

            var color = this.btnExecuteOffset.BackColor;
            this.btnExecuteOffset.EnabledChanged += (s, e) =>
            {
                if (this.btnExecuteOffset.Enabled)
                    this.btnExecuteOffset.BackColor = color;
                else
                    this.btnExecuteOffset.BackColor = SystemColors.Control;
            };
            this.btnExecuteOffset.Enabled = false;
        }

        private DateTime _datetime = DateTime.MinValue;

        private bool _suspendUpdate = false;

        public void Update(float Frame, float ElapsedTime)
        {
            if (_suspendUpdate)
                return;
            if ((DateTime.Now - _datetime).TotalMilliseconds < 200)
            {
                return;
            }
            if (!this.Enabled)
                return;
            _datetime = DateTime.Now;

            var disableOffset = false;
            if (_scene?.ActiveModel != null)
            {
                if (this._offsetAdder != null)
                {
                    if (this._offsetAdder.FrameNumber > 0 && this._scene.MarkerPosition != this._offsetAdder.FrameNumber)
                        //フレーム位置が変わった→オフセット付加をオフにする
                        disableOffset = true;
                    if (this._prevModelId != _scene.ActiveModel.ID)
                        //モデルが変わった→オフセット付加をオフにする
                        disableOffset = true;
                }
                this._prevModelId = _scene.ActiveModel.ID;
            }
            else
            {
                //アクティブモデルが無い→オフセット付加をオフにする
                disableOffset = true;
            }
            if (disableOffset && this._offsetAdder != null)
                // オフセット付加モードを解除
                InitializeOffsetService(false);

            if (this._offsetAdder != null)
            {
                UpdateDataGridView();
            }
        }

        private List<IMotionFrameData> _prefSelectedFrames = new List<IMotionFrameData>();

        /// <summary>
        /// オフセット量をグリッドに反映する
        /// </summary>
        /// <returns></returns>
        private bool UpdateDataGridView()
        {
            if (_offsetAdder == null)
                // オフセット付加モードでなければ何もしない
                return false;
            if (this._scene.Mode != EditMode.ModelMode)
                return false;
            if (_suspendUpdate)
                return false;
            var currentSelectedFrames = this._scene?.ActiveModel?.Bones.SelectMany(n => n.Layers).SelectMany(n => n.SelectedFrames).ToList();
            if (this._prefSelectedFrames?.Count != currentSelectedFrames?.Count)
            {
                // 選択フレーム数が変化した場合はいったん処理をスキップする
                // 選択フレーム数が変換してすぐに↓の処理を行うとなぜかモデルの変更量が戻ってしまうことがあるため
                _suspendUpdate = true;
                this.timer1.Start();

                _prefSelectedFrames = currentSelectedFrames;
                return false;
            }
            _prefSelectedFrames = currentSelectedFrames;
            var date = DateTime.Now;

            try
            {
                if (this._scene?.ActiveModel != null)
                {
                    // offsetState.ItemsをDataGridViewに反映
                    var gridData = OffsetAdderUtil.UpdateDataGridView(this._scene, dataGridView1, this._offsetAdder.PreviousStates);

                    this.btnExecuteOffset.Enabled = gridData.Any(n => n.FrameCount > 0 && (n.MoveValue != new Vector3() || n.RotationValue != new Quaternion(0, 0, 0, 1)));
                }
            }
            catch
            {
            }
            finally
            {
                _prefSelectedFrames = currentSelectedFrames;
                //Debug.WriteLine($"grid update time: {(DateTime.Now - date).TotalMilliseconds} ms");
            }
            return false;
        }

        private void InitializeOffsetService(bool mode)
        {
            Debug.WriteLine($"mode: {mode}");
            if (mode)
            {
                //オフセット付加モード開始
                _offsetAdder = null;
                _offsetAdder = new OffsetAdderService(this);
                _offsetAdder.ProgressChanged = null;
                _offsetAdder.ProgressChanged += (s, ev) =>
                {
                    this.Invoke(new Action(() =>
                    {
                        if (ev.Value == ev.Maximum)
                            this.progressBar1.Visible = false;
                        else
                            this.progressBar1.Visible = true;

                        this.progressBar1.Maximum = ev.Maximum;
                        this.progressBar1.Value = ev.Value;
                    }));
                };
                _offsetAdder.Initialize(this._scene, this._parentForm);
                OffsetAdderUtil.ConfigureDataGridViewColumns(this.dataGridView1);
                // 現在の状態を保存しなおす

                btnOffset.Text = "戻る";
                this.BackColor = Color.Black;
            }
            else
            {
                btnOffset.Text = "オフセット付加準備";
                this.BackColor = _defBackColor;
                //オフセット付加モードオフなら何もしない
                // DataGridViewもクリア
                this.dataGridView1.DataSource = null;
                //オフセット付加モード解除
                if (_offsetAdder != null)
                    _offsetAdder.ProgressChanged = null;
                _offsetAdder = null;
            }

            this.dataGridView1.Visible = mode;
            this.pnlOffset.Visible = mode;
            this.panel.Visible = !mode;
        }

        private void btnOffset_Click(object sender, EventArgs e)
        {
            var mode = (btnOffset.Text == "オフセット付加準備");
            if (_scene?.ActiveModel == null)
            {
                mode = false;
            }
            InitializeOffsetService(mode);
        }

        private void CreateControls()
        {
            this.panel.BeginUpdate();
            this.panel.Enabled = false;
            try
            {
                var keysList = new List<Keys>();
                keysList.Add(Keys.Enter);
                keysList.Add(Keys.Space);
                keysList.Add(Keys.ShiftKey);
                keysList.Add(Keys.ControlKey);
                // A-Zのキーを追加
                for (Keys key = Keys.A; key <= Keys.Z; key++)
                {
                    switch (key)
                    {
                        case Keys.D:
                        case Keys.G:
                        case Keys.I:
                        case Keys.K:
                        case Keys.P:
                        case Keys.W:
                            // MMMのショートカットキーのうち制作に影響がでそうなものと被るキーは除外
                            continue;
                    }
                    keysList.Add(key);
                }
                var comboBoxes = GetAllComboBoxes();
                var chkboxes = GetAllChkBoxes();
                foreach (var cbo in comboBoxes)
                {
                    cbo.Items.Clear();
                    cbo.Items.Add(Keys.None);

                    var serviceType = (ServiceType)cbo.Tag;
                    ConfigItem serviceItem = null;

                    if (_configs.Services.Any(s => s.ServiceType == serviceType))
                        serviceItem = _configs.Services.FirstOrDefault(p => p.ServiceType == serviceType);

                    cbo.SelectedItem = Keys.None;
                    foreach (var key in keysList)
                    {
                        if (!_configs.Services.Any(n => n.Keys == key) || serviceItem?.Keys == key)
                        {
                            cbo.Items.Add(key);
                            if (serviceItem?.Keys == key)
                            {
                                cbo.SelectedItem = key;
                            }
                        }
                    }
                    var chkbox = chkboxes.FirstOrDefault(n => n.Tag != null && n.Tag.Equals(serviceType));
                    if (chkbox != null && serviceItem != null)
                    {
                        chkbox.Checked = serviceItem.Inverse;
                    }
                }
                var interpolateService = _configs.Services.FirstOrDefault(n => n.ServiceType == ServiceType.InterpolateSetterService);
                if (interpolateService != null)
                {
                    switch (interpolateService.InterpolateType)
                    {
                        case InterpolateType.R:
                            rbInterpolateR.Checked = true;
                            break;

                        case InterpolateType.X:
                            rbInterpolateX.Checked = true;
                            break;

                        case InterpolateType.Y:
                            rbInterpolateY.Checked = true;
                            break;

                        case InterpolateType.Z:
                            rbInterpolateZ.Checked = true;
                            break;

                        case InterpolateType.ALL:
                            rbInterpolateAll.Checked = true;
                            break;

                        default:
                            rbInterpolateR.Checked = true;
                            break;
                    }
                }
            }
            finally
            {
                this.panel.Enabled = true;
                this.panel.EndUpdate();
            }
        }

        private List<ComboBox> GetAllComboBoxes()
        {
            var list = new List<ComboBox>();
            foreach (Control ctrl in this.panel.Controls.OfType<Control>().OrderBy(c => c.Location.Y))
            {
                if (ctrl is ComboBox cbo)
                {
                    if (ctrl.Tag != null && ctrl.Tag.GetType() == typeof(ServiceType))
                        list.Add(cbo);
                }
            }
            return list;
        }

        private List<CheckBox> GetAllChkBoxes()
        {
            var list = new List<CheckBox>();
            foreach (Control ctrl in this.panel.Controls.OfType<Control>().OrderBy(c => c.Location.Y))
            {
                if (ctrl is CheckBox cbo)
                {
                    if (ctrl.Tag.GetType() == typeof(ServiceType))
                        list.Add(cbo);
                }
            }
            return list;
        }

        private void cboGapSelector_Format(object sender, ListControlConvertEventArgs e)
        {
            var key = (Keys)e.Value;
            if (key == Keys.None)
            {
                e.Value = "なし";
            }
            else if (key == Keys.Enter)
            {
                e.Value = "Enter";
            }
            else
            {
                e.Value = key.ToString();
            }
        }

        private void cboGapSelector_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!this.panel.Enabled)
                return;

            SaveConfig();

            this.CreateControls();
            this.cboGapSelector.Focus();
        }

        public void SaveConfig()
        {
            var comboBoxes = GetAllComboBoxes();
            var chkBoxes = GetAllChkBoxes();
            _configs.Services.Clear();
            foreach (var tmpCombo in comboBoxes)
            {
                var serviceType = (ServiceType)tmpCombo.Tag;
                var key = (Keys)tmpCombo.SelectedItem;
                var chkbox = chkBoxes.FirstOrDefault(n => n.Tag != null && n.Tag.Equals(serviceType));
                var inverse = chkbox == null ? false : chkbox.Checked;

                if (!_configs.Services.Any(s => s.ServiceType == serviceType))
                {
                    _configs.Services.Add(new ConfigItem() { ServiceType = serviceType, Keys = key, Inverse = inverse });
                }
            }

            if (chkLayerBoneSelector.Checked)
                btnLayerBoneSelector.Text = "選択中のキーの内レイヤーボーン以外を選択";
            else
                btnLayerBoneSelector.Text = "選択中のキーの内レイヤーボーンのみ選択";
            if (chkModifiedLayerSelector.Checked)
                btnModifiedLayerSelector.Text = "現Frで変更が加えられてないボーンを全選択";
            else
                btnModifiedLayerSelector.Text = "現Frで変更が加えられているボーンを全選択";

            var interpolateConfig = Configs.CreateInterpolateSetterService();
            interpolateConfig.InterpolateType = GetInterpolateType();
            if (interpolateConfig.InterpolateType == InterpolateType.none)
                interpolateConfig.InterpolateType = InterpolateType.R;

            _configs.Services.Add(interpolateConfig);
            _configs.ClickOffsetBtnByShiftEnter = this.chkClickOffsetBtnByShiftEnter.Checked;
            try
            {
                MyUtility.Serializer.Serialize(this._configs, Configs.GetConfigFilePath());
            }
            catch (Exception)
            {
                // ログ出力やエラーハンドリングが必要に応じて追加
            }
        }

        private void btnGapSelector_Click(object sender, EventArgs e)
        {
            if (ServiceFactory.IsBusy)
            {
                return;
            }
            var serviceType = (ServiceType)((Button)sender).Tag;
            var config = _configs.Services.FirstOrDefault(n => n.ServiceType == serviceType);
            ExecuteService(config);
        }

        public bool TryClickOffsetButton()
        {
            if (chkClickOffsetBtnByShiftEnter.Checked)
                if (this.btnExecuteOffset.Visible && this.btnExecuteOffset.Enabled)
                {
                    this.btnExecuteOffset.PerformClick();
                    return true;
                }

            return false;
        }

        /// <summary>
        /// 各種サービスを実行する
        /// </summary>
        /// <param name="config"></param>
        /// <param name="key"></param>
        public bool ExecuteService(ConfigItem config, Keys key = Keys.None)
        {
            if (config == null)
                return false;

            var service = ServiceFactory.Create(config.ServiceType, this._scene, this._parentForm);
            if (service == null)
                return false;

            if (service is SelectedKeysLoaderService loaderService)
                loaderService.HistoryIndex = this.cboHistory.SelectedIndex;
            if (service is InterpolateService interpolateSetterService)
            {
                int palletteindex = GetPalletteIndex(key);
                InterpolateType interpolateType = GetInterpolateType();
                interpolateSetterService.SetTypeAndPallette(palletteindex, interpolateType);
                if (config.InterpolateType == InterpolateType.none)
                    config.InterpolateType = InterpolateType.R;
            }
            service.Execute(config);
            ApplyService(service, config);
            MMDUtilility.SetForegroundWindow(this._parentForm.Handle);
            return true;
        }

        private InterpolateType GetInterpolateType()
        {
            if (rbInterpolateR.Checked)
                return InterpolateType.R;
            else if (rbInterpolateX.Checked)
                return InterpolateType.X;
            else if (rbInterpolateY.Checked)
                return InterpolateType.Y;
            else if (rbInterpolateZ.Checked)
                return InterpolateType.Z;
            else if (rbInterpolateAll.Checked)
                return InterpolateType.ALL;
            else
                return InterpolateType.none;
        }

        private int GetPalletteIndex(Keys key)
        {
            switch (key)
            {
                case Keys.D0:
                case Keys.NumPad0:
                    return -1; // デフォルト値
                case Keys.D1:
                case Keys.NumPad1:
                    return 0;

                case Keys.D2:
                case Keys.NumPad2:
                    return 1;

                case Keys.D3:
                case Keys.NumPad3:
                    return 2;

                case Keys.D4:
                case Keys.NumPad4:
                    return 3;

                case Keys.D5:
                case Keys.NumPad5:
                    return 4;

                case Keys.D6:
                case Keys.NumPad6:
                    return 5;

                default:
                    return -10;
            }
        }

        /// <summary>
        /// サービスの実行結果をフォームに反映させる
        /// </summary>
        /// <param name="service"></param>
        /// <param name="config"></param>
        private void ApplyService(BaseService service, ConfigItem config)
        {
            if (service == null || config == null)
                return;
            if (service is SelectedKeysSaverService saverService)
            {
                this.cboHistory.BeginUpdate();
                try
                {
                    this.cboHistory.Items.Clear();
                    if (SelectedKeysSaverService.Histories.Count > 0)
                    {
                        this.cboHistory.Items.AddRange(SelectedKeysSaverService.Histories.ToArray());
                        this.cboHistory.SelectedIndex = 0;
                    }
                }
                finally
                {
                    this.cboHistory.EndUpdate();
                }
            }

            if (service is InterpolateService interpolateSetterService)
                SaveConfig();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            this.timer1.Stop();
            _suspendUpdate = false;
        }

        private void frmMain_Load(object sender, EventArgs e)
        {
        }

        private int _offsetExecuted = 0;

        private int OffsetExecuted
        {
            get { return _offsetExecuted; }
            set
            {
                _offsetExecuted = value;
                if (_offsetExecuted == 0)
                {
                    btnUndo.Text = $"アンドゥ";
                    btnUndo.Enabled = false;
                }
                else
                {
                    btnUndo.Text = $"アンドゥ*{_offsetExecuted}";
                    btnUndo.Enabled = true;
                }
            }
        }

        private void btnExecuteOffset_Click(object sender, EventArgs e)
        {
            if (_offsetAdder == null)
                // オフセット付加モードでなければ何もしない
                return;

            // グリッド更新
            UpdateDataGridView();
            var gridData = dataGridView1.DataSource as List<OffsetGridItem>;
            if (gridData == null)
                return;

            var offsetExecuted = gridData.Sum(n => n.FrameCount);
            var msg = $"計 {offsetExecuted}個のキーにオフセット付加します。\r\n\r\nよろしいですか？\r\n\r\n*処理中はPCに触れないでください。\r\n正しくオフセットが付与されなくなる可能性があります";
            if (MessageBox.Show(this, msg, "確認", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) != DialogResult.Yes)
                return;

            this.Cursor = Cursors.WaitCursor;
            this._parentForm.Cursor = Cursors.WaitCursor;
            try
            {
                _offsetAdder.Execute(new ConfigItem() { ServiceType = ServiceType.OffsetAdderService });
                this.OffsetExecuted = offsetExecuted;
            }
            finally
            {
                this.Cursor = Cursors.Default;
                this._parentForm.Cursor = Cursors.Default;
                InitializeOffsetService(false);
            }
        }

        private void btnUndo_Click(object sender, EventArgs e)
        {
            var msg = $"{this.OffsetExecuted}回アンドゥを行います。\r\n\r\nよろしいですか？\r\n\r\n*処理中はPCに触れないでください。\r\n必要な回数アンドゥが適用されなくなる可能性があります*";
            if (MessageBox.Show(this, msg, "確認", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) != DialogResult.Yes)
                return;

            this.Enabled = false;
            this.Cursor = Cursors.WaitCursor;
            this._parentForm.Cursor = Cursors.WaitCursor;
            this.progressBar1.Visible = true;
            this.progressBar1.Maximum = this.OffsetExecuted;
            this.progressBar1.Value = 0;
            ServiceFactory.IsBusy = true;
            this._parentForm.BeginAndEndUpdate(false);
            try
            {
                // Ctrl+Zを複数回送信（実行回数分）
                for (int i = 0; i < this.OffsetExecuted; i++)
                {
                    MMMUtilility.SendCtrlZToFormWithKeyboardEvent(this._parentForm);
                    System.Threading.Thread.Sleep(5); // 各操作間に短い遅延を挿入
                    this.progressBar1.Value++;
                    System.Windows.Forms.Application.DoEvents();
                }
                this.OffsetExecuted = 0; // カウンターをリセット
            }
            finally
            {
                this.Enabled = true;
                this.Cursor = Cursors.Default;
                this._parentForm.Cursor = Cursors.Default;
                this.progressBar1.Visible = false;
                this._parentForm.BeginAndEndUpdate(true);
                ServiceFactory.IsBusy = false;
                InitializeOffsetService(false);
            }
        }

        private void cboHistory_Format(object sender, ListControlConvertEventArgs e)
        {
            var history = e.Value as service.KeySaverHistory;
            if (history == null)
            {
                e.Value = "";
            }
            else
            {
                e.Value = $"{history.DateTime.ToString("HH:mm:ss")} (b:{history.SelectedBones.Count},m:{history.SelectedMorphs.Count})";
            }
        }

        private void chkClickOffsetBtnByShiftEnter_CheckedChanged(object sender, EventArgs e)
        {
            if (this.Visible)
                SaveConfig();
        }
    }
}