using MyUtility;
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
    public partial class frmScrShot : NakedForm //リサイズ可能かつ上部バーが無いフォームを実現するためにNakedFormを継承
    {
        private bool _isInitializing = false;

        public frmScrShot(Form owner, LetterArgs letArgs, Rectangle startPos)
        {
            InitializeComponent();

            this.Initialize(owner, letArgs, startPos);
        }

        private void Initialize(Form owner, LetterArgs letArgs, Rectangle startPos)
        {
            _isInitializing = true;
            try
            {
                //上下
                if (letArgs.NamePos == 0)
                    this.rbNone.Checked = true;
                else if (letArgs.NamePos == 1)
                    this.rbDowner.Checked = true;
                else if (letArgs.NamePos == 2)
                    this.rbUpper.Checked = true;

                //背景色
                this.lblColor.BackColor = letArgs.Color;
                //ふち色
                this.lblRimColor.BackColor = letArgs.RimColor;
                //ふち幅
                int width = letArgs.RimWidth;
                if (!(width >= 1 && width <= 6)) width = 1;

                //MessageBox.Show(width.ToString ());
                this.numericUpDown1.Value = width;

                //font
                this.UpdateLblFont(letArgs.Font);

                TextBoxHelper.LimitInputToNum(this.txtWidth, false, false);
                TextBoxHelper.LimitInputToNum(this.txtHeight, false, false);
                if (startPos != new Rectangle())
                {
                    this.StartPosition = FormStartPosition.Manual;

                    Point pnlLocation = this.PointToScreen(this.pnlBase.Location);
                    Point myLocation = this.Location;
                    Point subtract = Point.Subtract(myLocation, new Size(pnlLocation.X, pnlLocation.Y));

                    this.Location = Point.Add(startPos.Location, new Size(subtract.X, subtract.Y));

                    var diff = new Size((this.Width - pnlBase.Width), (this.Height - pnlBase.Height));
                    var newSize = Size.Add(startPos.Size, diff);
                    this.Size = newSize;

                    this.Refresh();
                }
                else
                {
                    this.Size = new Size(316, 316);
                    this.Location = owner.Location;
                    //this.Left += owner.Width/2;
                }
                this.txtWidth.Text = this.pnlBase.Width.ToString();
                this.txtHeight.Text = this.pnlBase.Height.ToString();

                this.BackColor = Color.SkyBlue;
            }
            finally
            {
                _isInitializing = false;
            }
        }

        //マウスのクリック位置を記憶
        private Point mousePoint;

        //Form1のMouseDownイベントハンドラ
        //マウスのボタンが押されたとき
        private void frmShot_MouseDown(object sender,
            System.Windows.Forms.MouseEventArgs e)
        {
            if ((e.Button & MouseButtons.Left) == MouseButtons.Left)
            {
                //位置を記憶する
                mousePoint = new Point(e.X, e.Y);
            }
        }

        //Form1のMouseMoveイベントハンドラ
        //マウスが動いたとき
        private void frmShot_MouseMove(object sender,
            System.Windows.Forms.MouseEventArgs e)
        {
            if ((e.Button & MouseButtons.Left) == MouseButtons.Left)
            {
                this.Left += e.X - mousePoint.X;
                this.Top += e.Y - mousePoint.Y;
            }
        }

        private LetterArgs CreateLetterArgs()
        {
            LetterArgs letArgs = new LetterArgs();
            int namePos = 0;//なし
            if (rbDowner.Checked)
                namePos = 1;//下
            else if (rbUpper.Checked)
                namePos = 2;//上

            Font nameFont = (Font)this.lblFont.Tag;

            letArgs.NamePos = namePos;
            letArgs.FontName = nameFont.Name;
            letArgs.FontSize = nameFont.Size;
            letArgs.FontStyle = (int)nameFont.Style;
            letArgs.ColorInt = ColorTranslator.ToOle(this.lblColor.BackColor);
            letArgs.RimColorInt = ColorTranslator.ToOle(this.lblRimColor.BackColor);
            letArgs.RimWidth = (int)this.numericUpDown1.Value;

            return letArgs;
        }

        private Rectangle _frmPosition;

        public Rectangle FrmPosition
        {
            get { return this._frmPosition; }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.fontDialog1.Font = (Font)this.lblFont.Tag;
            if (this.fontDialog1.ShowDialog() == DialogResult.OK)
            {
                this.UpdateLblFont(this.fontDialog1.Font);
            }
        }

        private void UpdateLblFont(Font fnt)
        {
            this.lblFont.Tag = fnt;
            this.lblFont.Text = string.Format("{0}, {1}pt", fnt.Name, fnt.SizeInPoints);
            if (fnt.Style != FontStyle.Regular)
            {
                this.lblFont.Text += "," + fnt.Style.ToString();
            }
        }

        /// <summary>
        /// 色指定
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lblColor_Click(object sender, EventArgs e)
        {
            Label lbl = (Label)sender;
            this.colorDialog1.Color = lbl.BackColor;
            if (this.colorDialog1.ShowDialog() == DialogResult.OK)
            {
                //選択された色の取得
                lbl.BackColor = this.colorDialog1.Color;
            }
        }

        private LetterArgs _letterArgs = null;

        public LetterArgs LetterArgs
        {
            get { return this._letterArgs; }
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            this._letterArgs = null;
            if (sender == this.btnOK)
            {
                this._letterArgs = this.CreateLetterArgs();
                this._frmPosition = new Rectangle(this.PointToScreen(this.pnlBase.Location), this.pnlBase.Size);
                this.DialogResult = DialogResult.OK;
            }
            else
                this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void frmShot_Resize(object sender, EventArgs e)
        {
            if (_isInitializing) return;
            this.txtWidth.Text = this.pnlBase.Width.ToString();
            this.txtHeight.Text = this.pnlBase.Height.ToString();
        }

        private void txtWidth_TextChanged(object sender, EventArgs e)
        {
            if (_isInitializing) return;

            TextBox txt = (TextBox)sender;
            int num = txt.Text.ToInt();
            if (num < 150) num = 150;

            var diff = new Size((this.Width - pnlBase.Width), (this.Height - pnlBase.Height));

            if (sender == this.txtWidth)
                this.Width = num + diff.Width;
            else if (sender == this.txtHeight)
                this.Height = num + diff.Height;
        }

        private void frmShot_Shown(object sender, EventArgs e)
        {
            this.btnOK.Focus();
        }

        private void btnSwap_Click(object sender, EventArgs e)
        {
            this.BeginUpdate();
            try
            {
                int buf = this.txtWidth.Text.ToInt();

                this.Width = this.txtHeight.Text.ToInt() + 16;
                this.Height = buf + 16;
            }
            finally
            {
                this.EndUpdate();
            }
        }

        private void pnlBase_DoubleClick(object sender, EventArgs e)
        {
            this.btnCancel.PerformClick();
        }
    }
}