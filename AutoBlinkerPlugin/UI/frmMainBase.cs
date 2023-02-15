using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MyUtility;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Runtime.CompilerServices;
using System.Windows.Media.Media3D;

namespace AutoBlinkerPlugin
{
    public partial class frmMainBase : Form
    {
        /// <summary>
        /// MMDかMMMか
        /// </summary>
        public static OperatingMode OperationgMode { get; protected set; } = OperatingMode.OnMMM;

        /// <summary>
        /// 選択中のモデル
        /// </summary>
        protected ModelItem _currentModel = null;

        /// <summary>
        /// 最小化されてればtrue
        /// </summary>
        protected bool _isMinimized = false;

        private string _path = string.Empty;
        private SavedState _savedState = null;

        /// <summary>
        /// コピーされた値
        /// </summary>
        private ModelSetting _clipboard = null;

        public event EventHandler<ExecutedEventArgs> Executed;

        private bool _isInitialized = false;

        public frmMainBase()
        {
            InitializeComponent();
        }

        /// <summary>
        /// お気に入りを反映させます。
        /// </summary>
        private void ApplyFavourite(bool noselection = false)
        {
            if (_savedState.Favorites?.Count == 0 && string.IsNullOrWhiteSpace(_savedState.Version))
                _savedState.Favorites.AddRange(PresetCreator.CreateFavs());

            if (string.IsNullOrWhiteSpace(_savedState.Exceptions))
                _savedState.Exceptions = string.Join(",", PresetCreator.CreateExceptions());

            List<FavEntity> favs = _savedState.Favorites.Where(n => n != null).ToList();
            this.lstFav.Items.Clear();
            this.lstFav.Items.Add("");
            favs.ForEach(n => this.lstFav.Items.Add(n));
            if (!noselection)
                this.lstFav.SelectedIndex = 0;

            this.ApplyFavColumn();
            MyUtility.Serializer.Serialize<SavedState>(_savedState, _path);
        }

        /// <summary>
        /// 基本情報を反映させます。
        /// </summary>
        /// <param name="rawEntity"></param>
        private void ApplyBasicInfo(RawEntity rawEntity)
        {
            this.numEnter.Value = rawEntity.EnterFrames;
            this.numBlink.Value = rawEntity.BlinkingFrames;
            this.numExit.Value = rawEntity.ExitFrames;
            this.numExtraStart.Value = rawEntity.HandouFramesStart;
            this.numExtraEnd.Value = rawEntity.HandouFramesEnd;
            this.chkMayu.Checked = rawEntity.DoEyebrowSync;
            this.chkStartExtra.Checked = rawEntity.DoHandouStart;
            this.chkEndExtra.Checked = rawEntity.DoHandouEnd;

            this.chkHokan.Checked = rawEntity.DoHokan;
            this.chkEyeSync.Checked = rawEntity.DoEyeSync;
            this.chkEyeMotionLayer.Checked = rawEntity.CreateEyeMotionLayer;

            this.chkYuruyaka.Checked = rawEntity.DoYuruyaka;
            this.txtYuruyakaValue.Text = rawEntity.YuruyakaValue.ToString();
            this.numYuruyaka.Value = rawEntity.YuruyakaFrame;
        }

