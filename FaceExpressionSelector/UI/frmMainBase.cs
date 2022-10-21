using FaceExpressionHelper.UI;
using FaceExpressionHelper.UI.UserControls;
using MyUtility;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Net.Mime.MediaTypeNames;

namespace FaceExpressionHelper
{
    public partial class frmMainBase : Form
    {
        public static OperatingMode OperationgMode { get; protected set; }
        protected Args _args = null;
        protected ExpressionSet _currentExpressionSet { get; set; } = null;
        protected frmPicture _frmPic = null;
        protected frmScrShot _frmshot = null;

        /// <summary>
        /// アクティブモデルが変更された時に走るイベントハンドら
        /// </summary>
        public EventHandler<ActiveModelChangedEventArgs> ActiveModelChangedEventHandler { get; private set; }

        public EventHandler<MorphSelectedEventArgs> MorphSelectedEventHandler;

        /// <summary>
        /// constructor
        /// </summary>
        public frmMainBase()
        {
            InitializeComponent();

            this._args = this.TryLoadSettings();
            if (this._args.ExpressionSets.Count == 0)
                this._args.ExpressionSets.Add(new ExpressionSet() { Name = "Default" });

            this.cboSet.Items.Clear();
            this.cboSet.Items.AddRange(this._args.ExpressionSets.ToArray());
            var selectedExSet = this._args.ExpressionSets.Where(n => n.Name == this._args.SelectedExpressionSet).FirstOrDefault();
            if (selectedExSet != null)
                this.cboSet.SelectedItem = selectedExSet;
            else
                this.cboSet.SelectedIndex = 0;

            this.CreateListBox();

            //this.lstMissingMorphs.Format += this._listboxformatHandler;

            this.chkTopMost.Checked = this._args.TopMost;
            this.ActiveModelChangedEventHandler += this.OnActiveModelChanged;

            this.MorphSelectedEventHandler += this.OnMorphSelected;
            this.Disposed += ((s, e) =>
            {
                this.ActiveModelChangedEventHandler -= this.OnActiveModelChanged;
                this.MorphSelectedEventHandler -= this.OnMorphSelected;
            });

            this._frmPic = new frmPicture(this.MorphSelectedEventHandler);
            this._frmPic.Hide();

            this.lblActiveModel.Text = String.Empty;
            this.lblReplaced.Text = String.Empty;

            this.trackBar1.Value = 4;
            this.trackBar1_Scroll(null, null);
        }

        /// <summary>
        /// 処理中フラグ
        /// </summary>
        public bool IsBusy { get; protected set; }

        #region "protected"

        /// <summary>
        /// 置換などを考慮した一覧を返します。
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        protected List<DspMorphItem> GetApplyingMorphs(ExpressionItem item)
        {
            var allMorphs = this.GetAllMorphsForActiveModel();
            if (allMorphs == null || item == null)
                return new List<DspMorphItem>();

            //置換などを考慮した一覧を作成する
            return item.GetApplyingMorphs(this.ActiveModelName, this._currentExpressionSet.ReplacedMorphs, allMorphs);
        }

        /// <summary>
        /// 現在のアクティブモデルに欠けているモーフ一覧を取得します。
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        protected List<DspMorphItem> GetMissingMorphs(ExpressionItem item)
        {
            //置換を考慮した一覧を作成する
            var applyingMorphs = this.GetApplyingMorphs(item);

            return applyingMorphs.Where(n => n.IsMissing).ToList();
        }

        #endregion "protected"

        #region "protected virtual"

        protected virtual string ActiveModelName => "";

        /// <summary>
        /// 現在のフレームを取得します。
        /// </summary>
        /// <returns></returns>
        protected virtual long GetCurrentFrame()
        {
            return 0;
        }

        /// <summary>
        /// モーフ一覧情報を返します。
        /// </summary>
        /// <returns></returns>
        protected virtual bool ApplyExpression(int bufferFrames, ExpressionItem item)
        {
            return false;
        }

        /// <summary>
        /// 今のアクティブモデルの有効なモーフをすべて返します。
        /// </summary>
        /// <returns></returns>
        protected virtual List<MorphItem> GetAllMorphsForActiveModel()
        {
            return null;
        }

