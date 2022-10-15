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

namespace FaceExpressionHelper.UI.UserControls
{
    public partial class ReplaceMorphCtr : UserControl
    {
        private List<MorphItem> _allMorphs = new List<MorphItem>();

        public ReplaceMorphCtr()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 元のモーフ名
        /// </summary>
        public string MorphName => this.lblActiveModel.Text;

        /// <summary>
        /// 置換後のモーフ名
        /// </summary>
        public string ReplacedMorphName => this.comboBox1.Text;

        /// <summary>
        /// 初期化
        /// </summary>
        /// <param name="morph">モーフ名</param>
        /// <param name="isMissing">不足モーフならtrue</param>
        /// <param name="allMorphs">全モーフ</param>
        public void Initialize(MorphItem morph, bool isMissing, List<MorphItem> allMorphs)
        {
            this.lblActiveModel.Text = morph.MortphNameWithType;
            this.lblMissing.Visible = isMissing;

            this._allMorphs.AddRange(allMorphs);
            this.comboBox1.Items.Clear();
            this.comboBox1.Items.Add("");
            this.comboBox1.Items.Add("【無視する】");
            this.comboBox1.Items.AddRange(allMorphs.Where(n => n.MorphType == morph.MorphType).ToArray());
        }

        private void comboBox1_Format(object sender, ListControlConvertEventArgs e)
        {
            if (!this.comboBox1.Visible)
                return;
            var morphItem = e.Value as MorphItem;
            if (morphItem != null)
                e.Value = morphItem.MortphNameWithType;
        }
    }
}