        /// <summary>
        /// 選択中のモデルを反映させます。
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        protected bool ApplyModel(ModelItem model)
        {
            this.cboBlink.Items.Clear();
            this.cboBikkuri.Items.Clear();
            this.cboMayuDown.Items.Clear();
            this.cboMayuUp.Items.Clear();

            this.cboBlink.Items.Add("");
            this.cboBikkuri.Items.Add("");
            this.cboMayuDown.Items.Add("");
            this.cboMayuUp.Items.Add("");

            this._currentModel = model;
            this.btnOK.Enabled = false;

            if (model == null)
                return false;

            this.btnOK.Enabled = true;

            this.Text = $"まばたき作成： {model.ModelName}";
            this.lblModel.Text = model.ModelName;

            var eyeMorphs = model.EyeMorphItems.Select(n => n.MorphName).ToArray();
            var mayuMorphs = model.BrowMorphItems.Select(n => n.MorphName).ToArray();

            if (eyeMorphs != null && eyeMorphs.Length > 0)
            {
                this.cboBlink.Items.AddRange(eyeMorphs);
                this.cboBikkuri.Items.AddRange(eyeMorphs);
            }
            if (mayuMorphs != null && mayuMorphs.Length > 0)
            {
                this.cboMayuDown.Items.AddRange(mayuMorphs);
                this.cboMayuUp.Items.AddRange(mayuMorphs);
            }

            //まばたきモーフ
            var morphnames = new string[]
            {
                "まばたき"
            };
            if (this.ApplyMorphToComboBox(eyeMorphs, this.cboBlink, morphnames)) { }
            else
                this.cboBlink.SelectedIndex = 0;

            //見開きモーフ
            morphnames = new string[]
            {
                "びっくり",
                "ビックリ",
                "見開き",
                "みひらき"
            };
            if (this.ApplyMorphToComboBox(eyeMorphs, this.cboBikkuri, morphnames)) { }
            else
                this.cboBikkuri.SelectedIndex = 0;

            //まゆ下モーフ
            morphnames = new string[]
            {
                "下",
                "まゆ下",
                "眉下",
                "眉毛下",
                "眉毛下げ"
            };
            if (this.ApplyMorphToComboBox(mayuMorphs, this.cboMayuDown, morphnames)) { }
            else
                this.cboMayuDown.SelectedIndex = 0;

            //まゆ上モーフ
            morphnames = new string[]
            {
                "上",
                "まゆ上",
                "眉上",
                "眉毛上",
                "眉毛上げ"
            };
            if (this.ApplyMorphToComboBox(mayuMorphs, this.cboMayuUp, morphnames)) { }
            else
                this.cboMayuUp.SelectedIndex = 0;

            var modelInfo = _savedState.ModelInfos.Where(n => n.ModelName.TrimSafe() == model.ModelName.TrimSafe()).FirstOrDefault();
            this.ApplyModelInfo(modelInfo);

            return true;
        }

        /// <summary>
        /// モデル固有の情報を適用します。
        /// </summary>
        /// <param name="modelInfo"></param>
        /// <returns></returns>
        private bool ApplyModelInfo(ModelSetting modelInfo)
        {
            var eyeMorphs = this._currentModel.EyeMorphItems.Select(n => n.MorphName).ToArray();
            var mayuMorphs = this._currentModel.BrowMorphItems.Select(n => n.MorphName).ToArray();

            //両目ボーン
            this.cboEyeBone.Items.Clear();
            this.cboEyeBone.Items.Add("");
            this.cboEyeBone.Items.AddRange(this._currentModel.Bones.ToArray());
            this.ApplyBoneToComboBox(this.cboEyeBone, "両目");

            this.tbBikkuri.Value = 20;
            this.tbMayuDown.Value = 4;
            this.tbMayuUp.Value = 4;
            this.lblEyeSync.Tag = new float[] { 1f, 10f };

            if (modelInfo != null)
            {
                if (modelInfo.EyeSyncValueDown == 0f && modelInfo.EyeSyncValueUp == 0f)
                {
                    //前バージョンから来たっぽい
                    var tmpeyesyncvalue = this.lblEyeSync.Tag as float[];
                    modelInfo.EyeSyncValueUp = tmpeyesyncvalue[0];
                    modelInfo.EyeSyncValueDown = tmpeyesyncvalue[1];
                }
            }
            //Entityを生成する
            if (modelInfo != null)
            {
                //まばたきモーフ適用
                this.ApplyMorphToComboBox(eyeMorphs, this.cboBlink, new string[] { modelInfo.BlinkingMorphName });
                //びっくりモーフ適用
                this.ApplyMorphToComboBox(eyeMorphs, this.cboBikkuri, new string[] { modelInfo.BikkuriMorphName });

                //眉下適用
                this.ApplyMorphToComboBox(mayuMorphs, this.cboMayuDown, new string[] { modelInfo.EyebrowDownMorphName });
                //眉上適用
                this.ApplyMorphToComboBox(mayuMorphs, this.cboMayuUp, new string[] { modelInfo.EyebrowUpMorphName });

                this.tbBikkuri.Value = (int)(modelInfo.BikkuriMorphValue * 20);
                this.tbMayuDown.Value = (int)(modelInfo.EyebrowDownSyncValue * 20);
                this.tbMayuUp.Value = (int)(modelInfo.EyebrowUpSyncValue * 20);

                this.lblEyeSync.Tag = new float[] { modelInfo.EyeSyncValueUp, modelInfo.EyeSyncValueDown };

                if (!this.ApplyBoneToComboBox(this.cboEyeBone, modelInfo.EyeSyncBoneName))
                {
                    //MMDではボーン回転を加算で指定することができないため、両目ボーンそのものは指定しないほうがいい。
                    this.ApplyBoneToComboBox(this.cboEyeBone, "両目");
                }
            }
            var eyesyncvalue = this.lblEyeSync.Tag as float[];
            this.lblEyeSync.Text = $"{eyesyncvalue[0]}度, {eyesyncvalue[1]}度";

            this.chkEyeSync.Enabled = true;
            if (!_currentModel.Bones.Contains("両目"))
                this.chkEyeSync.Enabled = false;
            chkMayu_CheckedChanged(this, new EventArgs());
            chkEyeSync_CheckedChanged(this, new EventArgs());
            chuYuruyaka_CheckedChanged(this, new EventArgs());
            return true;
        }

