namespace FaceExpressionHelper.UI
{
    partial class frmScrShot
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.btnOK = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.numericUpDown1 = new System.Windows.Forms.NumericUpDown();
            this.lblRimColor = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.button2 = new System.Windows.Forms.Button();
            this.pnlAddName = new System.Windows.Forms.Panel();
            this.rbNone = new System.Windows.Forms.RadioButton();
            this.rbUpper = new System.Windows.Forms.RadioButton();
            this.rbDowner = new System.Windows.Forms.RadioButton();
            this.lblColor = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.lblFont = new System.Windows.Forms.Label();
            this.fontDialog1 = new System.Windows.Forms.FontDialog();
            this.colorDialog1 = new System.Windows.Forms.ColorDialog();
            this.label1 = new System.Windows.Forms.Label();
            this.txtWidth = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.txtHeight = new System.Windows.Forms.TextBox();
            this.panel2 = new System.Windows.Forms.Panel();
            this.label4 = new System.Windows.Forms.Label();
            this.pnlBase = new System.Windows.Forms.Panel();
            this.btnSwap = new System.Windows.Forms.Button();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).BeginInit();
            this.pnlAddName.SuspendLayout();
            this.panel2.SuspendLayout();
            this.pnlBase.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnOK
            // 
            this.btnOK.Location = new System.Drawing.Point(0, 0);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(61, 26);
            this.btnOK.TabIndex = 0;
            this.btnOK.Text = "確定";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(67, 0);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(61, 26);
            this.btnCancel.TabIndex = 1;
            this.btnCancel.Text = "キャンセル";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // numericUpDown1
            // 
            this.numericUpDown1.Font = new System.Drawing.Font("メイリオ", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.numericUpDown1.Location = new System.Drawing.Point(184, 53);
            this.numericUpDown1.Maximum = new decimal(new int[] {
            6,
            0,
            0,
            0});
            this.numericUpDown1.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numericUpDown1.Name = "numericUpDown1";
            this.numericUpDown1.Size = new System.Drawing.Size(29, 24);
            this.numericUpDown1.TabIndex = 35;
            this.numericUpDown1.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // lblRimColor
            // 
            this.lblRimColor.BackColor = System.Drawing.Color.Black;
            this.lblRimColor.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblRimColor.Location = new System.Drawing.Point(110, 54);
            this.lblRimColor.Name = "lblRimColor";
            this.lblRimColor.Size = new System.Drawing.Size(23, 21);
            this.lblRimColor.TabIndex = 32;
            this.lblRimColor.Click += new System.EventHandler(this.lblColor_Click);
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.BackColor = System.Drawing.Color.Transparent;
            this.label10.Font = new System.Drawing.Font("メイリオ", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.label10.Location = new System.Drawing.Point(151, 54);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(35, 20);
            this.label10.TabIndex = 34;
            this.label10.Text = "縁幅";
            this.label10.MouseDown += new System.Windows.Forms.MouseEventHandler(this.frmShot_MouseDown);
            this.label10.MouseMove += new System.Windows.Forms.MouseEventHandler(this.frmShot_MouseMove);
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.BackColor = System.Drawing.Color.Transparent;
            this.label11.Font = new System.Drawing.Font("メイリオ", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.label11.Location = new System.Drawing.Point(75, 54);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(35, 20);
            this.label11.TabIndex = 33;
            this.label11.Text = "縁色";
            this.label11.MouseDown += new System.Windows.Forms.MouseEventHandler(this.frmShot_MouseDown);
            this.label11.MouseMove += new System.Windows.Forms.MouseEventHandler(this.frmShot_MouseMove);
            // 
            // button2
            // 
            this.button2.Font = new System.Drawing.Font("メイリオ", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.button2.Location = new System.Drawing.Point(215, 28);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(68, 24);
            this.button2.TabIndex = 27;
            this.button2.Text = "ﾌｫﾝﾄ変更";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // pnlAddName
            // 
            this.pnlAddName.Controls.Add(this.rbNone);
            this.pnlAddName.Controls.Add(this.rbUpper);
            this.pnlAddName.Controls.Add(this.rbDowner);
            this.pnlAddName.Location = new System.Drawing.Point(73, 4);
            this.pnlAddName.Name = "pnlAddName";
            this.pnlAddName.Size = new System.Drawing.Size(192, 22);
            this.pnlAddName.TabIndex = 22;
            // 
            // rbNone
            // 
            this.rbNone.AutoSize = true;
            this.rbNone.Font = new System.Drawing.Font("MS UI Gothic", 9F);
            this.rbNone.Location = new System.Drawing.Point(147, 3);
            this.rbNone.Name = "rbNone";
            this.rbNone.Size = new System.Drawing.Size(42, 16);
            this.rbNone.TabIndex = 23;
            this.rbNone.Text = "なし";
            this.rbNone.UseVisualStyleBackColor = true;
            // 
            // rbUpper
            // 
            this.rbUpper.AutoSize = true;
            this.rbUpper.BackColor = System.Drawing.Color.Transparent;
            this.rbUpper.Font = new System.Drawing.Font("MS UI Gothic", 9F);
            this.rbUpper.Location = new System.Drawing.Point(75, 3);
            this.rbUpper.Name = "rbUpper";
            this.rbUpper.Size = new System.Drawing.Size(68, 16);
            this.rbUpper.TabIndex = 22;
            this.rbUpper.Text = "上に追加";
            this.rbUpper.UseVisualStyleBackColor = false;
            // 
            // rbDowner
            // 
            this.rbDowner.AutoSize = true;
            this.rbDowner.BackColor = System.Drawing.Color.Transparent;
            this.rbDowner.Checked = true;
            this.rbDowner.Font = new System.Drawing.Font("MS UI Gothic", 9F);
            this.rbDowner.Location = new System.Drawing.Point(3, 3);
            this.rbDowner.Name = "rbDowner";
            this.rbDowner.Size = new System.Drawing.Size(68, 16);
            this.rbDowner.TabIndex = 21;
            this.rbDowner.TabStop = true;
            this.rbDowner.Text = "下に追加";
            this.rbDowner.UseVisualStyleBackColor = false;
            // 
            // lblColor
            // 
            this.lblColor.BackColor = System.Drawing.Color.Black;
            this.lblColor.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblColor.Location = new System.Drawing.Point(52, 54);
            this.lblColor.Name = "lblColor";
            this.lblColor.Size = new System.Drawing.Size(23, 21);
            this.lblColor.TabIndex = 24;
            this.lblColor.Click += new System.EventHandler(this.lblColor_Click);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.BackColor = System.Drawing.Color.Transparent;
            this.label7.Font = new System.Drawing.Font("メイリオ", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.label7.Location = new System.Drawing.Point(4, 54);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(48, 20);
            this.label7.TabIndex = 25;
            this.label7.Text = "文字色";
            this.label7.MouseDown += new System.Windows.Forms.MouseEventHandler(this.frmShot_MouseDown);
            this.label7.MouseMove += new System.Windows.Forms.MouseEventHandler(this.frmShot_MouseMove);
            // 
            // lblFont
            // 
            this.lblFont.AutoSize = true;
            this.lblFont.BackColor = System.Drawing.Color.Transparent;
            this.lblFont.Font = new System.Drawing.Font("メイリオ", 9F);
            this.lblFont.Location = new System.Drawing.Point(5, 31);
            this.lblFont.Name = "lblFont";
            this.lblFont.Size = new System.Drawing.Size(101, 18);
            this.lblFont.TabIndex = 26;
            this.lblFont.Text = "メイリオ, 9.75pt";
            this.lblFont.MouseDown += new System.Windows.Forms.MouseEventHandler(this.frmShot_MouseDown);
            this.lblFont.MouseMove += new System.Windows.Forms.MouseEventHandler(this.frmShot_MouseMove);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Font = new System.Drawing.Font("MS UI Gothic", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.label1.Location = new System.Drawing.Point(26, 62);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(259, 48);
            this.label1.TabIndex = 26;
            this.label1.Text = "スクショを取りたい場所で\r\n確定ボタンを押してください";
            this.label1.MouseDown += new System.Windows.Forms.MouseEventHandler(this.frmShot_MouseDown);
            this.label1.MouseMove += new System.Windows.Forms.MouseEventHandler(this.frmShot_MouseMove);
            // 
            // txtWidth
            // 
            this.txtWidth.ImeMode = System.Windows.Forms.ImeMode.Off;
            this.txtWidth.Location = new System.Drawing.Point(3, 38);
            this.txtWidth.Name = "txtWidth";
            this.txtWidth.Size = new System.Drawing.Size(40, 19);
            this.txtWidth.TabIndex = 27;
            this.txtWidth.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.txtWidth.Leave += new System.EventHandler(this.txtWidth_TextChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("メイリオ", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.label2.Location = new System.Drawing.Point(45, 38);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(19, 20);
            this.label2.TabIndex = 28;
            this.label2.Text = "×";
            // 
            // txtHeight
            // 
            this.txtHeight.ImeMode = System.Windows.Forms.ImeMode.Off;
            this.txtHeight.Location = new System.Drawing.Point(67, 38);
            this.txtHeight.Name = "txtHeight";
            this.txtHeight.Size = new System.Drawing.Size(40, 19);
            this.txtHeight.TabIndex = 29;
            this.txtHeight.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.txtHeight.Leave += new System.EventHandler(this.txtWidth_TextChanged);
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.label4);
            this.panel2.Controls.Add(this.numericUpDown1);
            this.panel2.Controls.Add(this.button2);
            this.panel2.Controls.Add(this.lblRimColor);
            this.panel2.Controls.Add(this.lblFont);
            this.panel2.Controls.Add(this.label10);
            this.panel2.Controls.Add(this.label7);
            this.panel2.Controls.Add(this.label11);
            this.panel2.Controls.Add(this.lblColor);
            this.panel2.Controls.Add(this.pnlAddName);
            this.panel2.Location = new System.Drawing.Point(2, 114);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(380, 79);
            this.panel2.TabIndex = 31;
            this.panel2.MouseDown += new System.Windows.Forms.MouseEventHandler(this.frmShot_MouseDown);
            this.panel2.MouseMove += new System.Windows.Forms.MouseEventHandler(this.frmShot_MouseMove);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.BackColor = System.Drawing.Color.Transparent;
            this.label4.Font = new System.Drawing.Font("メイリオ", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.label4.Location = new System.Drawing.Point(1, 5);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(74, 20);
            this.label4.TabIndex = 36;
            this.label4.Text = "ファイル名";
            this.label4.MouseDown += new System.Windows.Forms.MouseEventHandler(this.frmShot_MouseDown);
            this.label4.MouseMove += new System.Windows.Forms.MouseEventHandler(this.frmShot_MouseMove);
            // 
            // pnlBase
            // 
            this.pnlBase.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.pnlBase.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlBase.Controls.Add(this.btnSwap);
            this.pnlBase.Controls.Add(this.btnCancel);
            this.pnlBase.Controls.Add(this.btnOK);
            this.pnlBase.Controls.Add(this.txtHeight);
            this.pnlBase.Controls.Add(this.label1);
            this.pnlBase.Controls.Add(this.label2);
            this.pnlBase.Controls.Add(this.txtWidth);
            this.pnlBase.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlBase.Location = new System.Drawing.Point(0, 0);
            this.pnlBase.MinimumSize = new System.Drawing.Size(150, 150);
            this.pnlBase.Name = "pnlBase";
            this.pnlBase.Size = new System.Drawing.Size(704, 564);
            this.pnlBase.TabIndex = 32;
            this.pnlBase.DoubleClick += new System.EventHandler(this.pnlBase_DoubleClick);
            this.pnlBase.MouseDown += new System.Windows.Forms.MouseEventHandler(this.frmShot_MouseDown);
            this.pnlBase.MouseMove += new System.Windows.Forms.MouseEventHandler(this.frmShot_MouseMove);
            // 
            // btnSwap
            // 
            this.btnSwap.Location = new System.Drawing.Point(110, 34);
            this.btnSwap.Name = "btnSwap";
            this.btnSwap.Size = new System.Drawing.Size(61, 26);
            this.btnSwap.TabIndex = 31;
            this.btnSwap.Text = "縦横反転";
            this.btnSwap.UseVisualStyleBackColor = true;
            this.btnSwap.Click += new System.EventHandler(this.btnSwap_Click);
            // 
            // frmScrShot
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.SkyBlue;
            this.ClientSize = new System.Drawing.Size(704, 564);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.pnlBase);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            this.MinimumSize = new System.Drawing.Size(166, 166);
            this.Name = "frmScrShot";
            this.Opacity = 0.5D;
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "frmShot";
            this.TopMost = true;
            this.Shown += new System.EventHandler(this.frmShot_Shown);
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.frmShot_MouseDown);
            this.MouseMove += new System.Windows.Forms.MouseEventHandler(this.frmShot_MouseMove);
            this.Resize += new System.EventHandler(this.frmShot_Resize);
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).EndInit();
            this.pnlAddName.ResumeLayout(false);
            this.pnlAddName.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.pnlBase.ResumeLayout(false);
            this.pnlBase.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.NumericUpDown numericUpDown1;
        private System.Windows.Forms.Label lblRimColor;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Panel pnlAddName;
        private System.Windows.Forms.RadioButton rbNone;
        private System.Windows.Forms.RadioButton rbUpper;
        private System.Windows.Forms.RadioButton rbDowner;
        private System.Windows.Forms.Label lblColor;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label lblFont;
        private System.Windows.Forms.FontDialog fontDialog1;
        private System.Windows.Forms.ColorDialog colorDialog1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtWidth;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtHeight;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Panel pnlBase;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button btnSwap;
        private System.Windows.Forms.ToolTip toolTip1;
    }
}