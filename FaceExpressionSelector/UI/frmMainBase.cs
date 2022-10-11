using FaceExpressionHelper.UI;
using MyUtility;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FaceExpressionHelper
{
    public partial class frmMainBase : Form
    {
        protected Args _args = null;
        protected frmPicture _frmPic = null;
        protected frmScrShot _frmshot = null;

        private ListControlConvertEventHandler _listboxformatHandler = new ListControlConvertEventHandler((s, e) =>
        {
            var morph = e.Value as MorphItem;
            if (morph != null)
                e.Value = $"{morph.MorphName}：  {Math.Round(morph.Weight, 3)}";
        });

        /// <summary>
        /// constructor
        /// </summary>
        public frmMainBase()
        {
            InitializeComponent();

            this._args = this.TryLoadSettings();

            this._frmPic = new frmPicture(this._listboxformatHandler);
            this._frmPic.Hide();

            this.CreateListBox();

            this.lstValue.Format += this._listboxformatHandler;
            this.lstValue.Disposed += (s, e) => this.lstValue.Format -= this._listboxformatHandler;

            this.chkTopMost.Checked = this._args.TopMost;
        }

        #region "protected virtual"

        /// <summary>
        /// モーフ一覧情報を返します。
        /// </summary>
        /// <returns></returns>
        protected virtual bool ApplyExpression(int bufferFrames, ExpressionItem item)
        {
            return false;
        }

        /// <summary>
        /// モーフ一覧情報を返します。
        /// </summary>
        /// <returns></returns>
        protected virtual List<MorphItem> CreateCurrentMorphList(bool showErrorMsgbox)
        {
            return null;
        }

        #endregion "protected virtual"

        /// <summary>
        /// 現在のモーフ情報を表示します。
        /// </summary>
        public virtual void ApplyCurrentMorph()
        {
        }

        /// <summary>
        /// 表情情報の追加・編集
        /// </summary>
        /// <param name="currentItem">編集対象の場合、その編集対象</param>
        /// <param name="makeScreenShot">スクショを取るか</param>
        /// <returns></returns>
        private bool AddOrEditItem(ExpressionItem currentItem, bool makeScreenShot)
        {
            var expName = String.Empty;
            var prevName = string.Empty;
            if (currentItem != null)
                prevName = currentItem.Name;

            var currentMorphs = this.CreateCurrentMorphList(true);
            if (currentItem != null)
                //編集
                currentMorphs = currentItem.MorphItems;
            using (frmName frmname = new frmName(this._listboxformatHandler, this._args, currentItem, currentMorphs, prevName))
            {
                frmname.ShowDialog(this);
                if (frmname.DialogResult != DialogResult.OK)
                    return false;

                expName = frmname.Result.TrimSafe();
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
                    catch (Exception)
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
                            var morphitems = this.CreateCurrentMorphList(false);
                            var item = new ExpressionItem()
                            {
                                Name = expName.TrimSafe(),
                                MorphItems = morphitems,
                            };
                            this._args.Items.Add(item);
                        }
                        else
                        {
                            //名前変更
                            currentItem.Name = expName.TrimSafe();
                        }

                        this.CreateListBox();
                        this.TrySaveSettings();
                    }

                    this.Show();
                }
            });

            if (makeScreenShot)
            {
                this._frmshot = new frmScrShot(this, this._args.LetterArgs, this._args.PreviousRectangle);
                this._frmshot.Show();
                this._frmshot.FormClosed += closedHandler;
                this.Hide();
            }
            else if (currentItem != null)
            {
                currentItem.Name = expName;
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
            var xmlpath = System.IO.Path.Combine(dir, "FaceExpressionHelperSetting.xml");
            var ret = MyUtility.Serializer.Deserialize<Args>(xmlpath);
            if (ret == null)
            {
                ret = new Args()
                {
                    LetterArgs = LetterArgs.CreateInitialInstance(),
                };
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
            var xmlpath = System.IO.Path.Combine(dir, "FaceExpressionHelperSetting.xml");

            return MyUtility.Serializer.Serialize(this._args, xmlpath);
        }

        #region "Events"

        /// <summary>
        /// 追加ボタン
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAdd_Click(object sender, EventArgs e)
        {
            ExpressionItem currentItem = null;
            var makeScreenShot = false;
            if (sender == this.編集ToolStripMenuItem)
            {
                //編集
                currentItem = this.listBox1.SelectedItem as ExpressionItem;
                if (currentItem == null)
                    return;

                if (MessageBox.Show("スクリーンショットを取り直しますか？", "確認", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
                    makeScreenShot = true;
            }
            else
            {
                //新規追加
                makeScreenShot = true;
            }

            if (this.CreateCurrentMorphList(true) == null)
                return;

            AddOrEditItem(currentItem, makeScreenShot);
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            var item = this.listBox1.SelectedItem as ExpressionItem;
            if (item == null)
                return;

            //**フレーム後に表情をつける
            this.ApplyExpression((int)numericUpDown1.Value, item);
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

        /// <summary>
        /// ListBoxを作成します。
        /// </summary>
        private void CreateListBox()
        {
            this.listBox1.Items.Clear();
            if (this._args == null || this._args.Items == null)
                return;

            this.listBox1.Items.AddRange(this._args.Items.ToArray());

            this.lstValue.Items.Clear();
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

        private void listBox1_MouseLeave(object sender, EventArgs e)
        {
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

                this._frmPic.Show(point, this, item);
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

        private void chkTopMost_CheckedChanged(object sender, EventArgs e)
        {
            this.TopMost = this._args.TopMost;
            if (this.Visible)
            {
                this._args.TopMost = this.chkTopMost.Checked;                
                this.TrySaveSettings();
            }
        }

        #endregion "Events"
    }
}