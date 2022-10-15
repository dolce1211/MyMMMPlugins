using MyUtility;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FaceExpressionHelper.UI.UserControls
{
    public partial class ReplaceMorphCtr : UserControl
    {
        private List<MorphItem> _allMorphs = new List<MorphItem>();

        private MorphItem _morphItem = null;
        private bool _isMissing = false;

        public ReplaceMorphCtr()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 元のモーフ名
        /// </summary>
        private string MorphName => this.lblActiveModel.Text;

        /// <summary>
        /// 置換後のモーフ名
        /// </summary>
        private string ReplacedMorphName
        {
            get
            {
                if (comboBox1.SelectedItem == null)
                    return string.Empty;
                if (comboBox1.SelectedItem is MorphItem mi)
                {
                    return mi.MorphName;
                }
                else
                {
                    return comboBox1.Text;
                }
            }
        }

        /// <summary>
        /// 帰り値
        /// </summary>
        public ReplacedItem Result
        {
            get
            {
                if (!string.IsNullOrWhiteSpace(this.ReplacedMorphName))
                {
                    var ignore = false;
                    if (this.ReplacedMorphName == "【無視する】")
                        ignore = true;

                    return new ReplacedItem()
                    {
                        Ignore = ignore,
                        MorphName = this._morphItem.MorphName,
                        RepalcedMorphName = this.ReplacedMorphName
                    };
                }
                return null;
            }
        }

        /// <summary>
        /// 初期化
        /// </summary>
        /// <param name="morphItem">モーフ名</param>
        /// <param name="isMissing">不足モーフならtrue</param>
        /// <param name="allMorphs">全モーフ</param>
        public void Initialize(MorphItem morphItem, ReplacedItem rp, bool isMissing, List<MorphItem> allMorphs)
        {
            this.lblActiveModel.Text = morphItem.MortphNameWithType;

            this._isMissing = isMissing;
            this._morphItem = morphItem;
            this._allMorphs.AddRange(allMorphs);
            this.comboBox1.Items.Clear();
            this.comboBox1.Items.Add("");
            this.comboBox1.Items.Add("【無視する】");

            var targetMorphs = allMorphs.Where(n => n.MorphType == morphItem.MorphType).ToArray();

            this.comboBox1.Items.AddRange(targetMorphs);
            this.comboBox1.SelectedIndex = 0;
            if (rp != null)
            {
                if (rp.Ignore)
                    this.comboBox1.SelectedIndex = 1;
                else
                {
                    var targetmorph = targetMorphs.Where(n => n.MorphName == rp.RepalcedMorphName).FirstOrDefault();
                    if (targetmorph != null) { }
                    this.comboBox1.SelectedItem = targetmorph;
                }
                
            }
            comboBox1_SelectedValueChanged(this, new EventArgs());
        }

        private void comboBox1_Format(object sender, ListControlConvertEventArgs e)
        {
            if (!this.comboBox1.Visible)
                return;
            var morphItem = e.Value as MorphItem;
            if (morphItem != null)
                e.Value = morphItem.MortphNameWithType;
        }

        private void comboBox1_SelectedValueChanged(object sender, EventArgs e)
        {
            if (this._isMissing)
            {
                if (this.comboBox1.SelectedItem != null && !string.IsNullOrEmpty(this.comboBox1.Text))
                {
                    this.lblMissing.Text = "●";
                    this.lblMissing.ForeColor = Color.LightBlue;
                }
                else
                {
                    this.lblMissing.Text = "✗";
                    this.lblMissing.ForeColor = Color.Red;
                }
            }
            this.lblMissing.Visible = this._isMissing;
        }
    }
}