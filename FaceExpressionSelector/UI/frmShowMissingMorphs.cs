using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FaceExpressionHelper.UI
{
    public partial class frmShowMissingMorphs : Form
    {
        public frmShowMissingMorphs(string activeModelName, IEnumerable<string> missingMorphs)
        {
            InitializeComponent();

            this.lblMsg.Text = $"{activeModelName}には以上のモーフがありません。\r\n続行しますか？";
            this.lstMissingMorph.Items.Clear();
            this.lstMissingMorph.Items.AddRange(missingMorphs.ToArray());
        }

        public bool OpenReplace { get; private set; } = false;

        private void btnOK_Click(object sender, EventArgs e)
        {
            this.OpenReplace = false;
            if (sender == this.btnOK)
                this.DialogResult = DialogResult.OK;
            else if (sender == this.btnReplace)
                this.OpenReplace = true;

            this.Close();
        }
    }
}