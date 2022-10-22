using MyUtility;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;

namespace FaceExpressionHelper.UI
{
    public partial class frmEditSet : Form
    {
        private Args _args = null;
        private ExpressionSet _exSet = null;

        public frmEditSet(Args args, ExpressionSet exSet)
        {
            InitializeComponent();
            this._args = args;
            this._exSet = exSet;

            if (this._exSet != null)
                //編集
                this.txtName.Text = this._exSet.Name.TrimSafe();
            else
            {
                //新規
                this.txtName.Text = String.Empty;
                //新規のときは削除ボタン不要
                this.btnDelete.Visible = false;
                //新規のときはフォルダ開くボタン不要
                this.btnOpenFolder.Visible = false;
            }
        }

        public string Result { get; private set; }

        public bool DeleteFlg { get; private set; }

        private void txtName_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Return)
                this.btnOK.PerformClick();
        }

        private void btnOpenFolder_Click(object sender, EventArgs e)
        {
            var dir = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
            var baseDir = System.IO.Path.Combine(dir, "faceExpressions");
            var setdir = System.IO.Path.Combine(baseDir, this._exSet.Name);
            if (System.IO.Directory.Exists(setdir))
            {
                System.Diagnostics.Process.Start(setdir);
            }
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            this.DeleteFlg = false;
            this.Result = string.Empty;

            if (sender == this.btnOK)
            {
                //確定
                //ファイル名に使用できない文字を取得
                char[] invalidChars = System.IO.Path.GetInvalidFileNameChars();
                if (this.txtName.Text.IndexOfAny(invalidChars) >= 0)
                {
                    MessageBox.Show("フォルダ名に使用できない文字が使われています");
                    return;
                }
                if (this._args.ExpressionSets.Where(n => n != this._exSet).Any(n => n.Name.ToLower().TrimSafe() == this.txtName.Text.ToLower().TrimSafe()))
                {
                    MessageBox.Show("この表情セット名は存在します");
                    return;
                }

                this.Result = this.txtName.Text.TrimSafe();
                this.DialogResult = DialogResult.OK;
            }
            else if (sender == this.btnDelete)
            {
                //削除
                if (this._args.ExpressionSets.Count <= 1)
                {
                    MessageBox.Show("表情セットをすべて削除することは出来ません");
                    return;
                }
                var dirname = System.IO.Path.Combine(System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location), this._exSet.Name);
                var msg = $@"{dirname}

以上のフォルダを削除します。
この操作は取り消すことは出来ません。

よろしいですか？";
                if (MessageBox.Show(msg, "確認", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button2) == DialogResult.No)
                    return;

                this.DeleteFlg = true;
                this.DialogResult = DialogResult.OK;
            }

            this.Close();
        }
    }
}