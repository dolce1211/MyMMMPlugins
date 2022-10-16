using FaceExpressionHelper.UI;
using MyUtility;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Net.Mime.MediaTypeNames;

namespace FaceExpressionHelper
{
    public partial class frmMainBase : Form
    {
        protected Args _args = null;
        protected frmPicture _frmPic = null;
        protected frmScrShot _frmshot = null;

        public EventHandler<ActiveModelChangedEventArgs> ActiveModelChangedEventHandler;

        private ListControlConvertEventHandler _listboxformatHandler = new ListControlConvertEventHandler((s, e) =>
                {
                    if (e.Value is DspMorphItem dspmorph)
                    {
                        if (dspmorph.Ignore)
                            e.Value = $"【無視する】 {dspmorph.MorphName}：  {Math.Round(dspmorph.Weight, 3)}";
                        else if (dspmorph.ReplacedItem != null)
                            e.Value = $"【置換】{dspmorph.MorphName} → {dspmorph.ReplacedItem.RepalcedMorphName}  {Math.Round(dspmorph.Weight, 3)}";
                        else
                            e.Value = $"　　　　 {dspmorph.MorphName}：  {Math.Round(dspmorph.Weight, 3)}";
                    }
                    if (e.Value is MorphItem morph)
                    {
                        e.Value = $"{morph.MorphName}：  {Math.Round(morph.Weight, 3)}";
                    }
                });

        private void listBox_DrawItem(object sender, DrawItemEventArgs e)
        {
            var lstbox = sender as ListBox;
            if (lstbox == null)
                return;
            var dspMorphItem = lstbox.Items[e.Index] as DspMorphItem;
            if (dspMorphItem == null)
                return;

            e.DrawBackground();

            //背景を描画する
            //項目が選択されている時は強調表示される

            //ListBoxが空のときにListBoxが選択されるとe.Indexが-1になる
            Brush b = null;
            b = new SolidBrush(e.ForeColor);
            if (e.Index > -1)
            { //空でない場合
                //文字を描画する色の選択

                if (dspMorphItem.MorphType == MMDUtil.MMDUtilility.MorphType.Brow)
                { //違反ありの場合
                    b = new SolidBrush(Color.Green);
                    //赤字にする
                }
            }
            e.Graphics.DrawString(dspMorphItem.DspMorphName, e.Font, b, e.Bounds);

            b.Dispose();

            //e.DrawFocusRectangle();
        }

        /// <summary>
        /// constructor
        /// </summary>
        public frmMainBase()
        {
            InitializeComponent();

            this._args = this.TryLoadSettings();

            this._frmPic = new frmPicture();
            this._frmPic.Hide();

            this.CreateListBox();

            //this.lstMissingMorphs.Format += this._listboxformatHandler;

            this.chkTopMost.Checked = this._args.TopMost;
            ActiveModelChangedEventHandler += this.OnActiveModelChanged;

            this.Disposed += ((s, e) =>
            {
                ActiveModelChangedEventHandler -= this.OnActiveModelChanged;
                //this.lstMissingMorphs.Format -= this._listboxformatHandler;
            });

            this.lblActiveModel.Text = String.Empty;
            this.lblReplaced.Text = String.Empty;
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
            return item.GetApplyingMorphs(this.ActiveModelName, this._args.ReplacedMorphs, allMorphs);
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
                var replaced = this._args.ReplacedMorphs.Where(n => n.ModelName == e.CurrentActiveModelName).FirstOrDefault();
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
                            };
                            this._args.Items.Add(item);
                        }
                        else
                        {
                            //名前変更
                            currentItem.Name = expName.TrimSafe();
                            currentItem.MorphItems = currentMorphItems;
                        }