        private bool ApplyBoneToComboBox(ComboBox cbo, string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                return false;

            try
            {
                cboEyeBone.SelectedItem = value;
                return true;
            }
            catch (Exception)
            {
                cboEyeBone.SelectedIndex = 0;
                return false;
            }
        }

        /// <summary>
        /// モーフコンボに適用します。
        /// </summary>
        /// <param name="morphs"></param>
        /// <param name="cbo"></param>
        /// <param name="nameArray"></param>
        /// <returns></returns>
        private bool ApplyMorphToComboBox(string[] morphs, ComboBox cbo, string[] nameArray)
        {
            foreach (var name in nameArray.Where(n => n.TrimSafe() != ""))
            {
                cbo.SelectedIndex = 0;
                if (morphs.Contains(name))
                {
                    try
                    {
                        cbo.SelectedItem = name;
                    }
                    catch (Exception)
                    {
                    }
                    if (cbo.SelectedIndex > 0)
                        return true;
                }
            }
            return false;
        }

        /// <summary>
        /// 画面の状態からEntityのインスタンスを生成します。
        /// </summary>
        /// <returns></returns>
        private Args CreateEntityInstance()
        {
            if (_currentModel == null || string.IsNullOrEmpty(_currentModel.ModelName))
                return null;
            var eyesyncvalue = this.lblEyeSync.Tag as float[];
            if (eyesyncvalue == null || eyesyncvalue.Length < 2)
                eyesyncvalue = new float[] { 1f, 10f };

            ModelSetting modelInfo = new ModelSetting()
            {
                ModelName = this._currentModel.ModelName,
                BlinkingMorphName = cboBlink.SelectedItem.ToString(),
                BikkuriMorphName = cboBikkuri.SelectedItem.ToString(),
                BikkuriMorphValue = (float)this.tbBikkuri.Value / 20,
                EyebrowDownMorphName = cboMayuDown.SelectedItem.ToString(),
                EyebrowUpMorphName = cboMayuUp.SelectedItem.ToString(),
                EyebrowDownSyncValue = (float)this.tbMayuDown.Value / 20,
                EyebrowUpSyncValue = (float)this.tbMayuUp.Value / 20,
                EyeSyncValueUp = eyesyncvalue[0],
                EyeSyncValueDown = eyesyncvalue[1],
                EyeSyncBoneName = this.cboEyeBone.Text,
            };

            Args entity = new Args()
            {
                ModelInfo = modelInfo,
                EnterFrames = Convert.ToInt32(numEnter.Value),
                BlinkingFrames = Convert.ToInt32(numBlink.Value),
                ExitFrames = Convert.ToInt32(numExit.Value),

                DoHandouStart = this.chkStartExtra.Enabled && this.chkStartExtra.Checked,
                DoHandouEnd = this.chkEndExtra.Enabled && this.chkEndExtra.Checked,
                HandouFramesStart = Convert.ToInt32(this.numExtraStart.Value),
                HandouFramesEnd = Convert.ToInt32(this.numExtraEnd.Value),
                DoEyebrowSync = this.chkMayu.Checked,
                DoEyeSync = this.chkEyeSync.Enabled && this.chkEyeSync.Checked,
                DoHokan = this.chkHokan.Checked,
                CreateEyeMotionLayer = this.chkEyeMotionLayer.Checked,
                DoYuruyaka = this.chkYuruyaka.Checked,
                YuruyakaValue = float.Parse(this.txtYuruyakaValue.Text),
                YuruyakaFrame = Convert.ToInt32(this.numYuruyaka.Value)
            };

            entity.Exceptions = _savedState.Exceptions;
            return entity;
        }