        /// <summary>
        /// アクティブモデルの今のフレームのモーフ適用状態を返します。
        /// </summary>
        /// <returns>null:アクティブモデルなし</returns>
        protected virtual List<MorphItem> GetCurrentMorphState()
        {
            return null;
        }

        /// <summary>
        /// 選択されたモーフを探してMMD上で選択します。(MMDでのみ使用)
        /// </summary>
        /// <param name="sender">アクティブモデル</param>
        /// <param name="e"></param>
        /// <returns></returns>
        protected virtual void OnMorphSelected(object sender, MorphSelectedEventArgs e)
        {
            return;
        }

        /// <summary>
        /// アクティブモデルが変わった時に発火するイベントです。
        /// </summary>
        protected virtual void OnActiveModelChanged(object sender, ActiveModelChangedEventArgs e)
        {
            var enabled = true;
            this.lblActiveModel.Text = String.Empty;
            var replacedTxt = "";
            if (!string.IsNullOrWhiteSpace(e.CurrentActiveModelName))
            {
                this.lblActiveModel.Text = e.CurrentActiveModelName;
                replacedTxt = "置換設定なし";
                var replaced = this._currentExpressionSet.ReplacedMorphs.Where(n => n.ModelName == e.CurrentActiveModelName).FirstOrDefault();
                if (replaced != null && replaced.ReplacedMorphSetList.Count > 0)
                    replacedTxt = $"置換設定 {replaced.ReplacedMorphSetList.Count}件";
            }
            else
            {
                this.lblActiveModel.Text = "モデルを選択してください";
                enabled = false;
            }
            //this.listBox1.Enabled = enabled;
            this.pnlTop.Enabled = enabled;
            this.pnlBottom.Enabled = enabled;

            this.lblReplaced.Text = replacedTxt;
            this.listBox1_SelectedIndexChanged(null, null);
        }

        #endregion "protected virtual"

        /// <summary>
        /// 表情情報の追加・編集
        /// </summary>
        /// <param name="currentItem">編集対象の場合、その編集対象</param>
        /// <param name="mode">0:新規追加 1:名前を変える 2:スクショを取り直す 3:現在の状態に変更する</param>
        /// <returns></returns>
        private bool AddOrEditItem(ExpressionItem currentItem, int mode)
        {
            var expName = String.Empty;
            var prevName = string.Empty;
            if (currentItem != null)
                prevName = currentItem.Name;

            List<MorphItem> currentMorphItems = null;
            switch (mode)
            {
                case 0:
                case 3:
                    //新規作成、現在の状態に更新
                    currentMorphItems = this.GetCurrentMorphState();
                    break;

                default:
                    //名前変更、スクショ取り直し
                    //編集
                    currentMorphItems = currentItem.MorphItems;
                    break;
            }

            if (currentMorphItems == null)
                return false;

            //スクショ取る？
            var makeScreenShot = false;
            //名前入力する？
            var enterName = false;

            switch (mode)
            {
                case 0:
                case 2:
                case 3:
                    //新規追加、スクショを取り直す、現在の状態に変更する
                    makeScreenShot = true;
                    break;

                default:
                    break;
            }
            switch (mode)
            {
                case 0:
                case 1:
                    //新規追加、名前を変える
                    enterName = true;
                    break;

                case 2:
                case 3:
                    expName = currentItem.Name;
                    break;

                default:
                    break;
            }

            FormClosedEventHandler closedHandler = null;
            closedHandler = new FormClosedEventHandler(async (s, e) =>
            {
                this._frmshot.FormClosed -= closedHandler;
                await Task.Delay(100);
                var dir = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
                if (this._frmshot.DialogResult == DialogResult.OK)
                {
                    var pos = this._frmshot.FrmPosition;
                    var shotdir = System.IO.Path.Combine(dir, "faceExpressions");
                    shotdir = System.IO.Path.Combine(shotdir, this._currentExpressionSet.Name);
                    if (!System.IO.Directory.Exists(shotdir))
                        System.IO.Directory.CreateDirectory(shotdir);

                    var retPicturePath = string.Empty;
                    if (currentItem != null)
                        //サムネイル画像を削除する
                        currentItem.TryDeleteThumbnail();

                    var ret = false;
                    try
                    {
                        ret = ScreenShotSaver.SaveScr(pos, shotdir, expName.TrimSafe(), this._args.LetterArgs, ref retPicturePath);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("スクリーンショットの取得に失敗しました");
                    }
                    if (ret)
                    {
                        this._args.PreviousRectangle = pos;
                        this._args.LetterArgs = this._frmshot.LetterArgs;

                        if (currentItem == null)
                        {
                            //新規追加
                            //モーフ一覧を取得します。
                            var item = new ExpressionItem()
                            {
                                Name = expName.TrimSafe(),
                                MorphItems = currentMorphItems,
                                Folder = this._currentExpressionSet.Name,
                            };
                            this._currentExpressionSet.Items.Add(item);
                        }
                        else
                        {
                            //名前変更
                            currentItem.Name = expName.TrimSafe();
                            currentItem.MorphItems = currentMorphItems;
                            currentItem.Folder = this._currentExpressionSet.Name;
                        }

                        this.CreateListBox();
                        this.TrySaveSettings();
                    }
                }

                this.Show();
            });

            if (enterName)
            {
                using (frmName frmname = new frmName(this._currentExpressionSet, currentItem, currentMorphItems, prevName))
                {
                    frmname.ShowDialog(this);
                    if (frmname.DialogResult != DialogResult.OK)
                        return false;

                    expName = frmname.Result.TrimSafe();
                }
            }

            if (makeScreenShot)
            {
                this._frmshot = new frmScrShot(this, this._args.LetterArgs, this._args.PreviousRectangle);
                this._frmshot.Show();
                this._frmshot.FormClosed += closedHandler;
                this.Hide();
            }
            else if (currentItem != null)
            {
                currentItem.ChangeName(expName);
                this.CreateListBox();
                this.TrySaveSettings();
            }
            return true;
        }

