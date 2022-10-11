namespace FaceExpressionHelper
{
    partial class frmShot
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
            this.label6 = new System.Windows.Forms.Label();
            this.txtWaitTime = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.pnlBase = new System.Windows.Forms.Panel();
            this.pnlWaitTime = new System.Windows.Forms.Panel();
            this.pnlAnimation = new System.Windows.Forms.Panel();
            this.label14 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            this.txtWaitTimeForMotion = new System.Windows.Forms.TextBox();
            this.chkLimitMaxFrames = new System.Windows.Forms.CheckBox();
            this.lblFr = new System.Windows.Forms.Label();
            this.txtMaxFrame = new System.Windows.Forms.TextBox();
            this.cboFramerate = new System.Windows.Forms.ComboBox();
            this.label9 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.cboFileType = new System.Windows.Forms.ComboBox();
            this.lblTabName = new System.Windows.Forms.Label();
            this.btnSwap = new System.Windows.Forms.Button();
            this.lblFileName = new System.Windows.Forms.Label();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.chkNoInitialization = new System.Windows.Forms.CheckBox();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).BeginInit();
            this.pnlAddName.SuspendLayout();
            this.panel2.SuspendLayout();
            this.pnlBase.SuspendLayout();
            this.pnlWaitTime.SuspendLayout();
            this.pnlAnimation.SuspendLayout();
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
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.BackColor = System.Drawing.Color.Transparent;
            this.label6.Font = new System.Drawing.Font("メイリオ", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.label6.Location = new System.Drawing.Point(149, 28);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(27, 20);
            this.label6.TabIndex = 40;
            this.label6.Text = "ms";
            this.label6.MouseDown += new System.Windows.Forms.MouseEventHandler(this.frmShot_MouseDown);
            this.label6.MouseMove += new System.Windows.Forms.MouseEventHandler(this.frmShot_MouseMove);
            // 
            // txtWaitTime
            // 
            this.txtWaitTime.ImeMode = System.Windows.Forms.ImeMode.Off;
            this.txtWaitTime.Location = new System.Drawing.Point(107, 28);
            this.txtWaitTime.Name = "txtWaitTime";
            this.txtWaitTime.Size = new System.Drawing.Size(40, 19);
            this.txtWaitTime.TabIndex = 38;
            this.txtWaitTime.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.toolTip1.SetToolTip(this.txtWaitTime, "ポーズ変更からスクショを取るまでの猶予です。\r\nうまくスクショが取れない場合、ここの数値を大きくしてみてください。");
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.BackColor = System.Drawing.Color.Transparent;
            this.label5.Font = new System.Drawing.Font("ＭＳ Ｐゴシック", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.label5.Location = new System.Drawing.Point(4, 3);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(307, 22);
            this.label5.TabIndex = 39;
            this.label5.Text = "ポーズ変更からスクショを取るまでの猶予です。(単位：1/1000秒)\r\nうまくスクショが取れない場合、ここの数値を大きくしてみてください";
            this.label5.MouseDown += new System.Windows.Forms.MouseEventHandler(this.frmShot_MouseDown);
            this.label5.MouseMove += new System.Windows.Forms.MouseEventHandler(this.frmShot_MouseMove);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.BackColor = System.Drawing.Color.Transparent;
            this.label3.Font = new System.Drawing.Font("メイリオ", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.label3.Location = new System.Drawing.Point(4, 28);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(100, 20);
            this.label3.TabIndex = 37;
            this.label3.Text = "ウェイトタイム";
            this.label3.MouseDown += new System.Windows.Forms.MouseEventHandler(this.frmShot_MouseDown);
            this.label3.MouseMove += new System.Windows.Forms.MouseEventHandler(this.frmShot_MouseMove);
            // 
            // pnlBase
            // 
            this.pnlBase.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.pnlBase.Controls.Add(this.pnlWaitTime);
            this.pnlBase.Controls.Add(this.pnlAnimation);
            this.pnlBase.Controls.Add(this.lblTabName);
            this.pnlBase.Controls.Add(this.btnSwap);
            this.pnlBase.Controls.Add(this.lblFileName);
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
            // pnlWaitTime
            // 
            this.pnlWaitTime.Controls.Add(this.chkNoInitialization);
            this.pnlWaitTime.Controls.Add(this.label5);
            this.pnlWaitTime.Controls.Add(this.label3);
            this.pnlWaitTime.Controls.Add(this.label6);
            this.pnlWaitTime.Controls.Add(this.txtWaitTime);
            this.pnlWaitTime.Location = new System.Drawing.Point(1, 193);
            this.pnlWaitTime.Name = "pnlWaitTime";
            this.pnlWaitTime.Size = new System.Drawing.Size(330, 77);
            this.pnlWaitTime.TabIndex = 47;
            this.pnlWaitTime.MouseDown += new System.Windows.Forms.MouseEventHandler(this.frmShot_MouseDown);
            this.pnlWaitTime.MouseMove += new System.Windows.Forms.MouseEventHandler(this.frmShot_MouseMove);
            // 
            // pnlAnimation
            // 
            this.pnlAnimation.Controls.Add(this.label14);
            this.pnlAnimation.Controls.Add(this.label12);
            this.pnlAnimation.Controls.Add(this.label13);
            this.pnlAnimation.Controls.Add(this.txtWaitTimeForMotion);
            this.pnlAnimation.Controls.Add(this.chkLimitMaxFrames);
            this.pnlAnimation.Controls.Add(this.lblFr);
            this.pnlAnimation.Controls.Add(this.txtMaxFrame);
            this.pnlAnimation.Controls.Add(this.cboFramerate);
            this.pnlAnimation.Controls.Add(this.label9);
            this.pnlAnimation.Controls.Add(this.label8);
            this.pnlAnimation.Controls.Add(this.cboFileType);
            this.pnlAnimation.Location = new System.Drawing.Point(2, 271);
            this.pnlAnimation.Name = "pnlAnimation";
            this.pnlAnimation.Size = new System.Drawing.Size(329, 159);
            this.pnlAnimation.TabIndex = 42;
            this.pnlAnimation.MouseDown += new System.Windows.Forms.MouseEventHandler(this.frmShot_MouseDown);
            this.pnlAnimation.MouseMove += new System.Windows.Forms.MouseEventHandler(this.frmShot_MouseMove);
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.BackColor = System.Drawing.Color.Transparent;
            this.label14.Font = new System.Drawing.Font("ＭＳ Ｐゴシック", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.label14.Location = new System.Drawing.Point(3, 54);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(292, 11);
            this.label14.TabIndex = 52;
            this.label14.Text = "うまく全フレーム撮れない場合、↓数値を大きくしてみてください";
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.BackColor = System.Drawing.Color.Transparent;
            this.label12.Font = new System.Drawing.Font("メイリオ", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.label12.Location = new System.Drawing.Point(3, 65);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(100, 20);
            this.label12.TabIndex = 49;
            this.label12.Text = "ウェイトタイム";
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.BackColor = System.Drawing.Color.Transparent;
            this.label13.Font = new System.Drawing.Font("メイリオ", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.label13.Location = new System.Drawing.Point(145, 65);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(27, 20);
            this.label13.TabIndex = 51;
            this.label13.Text = "ms";
            // 
            // txtWaitTimeForMotion
            // 
            this.txtWaitTimeForMotion.ImeMode = System.Windows.Forms.ImeMode.Off;
            this.txtWaitTimeForMotion.Location = new System.Drawing.Point(105, 65);
            this.txtWaitTimeForMotion.Name = "txtWaitTimeForMotion";
            this.txtWaitTimeForMotion.Size = new System.Drawing.Size(40, 19);
            this.txtWaitTimeForMotion.TabIndex = 50;
            this.txtWaitTimeForMotion.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.toolTip1.SetToolTip(this.txtWaitTimeForMotion, "モーション一コマあたりの猶予です。\r\nうまく全フレーム撮れない場合、ここの数値を大きくしてみてください。");
            // 
            // chkLimitMaxFrames
            // 
            this.chkLimitMaxFrames.AutoSize = true;
            this.chkLimitMaxFrames.Location = new System.Drawing.Point(5, 85);
            this.chkLimitMaxFrames.Name = "chkLimitMaxFrames";
            this.chkLimitMaxFrames.Size = new System.Drawing.Size(209, 16);
            this.chkLimitMaxFrames.TabIndex = 48;
            this.chkLimitMaxFrames.Text = "最大フレーム数指定  (保存されません)";
            this.chkLimitMaxFrames.UseVisualStyleBackColor = true;
            this.chkLimitMaxFrames.CheckedChanged += new System.EventHandler(this.chkLimitMaxFrames_CheckedChanged);
            // 
            // lblFr
            // 
            this.lblFr.AutoSize = true;
            this.lblFr.BackColor = System.Drawing.Color.Transparent;
            this.lblFr.Enabled = false;
            this.lblFr.Font = new System.Drawing.Font("メイリオ", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.lblFr.Location = new System.Drawing.Point(143, 101);
            this.lblFr.Name = "lblFr";
            this.lblFr.Size = new System.Drawing.Size(22, 20);
            this.lblFr.TabIndex = 46;
            this.lblFr.Text = "fr ";
            // 
            // txtMaxFrame
            // 
            this.txtMaxFrame.Enabled = false;
            this.txtMaxFrame.ImeMode = System.Windows.Forms.ImeMode.Off;
            this.txtMaxFrame.Location = new System.Drawing.Point(105, 101);
            this.txtMaxFrame.Name = "txtMaxFrame";
            this.txtMaxFrame.Size = new System.Drawing.Size(40, 19);
            this.txtMaxFrame.TabIndex = 45;
            this.txtMaxFrame.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.toolTip1.SetToolTip(this.txtMaxFrame, "ポーズ変更からスクショを取るまでの猶予です。\r\nうまくスクショが取れない場合、ここの数値を大きくしてみてください。");
            // 
            // cboFramerate
            // 
            this.cboFramerate.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboFramerate.FormattingEnabled = true;
            this.cboFramerate.Location = new System.Drawing.Point(105, 27);
            this.cboFramerate.Name = "cboFramerate";
            this.cboFramerate.Size = new System.Drawing.Size(69, 20);
            this.cboFramerate.TabIndex = 43;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.BackColor = System.Drawing.Color.Transparent;
            this.label9.Font = new System.Drawing.Font("メイリオ", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.label9.Location = new System.Drawing.Point(4, 27);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(100, 20);
            this.label9.TabIndex = 42;
            this.label9.Text = "フレームレート";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.BackColor = System.Drawing.Color.Transparent;
            this.label8.Font = new System.Drawing.Font("メイリオ", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.label8.Location = new System.Drawing.Point(4, 4);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(100, 20);
            this.label8.TabIndex = 41;
            this.label8.Text = "ファイルタイプ";
            // 
            // cboFileType
            // 
            this.cboFileType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboFileType.FormattingEnabled = true;
            this.cboFileType.Location = new System.Drawing.Point(105, 3);
            this.cboFileType.Name = "cboFileType";
            this.cboFileType.Size = new System.Drawing.Size(155, 20);
            this.cboFileType.TabIndex = 0;
            // 
            // lblTabName
            // 
            this.lblTabName.AutoSize = true;
            this.lblTabName.BackColor = System.Drawing.Color.Transparent;
            this.lblTabName.Font = new System.Drawing.Font("MS UI Gothic", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.lblTabName.Location = new System.Drawing.Point(134, 3);
            this.lblTabName.Name = "lblTabName";
            this.lblTabName.Size = new System.Drawing.Size(122, 13);
            this.lblTabName.TabIndex = 41;
            this.lblTabName.Text = "なんとかかんとか.vbp";
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
            // lblFileName
            // 
            this.lblFileName.AutoSize = true;
            this.lblFileName.BackColor = System.Drawing.Color.Transparent;
            this.lblFileName.Font = new System.Drawing.Font("MS UI Gothic", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.lblFileName.Location = new System.Drawing.Point(134, 20);
            this.lblFileName.Name = "lblFileName";
            this.lblFileName.Size = new System.Drawing.Size(122, 13);
            this.lblFileName.TabIndex = 30;
            this.lblFileName.Text = "なんとかかんとか.vbp";
            this.lblFileName.MouseDown += new System.Windows.Forms.MouseEventHandler(this.frmShot_MouseDown);
            this.lblFileName.MouseMove += new System.Windows.Forms.MouseEventHandler(this.frmShot_MouseMove);
            // 
            // chkNoInitialization
            // 
            this.chkNoInitialization.AutoSize = true;
            this.chkNoInitialization.Location = new System.Drawing.Point(6, 53);
            this.chkNoInitialization.Name = "chkNoInitialization";
            this.chkNoInitialization.Size = new System.Drawing.Size(242, 16);
            this.chkNoInitialization.TabIndex = 54;
            this.chkNoInitialization.Text = "適用前にポーズ初期化しない  (保存しません)";
            this.chkNoInitialization.UseVisualStyleBackColor = true;
            // 
            // frmShot
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.SkyBlue;
            this.ClientSize = new System.Drawing.Size(704, 564);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.pnlBase);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            this.MinimumSize = new System.Drawing.Size(166, 166);
            this.Name = "frmShot";
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
            this.pnlWaitTime.ResumeLayout(false);
            this.pnlWaitTime.PerformLayout();
            this.pnlAnimation.ResumeLayout(false);
            this.pnlAnimation.PerformLayout();
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
        private System.Windows.Forms.Label lblFileName;
        private System.Windows.Forms.TextBox txtWaitTime;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label lblTabName;
        private System.Windows.Forms.Panel pnlAnimation;
        private System.Windows.Forms.ComboBox cboFramerate;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.ComboBox cboFileType;
        private System.Windows.Forms.Panel pnlWaitTime;
        private System.Windows.Forms.Label lblFr;
        private System.Windows.Forms.TextBox txtMaxFrame;
        private System.Windows.Forms.CheckBox chkLimitMaxFrames;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.TextBox txtWaitTimeForMotion;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.CheckBox chkNoInitialization;
    }
}