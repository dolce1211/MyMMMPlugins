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

namespace FaceExpressionHelper.UI
{
    public partial class frmName : Form
    {
        private Args _args = null;

        private ExpressionItem _currentItem = null;

        /// <summary>
        /// constructors
        /// </summary>
        /// <param name="args"></param>
        /// <param name="currentItem">編集時の編集対象</param>
        /// <param name="currentMorphs">現在の表情</param>
        /// <param name="prevName"></param>
        public frmName(ListControlConvertEventHandler listboxFormatHandler, Args args, ExpressionItem currentItem, List<MorphItem> currentMorphs, string prevName)
        {
            InitializeComponent();
            this._args = args;
            this._currentItem = currentItem;
            this.txtName.Text = prevName;

            this.lstValue.Items.Clear();
            if (currentMorphs != null)
                this.lstValue.Items.AddRange(currentMorphs.ToArray());

            this.lstValue.Format += listboxFormatHandler;
            this.lstValue.Disposed += (s, e) => this.lstValue.Format -= listboxFormatHandler;
        }

        public string Result { get; private set; } = string.Empty;

        private void btnOK_Click(object sender, EventArgs e)
        {
            this.Result = string.Empty;
            if (sender == this.btnOK)
            {
                //ファイル名に使用できない文字を取得
                char[] invalidChars = System.IO.Path.GetInvalidFileNameChars();
                if (this.txtName.Text.IndexOfAny(invalidChars) >= 0)
                {
                    MessageBox.Show("ファイル名に使用できない文字が使われています");
                    return;
                }

                if (this._args.Items.Where(n => n != this._currentItem).Any(n => n.Name.ToLower().TrimSafe() == this.txtName.Text.ToLower().TrimSafe()))
                {
                    MessageBox.Show("この表情名は存在します");
                    return;
                }
                this.Result = this.txtName.Text;
                this.DialogResult = DialogResult.OK;
            }
            this.Close();
        }

        private void txtName_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Return)
                this.btnOK.PerformClick();
        }
    }
}