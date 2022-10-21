using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FaceExpressionHelper.UI
{
    public partial class frmPicture : Form
    {
        public frmPicture(EventHandler<MorphSelectedEventArgs> morphselectedHandler)
        {
            InitializeComponent();
            this.TopMost = true;
            this._morphselectedHandler = morphselectedHandler;
        }

        /// <summary>
        /// 現在表示中の表情
        /// </summary>
        public ExpressionItem CurrentItem { get; private set; } = null;

        private EventHandler<MorphSelectedEventArgs> _morphselectedHandler = null;
        private string _activeModel = string.Empty;

        /// <summary>
        /// 表示
        /// </summary>
        /// <param name="pt"></param>
        /// <param name="ownerForm">オーナーフォーム</param>
        /// <param name="activeModel">選択中のモデル</param>
        /// <param name="exSet">表情セット</param>
        /// <param name="item"></param>
        /// <param name="allMorphs">このモデルの全モーフ</param>
        public void Show(Point pt, Form ownerForm, string activeModel, ExpressionSet exSet, ExpressionItem item, List<MorphItem> allMorphs)
        {
            if (!this.Visible)
                this.Show();
            this.Owner = ownerForm;
            this.Location = new Point(ownerForm.Left + ownerForm.Width - 10, ownerForm.Top + pt.Y);
            Image img = null;
            if (item.ThumbNail != null)
                img = item.ThumbNail.Clone() as Image;
            this.pictureBox1.Image = img;
            this.CurrentItem = item;

            this.pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;

            this.lstMorphs.Items.Clear();
            if (item != null && item.MorphItems != null)
            {
                var applyingMorphs = item.GetApplyingMorphs(activeModel, exSet.ReplacedMorphs, allMorphs);
                this.lstMorphs.Items.AddRange(applyingMorphs.ToArray());
            }
            this._activeModel = activeModel;
        }

        private void frmPicture_VisibleChanged(object sender, EventArgs e)
        {
            if (this.pictureBox1.Image != null)
            {
                this.pictureBox1.Image.Dispose();
                this.pictureBox1.Image = null;
            }

            if (this.Owner != null)
                this.Owner.Focus();

            if (!this.Visible)
            {
                this.timer1.Enabled = false;
                this.pictureBox1.Image = null;
                this.CurrentItem = null;
            }
        }

        /// <summary>
        /// 今自分にマウスカーソルが乗ってるならtrue
        /// </summary>
        public bool IsMouseOn { get; private set; }

        private void frmPicture_MouseEnter(object sender, EventArgs e)
        {
            this.timer1.Enabled = true;
            this.IsMouseOn = true;
        }

        private void frmPicture_MouseLeave(object sender, EventArgs e)
        {
            //this.BeginInvoke(new Action(async () =>
            //{
            //    await Task.Delay(100);
            //    if (!this.IsMouseOn)
            //        this.Hide();
            //}));
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            var rect = new Rectangle(new Point(this.Bounds.X - 10, this.Bounds.Y), this.Bounds.Size + new Size(20, 20));
            if (!rect.Contains(Cursor.Position))
            {
                this.IsMouseOn = false;
                this.Hide();
            }
        }

        private void lstMorphs_SelectedValueChanged(object sender, EventArgs e)
        {
            var dspmorph = this.lstMorphs.SelectedItem as DspMorphItem;
            if (dspmorph != null)
            {
                this._morphselectedHandler?.Invoke(this, new MorphSelectedEventArgs(this._activeModel, dspmorph.DspMorphName));
            }
        }
    }
}