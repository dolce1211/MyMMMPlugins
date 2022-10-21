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
using System.Windows.Interop;
using System.Windows.Media;

namespace FaceExpressionHelper.UI
{
    public partial class frmExceptions : Form
    {
        /// <summary>
        /// 0:対象外の目まゆリップモーフ 1:対象のその他モーフ
        /// </summary>
        private int _mode = 0;

        private List<string> _allTargetMorphs = new List<string>();

        private List<string> _allMorphs = new List<string>();

        private string _modelName = string.Empty;

        /// <summary>
        /// モーフが選択された時に走るイベント
        /// </summary>
        private EventHandler<MorphSelectedEventArgs> _morphselectedHandler = null;

        /// <summary>
        /// アクティブモデルが変わった
        /// </summary>
        /// <param name="modelName"></param>
        /// <param name="allMorphs"></param>
        public void ActiveModelChanged(string modelName, List<string> allMorphs)
        {
            this._modelName = modelName;
            this._allMorphs = allMorphs;
        }

        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="mode">0:対象外の目まゆリップモーフ 1:対象のその他モーフ</param>
        /// <param name="modelName">選択中モデル名</param>
        /// <param name="allMorphs">選択中モデルの全モーフ</param>
        /// <param name="allExceptionMorphs">対象外モーフ一覧</param>
        public frmExceptions(int mode, string modelName, List<string> allMorphs, List<string> allExceptionMorphs, EventHandler<MorphSelectedEventArgs> morphselectedHandler)
        {
            InitializeComponent();
            System.Drawing.Color backcolor = System.Drawing.Color.OrangeRed;
            var title = $"処理対象外の\r\n「目・まゆ・リップ」モーフ";
            this._mode = mode;
            if (this._mode == 1)
            {
                backcolor = System.Drawing.Color.LightGreen;
                title = $"処理対象の\r\n「その他」モーフ";
            }
            this.lblTitle.BackColor = backcolor;
            this.lblTitle.Text = title;
            this.Text = title;

            this._modelName = modelName;
            this._allMorphs = allMorphs;
            this._allTargetMorphs = allExceptionMorphs;

            this._morphselectedHandler = morphselectedHandler;

            this.CreateListBox();
        }

        /// <summary>
        /// 結果
        /// </summary>
        public List<string> Result { get; private set; } = new List<string>();

        private void btnAdd_Click(object sender, EventArgs e)
        {
            var txt = this.textBox1.Text.TrimSafe();
            if (string.IsNullOrWhiteSpace(txt))
                return;
            if (this.listBox.Items.Contains(txt))
                return;

            this.listBox.Items.Add(txt);
            this.textBox1.Text = "";
            this.textBox1.Focus();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            this.Result = new List<string>();
            if (sender == this.btnOK)
            {
                this.Result.AddRange(this.listBox.Items.Cast<string>());
                this.DialogResult = DialogResult.OK;
            }
            this.Close();
        }

        private frmBulkExceptions _frmBulkExceptions = null;

        private void btnBulk_Click(object sender, EventArgs e)
        {
            if (_frmBulkExceptions != null)
                return;

            this._frmBulkExceptions = new frmBulkExceptions(this._mode, this._modelName, this._allMorphs, this._allTargetMorphs);
            this._frmBulkExceptions.Show(this);
            this._frmBulkExceptions.FormClosed += (ss, ee) =>
            {
                if (this._frmBulkExceptions.DialogResult == DialogResult.OK)
                {
                    this._allTargetMorphs.AddRange(this._frmBulkExceptions.Result);
                    this._allTargetMorphs = this._allTargetMorphs.Distinct().ToList();
                    this.CreateListBox();
                }
                this._frmBulkExceptions = null;
            };
        }

        private void CreateListBox()
        {
            this.listBox.BeginAndEndUpdate(false);
            try
            {
                this.listBox.Items.Clear();
                this.listBox.Items.AddRange(this._allTargetMorphs.ToArray());
            }
            finally
            {
                this.listBox.BeginAndEndUpdate(true);
            }
        }

        private void listBox2_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                var index = listBox.IndexFromPoint(e.Location);
                if (index < 0)
                    return;

                listBox.SelectedItem = listBox.Items[index];
            }
            else if (e.Button == MouseButtons.Left && e.Clicks == 2)
            {
                //ダブルクリックで削除
                削除ToolStripMenuItem_Click(sender, e);
            }
        }

        private void 削除ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (this.listBox.SelectedItem == null)
                return;

            var deletingItem = this.listBox.SelectedItem.ToString();
            this._allTargetMorphs.Remove(deletingItem);
            this.CreateListBox();
        }

        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Return)
                this.btnAdd.PerformClick();
        }

        private void listBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            var morphname = this.listBox.SelectedItem.ToString();
            this._morphselectedHandler?.Invoke(this, new MorphSelectedEventArgs(this._modelName, morphname));
        }
    }
}