        /// <summary>
        /// 現在の状態を保存します。
        /// </summary>
        private void SaveCurrentState()
        {
            var entity = this.CreateEntityInstance();
            if (entity == null)
                return;
            //状態を保存する
            var savedState = entity.CloneToSavedState();
            savedState.Favorites = new List<FavEntity>();
            savedState.ModelInfos.AddRange(_savedState.ModelInfos);
            savedState.Favorites.AddRange(_savedState.Favorites.Where(n => n != null));
            savedState.IsFavOpen = _savedState.IsFavOpen;
            savedState.Version = _savedState.Version;
            savedState.TopMost = this.chkTopMost.Checked;
            this._savedState = savedState;
            var oldvalue = this._savedState.ModelInfos.Where(n => n.ModelName == this._currentModel.ModelName).FirstOrDefault();
            if (oldvalue != null)
                _savedState.ModelInfos.Remove(oldvalue);
            _savedState.ModelInfos.Add(entity.ModelInfo);
            MyUtility.Serializer.Serialize<SavedState>(_savedState, _path);
        }

        /// <summary>
        /// groupboxの開け閉めを行います。
        /// </summary>
        /// <param name="gb"></param>
        /// <param name="isopen"></param>
        /// <param name="gbheight"></param>
        private void OpenAndClose(GroupBox gb, bool isopen, int gbheight)
        {
            this.BeginUpdate();
            try
            {
                var prevHeight = gb.Height;
                if (isopen)
                    gb.Height = gbheight;
                else
                    gb.Height = 22;

                this.Height += (gb.Height - prevHeight);
                this.tlp.Height += (gb.Height - prevHeight);
            }
            finally
            {
                if (this._isInitialized)
                    this.EndUpdate();
            }
        }

