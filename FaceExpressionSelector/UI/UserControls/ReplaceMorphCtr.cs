using MyUtility;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
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

        /// <summary>
        /// アクティブモデルの対象モーフ一覧
        /// </summary>
        private IEnumerable<MorphItem> _targetMorphs = null;

        public ReplaceMorphCtr()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 元のモーフ名
        /// </summary>
        private string MorphName => this.lblActiveModel.Text;

        private (ComboBox, NumericUpDown) GetControlSet(int index)
        {
            ComboBox cmb = null;
            NumericUpDown nm = null;
            switch (index)
            {
                case 1:
                    cmb = this.comboBox1;
                    nm = this.numericUpDown1;
                    break;

                case 2:
                    cmb = this.comboBox2;
                    nm = this.numericUpDown2;
                    break;

                case 3:
                    cmb = this.comboBox3;
                    nm = this.numericUpDown3;
                    break;

                default:
                    break;
            }
            return (cmb, nm);
        }

        /// <summary>
        /// 帰り値
        /// </summary>
        public ReplacedMorphSet Result
        {
            get
            {
                var ignore = false;
                if (this.comboBox1.SelectedItem.ToString() == "【無視する】")
                    ignore = true;

                var replacedItems = new List<ReplacedItem>();

                for (int i = 1; i <= 3; i++)
                {
                    if (i >= 2 && ignore)
                        continue;
                    if (i > this.Count)
                        break;

                    var ctrtuple = GetControlSet(i);

                    string replacedMorph = String.Empty;
                    float correction = 1f;
                    if (ctrtuple.Item1.SelectedIndex > 0)
                    {
                        var dsp = ctrtuple.Item1.SelectedItem as MorphItem;
                        replacedMorph = dsp.MorphName;
                    }
                    if (ctrtuple.Item2.Enabled && (float)ctrtuple.Item2.Value != 1f)
                        correction = (float)Math.Round(ctrtuple.Item2.Value, 3);

                    if (!string.IsNullOrEmpty(replacedMorph) || correction != 1f)
                    {
                        if (string.IsNullOrEmpty(replacedMorph))
                            replacedMorph = this._morphItem.MorphName;
                        replacedItems.Add(new ReplacedItem()
                        {
                            MorphName = this._morphItem.MorphName,
                            RepalcedMorphName = replacedMorph,
                            Correction = correction,
                        });
                    }
                }
                if (replacedItems.Count > 0)
                {
                    return new ReplacedMorphSet()
                    {
                        Ignore = ignore,
                        MorphName = this._morphItem.MorphName,
                        ReplacedItems = replacedItems,
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
        public void Initialize(MorphItem morphItem, ReplacedMorphSet rps, bool isMissing, List<MorphItem> allMorphs)
        {
            this.lblActiveModel.Text = morphItem.MortphNameWithType;

            this._isMissing = isMissing;
            this._morphItem = morphItem;
            this._allMorphs.AddRange(allMorphs);
            this.comboBox1.Items.Clear();
            this.comboBox2.Items.Clear();
            this.comboBox3.Items.Clear();

            this.comboBox1.Items.Add("");
            this.comboBox2.Items.Add("");
            this.comboBox3.Items.Add("");
            this.comboBox1.Items.Add("【無視する】");
            this.comboBox1.SelectedIndex = 0;
            this.comboBox2.SelectedIndex = 0;
            this.comboBox3.SelectedIndex = 0;

            this._targetMorphs = allMorphs.Where(n => n.MorphType == morphItem.MorphType).ToArray();
            this.Count = 1;
            if (rps != null)
            {
                this.comboBox1.Items.AddRange(this._targetMorphs.ToArray());
                this.comboBox2.Items.AddRange(this._targetMorphs.ToArray());
                this.comboBox3.Items.AddRange(this._targetMorphs.ToArray());
                if (rps.Ignore)
                    this.comboBox1.SelectedIndex = 1;
                else
                {
                    foreach (var rp in rps.ReplacedItems)
                    {
                        var ctrtuple = GetControlSet(this.Count);
                        var targetmorph = this._targetMorphs.Where(n => n.MorphName == rp.RepalcedMorphName).FirstOrDefault();
                        if (targetmorph == null)
                            continue;
                        ctrtuple.Item1.SelectedItem = targetmorph;
                        ctrtuple.Item2.Value = (decimal)rp.Correction;

                        if (rp != rps.ReplacedItems.LastOrDefault())
                            this.Count++;
                    }
                }
            }

            comboBox1_SelectedValueChanged(this.comboBox1, new EventArgs());
        }

        private void comboBox1_Format(object sender, ListControlConvertEventArgs e)
        {
            var cmb = sender as ComboBox;
            if (!cmb.Visible)
                return;
            var morphItem = e.Value as MorphItem;
            if (morphItem != null)
                e.Value = morphItem.MortphNameWithType;
        }

        private void comboBox1_SelectedValueChanged(object sender, EventArgs e)
        {
            var cmb = sender as ComboBox;
            var ignore = false;
            if (this.comboBox1.SelectedItem.ToString() == "【無視する】")
                ignore = true;

            this.panel2.Enabled = !ignore;
            this.panel3.Enabled = !ignore;
            this.btnAdd.Enabled = !ignore;

            if (this._isMissing)
            {
                if (cmb.SelectedItem != null && !string.IsNullOrEmpty(cmb.Text))
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

        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (comboBox1.SelectedIndex < 0)
                return;
            this.Count++;
        }

        private void btnRemove_Click(object sender, EventArgs e)
        {
            if (this.Count > 1)
                this.Count--;
        }

        private int _count = 1;

        private int Count
        {
            get => _count;
            set
            {
                if (value < 1 || value > 3)
                    return;

                _count = value;
                if (this.Parent != null)
                    this.Parent.BeginAndEndUpdate(false);
                try
                {
                    this.btnAdd.Enabled = true;
                    this.btnRemove.Enabled = true;
                    switch (_count)
                    {
                        case 1:
                            this.Height = 30;
                            this.btnRemove.Enabled = false;
                            break;

                        case 2:
                            this.Height = 55;
                            break;

                        default:
                            this.Height = 82;
                            this.btnAdd.Enabled = false;
                            break;
                    }
                }
                finally
                {
                    if (this.Parent != null)
                        this.Parent.BeginAndEndUpdate(true);
                }
            }
        }

        private void comboBox1_DropDown(object sender, EventArgs e)
        {
            var cmd = sender as ComboBox;
            if (cmd.Items.Count <= 2)
            {
                //フォーム開くタイミングで全部入れると時間がかかるので初回展開時に入れる
                cmd.Items.AddRange(this._targetMorphs.ToArray());
            }
        }
    }
}