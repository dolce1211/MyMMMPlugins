using System.Windows.Forms;

namespace FaceExpressionHelper.UI
{
    partial class frmPicture
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
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.lstValue = new System.Windows.Forms.ListBox();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // pictureBox1
            // 
            this.pictureBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pictureBox1.InitialImage = null;
            this.pictureBox1.Location = new System.Drawing.Point(0, 0);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(250, 251);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox1.TabIndex = 3;
            this.pictureBox1.TabStop = false;
            this.pictureBox1.MouseEnter += new System.EventHandler(this.frmPicture_MouseEnter);
            this.pictureBox1.MouseLeave += new System.EventHandler(this.frmPicture_MouseLeave);
            this.pictureBox1.MouseHover += new System.EventHandler(this.pictureBox1_MouseHover);
            // 
            // lstValue
            // 
            this.lstValue.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lstValue.BackColor = System.Drawing.SystemColors.Control;
            this.lstValue.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.lstValue.Font = new System.Drawing.Font("MS UI Gothic", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.lstValue.FormattingEnabled = true;
            this.lstValue.IntegralHeight = false;
            this.lstValue.ItemHeight = 12;
            this.lstValue.Items.AddRange(new object[] {
            "あああ",
            "いいい",
            "あああ",
            "いいい",
            "あああ",
            "いいい",
            "あああ",
            "いいい",
            "あああ",
            "いいい"});
            this.lstValue.Location = new System.Drawing.Point(0, 252);
            this.lstValue.Name = "lstValue";
            this.lstValue.Size = new System.Drawing.Size(250, 70);
            this.lstValue.TabIndex = 7;
            this.lstValue.MouseEnter += new System.EventHandler(this.frmPicture_MouseEnter);
            this.lstValue.MouseLeave += new System.EventHandler(this.frmPicture_MouseLeave);
            // 
            // frmPicture
            // 
            this.AllowDrop = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(250, 323);
            this.Controls.Add(this.lstValue);
            this.Controls.Add(this.pictureBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.KeyPreview = true;
            this.Name = "frmPicture";
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "frmPicture";
            this.TopMost = true;
            this.VisibleChanged += new System.EventHandler(this.frmPicture_VisibleChanged);
            this.MouseEnter += new System.EventHandler(this.frmPicture_MouseEnter);
            this.MouseLeave += new System.EventHandler(this.frmPicture_MouseLeave);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private PictureBox pictureBox1;
        private ListBox lstValue;
    }
}