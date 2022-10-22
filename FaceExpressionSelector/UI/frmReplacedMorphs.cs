using FaceExpressionHelper.UI.UserControls;
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

        /// <summary>
        /// 選択中の表情セット
        /// </summary>

        private ExpressionSet _exSet = null;

        /// <summary>
        /// 置換情報
        /// </summary>
        private ReplacedMorphNameItem _replacedItem = null;

        /// <summary>
        /// 帰り値
        /// </summary>
        public List<ReplacedMorphSet> Result { get; private set; }

        /// <summary>
        /// モーフが選択された時に走るイベント
        /// </summary>
        private EventHandler<MorphSelectedEventArgs> _morphselectedHandler = null;

        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="modelName">選択中モデル名</param>
        /// <param name="allMorphs">選択中モデルの全モーフ</param>
        /// <param name="allExceptionMorphs">対象外モーフ一覧</param>
        public frmReplacedMorphs(string modelName, List<MorphItem> allMorphs, Args args, ExpressionSet exSet, ReplacedMorphNameItem replacedItem, EventHandler<MorphSelectedEventArgs> morphselectedHandler)
        {
            InitializeComponent();
            this._modelName = modelName;
            this._allMorphs = allMorphs;
            this._replacedItem = replacedItem;
            this._args = args;
            this._exSet = exSet;
            this._morphselectedHandler = morphselectedHandler;

            this.chkApplyMorph.Checked = true;
        }

        /// <summary>
        /// モーフ選択したときにはしるメソッド
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnMorphSelected(MorphSelectedEventArgs e)
        {
            this.CheckMissing();
            if (!this.chkApplyMorph.Checked)
                return;
            if (!this.Enabled)
                return;
            this._morphselectedHandler?.Invoke(this, e);
        }

        private void CreateControls()
        {
            this.Text = $"「{this._modelName}」のモーフ置換設定";
            this.lblTitle.Text = this.Text;
            var validMorphs = this._exSet.Items.SelectMany(n => n.MorphItems)
                                    .GroupBy(n => n.MortphNameWithType)
                                    .Select(n => n.FirstOrDefault()).ToList();

            this.pnlBody.Visible = false;
            this.pnlBody.SuspendLayout();
            try
            {
                this.pnlBody.Controls.Clear();
                foreach (MorphItem morph in validMorphs.OrderByDescending(n => n.MorphType)
                                                        .ThenByDescending(n => n.MorphName))
                {
                    var isMissing = false;
                    if (!this._allMorphs.Any(n => n.MorphName == morph.MorphName))
                        isMissing = true;
                    var replacedctr = new ReplaceMorphCtr();
                    replacedctr.Visible = false;
                    replacedctr.Dock = DockStyle.Top;
                    var rp = this._replacedItem.ReplacedMorphSetList.Where(n => n.MorphName == morph.MorphName).FirstOrDefault();
                    replacedctr.Initialize(this._modelName, morph, rp, isMissing, this._allMorphs, this.OnMorphSelected);
                    this.pnlBody.Controls.Add(replacedctr);
                    this.pnlBody.BringToFront();
                    replacedctr.Visible = true;
                }
                this.CheckMissing();
            }
            finally
            {
                this.pnlBody.AutoScrollPosition = new Point(0, 0);
                this.pnlBody.Visible = true;
                this.pnlBody.ResumeLayout();
            }
        }

        private void CheckMissing()
        {
            var count = 0;
            foreach (Control ctrl in this.pnlBody.Controls)
            {
                if (ctrl is ReplaceMorphCtr replacedctr)
                    if (replacedctr.IsMissing)
                        count++;
            }

            if (count == 0)
                this.lblRemaining.Visible = false;
            else
            {
                this.lblRemaining.Visible = true;
                this.lblRemaining.Text = $"未処理モーフ {count}件";
            }
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            this.Result = new List<ReplacedMorphSet>();
            if (sender == this.btnOK)
            {
                foreach (Control ctrl in this.pnlBody.Controls)
                {
                    if (ctrl is ReplaceMorphCtr replacedctr)
                    {
                        if (replacedctr.Result != null)
                            this.Result.Add(replacedctr.Result);
                    }
                }

                this._morphselectedHandler?.Invoke(this, new MorphSelectedEventArgs(this._modelName, ""));
                this.DialogResult = DialogResult.OK;
            }
            this.Close();
        }

        private void frmReplacedMorphs_Load(object sender, EventArgs e)
        {
            this.CreateControls();
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show($"すべての置換設定をクリアします。\r\n\r\nよろしいですか？", "確認", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.No)
                return;

            this.BeginAndEndUpdate(false);
            foreach (Control ctrl in this.pnlBody.Controls)
            {
                if (ctrl is ReplaceMorphCtr replacedctr)
                {
                    replacedctr.Reset();
                }
            }
            this.BeginAndEndUpdate(true);
        }

        /// <summary>
        /// 実績から引用
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnQuote_Click(object sender, EventArgs e)
        {
            this.Enabled = false;
            this.Cursor = Cursors.WaitCursor;
            try
            {
                //ReplaceMorphCtrのハッシュを作る
                var ctrHash = new Dictionary<string, ReplaceMorphCtr>();
                foreach (Control ctrl in this.pnlBody.Controls)
                {
                    if (ctrl is ReplaceMorphCtr replacedctr)
                    {
                        if (!ctrHash.ContainsKey(replacedctr.MorphName))
                            ctrHash.Add(replacedctr.MorphName, replacedctr);
                    }
                }

                //他のセットでの今のモデルの置換セット
                var myModelsReplacedNameItemList = new List<ReplacedMorphNameItem>();
                //全セットにわたっての他のモデルの置換セット
                var otherModelsReplacedNameItemList = new List<ReplacedMorphNameItem>();
                foreach (var otherset in this._args.ExpressionSets.Where(n => n.Name != this._exSet.Name))
                {
                    var myReplacedNameItem = otherset.ReplacedMorphs.Where(n => n.ModelName == this._modelName).FirstOrDefault();
                    if (myReplacedNameItem != null)
                        myModelsReplacedNameItemList.Add(myReplacedNameItem);
                }
                foreach (var set in this._args.ExpressionSets)
                {
                    var myReplacedNameItems = set.ReplacedMorphs.Where(n => n.ModelName != this._modelName).ToArray();
                    if (myReplacedNameItems != null)
                        otherModelsReplacedNameItemList.AddRange(myReplacedNameItems);
                }

                //自モデルの置換設定があるなら値ごと引用する
                foreach (var replacedName in myModelsReplacedNameItemList)
                {
                    foreach (var replacedset in replacedName.ReplacedMorphSetList)
                    {
                        if (ctrHash.ContainsKey(replacedset.MorphName))
                        {
                            var replacedctrl = ctrHash[replacedset.MorphName];

                            //要置換
                            replacedctrl.ApplyValue(replacedset, true);
                        }
                    }
                }

                //他モデルのち缶設定があるなら値以外を引用する
                foreach (var replacedName in otherModelsReplacedNameItemList)
                {
                    foreach (var replacedset in replacedName.ReplacedMorphSetList)
                    {
                        if (ctrHash.ContainsKey(replacedset.MorphName))
                        {
                            var replacedctrl = ctrHash[replacedset.MorphName];
                            if (replacedctrl.IsMissing && replacedset.ReplacedItems.Count == 1)
                            {
                                //要置換
                                replacedctrl.ApplyValue(replacedset, false);
                            }
                        }
                    }
                }
            }
            finally
            {
                this.Enabled = true;
                this.Cursor = Cursors.Default;
            }
        }
    }
}