                        this.CreateListBox();
                        this.TrySaveSettings();
                    }
                }

                this.Show();
            });

            if (enterName)
            {
                using (frmName frmname = new frmName(this._args, currentItem, currentMorphItems, prevName))
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
            var dir = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
            var shotdir = System.IO.Path.Combine(dir, "faceExpressions");
            var xmlpath = System.IO.Path.Combine(shotdir, "FaceExpressionHelperSetting.xml");
            var ret = MyUtility.Serializer.Deserialize<Args>(xmlpath);
            if (ret == null)
            {
                ret = new Args()
                {
                    LetterArgs = LetterArgs.CreateInitialInstance(),
                };
            }
            else
            {
                xmlpath = System.IO.Path.Combine(shotdir, "モーフ置換設定.xml");
                var replacedMorphs = MyUtility.Serializer.Deserialize<List<ReplacedMorphNameItem>>(xmlpath);
                if (replacedMorphs == null) replacedMorphs = new List<ReplacedMorphNameItem>();

                xmlpath = System.IO.Path.Combine(shotdir, "処理対象外の「目・まゆ・リップ」モーフ.xml");
                var exceptionMainMorphs = MyUtility.Serializer.Deserialize<List<string>>(xmlpath);
                if (exceptionMainMorphs == null) exceptionMainMorphs = new List<string>();

                xmlpath = System.IO.Path.Combine(shotdir, "処理対象の「その他」モーフ.xml");
                var targetOtherMorphs = MyUtility.Serializer.Deserialize<List<string>>(xmlpath);
                if (targetOtherMorphs == null) targetOtherMorphs = new List<string>();

                ret.ReplacedMorphs = replacedMorphs;
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
            var shotdir = System.IO.Path.Combine(dir, "faceExpressions");
            var xmlpath = System.IO.Path.Combine(shotdir, "FaceExpressionHelperSetting.xml");

            var ret = MyUtility.Serializer.Serialize(this._args, xmlpath);
            if (ret)
            {
                xmlpath = System.IO.Path.Combine(shotdir, "モーフ置換設定.xml");
                ret = MyUtility.Serializer.Serialize(this._args.ReplacedMorphs, xmlpath);
                xmlpath = System.IO.Path.Combine(shotdir, "処理対象外の「目・まゆ・リップ」モーフ.xml");
                ret = MyUtility.Serializer.Serialize(this._args.ExceptionMainMorphs, xmlpath);
                xmlpath = System.IO.Path.Combine(shotdir, "処理対象の「その他」モーフ.xml");
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
            if (this._frmReplacedMorphs != null)
                return;

            var morphNames = this.GetAllMorphsForActiveModel();
            if (morphNames == null)
                morphNames = new List<MorphItem>();
            morphNames = morphNames.Where(n => !this._args.ExceptionMainMorphs.Contains(n.MorphName)).ToList();

            if (morphNames?.Count == 0)
                return;

            var rmn = this._args.ReplacedMorphs.Where(n => n.ModelName == this.ActiveModelName).FirstOrDefault();
            if (rmn == null)
                rmn = new ReplacedMorphNameItem();

            this._frmReplacedMorphs = new frmReplacedMorphs(this.lblActiveModel.Text, morphNames, this._args, rmn);
            this.Cursor = Cursors.WaitCursor;
            this._frmReplacedMorphs.Show(this);
            this.Cursor = Cursors.Default;

            this._frmReplacedMorphs.FormClosed += (ss, ee) =>
            {
                if (this._frmReplacedMorphs.DialogResult == DialogResult.OK)
                {
                    this._args.ReplacedMorphs.Remove(rmn);
                    var rpList = this._frmReplacedMorphs.Result;
                    if (rpList?.Count > 0)
                    {
                        var newRmn = new ReplacedMorphNameItem()
                        {
                            ModelName = this.ActiveModelName,
                            ReplacedMorphSetList = this._frmReplacedMorphs.Result,
                        };
                        this._args.ReplacedMorphs.Add(newRmn);
                    }
                    this.TrySaveSettings();
                    listBox1_SelectedIndexChanged(this, new EventArgs());
                }
                this._frmReplacedMorphs = null;
            };
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (!this.btnOK.Enabled)
                return;
            var item = this.listBox1.SelectedItem as ExpressionItem;
            if (item == null)
                return;
            if (string.IsNullOrWhiteSpace(this.ActiveModelName))
            {
                MessageBox.Show(this, "モデルを選択してください");
                return;
            }
            //**フレーム後に表情をつける
            this.IsBusy = true;
            try
            {
                this.ApplyExpression((int)numericUpDown1.Value, item);
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

            this._args.Items = itemList;

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
            this.listBox1.Items.Clear();
            if (this._args == null || this._args.Items == null)
                return;

            this.listBox1.Items.AddRange(this._args.Items.ToArray());

            this.lstMissingMorphs.Items.Clear();
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
                this._frmPic.Show(point, this, this.ActiveModelName, this._args, item, allmorphs);
            }
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            var item = this.listBox1.SelectedItem as ExpressionItem;
            if (item == null)
                return;

            this.lblMissingMorphs.Visible = false;
            this.lstMissingMorphs.Items.Clear();

            //置換を考慮した一覧を作成する
            var missingMorphs = this.GetMissingMorphs(item);

            if (missingMorphs != null)
            {
                missingMorphs = missingMorphs.Where(n =>
                {
                    if (n is DspMorphItem dspm)
                    {
                        if (dspm.ReplacedItem != null)
                            return false;
                    }
                    return true;
                }).ToList();
                if (missingMorphs.Count > 0)
                {
                    //足りないモーフあり
                    this.lblMissingMorphs.Visible = true;
                    this.lstMissingMorphs.Items.AddRange(missingMorphs.ToArray());
                    this.lstMissingMorphs.Refresh();
                }
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
            this._args.Items.Remove(item);
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
            if (this._frmExceptions != null)
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

            this._frmExceptions = new frmExceptions(mode, this.lblActiveModel.Text, morphNames, targetMorphs);
            this._frmExceptions.Show(this);
            this._frmExceptions.FormClosed += (ss, ee) =>
            {
                if (this._frmExceptions.DialogResult == DialogResult.OK)
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
                }
                this._frmExceptions = null;
            };
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
        }

        private void label1_Click(object sender, EventArgs e)
        {
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
}