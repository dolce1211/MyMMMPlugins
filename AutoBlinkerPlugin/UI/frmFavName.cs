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
    public partial class frmFavName : Form
    {
        private SavedState _savedState;

        public frmFavName(SavedState savedstate)
        {
            InitializeComponent();

            this._savedState = savedstate;
        }

        public string Result { get; set; }

        private void btnOK_Click(object sender, EventArgs e)
        {
            this.Result = string.Empty;

            if (sender == this.btnOK)
            {
                var ret = this.txtName.Text;
                if (string.IsNullOrWhiteSpace(ret))
                {
                    MessageBox.Show("名称を入力してください");
                    return;
                }

                if (_savedState.Favorites.Where(n => n != null).Any(n => n.FavName == ret))
                {
                    if (MessageBox.Show(this, $"この名前のお気に入りは既に存在します。\r\n上書きしますか？"
                        , "確認", MessageBoxButtons.YesNo
                        , MessageBoxIcon.Question
                        , MessageBoxDefaultButton.Button2) == DialogResult.No)
                        return;
                }
                this.Result = ret;
                this.DialogResult = DialogResult.OK;
            }
            else
            {
                this.DialogResult = DialogResult.Cancel;
            }
            this.Close();
        }
    }
}