        /// <summary>
        /// 設定をロードします。
        /// </summary>
        /// <returns></returns>
        private Args TryLoadSettings()
        {
            var dirpath = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
            var baseDir = new DirectoryInfo(System.IO.Path.Combine(dirpath, "faceExpressions"));
            var xmlpath = System.IO.Path.Combine(baseDir.FullName, "_基本設定.xml");
            var ret = MyUtility.Serializer.Deserialize<Args>(xmlpath);

            if (ret == null)
            {
                ret = new Args()
                {
                    LetterArgs = LetterArgs.CreateInitialInstance(),
                    ExpressionSets = new List<ExpressionSet>() { new ExpressionSet() { Name = "Default" } }
                };
            }
            else
            {
                foreach (var dir in baseDir.GetDirectories())
                {
                    xmlpath = System.IO.Path.Combine(dir.FullName, "_表情セット.xml");
                    if (System.IO.File.Exists(xmlpath))
                    {
                        var expressionSet = MyUtility.Serializer.Deserialize<ExpressionSet>(xmlpath);
                        if (expressionSet != null)
                        {
                            expressionSet.Name = dir.Name;
                            expressionSet.Items.ForEach(n => n.Folder = dir.Name);

                            //モーフ置換設定
                            xmlpath = System.IO.Path.Combine(dir.FullName, "_モーフ置換設定.xml");
                            if (System.IO.File.Exists(xmlpath))
                            {
                                var replacedMorphs = MyUtility.Serializer.Deserialize<List<ReplacedMorphNameItem>>(xmlpath);
                                if (replacedMorphs == null) replacedMorphs = new List<ReplacedMorphNameItem>();
                                expressionSet.ReplacedMorphs = replacedMorphs;
                            }
                            ret.ExpressionSets.Add(expressionSet);
                        }
                    }
                }

                //対象・対象外モーフ
                xmlpath = System.IO.Path.Combine(baseDir.FullName, "_処理対象外の「目・まゆ・リップ」モーフ.xml");
                var exceptionMainMorphs = MyUtility.Serializer.Deserialize<List<string>>(xmlpath);
                if (exceptionMainMorphs == null) exceptionMainMorphs = new List<string>();

                xmlpath = System.IO.Path.Combine(baseDir.FullName, "_処理対象の「その他」モーフ.xml");
                var targetOtherMorphs = MyUtility.Serializer.Deserialize<List<string>>(xmlpath);
                if (targetOtherMorphs == null) targetOtherMorphs = new List<string>();

                ret.ExceptionMainMorphs = exceptionMainMorphs;
                ret.TargetOtherMorphs = targetOtherMorphs;
            }

            return ret;
        }

