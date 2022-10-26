using MyUtility;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AutoBlinkerPlugin
{
    public partial class frmEye : Form
    {
        public frmEye()
        {
            InitializeComponent();
        }

        public void Initialize(float[] defvalue)
        {
            if (defvalue.Length < 2)
                defvalue = new float[] { 1f, 10f };

            this.txtUp.Text = defvalue[0].ToString();
            this.txtDown.Text = defvalue[1].ToString();
        }

        public float[] Result { get; private set; }

        private void btnOK_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Result = new float[] { 1f, 10f };
            if (sender == this.btnOK)
            {
                this.Result = new float[]
                {
                    this.txtUp.Text.ToFloat(),
                    this.txtDown.Text.ToFloat()
                };
                this.DialogResult = DialogResult.OK;
            }
            this.Close();
        }

        private void frmEye_Load(object sender, EventArgs e)
        {
            this.txtUp.LimitInputToNum(true, true);
            this.txtDown.LimitInputToNum(true, true);
        }
    }
}