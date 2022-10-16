namespace FaceExpressionHelper.UI
{
    partial class frmName
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
            this.btnOK = new System.Windows.Forms.Button();
            this.txtName = new System.Windows.Forms.TextBox();
            this.btnCancel = new System.Windows.Forms.Button();
            this.lstMorphs = new FaceExpressionHelper.UI.UserControls.MorphListBox();
            this.SuspendLayout();
            // 
            // btnOK
            // 
            this.btnOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOK.Location = new System.Drawing.Point(223, 33);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(85, 30);
            this.btnOK.TabIndex = 1;
            this.btnOK.Text = "確定";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // txtName
            // 
            this.txtName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtName.ImeMode = System.Windows.Forms.ImeMode.On;
            this.txtName.Location = new System.Drawing.Point(7, 9);
            this.txtName.Name = "txtName";
            this.txtName.Size = new System.Drawing.Size(392, 19);
            this.txtName.TabIndex = 0;
            this.txtName.Text = "あいうえお";
            this.txtName.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtName_KeyDown);
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.Location = new System.Drawing.Point(314, 33);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(85, 30);
            this.btnCancel.TabIndex = 2;
            this.btnCancel.Text = "キャンセル";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // lstMorphs
            // 
            this.lstMorphs.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.lstMorphs.BackColor = System.Drawing.SystemColors.Control;
            this.lstMorphs.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.lstMorphs.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.lstMorphs.FormattingEnabled = true;
            this.lstMorphs.IntegralHeight = false;
            this.lstMorphs.Location = new System.Drawing.Point(9, 33);
            this.lstMorphs.Name = "lstMorphs";
            this.lstMorphs.Size = new System.Drawing.Size(207, 200);
            this.lstMorphs.TabIndex = 23;
            // 
            // frmName
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(411, 240);
            this.ControlBox = false;
            this.Controls.Add(this.lstMorphs);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.txtName);
            this.Controls.Add(this.btnCancel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            this.Name = "frmName";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "表情名称";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.TextBox txtName;
        private System.Windows.Forms.Button btnCancel;
        private UserControls.MorphListBox lstMorphs;
    }
}