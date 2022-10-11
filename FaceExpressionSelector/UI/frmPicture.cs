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
        public frmPicture(ListControlConvertEventHandler listboxFormatHandler)
        {
            InitializeComponent();
            this.lstValue.Format += listboxFormatHandler;
            this.lstValue.Disposed += (s, e) => this.lstValue.Format -= listboxFormatHandler;
        }

        /// <summary>
        /// 現在表示中の表情
        /// </summary>
        public ExpressionItem CurrentItem { get; private set; } = null;

        /// <summary>
        /// 表示
        /// </summary>
        /// <param name="pt"></param>
        /// <param name="ownerForm">オーナーフォーム</param>
        /// <param name="item"></param>
        public void Show(Point pt, Form ownerForm, ExpressionItem item)
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

            this.lstValue.Items.Clear();
            if (item != null && item.MorphItems != null)
                this.lstValue.Items.AddRange(item.MorphItems.ToArray());
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
            this.IsMouseOn = true;
        }

        private void frmPicture_MouseLeave(object sender, EventArgs e)
        {
            this.IsMouseOn = false;
            this.Hide();
        }
    }
}