        /// <summary>
        /// 設定をセーブします。
        /// </summary>
        /// <returns></returns>
        private bool TrySaveSettings()
        {
            var dir = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
            var baseDir = System.IO.Path.Combine(dir, "faceExpressions");
            var xmlpath = System.IO.Path.Combine(baseDir, "_基本設定.xml");

            this._args.SelectedExpressionSet = this.cboSet.Text;
            var ret = MyUtility.Serializer.Serialize(this._args, xmlpath);
            if (ret)
            {
                foreach (var expressionSet in this._args.ExpressionSets)
                {
                    var setdir = System.IO.Path.Combine(baseDir, expressionSet.Name);
                    if (System.IO.Directory.Exists(setdir))
                    {
                        xmlpath = System.IO.Path.Combine(setdir, "_表情セット.xml");
                        ret = MyUtility.Serializer.Serialize(expressionSet, xmlpath);
                        xmlpath = System.IO.Path.Combine(setdir, "_モーフ置換設定.xml");
                        ret = MyUtility.Serializer.Serialize(expressionSet.ReplacedMorphs, xmlpath);
                    }
                }
                xmlpath = System.IO.Path.Combine(baseDir, "_処理対象外の「目・まゆ・リップ」モーフ.xml");
                ret = MyUtility.Serializer.Serialize(this._args.ExceptionMainMorphs, xmlpath);
                xmlpath = System.IO.Path.Combine(baseDir, "_処理対象の「その他」モーフ.xml");
                ret = MyUtility.Serializer.Serialize(this._args.TargetOtherMorphs, xmlpath);
            }
            return ret;
        }

        #region "Events"

        private frmExceptions _frmExceptions = null;
        private frmReplacedMorphs _frmReplacedMorphs = null;

