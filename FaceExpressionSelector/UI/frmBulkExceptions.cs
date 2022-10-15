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
    public partial class frmBulkExceptions : Form
    {
        private List<string> _allExceptionMorphs = new List<string>();

        private List<string> _allMorphs = new List<string>();

        private string _modelName = string.Empty;

        private List<string> _selected = new List<string>();

        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="modelName">選択中モデル名</param>
        /// <param name="allMorphs">選択中モデルの全モーフ</param>
        /// <param name="allExceptionMorphs">対象外モーフ一覧</param>
        public frmBulkExceptions(string modelName, List<string> allMorphs, List<string> allExceptionMorphs)
        {
            InitializeComponent();
            this._modelName = modelName;
            this._allMorphs = allMorphs;
            this._allExceptionMorphs = allExceptionMorphs;

            this._selected = this._allMorphs.Where(n => this._allExceptionMorphs.Contains(n)).ToList();

            this.CreateListBox();
        }

        /// <summary>
        /// listbox再作成
        /// </summary>
        private void CreateListBox()
        {
            this.listBox1.BeginAndEndUpdate(false);
            this.listBox2.BeginAndEndUpdate(false);
            try
            {
                this.listBox1.Items.Clear();
                this.listBox2.Items.Clear();
                foreach (string morph in this._allMorphs)
                {
                    if (this._selected.Contains(morph))
                        //右
                        this.listBox2.Items.Add(morph);
                    else
                        //左
                        this.listBox1.Items.Add(morph);
                }
            }
            finally
            {
                this.listBox1.BeginAndEndUpdate(true);
                this.listBox2.BeginAndEndUpdate(true);
            }
        }

        /// <summary>
        /// 結果
        /// </summary>
        public List<string> Result { get; private set; } = new List<string>();

        private void btnAdd_Click(object sender, EventArgs e)
        {
            ListBox lstbox = null;
            if (sender == this.btnAdd)
                lstbox = this.listBox1;
            else
                lstbox = this.listBox2;

            var selectedItems = lstbox.SelectedItems.Cast<string>().ToList();
            if (selectedItems?.Count == 0)
                return;

            if (sender == this.btnAdd)
            {
                this._selected.AddRange(selectedItems);
            }
            else
            {
                selectedItems.ForEach(n => this._selected.Remove(n));
            }
            this.CreateListBox();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            this.Result = new List<string>();
            if (sender == this.btnOK)
            {
                this.Result.AddRange(this._selected);
                this.DialogResult = DialogResult.OK;
            }
            this.Close();
        }
    }
}