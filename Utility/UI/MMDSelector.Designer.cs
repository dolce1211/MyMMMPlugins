namespace MMDUtil
{
    partial class MMDSelectorControl
    {
        /// <summary> 
        /// 必要なデザイナー変数です。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// 使用中のリソースをすべてクリーンアップします。
        /// </summary>
        /// <param name="disposing">マネージド リソースを破棄する場合は true を指定し、その他の場合は false を指定します。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region コンポーネント デザイナーで生成されたコード

        /// <summary> 
        /// デザイナー サポートに必要なメソッドです。このメソッドの内容を 
        /// コード エディターで変更しないでください。
        /// </summary>
        private void InitializeComponent()
        {
            this.btnSelectMMD = new System.Windows.Forms.Button();
            this.lblMMPlus = new System.Windows.Forms.Label();
            this.lblMMD = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // btnSelectMMD
            // 
            this.btnSelectMMD.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnSelectMMD.Location = new System.Drawing.Point(4, 6);
            this.btnSelectMMD.Name = "btnSelectMMD";
            this.btnSelectMMD.Size = new System.Drawing.Size(90, 21);
            this.btnSelectMMD.TabIndex = 41;
            this.btnSelectMMD.Text = "mmd選択";
            this.btnSelectMMD.UseVisualStyleBackColor = true;
            // 
            // lblMMPlus
            // 
            this.lblMMPlus.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lblMMPlus.AutoSize = true;
            this.lblMMPlus.Font = new System.Drawing.Font("メイリオ", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.lblMMPlus.Location = new System.Drawing.Point(100, 9);
            this.lblMMPlus.Name = "lblMMPlus";
            this.lblMMPlus.Size = new System.Drawing.Size(87, 18);
            this.lblMMPlus.TabIndex = 42;
            this.lblMMPlus.Text = "MMPlus導入済";
            this.lblMMPlus.Visible = false;
            // 
            // lblMMD
            // 
            this.lblMMD.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.lblMMD.Enabled = false;
            this.lblMMD.Font = new System.Drawing.Font("メイリオ", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.lblMMD.ForeColor = System.Drawing.SystemColors.GrayText;
            this.lblMMD.Location = new System.Drawing.Point(-544, 30);
            this.lblMMD.Name = "lblMMD";
            this.lblMMD.Size = new System.Drawing.Size(894, 19);
            this.lblMMD.TabIndex = 43;
            this.lblMMD.Text = "K:\\mmd\\UserFile\\tools\\caxap式動作補助ボーン導入支援\\testMotions\\05_2.走り75L_ダッシュ_(16f_前移動60).v" +
    "md";
            this.lblMMD.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // MMDSelector
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.lblMMD);
            this.Controls.Add(this.btnSelectMMD);
            this.Controls.Add(this.lblMMPlus);
            this.Name = "MMDSelector";
            this.Size = new System.Drawing.Size(353, 50);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        protected System.Windows.Forms.Button btnSelectMMD;
        protected System.Windows.Forms.Label lblMMPlus;
        protected System.Windows.Forms.Label lblMMD;
    }
}