        /// <summary>
        /// 追加ボタン
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAdd_Click(object sender, EventArgs e)
        {
            ExpressionItem currentItem = null;
            int mode = 0;
            if (sender == this.編集ToolStripMenuItem)
            {
                //編集
                currentItem = this.listBox1.SelectedItem as ExpressionItem;
                if (currentItem == null)
                    return;
                mode = 1;
            }
            else if (sender == this.スクショを取り直すToolStripMenuItem)
            {
                //スクショ取り直す
                currentItem = this.listBox1.SelectedItem as ExpressionItem;
                if (currentItem == null)
                    return;
                mode = 2;
            }
            else if (sender == this.現在の状態に更新するToolStripMenuItem)
            {
                //現在の状態に更新する
                currentItem = this.listBox1.SelectedItem as ExpressionItem;
                if (currentItem == null)
                    return;
                var msg = $"「{currentItem.Name}」\r\nの内容を現在の状態に更新します。\r\n\r\nよろしいですか？";
                if (MessageBox.Show(msg, "確認", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.No)
                    return;

                mode = 3;
            }

            if (string.IsNullOrWhiteSpace(this.ActiveModelName))
            {
                //モデルが選択されていない
                MessageBox.Show(this, "モデルを選択してください");
                return;
            }

            AddOrEditItem(currentItem, mode);
        }

        private void btnReplace_Click(object sender, EventArgs e)
        {
            Console.WriteLine("frmreplace");
            if (this._frmReplacedMorphs != null && this._frmReplacedMorphs.Visible)
                return;

            var morphNames = this.GetAllMorphsForActiveModel();
            if (morphNames == null)
                morphNames = new List<MorphItem>();
            morphNames = morphNames.Where(n => !this._args.ExceptionMainMorphs.Contains(n.MorphName)).ToList();

            if (morphNames?.Count == 0)
                return;

            var rmn = this._currentExpressionSet.ReplacedMorphs.Where(n => n.ModelName == this.ActiveModelName).FirstOrDefault();
            if (rmn == null)
                rmn = new ReplacedMorphNameItem();

            this._frmReplacedMorphs = new frmReplacedMorphs(this.lblActiveModel.Text, morphNames, this._args, this._currentExpressionSet, rmn, this.MorphSelectedEventHandler);

            this.ShowDialog(this._frmReplacedMorphs, new Action(() =>
            {
                this._currentExpressionSet.ReplacedMorphs.Remove(rmn);
                var rpList = this._frmReplacedMorphs.Result;
                if (rpList?.Count > 0)
                {
                    var newRmn = new ReplacedMorphNameItem()
                    {
                        ModelName = this.ActiveModelName,
                        ReplacedMorphSetList = this._frmReplacedMorphs.Result,
                    };
                    this._currentExpressionSet.ReplacedMorphs.Add(newRmn);
                }
                this.TrySaveSettings();
                listBox1_SelectedIndexChanged(this, new EventArgs());
            }));

            //this._frmReplacedMorphs.FormClosed += (ss, ee) =>
            //{
            //    if (this._frmReplacedMorphs.DialogResult == DialogResult.OK)
            //    {
            //        this._args.ReplacedMorphs.Remove(rmn);
            //        var rpList = this._frmReplacedMorphs.Result;
            //        if (rpList?.Count > 0)
            //        {
            //            var newRmn = new ReplacedMorphNameItem()
            //            {
            //                ModelName = this.ActiveModelName,
            //                ReplacedMorphSetList = this._frmReplacedMorphs.Result,
            //            };
            //            this._args.ReplacedMorphs.Add(newRmn);
            //        }
            //        this.TrySaveSettings();
            //        listBox1_SelectedIndexChanged(this, new EventArgs());
            //    }
            //    this._frmReplacedMorphs = null;
            //};
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (!this.btnOK.Enabled)
                return;
            var item = this.listBox1.SelectedItem as ExpressionItem;
            if (sender == this.btnReset)
            {
                //表情リセット
                this.listBox1.SelectedIndex = -1;
                this.lstMorphs.Items.Clear();

                item = new ExpressionItem() { Name = "表情リセット" };
            }

            if (item == null)
                return;
            if (string.IsNullOrWhiteSpace(this.ActiveModelName))
            {
                MessageBox.Show(this, "モデルを選択してください");
                return;
            }
            if (this.trackBar1.Value < 0)
            {
                if (this.GetCurrentFrame() < this.trackBar1.Value * -1)
                {
                    MessageBox.Show(this, $"{this.trackBar1.Value * -1}フレーム以降で処理してください");
                    return;
                }
            }

            //**フレーム後に表情をつける
            this.IsBusy = true;
            try
            {
                this.ApplyExpression(this.trackBar1.Value, item);
            }
            finally
            {
                this.IsBusy = false;
            }
        }

        private void btnUp_Click(object sender, EventArgs e)
        {
            var selectedItem = this.listBox1.SelectedItem as ExpressionItem;
            if (selectedItem == null)
                return;

            int addCnt = 1;
            if (sender == this.btnUp)
                addCnt = -1;
            var selectedindex = this.listBox1.SelectedIndex + addCnt;
            if (selectedindex < 0 || selectedindex >= this.listBox1.Items.Count)
                return;

            List<ExpressionItem> itemList = new List<ExpressionItem>();
            itemList.AddRange(this.listBox1.Items.Cast<ExpressionItem>().Where(n => n != selectedItem));

            if (selectedindex < itemList.Count)
                itemList.Insert(selectedindex, selectedItem);
            else
                itemList.Add(selectedItem);

            this._currentExpressionSet.Items = itemList;
            this.TrySaveSettings();
            this.CreateListBox();
            this.listBox1.SelectedItem = selectedItem;
        }

        private void chkTopMost_CheckedChanged(object sender, EventArgs e)
        {
            this.TopMost = this.chkTopMost.Checked;
            if (this.Visible)
            {
                this._args.TopMost = this.chkTopMost.Checked;
                this.TrySaveSettings();
            }
        }

        /// <summary>
        /// ListBoxを作成します。
        /// </summary>
        private void CreateListBox()
        {
            this.listBox1.SuspendLayout();
            this.listBox1.Items.Clear();
            if (this._currentExpressionSet == null || this._currentExpressionSet.Items == null)
                return;

            this.listBox1.Items.AddRange(this._currentExpressionSet.Items.ToArray());

            this.lstMorphs.Items.Clear();
            this.listBox1.ResumeLayout();
        }

        private void frmMainBase_FormClosed(object sender, FormClosedEventArgs e)
        {
            MyUtility.ControlHelper.TrySaveMyPosition("FaceExpressionSelector", this);
        }

        private void frmMainBase_Load(object sender, EventArgs e)
        {
            MyUtility.ControlHelper.TryLoadMyPosition("FaceExpressionSelector", this);
        }

        private void listBox1_Format(object sender, ListControlConvertEventArgs e)
        {
            var item = e.Value as ExpressionItem;
            if (item != null)
                e.Value = item.Name;
        }

        private void listBox1_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                var index = listBox1.IndexFromPoint(e.Location);
                if (index < 0)
                    return;

                listBox1.SelectedItem = listBox1.Items[index];
            }
        }