        protected virtual void frmMain_Load(object sender, EventArgs e)
        {
            this.BeginAndEndUpdate(false);
            try
            {
                this.lblModel.Text = string.Empty;

                string basepath = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
                _path = System.IO.Path.Combine(basepath, "AutoBlinkerPluginPatterns.xml");

                _savedState = MyUtility.Serializer.Deserialize<SavedState>(_path);
                if (_savedState == null)
                    _savedState = new SavedState();

                if (_savedState.Favorites == null)
                    _savedState.Favorites = new List<FavEntity>();
                if (string.IsNullOrWhiteSpace(_savedState.Version))
                    _savedState.Version = string.Empty;

                this.chkTopMost.Checked = _savedState.TopMost;
                this.ApplyBasicInfo(_savedState);
                this.ApplyFavourite();

                System.Diagnostics.FileVersionInfo vi =
                    System.Diagnostics.FileVersionInfo.GetVersionInfo(
                         System.IO.Path.Combine(basepath, "AutoBlinkerPlugin.dll"));
                _savedState.Version = vi.FileVersion;

                chkMayu_CheckedChanged(this, new EventArgs());
                chkEyeSync_CheckedChanged(this, new EventArgs());
                chuYuruyaka_CheckedChanged(this, new EventArgs());

                this.txtYuruyakaValue.LimitInputToNum(true, true);
                this._isInitialized = true;
            }
            finally
            {
                this.BeginAndEndUpdate(true);
            }
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (!this.btnOK.Enabled)
                return;

            //Entityを生成
            var entity = this.CreateEntityInstance();
            if (entity == null)
                return;

            //状態を保存する
            this.SaveCurrentState();

            if (!entity.DoHandouStart)
                entity.HandouFramesStart = 0;
            if (!entity.DoHandouEnd)
                entity.HandouFramesEnd = 0;
            if (!entity.DoYuruyaka)
            {
                entity.YuruyakaFrame = 0;
                entity.YuruyakaValue = 0;
            }
            if (entity.DoEyeSync)
                if (string.IsNullOrWhiteSpace(entity.ModelInfo.EyeSyncBoneName))
                    entity.DoEyeSync = false;

            this.btnOK.Enabled = false;
            this.Executed?.Invoke(this, new ExecutedEventArgs(entity));
            this.btnOK.Enabled = true;
        }

        private void cboBikkuri_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.chkStartExtra.Enabled = false;
            this.chkEndExtra.Enabled = false;
            this.lblB.Enabled = false;
            this.lblBikkuriValue.Enabled = false;
            this.tbBikkuri.Enabled = false;
            if (this.cboBikkuri.SelectedItem.ToString() != string.Empty)
            {
                this.chkStartExtra.Enabled = true;
                this.chkEndExtra.Enabled = true;
                this.lblB.Enabled = true;
                this.lblBikkuriValue.Enabled = true;
                this.tbBikkuri.Enabled = true;
            }
            this.chkStartExtra_CheckedChanged(this, new EventArgs());
        }

