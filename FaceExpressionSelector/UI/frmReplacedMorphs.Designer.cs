namespace FaceExpressionHelper.UI
{
    partial class frmReplacedMorphs
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
            this.lblTitle = new System.Windows.Forms.Label();
            this.pnlBody = new MMDUtil.FixedPanel();
            this.replaceMorphCtr1 = new FaceExpressionHelper.UI.UserControls.ReplaceMorphCtr();
            this.btnOK = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnQuote = new System.Windows.Forms.Button();
            this.chkApplyMorph = new System.Windows.Forms.CheckBox();
            this.btnClear = new System.Windows.Forms.Button();
            this.pnlBody.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblTitle
            // 
            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new System.Drawing.Font("MS UI Gothic", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.lblTitle.Location = new System.Drawing.Point(12, 8);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(144, 15);
            this.lblTitle.TabIndex = 10;
            this.lblTitle.Text = "****のモーフ置換設定";
            // 
            // pnlBody
            // 
            this.pnlBody.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pnlBody.AutoScroll = true;
            this.pnlBody.Controls.Add(this.replaceMorphCtr1);
            this.pnlBody.Location = new System.Drawing.Point(6, 31);
            this.pnlBody.Name = "pnlBody";
            this.pnlBody.Size = new System.Drawing.Size(590, 366);
            this.pnlBody.TabIndex = 11;
            // 
            // replaceMorphCtr1
            // 
            this.replaceMorphCtr1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.replaceMorphCtr1.Dock = System.Windows.Forms.DockStyle.Top;
            this.replaceMorphCtr1.Location = new System.Drawing.Point(0, 0);
            this.replaceMorphCtr1.Name = "replaceMorphCtr1";
            this.replaceMorphCtr1.Size = new System.Drawing.Size(590, 28);
            this.replaceMorphCtr1.TabIndex = 0;
            // 
            // btnOK
            // 
            this.btnOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOK.Location = new System.Drawing.Point(413, 407);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(85, 30);
            this.btnOK.TabIndex = 12;
            this.btnOK.Text = "確定";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.Location = new System.Drawing.Point(504, 407);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(85, 30);
            this.btnCancel.TabIndex = 13;
            this.btnCancel.Text = "キャンセル";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // btnQuote
            // 
            this.btnQuote.Location = new System.Drawing.Point(481, 6);
            this.btnQuote.Name = "btnQuote";
            this.btnQuote.Size = new System.Drawing.Size(115, 23);
            this.btnQuote.TabIndex = 14;
            this.btnQuote.Text = "実績より引用";
            this.btnQuote.UseVisualStyleBackColor = true;
            // 
            // chkApplyMorph
            // 
            this.chkApplyMorph.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.chkApplyMorph.AutoSize = true;
            this.chkApplyMorph.Location = new System.Drawing.Point(6, 412);
            this.chkApplyMorph.Name = "chkApplyMorph";
            this.chkApplyMorph.Size = new System.Drawing.Size(184, 16);
            this.chkApplyMorph.TabIndex = 15;
            this.chkApplyMorph.Text = "選択したモーフの適用結果を反映";
            this.chkApplyMorph.UseVisualStyleBackColor = true;
            // 
            // btnClear
            // 
            this.btnClear.Location = new System.Drawing.Point(413, 6);
            this.btnClear.Name = "btnClear";
            this.btnClear.Size = new System.Drawing.Size(54, 23);
            this.btnClear.TabIndex = 16;
            this.btnClear.Text = "クリア";
            this.btnClear.UseVisualStyleBackColor = true;
            this.btnClear.Click += new System.EventHandler(this.btnClear_Click);
            // 
            // frmReplacedMorphs
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(601, 444);
            this.ControlBox = false;
            this.Controls.Add(this.btnClear);
            this.Controls.Add(this.chkApplyMorph);
            this.Controls.Add(this.btnQuote);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.pnlBody);
            this.Controls.Add(this.lblTitle);
            this.Name = "frmReplacedMorphs";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "****のモーフ置換情報";
            this.Load += new System.EventHandler(this.frmReplacedMorphs_Load);
            this.pnlBody.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        protected System.Windows.Forms.Label lblTitle;
        private MMDUtil.FixedPanel pnlBody;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button btnCancel;
        private UserControls.ReplaceMorphCtr replaceMorphCtr1;
        private System.Windows.Forms.Button btnQuote;
        private System.Windows.Forms.CheckBox chkApplyMorph;
        private System.Windows.Forms.Button btnClear;
    }
}