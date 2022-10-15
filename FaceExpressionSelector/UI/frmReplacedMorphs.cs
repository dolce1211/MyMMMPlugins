﻿using FaceExpressionHelper.UI.UserControls;
using MikuMikuPlugin;
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
    public partial class frmReplacedMorphs : Form
    {
        private string _modelName = string.Empty;

        private List<MorphItem> _allMorphs = null;

        /// <summary>
        /// 有効な全モーフ
        /// </summary>
        private Args _args = null;

        private ReplacedMorphNameItem _replacedItem = null;

        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="modelName">選択中モデル名</param>
        /// <param name="allMorphs">選択中モデルの全モーフ</param>
        /// <param name="allExceptionMorphs">対象外モーフ一覧</param>
        public frmReplacedMorphs(string modelName, List<MorphItem> allMorphs, Args args, ReplacedMorphNameItem replacedItem)
        {
            InitializeComponent();
            this._modelName = modelName;
            this._allMorphs = allMorphs;
            this._replacedItem = replacedItem;
            this._args = args;
            this.CreateControls();
        }

        private void CreateControls()
        {
            this.Text = $"「{this._modelName}」のモーフ置換情報";
            this.lblTitle.Text = this.Text;
            var validMorphs = this._args.Items.SelectMany(n => n.MorphItems)
                                    .GroupBy(n => n.MortphNameWithType)
                                    .Select(n => n.FirstOrDefault()).ToList();

            this.pnlBody.Visible = false;
            try
            {
                this.pnlBody.Controls.Clear();
                foreach (MorphItem morph in validMorphs.OrderByDescending(n => n.MorphType)
                                                        .ThenBy(n => n.MorphName))
                {
                    var isMissing = false;
                    if (!this._allMorphs.Any(n => n.MorphName == morph.MorphName))
                        isMissing = true;
                    var replacedctr = new ReplaceMorphCtr();
                    replacedctr.Visible = false;
                    replacedctr.Dock = DockStyle.Top;
                    replacedctr.Initialize(morph, isMissing, this._allMorphs);
                    this.pnlBody.Controls.Add(replacedctr);
                    replacedctr.Visible = true;
                }
            }
            finally
            {
                this.pnlBody.Visible = true;
            }
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (sender == this.btnOK)
            {
                this.DialogResult = DialogResult.OK;
            }
            this.Close();
        }

        private void frmReplacedMorphs_Load(object sender, EventArgs e)
        {
        }
    }
}