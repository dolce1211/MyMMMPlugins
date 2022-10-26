using MikuMikuPlugin;
using MyUtility;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
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
        private bool _isCorrected = false;
        public bool IsMissing => _isMissing && !_isCorrected;

        /// <summary>
        /// アクティブモデルの対象モーフ一覧
        /// </summary>
        private IEnumerable<MorphItem> _targetMorphs = null;

        private Action<MorphSelectedEventArgs> _morphSelectedAction = null;
        private string _activeModel = null;

        public ReplaceMorphCtr()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 値をリセットします。
        /// </summary>
        public void Reset()
        {
            this.Count = 1;
            for (int i = 1; i <= 3; i++)
            {
                var tuple = this.GetControlSet(i);
                tuple.Item1.SelectedIndex = 0;
                tuple.Item2.Value = 1;
            }
        }

        /// <summary>
        /// 引数の値を反映させます。
        /// </summary>
        /// <param name="replacedItems">置換情報</param>
        /// <param name="isFromSameModel">同一モデルからの引用ならtrue。Correctionも反映させる</param>
        /// <returns></returns>
        public bool ApplyValue(ReplacedMorphSet replacedmorphSet, bool isFromSameModel)
        {
            if (replacedmorphSet.Ignore)
            {
                if (isFromSameModel)
                {
                    //【無視する】
                    this.Count = 1;
                    this.comboBox1.SelectedIndex = 1;
                }
                //自モデルから以外の「無視する」はスルーする
                return true;
            }

            List<ReplacedItem> replacedItems = replacedmorphSet.ReplacedItems;
            this.Count = Math.Min(replacedItems.Count, 3);
            var index = 1;
            var ret = false;
            foreach (var rp in replacedItems)
            {
                var replacingMorph = _targetMorphs.Where(n => n.MorphName == rp.RepalcedMorphName).FirstOrDefault();
                if (replacingMorph != null)
                {
                    var ctrtuple = this.GetControlSet(index);
                    var maxnum = 1;
                    if (ctrtuple.Item1 == this.comboBox1)
                        maxnum = 2;
                    if (ctrtuple.Item1.Items.Count <= maxnum)
                        ctrtuple.Item1.Items.AddRange(this._targetMorphs.ToArray());

                    if (ctrtuple.Item1.SelectedItem == null || string.IsNullOrEmpty(ctrtuple.Item1.SelectedItem.ToString()))
                    {
                        ctrtuple.Item1.SelectedItem = replacingMorph;
                        if (isFromSameModel)
                            ctrtuple.Item2.Value = (decimal)rp.Correction;
                    }

                    ret = true;
                }
                index++;
            }
            return ret;
        }

        /// <summary>
        /// 元のモーフ名
        /// </summary>
        public string MorphName { get; set; } = String.Empty;

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
                        if (dsp != null)
                            replacedMorph = dsp.MorphName;
                    }
                    if (ctrtuple.Item2.Enabled && (float)ctrtuple.Item2.Value != 1f)
                        correction = (float)Math.Round(ctrtuple.Item2.Value, 3);

                    if (ignore || !string.IsNullOrEmpty(replacedMorph) || correction != 1f)
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
        public void Initialize(string activeModel, MorphItem morphItem, ReplacedMorphSet rps, bool isMissing, List<MorphItem> allMorphs, Action<MorphSelectedEventArgs> morphSelectedAction)
        {
            this.MorphName = morphItem.MorphName;
            this.lblActiveModel.Text = morphItem.MortphNameWithType;
            this._morphSelectedAction = morphSelectedAction;
            this._activeModel = activeModel;
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

                        ctrtuple.Item1.Items.AddRange(this._targetMorphs.ToArray());
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
            (ComboBox, NumericUpDown) ctrtuple = default;
            for (int i = 1; i <= 3; i++)
            {
                var tmptuple = GetControlSet(i);
                if (tmptuple.Item1 == sender || tmptuple.Item2 == sender)
                {
                    ctrtuple = tmptuple;
                    break;
                }
            }
            if (ctrtuple == default)
                return;

            var ignore = false;
            if (this.comboBox1.SelectedItem.ToString() == "【無視する】")
                ignore = true;

            this.panel2.Enabled = !ignore;
            this.panel3.Enabled = !ignore;
            this.btnAdd.Enabled = !ignore;

            this._isCorrected = false;
            if (this._isMissing)
            {
                if (ctrtuple.Item1.SelectedItem != null && !string.IsNullOrEmpty(ctrtuple.Item1.Text))
                {
                    this.lblMissing.Text = "●";
                    this.lblMissing.ForeColor = Color.LightBlue;
                    this._isCorrected = true;
                }
                else
                {
                    this.lblMissing.Text = "✗";
                    this.lblMissing.ForeColor = Color.Red;
                }
            }
            this.lblMissing.Visible = this._isMissing;

            if (!this.Visible)
                return;

            //最初に全部入れるとクッソ重いので初回開いた時に開く
            var maxnum = 1;
            if (ctrtuple.Item1 == this.comboBox1)
                maxnum = 2;
            if (ctrtuple.Item1.Items.Count <= maxnum)
            {
                //フォーム開くタイミングで全部入れると時間がかかるので初回展開時に入れる
                ctrtuple.Item1.Items.AddRange(this._targetMorphs.ToArray());
            }

            bool resetFrame = true;
            if (sender is NumericUpDown)
                //numeriupdown変更時はフレームリセットはしない
                resetFrame = false;

            var morph = ctrtuple.Item1.SelectedItem as MorphItem;
            if (morph != null)
                this._morphSelectedAction?.Invoke(new MorphSelectedEventArgs(this._activeModel, morph.MorphName, (float)(ctrtuple.Item2.Value), resetFrame));
            else
                this._morphSelectedAction?.Invoke(new MorphSelectedEventArgs(this._activeModel, "", 0));
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

        private int _count = -1;

        private int Count
        {
            get => _count;
            set
            {
                if (value < 1 || value > 3)
                    return;
                if (_count != value)
                {
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
        }
    }
}