        private void chkMayu_CheckedChanged(object sender, EventArgs e)
        {
            this.OpenAndClose(gbMayu, this.chkMayu.Checked, 160);
            this.trackBar1_Scroll(this, new EventArgs());
        }

        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            this.lblBikkuriValue.Text = ((float)this.tbBikkuri.Value / 20).ToString();
            this.lblDownValue.Text = ((float)this.tbMayuDown.Value / 20).ToString();
            this.lblUpValue.Text = ((float)this.tbMayuUp.Value / 20).ToString();
        }

        private void chkStartExtra_CheckedChanged(object sender, EventArgs e)
        {
            this.numExtraStart.Enabled = false;
            this.numExtraEnd.Enabled = false;

            if (chkStartExtra.Checked)
            {
                this.numExtraStart.Enabled = true;
            }

            if (chkEndExtra.Checked)
            {
                this.numExtraEnd.Enabled = true;
            }
        }

        private void chkEyeSync_CheckedChanged(object sender, EventArgs e)
        {
            this.OpenAndClose(this.gbEyeSync, this.chkEyeSync.Checked, 96);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var eyesyncvalue = this.lblEyeSync.Tag as float[];
            if (eyesyncvalue == null)
                return;
            if (eyesyncvalue.Length < 2)
                eyesyncvalue = new float[] { 1f, 10f };
            using (var frm = new frmEye())
            {
                frm.Initialize(eyesyncvalue);
                frm.ShowDialog(this);
                if (frm.DialogResult == DialogResult.OK)
                {
                    this.lblEyeSync.Tag = frm.Result;
                    this.lblEyeSync.Text = $"{frm.Result[0]}度, {frm.Result[1]}度";
                }
            }
        }

        private void chuYuruyaka_CheckedChanged(object sender, EventArgs e)
        {
            this.OpenAndClose(this.gbYuruyaka, this.chkYuruyaka.Checked, 48);
        }

        private void btnCopy_Click(object sender, EventArgs e)
        {
            var entity = this.CreateEntityInstance();
            if (entity != null)
                _clipboard = entity.ModelInfo;
        }

        private void btnPaste_Click(object sender, EventArgs e)
        {
            if (_clipboard == null)
                return;

            if (MessageBox.Show(this, $"「{_clipboard.ModelName.TrimSafe()}」 の青字設定を、\r\nこのモデルに適用します。\r\nよろしいですか？"
                        , "確認", MessageBoxButtons.YesNo
                        , MessageBoxIcon.Question
                        , MessageBoxDefaultButton.Button2) == DialogResult.No)
                return;

            this.ApplyModelInfo(_clipboard);
        }

        private void lstFav_Format(object sender, ListControlConvertEventArgs e)
        {
            var entity = e.Value as FavEntity;
            if (entity != null)
            {
                e.Value = entity.FavName;
            }
        }

        private void lstFav_SelectedIndexChanged(object sender, EventArgs e)
        {
            var entity = this.lstFav.SelectedItem as FavEntity;
            if (entity == null)
                return;

            this.ApplyBasicInfo(entity);
        }

        private void btnOpenFav_Clicked(object sender, EventArgs e)
        {
            _savedState.IsFavOpen = !_savedState.IsFavOpen;
            this.ApplyFavColumn();
        }

        private void ApplyFavColumn(bool stayupdated = false)
        {
            this.BeginUpdate();
            try
            {
                var width = 0;
                this.btnOpenFav.Text = ">>";
                if (_savedState.IsFavOpen)
                {
                    width = 120;
                    this.btnOpenFav.Text = "<<";
                }

                this.tlp.ColumnStyles[1] = new ColumnStyle(SizeType.Absolute, width);
                this.Width = 450 - (120 - width);
            }
            finally
            {
                if (this._isInitialized)
                    this.EndUpdate();
            }
        }

        private void 削除ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var entity = this.lstFav.SelectedItem as FavEntity;
            if (entity == null)
                return;

            if (MessageBox.Show(this, $"「{entity.FavName}」 を削除します。\r\nよろしいですか？"
                        , "確認", MessageBoxButtons.YesNo
                        , MessageBoxIcon.Question
                        , MessageBoxDefaultButton.Button2) == DialogResult.No)
                return;

            _savedState.Favorites.Remove(entity);
            this.ApplyFavourite();
        }

        private void btnAddFav_Click(object sender, EventArgs e)
        {
            using (var frm = new frmFavName(this._savedState))
            {
                frm.ShowDialog(this);
                if (frm.DialogResult == DialogResult.OK)
                {
                    var currentState = this.CreateEntityInstance();
                    if (currentState == null)
                        return;
                    var modelInfo = currentState.ModelInfo;

                    var ret = frm.Result;
                    var currentItem = _savedState.Favorites.Where(n => n != null && n.FavName == ret).FirstOrDefault();
                    if (currentItem != null)
                    {
                        //同名のが存在してる。上書きだ。
                        this.EditFav(currentItem.FavName, true);
                    }
                    else
                    {
                        //同名のはおらん。追加だ。
                        var fav = new FavEntity();
                        fav.FavName = ret;
                        currentState.CloneTo(fav);
                        fav.Exceptions = "";//このエンティティに対象外モーフ情報はナシ
                        this._savedState.Favorites.Add(fav);
                        this.ApplyFavourite();
                    }
                }
            }
        }

        private void btnEditFav_Click(object sender, EventArgs e)
        {
            var entity = this.lstFav.SelectedItem as FavEntity;
            if (entity == null)
                return;

            this.EditFav(entity.FavName);
        }

        private void EditFav(string favName, bool noMsg = false)
        {
            FavEntity entity = _savedState.Favorites.Where(n => n.FavName == favName).FirstOrDefault();
            if (entity == null)
                return;

            var currentState = this.CreateEntityInstance();
            if (currentState == null)
                return;
            if (!noMsg)
            {
                if (MessageBox.Show(this, $"お気に入り「{entity.FavName}」 の内容を現在の状態に更新します。\r\nよろしいですか？"
                            , "確認", MessageBoxButtons.YesNo
                            , MessageBoxIcon.Question
                            , MessageBoxDefaultButton.Button2) == DialogResult.No)
                    return;
            }

            currentState.CloneTo(entity);
            entity.Exceptions = "";//このエンティティに対象外モーフ情報はナシ
            MyUtility.Serializer.Serialize<SavedState>(_savedState, _path);
        }

        public void SaveState()
        {
            this.SaveCurrentState();
        }

        private Size _prevSize = new Size();

        private void button3_Click(object sender, EventArgs e)
        {
            this.BeginUpdate();
            try
            {
                if (this.btnMinimize.Text == "最小化")
                {
                    _isMinimized = true;
                    this.btnMinimize.Text = "フル";
                    this.btnMinimize.Parent = this.pnlBottom;
                    this.btnMinimize.Top = 32;
                    this.pnlCopyPaste.Visible = false;
                    this.lblModel.Visible = false;
                    this.TopMost = true;
                    this.chkTopMost.Visible = false;
                    _prevSize = this.Size;
                    this.Size = new Size(199, 81);
                    this.btnMinimize.Left = 7;

                    if (frmMainBase.OperationgMode == OperatingMode.OnMMD)
                        this.mmdSelectorControl1.Visible = false;
                }
                else
                {
                    _isMinimized = false;
                    this.btnMinimize.Text = "最小化";
                    this.btnMinimize.Parent = this.gbPreset;
                    this.btnMinimize.Top = 13;
                    this.pnlCopyPaste.Visible = true;
                    this.lblModel.Visible = true;
                    this.Size = _prevSize;
                    this.btnMinimize.Left = 198;

                    this.TopMost = this.chkTopMost.Checked;
                    this.chkTopMost.Visible = true;
                    if (frmMainBase.OperationgMode == OperatingMode.OnMMD)
                        this.mmdSelectorControl1.Visible = true;
                }
            }
            finally
            {
                this.EndUpdate();
            }
        }

        private void btnUp_Click(object sender, EventArgs e)
        {
            var selentity = this.lstFav.SelectedItem as FavEntity;
            if (selentity == null)
                return;
            FavEntity entity = _savedState.Favorites.Where(n => n.FavName == selentity.FavName).FirstOrDefault();

            var index = _savedState.Favorites.IndexOf(entity);
            if (sender == btnUp)
                index -= 1;
            else if (sender == btnDown)
                index += 1;

            if (index < 0 || index >= _savedState.Favorites.Count)
                return;

            _savedState.Favorites.Remove(entity);
            if (index < _savedState.Favorites.Count)
                _savedState.Favorites.Insert(index, entity);
            else
                _savedState.Favorites.Add(entity);

            this.BeginUpdate();
            try
            {
                this.ApplyFavourite(noselection: true);
                this.lstFav.SelectedItem = entity;
            }
            catch (Exception)
            {
            }
            finally
            {
                this.EndUpdate();
            }
        }

        private void btnException_Click(object sender, EventArgs e)
        {
            using (var frm = new frmException(_savedState.Exceptions))
            {
                frm.ShowDialog(this);
                if (frm.DialogResult == DialogResult.OK)
                {
                    if (!string.IsNullOrWhiteSpace(frm.Result))
                    {
                        _savedState.Exceptions = frm.Result;
                        this.SaveCurrentState();
                    }
                }
            }
        }

        private void chkTopMost_CheckedChanged(object sender, EventArgs e)
        {
            this.TopMost = this.chkTopMost.Checked;
            if (this._isInitialized)
                this.SaveCurrentState();
        }

        protected virtual void cboEyeBone_SelectedIndexChanged(object sender, EventArgs e)
        {
        }

        private void lstFav_DoubleClick(object sender, EventArgs e)
        {
            this.btnOK.PerformClick();
        }
    }

    public class ExecutedEventArgs : EventArgs
    {
        public Args Args { get; }

        public ExecutedEventArgs(Args args)
        {
            this.Args = args;
        }
    }
}