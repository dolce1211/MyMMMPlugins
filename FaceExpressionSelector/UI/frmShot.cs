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
using System.Runtime.InteropServices;
using ScreenShot;

namespace FaceExpressionHelper
{
    public partial class frmShot : NakedForm
    {
        private bool _forceNoPoseInitialization = false;
        private bool _isInitializing = false;
        private string _xmlfilePath = string.Empty;
        //private List<ctrItem> _ctrList = null;

        //public List<ctrItem> CtrList
        //{
        //    get { return this._ctrList; }
        //}

        public frmShot(Form owner, string xmlfilePath, Rectangle startPos, string tabName, int limitedFrames,
                        bool forceNoPoseInitialization)
        {
            InitializeComponent();
            _xmlfilePath = xmlfilePath;

            this.Initialize(owner, startPos, limitedFrames);

            this.lblTabName.Text = tabName;
            //if (ctrList.Count == 1)
            //    this.lblFileName.Text = ctrList.FirstOrDefault().VpdName;
            //else
            //    this.lblFileName.Text = ctrList.Count().ToString() + "件";

            //2021/04/11
            this._forceNoPoseInitialization = forceNoPoseInitialization;
        }

        private void Initialize(Form owner, Rectangle startPos, int limitedFrames)
        {
            _isInitializing = true;
            try
            {
                LetterArgs letArgs = MyUtility.Serializer.Deserialize<LetterArgs>(this._xmlfilePath);
                if (letArgs == null)
                {
                    //デフォルト値を指定
                    Font defFont = new Font("メイリオ", 36, FontStyle.Bold);
                    letArgs = new LetterArgs();
                    letArgs.FontName = defFont.Name;
                    letArgs.FontSize = defFont.Size;
                    letArgs.FontStyle = (int)defFont.Style;
                    letArgs.ColorInt = ColorTranslator.ToOle(Color.Orange);
                    letArgs.RimColorInt = ColorTranslator.ToOle(Color.Black);
                    letArgs.RimWidth = 1;
                    letArgs.NamePos = 1;
                    letArgs.WaitTime = 50;
                    letArgs.WaitTimeForMotion = 50;
                }
                if (letArgs.WaitTimeForMotion <= 0)
                    letArgs.WaitTimeForMotion = 50;//規定値。0は許容しない。

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

                //ウェイトタイム
                this.txtWaitTime.Text = letArgs.WaitTime.ToString();

                TextBoxHelper.LimitInputToNum(this.txtWidth, false, false);
                TextBoxHelper.LimitInputToNum(this.txtHeight, false, false);
                TextBoxHelper.LimitInputToNum(this.txtWaitTime, false, false);
                TextBoxHelper.LimitInputToNum(this.txtWaitTimeForMotion, false, false);
                TextBoxHelper.LimitInputToNum(this.txtMaxFrame, false, false);

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

                this.pnlAnimation.Visible = false;
                this.BackColor = Color.SkyBlue;
                this.pnlWaitTime.Visible = true;

                //2021/04/11 適用前にポーズ初期化しないオプションを追加
                this.chkNoInitialization.Checked = false;

                this.chkLimitMaxFrames.Checked = false;
                this.txtMaxFrame.Text = string.Empty;
                if (limitedFrames > 0)
                {
                    this.chkLimitMaxFrames.Checked = true;
                    this.txtMaxFrame.Text = limitedFrames.ToString();
                }
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
                //または、つぎのようにする
                //this.Location = new Point(
                //    this.Location.X + e.X - mousePoint.X,
                //    this.Location.Y + e.Y - mousePoint.Y);
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

            letArgs.WaitTime = this.txtWaitTime.Text.ToInt();
            if (this.pnlAnimation.Visible)
            {
                letArgs.MaxFrameCount = 0;
                if (this.txtMaxFrame.Enabled && this.txtMaxFrame.Text.Trim() != string.Empty)
                    letArgs.MaxFrameCount = this.txtMaxFrame.Text.ToInt();

                letArgs.WaitTimeForMotion = this.txtWaitTimeForMotion.Text.ToInt();
                if (letArgs.WaitTimeForMotion <= 0)
                    letArgs.WaitTimeForMotion = 1;
            }

            if (pnlAnimation.Visible == false)
            {
                //モーションモードではポーズ初期化しないオプションは不要
                //2021/04/11 適用前にポーズ初期化しないオプションを追加
                if (_forceNoPoseInitialization)
                    letArgs.DoNotInitializeOnExecute_Temp = true;
                else
                    letArgs.DoNotInitializeOnExecute_Temp = (this.chkNoInitialization.Visible && this.chkNoInitialization.Checked);
            }

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

        private ScreenShot.LetterArgs _letterArgs = null;

        public ScreenShot.LetterArgs LetterArgs
        {
            get { return this._letterArgs; }
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            this._letterArgs = null;
            if (sender == this.btnOK)
            {
                this._letterArgs = this.CreateLetterArgs();
                if (!MyUtility.Serializer.Serialize(this._letterArgs, this._xmlfilePath))
                {
                    MessageBox.Show("設定が保存できませんでした");
                }

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
                //this.txtWidth.Text = this.txtHeight.Text;
                //this.txtHeight.Text = buf.ToString();

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

        private void chkLimitMaxFrames_CheckedChanged(object sender, EventArgs e)
        {
            this.txtMaxFrame.Enabled = this.chkLimitMaxFrames.Checked;
            this.lblFr.Enabled = this.chkLimitMaxFrames.Checked;
        }

        //private void InitializeComponent()
        //{
        //    this.SuspendLayout();
        //    //
        //    // frmShot
        //    //
        //    this.ClientSize = new System.Drawing.Size(552, 294);
        //    this.Name = "frmShot";
        //    this.ResumeLayout(false);

        //}

        //private void InitializeComponent()
        //{
        //    this.SuspendLayout();
        //    //
        //    // frmShot
        //    //
        //    this.ClientSize = new System.Drawing.Size(365, 315);
        //    this.Name = "frmShot";
        //    this.ResumeLayout(false);
        //}
    }
}