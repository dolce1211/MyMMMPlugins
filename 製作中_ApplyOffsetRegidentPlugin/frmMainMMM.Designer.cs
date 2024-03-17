namespace ApplyOffsetRegidentPlugin
{
    partial class frmMainMMM
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
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.rb1 = new System.Windows.Forms.RadioButton();
            this.rb001 = new System.Windows.Forms.RadioButton();
            this.rb01 = new System.Windows.Forms.RadioButton();
            this.chkLocalL = new System.Windows.Forms.CheckBox();
            this.chkLocalR = new System.Windows.Forms.CheckBox();
            this.btnOK = new System.Windows.Forms.Button();
            this.btnReset = new System.Windows.Forms.Button();
            this.myConrolZ = new MyConrol();
            this.myConrolY = new MyConrol();
            this.myConrolX = new MyConrol();
            this.myConrolRZ = new MyConrol();
            this.myConrolRY = new MyConrol();
            this.myConrolRX = new MyConrol();
            this.lblCaption = new System.Windows.Forms.Label();
            this.lblCount = new System.Windows.Forms.Label();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(255)))));
            this.label1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label1.Font = new System.Drawing.Font("メイリオ", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.label1.Location = new System.Drawing.Point(1, 1);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(251, 23);
            this.label1.TabIndex = 0;
            this.label1.Text = "移動";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label2
            // 
            this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(255)))));
            this.label2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label2.Font = new System.Drawing.Font("メイリオ", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.label2.Location = new System.Drawing.Point(1, 189);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(251, 23);
            this.label2.TabIndex = 1;
            this.label2.Text = "回転";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel1.Controls.Add(this.rb1);
            this.panel1.Controls.Add(this.rb001);
            this.panel1.Controls.Add(this.rb01);
            this.panel1.Location = new System.Drawing.Point(1, 25);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(251, 23);
            this.panel1.TabIndex = 5;
            // 
            // rb1
            // 
            this.rb1.AutoSize = true;
            this.rb1.Location = new System.Drawing.Point(195, 3);
            this.rb1.Name = "rb1";
            this.rb1.Size = new System.Drawing.Size(53, 16);
            this.rb1.TabIndex = 2;
            this.rb1.Text = "1単位";
            this.rb1.UseVisualStyleBackColor = true;
            this.rb1.CheckedChanged += new System.EventHandler(this.rb01_CheckedChanged);
            // 
            // rb001
            // 
            this.rb001.AutoSize = true;
            this.rb001.Location = new System.Drawing.Point(4, 3);
            this.rb001.Name = "rb001";
            this.rb001.Size = new System.Drawing.Size(67, 16);
            this.rb001.TabIndex = 1;
            this.rb001.Text = "0.01単位";
            this.rb001.UseVisualStyleBackColor = true;
            this.rb001.CheckedChanged += new System.EventHandler(this.rb01_CheckedChanged);
            // 
            // rb01
            // 
            this.rb01.AutoSize = true;
            this.rb01.Checked = true;
            this.rb01.Location = new System.Drawing.Point(99, 3);
            this.rb01.Name = "rb01";
            this.rb01.Size = new System.Drawing.Size(61, 16);
            this.rb01.TabIndex = 0;
            this.rb01.TabStop = true;
            this.rb01.Text = "0.1単位";
            this.rb01.UseVisualStyleBackColor = true;
            this.rb01.CheckedChanged += new System.EventHandler(this.rb01_CheckedChanged);
            // 
            // chkLocalL
            // 
            this.chkLocalL.AutoSize = true;
            this.chkLocalL.Location = new System.Drawing.Point(141, 161);
            this.chkLocalL.Name = "chkLocalL";
            this.chkLocalL.Size = new System.Drawing.Size(108, 16);
            this.chkLocalL.TabIndex = 6;
            this.chkLocalL.Text = "ローカル座標系で";
            this.chkLocalL.UseVisualStyleBackColor = true;
            // 
            // chkLocalR
            // 
            this.chkLocalR.AutoSize = true;
            this.chkLocalR.Location = new System.Drawing.Point(144, 326);
            this.chkLocalR.Name = "chkLocalR";
            this.chkLocalR.Size = new System.Drawing.Size(108, 16);
            this.chkLocalR.TabIndex = 7;
            this.chkLocalR.Text = "ローカル座標系で";
            this.chkLocalR.UseVisualStyleBackColor = true;
            // 
            // btnOK
            // 
            this.btnOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOK.Location = new System.Drawing.Point(172, 366);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(77, 27);
            this.btnOK.TabIndex = 11;
            this.btnOK.Text = "適用";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // btnReset
            // 
            this.btnReset.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnReset.Location = new System.Drawing.Point(4, 366);
            this.btnReset.Name = "btnReset";
            this.btnReset.Size = new System.Drawing.Size(77, 27);
            this.btnReset.TabIndex = 12;
            this.btnReset.Text = "リセット";
            this.btnReset.UseVisualStyleBackColor = true;
            this.btnReset.Click += new System.EventHandler(this.btnReset_Click);
            // 
            // myConrolZ
            // 
            this.myConrolZ.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.myConrolZ.Caption = "Z:";
            this.myConrolZ.Keisuu = 0.1F;
            this.myConrolZ.Location = new System.Drawing.Point(4, 126);
            this.myConrolZ.Maximum = 500;
            this.myConrolZ.Minimum = -500;
            this.myConrolZ.Name = "myConrolZ";
            this.myConrolZ.Size = new System.Drawing.Size(248, 29);
            this.myConrolZ.TabIndex = 15;
            // 
            // myConrolY
            // 
            this.myConrolY.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.myConrolY.Caption = "Y:";
            this.myConrolY.Keisuu = 0.1F;
            this.myConrolY.Location = new System.Drawing.Point(5, 89);
            this.myConrolY.Maximum = 500;
            this.myConrolY.Minimum = -500;
            this.myConrolY.Name = "myConrolY";
            this.myConrolY.Size = new System.Drawing.Size(248, 29);
            this.myConrolY.TabIndex = 14;
            // 
            // myConrolX
            // 
            this.myConrolX.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.myConrolX.Caption = "X:";
            this.myConrolX.Keisuu = 0.1F;
            this.myConrolX.Location = new System.Drawing.Point(5, 54);
            this.myConrolX.Maximum = 500;
            this.myConrolX.Minimum = -500;
            this.myConrolX.Name = "myConrolX";
            this.myConrolX.Size = new System.Drawing.Size(248, 29);
            this.myConrolX.TabIndex = 13;
            // 
            // myConrolRZ
            // 
            this.myConrolRZ.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.myConrolRZ.Caption = "Z:";
            this.myConrolRZ.Keisuu = 1F;
            this.myConrolRZ.Location = new System.Drawing.Point(5, 289);
            this.myConrolRZ.Maximum = 180;
            this.myConrolRZ.Minimum = -180;
            this.myConrolRZ.Name = "myConrolRZ";
            this.myConrolRZ.Size = new System.Drawing.Size(251, 31);
            this.myConrolRZ.TabIndex = 10;
            // 
            // myConrolRY
            // 
            this.myConrolRY.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.myConrolRY.Caption = "Y:";
            this.myConrolRY.Keisuu = 1F;
            this.myConrolRY.Location = new System.Drawing.Point(5, 252);
            this.myConrolRY.Maximum = 180;
            this.myConrolRY.Minimum = -180;
            this.myConrolRY.Name = "myConrolRY";
            this.myConrolRY.Size = new System.Drawing.Size(251, 31);
            this.myConrolRY.TabIndex = 9;
            // 
            // myConrolRX
            // 
            this.myConrolRX.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.myConrolRX.Caption = "X:";
            this.myConrolRX.Keisuu = 1F;
            this.myConrolRX.Location = new System.Drawing.Point(5, 215);
            this.myConrolRX.Maximum = 180;
            this.myConrolRX.Minimum = -180;
            this.myConrolRX.Name = "myConrolRX";
            this.myConrolRX.Size = new System.Drawing.Size(251, 31);
            this.myConrolRX.TabIndex = 8;
            // 
            // lblCaption
            // 
            this.lblCaption.AutoSize = true;
            this.lblCaption.Font = new System.Drawing.Font("メイリオ", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.lblCaption.Location = new System.Drawing.Point(2, 345);
            this.lblCaption.Name = "lblCaption";
            this.lblCaption.Size = new System.Drawing.Size(138, 18);
            this.lblCaption.TabIndex = 16;
            this.lblCaption.Text = "選択中ボーン数/キー数:";
            // 
            // lblCount
            // 
            this.lblCount.AutoSize = true;
            this.lblCount.Font = new System.Drawing.Font("メイリオ", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.lblCount.Location = new System.Drawing.Point(138, 345);
            this.lblCount.Name = "lblCount";
            this.lblCount.Size = new System.Drawing.Size(27, 18);
            this.lblCount.TabIndex = 17;
            this.lblCount.Text = "0/0";
            // 
            // frmMainMMM
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(253, 396);
            this.Controls.Add(this.lblCount);
            this.Controls.Add(this.lblCaption);
            this.Controls.Add(this.myConrolZ);
            this.Controls.Add(this.myConrolY);
            this.Controls.Add(this.myConrolX);
            this.Controls.Add(this.btnReset);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.myConrolRZ);
            this.Controls.Add(this.myConrolRY);
            this.Controls.Add(this.myConrolRX);
            this.Controls.Add(this.chkLocalR);
            this.Controls.Add(this.chkLocalL);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmMainMMM";
            this.Text = "frmMainMMM";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.frmMainMMM_FormClosed);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.RadioButton rb1;
        private System.Windows.Forms.RadioButton rb001;
        private System.Windows.Forms.RadioButton rb01;
        private System.Windows.Forms.CheckBox chkLocalL;
        private System.Windows.Forms.CheckBox chkLocalR;
        private MyConrol myConrolRZ;
        private MyConrol myConrolRY;
        private MyConrol myConrolRX;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button btnReset;
        private MyConrol myConrolX;
        private MyConrol myConrolY;
        private MyConrol myConrolZ;
        private System.Windows.Forms.Label lblCaption;
        private System.Windows.Forms.Label lblCount;
    }
}