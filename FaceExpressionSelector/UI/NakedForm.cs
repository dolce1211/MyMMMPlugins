using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace FaceExpressionHelper
{
    public class NakedForm : Form
    {
        private const int WS_BORDER = 0x00800000;
        private const int CS_DROPSHADOW = 0x00020000;

        protected override CreateParams CreateParams
        {
            get
            {
                //枠だけのフォーム
                CreateParams cp = base.CreateParams;

                if (this.FormBorderStyle != FormBorderStyle.None)
                {
                    cp.Style = cp.Style & (~WS_BORDER);
                }
                return cp;
            }
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();
            //
            // NakedForm
            //
            this.ClientSize = new System.Drawing.Size(134, 111);

            this.Name = "NakedForm";
            this.ResumeLayout(false);
        }
    }
}