        private async void listBox1_MouseLeave(object sender, EventArgs e)
        {
            await Task.Delay(100);
            if (this._frmPic != null && !this._frmPic.IsMouseOn)
                this._frmPic.Hide();
        }

        private void listBox1_MouseMove(object sender, MouseEventArgs e)
        {
            Point point = this.listBox1.PointToClient(Cursor.Position);
            int index = this.listBox1.IndexFromPoint(point);
            if (index < 0)
            {
                this._frmPic.Hide();
                return;
            }

            var item = this.listBox1.Items[index] as ExpressionItem;
            if (item == null)
            {
                this._frmPic.Hide();
                return;
            }
            if (this._frmPic.CurrentItem != item)
            {
                int longerEdge = 250;
                Size size = new Size(longerEdge, longerEdge);
                if (item.ThumbNail != null)
                {
                    if (item.ThumbNail.Width > item.ThumbNail.Height)
                        size = new Size(longerEdge, item.ThumbNail.Height * longerEdge / item.ThumbNail.Width + 70);
                    else
                        size = new Size(item.ThumbNail.Width * longerEdge / item.ThumbNail.Height, longerEdge + 70);
                }
                this._frmPic.Size = size;
                this._frmPic.TopMost = this.TopMost;
                var allmorphs = this.GetAllMorphsForActiveModel();
                this._frmPic.Show(point, this, this.ActiveModelName, this._currentExpressionSet, item, allmorphs);
            }
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            var item = this.listBox1.SelectedItem as ExpressionItem;
            if (item == null)
                return;

            this.lstMorphs.Items.Clear();

            //置換を考慮した一覧を作成する
            var lstMorphs = this.GetApplyingMorphs(item);

            if (lstMorphs != null)
            {
                this.lstMorphs.Items.AddRange(lstMorphs.ToArray());
                this.lstMorphs.Refresh();
            }
        }

        private void 削除ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var item = this.listBox1.SelectedItem as ExpressionItem;
            if (item == null)
                return;

            var msg = $"{item.Name}\r\n\r\n以上の表情を削除してよろしいですか？";
            if (MessageBox.Show(msg, "確認", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.No)
                return;

            //サムネイル画像を削除する
            item.TryDeleteThumbnail();
            this._currentExpressionSet.Items.Remove(item);
            this.TrySaveSettings();
            this.CreateListBox();
        }

        private void 閉じるXToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        #endregion "Events"

        private void 対象外の目眉リップモーフToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (this._frmExceptions != null && this._frmExceptions.Visible)
                return;

