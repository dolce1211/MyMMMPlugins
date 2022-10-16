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
            this.components = new System.ComponentModel.Container();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.lstMorphs = new FaceExpressionHelper.UI.UserControls.MorphListBox();
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
            this.pictureBox1.Size = new System.Drawing.Size(246, 227);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox1.TabIndex = 3;
            this.pictureBox1.TabStop = false;
            this.pictureBox1.MouseEnter += new System.EventHandler(this.frmPicture_MouseEnter);
            this.pictureBox1.MouseLeave += new System.EventHandler(this.frmPicture_MouseLeave);
            // 
            // timer1
            // 
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // lstMorphs
            // 
            this.lstMorphs.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lstMorphs.BackColor = System.Drawing.SystemColors.Control;
            this.lstMorphs.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.lstMorphs.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.lstMorphs.FormattingEnabled = true;
            this.lstMorphs.IntegralHeight = false;
            this.lstMorphs.Location = new System.Drawing.Point(0, 229);
            this.lstMorphs.Name = "lstMorphs";
            this.lstMorphs.Size = new System.Drawing.Size(246, 67);
            this.lstMorphs.TabIndex = 22;
            this.lstMorphs.MouseEnter += new System.EventHandler(this.frmPicture_MouseEnter);
            this.lstMorphs.MouseLeave += new System.EventHandler(this.frmPicture_MouseLeave);
            // 
            // frmPicture
            // 
            this.AllowDrop = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(246, 299);
            this.Controls.Add(this.lstMorphs);
            this.Controls.Add(this.pictureBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.KeyPreview = true;
            this.Name = "frmPicture";
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "frmPicture";
            this.VisibleChanged += new System.EventHandler(this.frmPicture_VisibleChanged);
            this.MouseEnter += new System.EventHandler(this.frmPicture_MouseEnter);
            this.MouseLeave += new System.EventHandler(this.frmPicture_MouseLeave);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private PictureBox pictureBox1;
        private UserControls.MorphListBox lstMorphs;
        private Timer timer1;
    }
}