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
        private Config _config;

        public frmMain(Form parentForm, Scene scene, Config config)
        {
            InitializeComponent();
            this.panel.Location = this.dataGridView1.Location;
            _parentForm = parentForm;
            _scene = scene;
            _config = config;

            DateTime lastPressed = DateTime.MinValue;

            _defBackColor = this.BackColor;
            this.cboGapSelector.Tag = ServiceType.GapSelectorService;
            this.btnGapSelector.Tag = ServiceType.GapSelectorService;

            this.cboLayerBoneSelector.Tag = ServiceType.LayerBoneSelectorService;
            this.btnLayerBoneSelector.Tag = ServiceType.LayerBoneSelectorService;

            this.cboModifiedLayerSelector.Tag = ServiceType.ModifiedLayerSelectorService;
            this.btnModifiedLayerSelector.Tag = ServiceType.ModifiedLayerSelectorService;

            this.cboSelectedKeySaver.Tag = ServiceType.SelectedKeysSaverService;
            this.btnSelectedKeySaver.Tag = ServiceType.SelectedKeysSaverService;

            this.cboSelectedKeyLoader.Tag = ServiceType.SelectedKeysLoaderService;
            this.btnSelectedKeyLoader.Tag = ServiceType.SelectedKeysLoaderService;

            CreateComboBoxes();
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

            this._offsetAdder.BeginAndEndUpdate(false);

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
                this._offsetAdder.BeginAndEndUpdate(true);
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

        private void chkLayerBoneSelector_CheckedChanged(object sender, EventArgs e)
        {
            if (chkLayerBoneSelector.Checked)
                this.btnLayerBoneSelector.Text = "選択中のキーの内レイヤーボーンを非選択";
            else
                this.btnLayerBoneSelector.Text = "選択中のキーの内レイヤーボーンを残して非選択";
        }

        private void CreateComboBoxes()
        {
            this.panel.Enabled = false;
            try
            {
                var keys = new List<Keys>();
                keys.Add(Keys.Enter);
                keys.Add(Keys.Space);

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
                    keys.Add(key);
                }
                var comboBoxes = GetAllComboBoxes();
                foreach (var cbo in comboBoxes)
                {
                    cbo.Items.Clear();
                    cbo.Items.Add(Keys.None);
                    foreach (var key in keys)
                    {
                        cbo.Items.Add(key);
                    }
                    var serviceType = (ServiceType)cbo.Tag;
                    if (_config.Services.Any(s => s.ServiceType == serviceType))
                    {
                        var serviceItem = _config.Services.FirstOrDefault(p => p.ServiceType == serviceType);
                        cbo.SelectedItem = serviceItem.Keys;
                        if (serviceItem.Keys != Keys.None)
                            keys.Remove(serviceItem.Keys); // 他のコンボボックスで選択されないようにする
                    }
                    else
                    {
                        cbo.SelectedItem = Keys.None;
                    }
                }
            }
            finally
            {
                this.panel.Enabled = true;
            }
        }

        private List<ComboBox> GetAllComboBoxes()
        {
            return new List<ComboBox> {
            this.cboGapSelector,
            this.cboLayerBoneSelector,
            this.cboModifiedLayerSelector,
            this.cboSelectedKeySaver,
            this.cboSelectedKeyLoader
            };
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
            ComboBox cbo = (ComboBox)sender;

            SaveConfig();

            this.CreateComboBoxes();
        }

        public void SaveConfig()
        {
            var comboBoxes = GetAllComboBoxes();
            _config.Services.Clear();
            foreach (var tmpCombo in comboBoxes)
            {
                var serviceType = (ServiceType)tmpCombo.Tag;
                var key = (Keys)tmpCombo.SelectedItem;

                if (!_config.Services.Any(s => s.ServiceType == serviceType))
                {
                    _config.Services.Add(new ConfigItem() { ServiceType = serviceType, Keys = key });
                }
            }

            try
            {
                MyUtility.Serializer.Serialize(this._config, Config.GetConfigFilePath());
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
            int mode = 0;
            if (serviceType == ServiceType.LayerBoneSelectorService)
                mode = chkLayerBoneSelector.Checked ? 1 : 0;
            var service = ServiceFactory.Create(serviceType, this._scene, this._parentForm);
            if (service != null)
                service.Execute(mode);

            MMDUtilility.SetForegroundWindow(this._parentForm.Handle);
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

            _offsetAdder.Execute(0);

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
    }
}