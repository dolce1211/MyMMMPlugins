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

        //マウスのクリック位置を記憶
        private Point mousePoint;

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
            _datetime = DateTime.Now;
            this.button1.Enabled = !(_scene?.ActiveModel == null);
            if (_offsetAdder == null)
                // オフセット付与モードでなければ何もしない
                return;

            ExecuteUpdate();
        }

        private List<IMotionFrameData> _prefSelectedFrames = new List<IMotionFrameData>();

        private bool ExecuteUpdate()
        {
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

            this._offsetAdder.Update();

            //this._offsetAdder.BeginAndEndUpdate(false);

            try
            {
                if (this._scene?.ActiveModel != null)
                {
                    var offsetState = OffsetAdderUtil.CreateOffsetState(this._scene);

                    // offsetState.ItemsをDataGridViewに反映
                    var gridData = OffsetAdderUtil.UpdateDataGridView(this._scene, dataGridView1, this._offsetAdder.PreviousStates, offsetState);

                    this.btnExecuteOffset.Enabled = gridData.Any(n => n.FrameCount > 0 && (n.MoveValue != new Vector3() || n.RotationValue != new Quaternion(0, 0, 0, 1)));
                }
            }
            catch
            {
            }
            finally
            {
                //this._offsetAdder.BeginAndEndUpdate(true);
                _prefSelectedFrames = currentSelectedFrames;
            }
            return false;
        }

        private void SwitchOffsetAddMode()
        {
            int mode = 0;
            if (_scene?.ActiveModel == null)
            {
                _offsetAdder = null;
            }
            if (_offsetAdder != null)
            {
                //オフセット付与モードオン
                mode = 1;
            }
            else
            {
                //オフセット付与モードオフ
                mode = 0;
            }

            ApplyAndGetCurrentMode(mode);
        }

        /// <summary>
        /// オフセット付与モードの適用と現在のモードの取得を行う
        /// </summary>
        /// <param name="mode">
        /// 0:オフセット付与モードオフ
        /// 1:オフセット付与モードオン
        /// </param>
        /// <returns>
        /// </returns>
        private int ApplyAndGetCurrentMode(int mode)
        {
            var ret = 0;
            if (_scene?.ActiveModel == null)
            {
                _offsetAdder = null;
            }
            if (mode == 1)
            {
                button1.Text = "戻る";
                this.BackColor = Color.Black;

                ret = 1;
            }
            else
            {
                button1.Text = "オフセット付与準備";
                this.BackColor = _defBackColor;
                //オフセット付与モードオフなら何もしない
                // DataGridViewもクリア
                this.dataGridView1.DataSource = null;
            }

            var visible = (mode == 1);
            this.dataGridView1.Visible = visible;
            this.pnlOffset.Visible = visible;
            this.panel.Visible = !visible;

            return ret;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (_scene?.ActiveModel == null)
            {
                return;
            }
            if (_offsetAdder == null)
            {
                //オフセット付与モード開始
                _offsetAdder = new OffsetAdderService();
                _offsetAdder.Initialize(this._scene, this._parentForm, this, this.progressBar1);
                OffsetAdderUtil.ConfigureDataGridViewColumns(this.dataGridView1);
            }
            else
            {
                //オフセット付与モード解除
                _offsetAdder = null;
            }
            ApplyAndGetCurrentMode(_offsetAdder == null ? 0 : 1);
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

            try
            {
                MyUtility.Serializer.Serialize(this._configs, Configs.GetConfigFilePath());
            }
            catch (Exception ex)
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

        public void ExecuteService(ConfigItem config)
        {
            if (config == null)
                return;
            var service = ServiceFactory.Create(config.ServiceType, this._scene, this._parentForm);
            if (service != null)
            {
                if (service is SelectedKeysLoaderService loaderService)
                    loaderService.HistoryIndex = this.cboHistory.SelectedIndex;

                service.Execute(config);
                ApplyService(service, config);
            }

            MMDUtilility.SetForegroundWindow(this._parentForm.Handle);
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
            ExecuteUpdate();
            if (_offsetAdder == null)
                return;

            var gridData = dataGridView1.DataSource as List<OffsetGridItem>;
            if (gridData == null)
                return;

            var offsetExecuted = gridData.Sum(n => n.FrameCount);
            var msg = $"計 {offsetExecuted}個のキーにオフセット付与します。\r\n\r\nよろしいですか？";
            if (MessageBox.Show(msg, "確認", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) != DialogResult.Yes)
                return;

            _offsetAdder.Execute(new ConfigItem() { ServiceType = ServiceType.OffsetAdderService });

            this.OffsetExecuted = offsetExecuted;
        }

        private void btnUndo_Click(object sender, EventArgs e)
        {
            var msg = $"{this.OffsetExecuted}回アンドゥを行います。\r\n\r\nよろしいですか？";
            if (MessageBox.Show(msg, "確認", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) != DialogResult.Yes)
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
    }
}