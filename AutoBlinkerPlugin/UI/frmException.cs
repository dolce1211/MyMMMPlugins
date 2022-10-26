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
    public partial class frmException : Form
    {
        public frmException(string exceptions)
        {
            InitializeComponent();

            this.lstException.Items.Clear();
            var array = exceptions.Split(',');
            this.lstException.Items.AddRange(array);
        }

        public string Result { get; set; }

        private void btnOK_Click(object sender, EventArgs e)
        {
            this.Result = string.Empty;
            if (sender == this.btnOK)
            {
                var ret = string.Empty;
                foreach (var item in this.lstException.Items)
                {
                    if (!string.IsNullOrWhiteSpace(ret))
                        ret += ',';
                    ret += item;
                }

                this.Result = ret;
                this.DialogResult = DialogResult.OK;
            }
            else
            {
                this.DialogResult = DialogResult.Cancel;
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            var itm = this.txtUp.Text.TrimSafe();
            if (string.IsNullOrWhiteSpace(itm))
                return;

            if (this.lstException.Items.Contains(itm))
            {
                MessageBox.Show("このモーフは既に登録されています");
                return;
            }
            this.lstException.Items.Add(itm);
            this.txtUp.Clear();
            this.txtUp.Focus();
        }

        private void 削除ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (this.lstException.SelectedItem == null)
                return;
            this.lstException.Items.Remove(this.lstException.SelectedItem);
        }

        private void txtUp_KeyPress_1(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                this.btnAdd.PerformClick();
            }
        }
    }
}