            var targetMorphs = new List<string>();
            int mode = 0;//0:対象外の目まゆリップモーフ 1:対象のその他モーフ
            if (sender == this.対象外の目まゆリップモーフToolStripMenuItem)
            {
                targetMorphs.AddRange(this._args.ExceptionMainMorphs);
                mode = 0;
            }
            else if (sender == this.対象のその他モーフToolStripMenuItem)
            {
                targetMorphs.AddRange(this._args.TargetOtherMorphs);
                mode = 1;
            }

            var allMorphs = this.GetAllMorphsForActiveModel();
            var morphNames = allMorphs.Where(n =>
            {
                var flg = (n.MorphType == MMDUtil.MMDUtilility.MorphType.Other || n.MorphType == MMDUtil.MMDUtilility.MorphType.none);
                if (mode == 0)
                    flg = !flg;
                return flg;
            }).Select(n => n.MorphName).Distinct().ToList();

            if (morphNames == null || morphNames.Count == 0)
                return;

            this._frmExceptions = new frmExceptions(mode, this.lblActiveModel.Text, morphNames, targetMorphs, this.MorphSelectedEventHandler);
            this.ShowDialog(this._frmExceptions, new Action(() =>
            {
                if (sender == this.対象外の目まゆリップモーフToolStripMenuItem)
                {
                    this._args.ExceptionMainMorphs.Clear();
                    this._args.ExceptionMainMorphs.AddRange(this._frmExceptions.Result);
                }
                else if (sender == this.対象のその他モーフToolStripMenuItem)
                {
                    this._args.TargetOtherMorphs.Clear();
                    this._args.TargetOtherMorphs.AddRange(this._frmExceptions.Result);
                }

                this.TrySaveSettings();
            }));
            //this._frmExceptions.Show(this);
            //this._frmExceptions.FormClosed += (ss, ee) =>
            //{
            //    if (this._frmExceptions.DialogResult == DialogResult.OK)
            //    {
            //        if (sender == this.対象外の目まゆリップモーフToolStripMenuItem)
            //        {
            //            this._args.ExceptionMainMorphs.Clear();
            //            this._args.ExceptionMainMorphs.AddRange(this._frmExceptions.Result);
            //        }
            //        else if (sender == this.対象のその他モーフToolStripMenuItem)
            //        {
            //            this._args.TargetOtherMorphs.Clear();
            //            this._args.TargetOtherMorphs.AddRange(this._frmExceptions.Result);
            //        }

