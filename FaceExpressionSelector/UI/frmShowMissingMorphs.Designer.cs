namespace FaceExpressionHelper.UI
{
    partial class frmShowMissingMorphs
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
            this.lblMsg = new System.Windows.Forms.Label();
            this.btnOK = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnReplace = new System.Windows.Forms.Button();
            this.lstMissingMorph = new System.Windows.Forms.ListBox();
            this.SuspendLayout();
            // 
            // lblMsg
            // 
            this.lblMsg.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblMsg.AutoSize = true;
            this.lblMsg.Font = new System.Drawing.Font("メイリオ", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.lblMsg.Location = new System.Drawing.Point(12, 91);
            this.lblMsg.Name = "lblMsg";
            this.lblMsg.Size = new System.Drawing.Size(339, 40);
            this.lblMsg.TabIndex = 20;
            this.lblMsg.Text = "{activeModel.Name}には以上のモーフがありません。\r\n続行しますか？\r\n";
            // 
            // btnOK
            // 
            this.btnOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOK.Location = new System.Drawing.Point(143, 136);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(85, 30);
            this.btnOK.TabIndex = 21;
            this.btnOK.Text = "続行";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.Location = new System.Drawing.Point(234, 136);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(85, 30);
            this.btnCancel.TabIndex = 22;
            this.btnCancel.Text = "キャンセル";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // btnReplace
            // 
            this.btnReplace.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnReplace.Location = new System.Drawing.Point(325, 136);
            this.btnReplace.Name = "btnReplace";
            this.btnReplace.Size = new System.Drawing.Size(126, 30);
            this.btnReplace.TabIndex = 23;
            this.btnReplace.Text = "モーフ置換設定を開く";
            this.btnReplace.UseVisualStyleBackColor = true;
            this.btnReplace.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // lstMissingMorph
            // 
            this.lstMissingMorph.BackColor = System.Drawing.SystemColors.Control;
            this.lstMissingMorph.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.lstMissingMorph.Font = new System.Drawing.Font("MS UI Gothic", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.lstMissingMorph.FormattingEnabled = true;
            this.lstMissingMorph.IntegralHeight = false;
            this.lstMissingMorph.ItemHeight = 12;
            this.lstMissingMorph.Items.AddRange(new object[] {
            "あああ",
            "いいい"});
            this.lstMissingMorph.Location = new System.Drawing.Point(16, 3);
            this.lstMissingMorph.Name = "lstMissingMorph";
            this.lstMissingMorph.Size = new System.Drawing.Size(205, 85);
            this.lstMissingMorph.TabIndex = 24;
            // 
            // frmShowMissingMorphs
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(463, 178);
            this.ControlBox = false;
            this.Controls.Add(this.lstMissingMorph);
            this.Controls.Add(this.btnReplace);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.lblMsg);
            this.Name = "frmShowMissingMorphs";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "不足モーフあり";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        public System.Windows.Forms.Label lblMsg;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnReplace;
        private System.Windows.Forms.ListBox lstMissingMorph;
    }
}