            //        this.TrySaveSettings();
            //    }
            //    this._frmExceptions = null;
            //};
        }

        private void ShowDialog(Form frm, Action afterAction)
        {
            if (frmMainBase.OperationgMode == OperatingMode.OnMMD)
            {
                //MMDなら普通にShowDialogする
                this.Cursor = Cursors.WaitCursor;
                frm.ShowDialog(this);
                this.Cursor = Cursors.Default;
                if (frm.DialogResult == DialogResult.OK)
                {
                    afterAction?.Invoke();
                }
            }
            else
            {
                //MMMはShowDialogするとMMM本体がいじれなくなってしまうため一工夫する
                this.Cursor = Cursors.WaitCursor;
                frm.Show(this);
                this.Cursor = Cursors.Default;
                frm.FormClosed += (ss, ee) =>
                {
                    if (frm.DialogResult == DialogResult.OK)
                    {
                        afterAction?.Invoke();
                    }
                };
            }
        }

        private void cboSet_SelectedIndexChanged(object sender, EventArgs e)
        {
            this._currentExpressionSet = this.cboSet.SelectedItem as ExpressionSet;
            if (this._currentExpressionSet != null)
            {
                this.CreateListBox();
            }
        }

        private void btnAddSet_Click(object sender, EventArgs e)
        {
            ExpressionSet exSet = null;
            if (sender == this.btnEditSet)
                exSet = this._currentExpressionSet;

            using (var frm = new frmEditSet(this._args, exSet))
            {
                if (frm.ShowDialog(this) == DialogResult.OK)
                {
                    var dir = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
                    var baseDir = System.IO.Path.Combine(dir, "faceExpressions");
                    var setDir = System.IO.Path.Combine(baseDir, this._currentExpressionSet.Name);
                    if (!string.IsNullOrEmpty(frm.Result))
                    {
                        var newDir = System.IO.Path.Combine(baseDir, frm.Result);
                        if (System.IO.Directory.Exists(newDir))
                        {
                            //んなことない
                            MessageBox.Show("セットに追加に失敗しました");
                            return;
                        }

                        if (exSet == null)
                        {
                            exSet = new ExpressionSet() { Name = frm.Result };
                            this._args.ExpressionSets.Add(exSet);
                            try
                            {
                                System.IO.Directory.CreateDirectory(newDir);
                            }
                            catch (Exception)
                            {
                            }
                        }
                        else
                        {
                            //編集
                            this._currentExpressionSet.Name = frm.Result;
                            try
                            {
                                System.IO.Directory.Move(setDir, newDir);
                            }
                            catch (Exception)
                            {
                            }
                        }

                        this.cboSet.Items.Clear();
                        this.cboSet.Items.AddRange(this._args.ExpressionSets.ToArray());
                        this.cboSet.SelectedItem = exSet;
                    }
                    else if (frm.DeleteFlg)
                    {
                        //セット削除

                        try
                        {
                            System.IO.Directory.Delete(setDir, true);
                        }
                        catch (Exception)
                        {
                        }
                        this._args.ExpressionSets.Remove(this._currentExpressionSet);
                        this.cboSet.Items.Clear();
                        this.cboSet.Items.AddRange(this._args.ExpressionSets.ToArray());
                        this.cboSet.SelectedIndex = 0;
                    }
                    this.TrySaveSettings();
                }
            }
        }

        private void lstMorphs_SelectedValueChanged(object sender, EventArgs e)
        {
            var morphItem = this.lstMorphs.SelectedItem as DspMorphItem;
            if (morphItem == null)
                return;

            this.MorphSelectedEventHandler?.Invoke(sender, new MorphSelectedEventArgs(this.ActiveModelName, morphItem.DspMorphName));
        }

        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            var btnokEnabled = true;
            var value = Math.Abs(this.trackBar1.Value);
            if (this.trackBar1.Value > 0)
                this.lblFrame.Text = $"{value}fr後に";
            else if (this.trackBar1.Value < 0)
                this.lblFrame.Text = $"{value}fr前から";
            else
            {
                btnokEnabled = false;
                this.lblFrame.Text = "-";
            }
            this.btnOK.Enabled = btnokEnabled;
        }

        private void cboSet_Format(object sender, ListControlConvertEventArgs e)
        {
            var exset = e.Value as ExpressionSet;
            if (exset != null)
                e.Value = exset.Name;
        }
    }

    /// <summary>
    /// アクティブモデル変更時イベントの引数クラスです。
    /// </summary>
    public class ActiveModelChangedEventArgs : EventArgs
    {
        public ActiveModelChangedEventArgs(string currentActiveModelName)
        {
            this.CurrentActiveModelName = currentActiveModelName;
        }

        /// <summary>
        /// 現在のアクティブなモデル名
        /// </summary>
        public string CurrentActiveModelName { get; }
    }

    public class MorphSelectedEventArgs : EventArgs
    {
        /// <summary>
        /// アクティブモデル名
        /// </summary>
        public string ActiveModelName { get; }

        /// <summary>
        /// 選択されたモーフ名
        /// </summary>
        public string MorphName { get; }

        /// <summary>
        /// 値
        /// </summary>
        public float Value { get; }

        /// <summary>
        /// 処理前にフレームをリセットするならtrue
        /// </summary>
        public bool ResetFrame { get; }

        public MorphSelectedEventArgs(string activeModelName, string morphName, float value = float.MinValue, bool resetFrame = false)
        {
            this.ActiveModelName = activeModelName;
            this.MorphName = morphName;
            this.Value = value;
            this.ResetFrame = resetFrame;
        }
    }

    /// <summary>
    /// MMD、MMMどっちで動いてる？
    /// </summary>
    public enum OperatingMode
    {
        /// <summary>
        /// MMMで動いている
        /// </summary>
        OnMMM,

        /// <summary>
        /// MMDで動いている
        /// </summary>
        OnMMD
    }
}