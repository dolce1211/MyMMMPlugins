using MikuMikuPlugin;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FaceExpressionHelper.UI.UserControls
{
    public class MorphListBox : ListBox
    {
        public MorphListBox()
        {
            this.DrawMode = DrawMode.OwnerDrawFixed;
            this.BorderStyle = BorderStyle.None;
            this.BackColor = System.Drawing.SystemColors.Control;
        }

        protected override void OnDrawItem(DrawItemEventArgs e)
        {
            e.DrawBackground();

            Brush b = null;
            b = new SolidBrush(e.ForeColor);
            var font = e.Font;
            if (e.Index >= 0 && this.Items.Count > 0)
            {
                var morph = this.Items[e.Index] as MorphItem;
                if (morph == null)
                    return;

                var txt = $"{morph.MorphName}：  {Math.Round(morph.Weight, 3)}";
                if (morph is DspMorphItem dspmorph)
                {
                    txt = $"{dspmorph.MorphName}：  {Math.Round(dspmorph.Weight, 3)}";
                    if (dspmorph.IsMissing)
                    {
                        //不足かつ未対処のモーフ
                        b = new SolidBrush(Color.Red);
                        txt += "  【不足】";
                    }
                    else if (dspmorph.Ignore)
                    {
                        //無視対象のモーフ
                        b = new SolidBrush(Color.Gray);
                        txt += "  【無視する】";
                    }
                    else if (dspmorph.ReplacedItem != null)
                    {
                        //置換対象があるモーフ
                        txt = $"{dspmorph.MorphName} → {dspmorph.ReplacedItem.RepalcedMorphName}  {Math.Round(dspmorph.PrevWeight, 3)}  ";

                        b = new SolidBrush(Color.Blue);
                        if (dspmorph.ReplacedItem.Correction != 1f)
                            txt += $"*{Math.Round(dspmorph.ReplacedItem.Correction, 3)}";

                        if (dspmorph.ReplacedItem.RepalcedMorphName != dspmorph.MorphName)
                            txt += "【置換】";
                    }
                }

                e.Graphics.DrawString(txt, font, b, e.Bounds);
            }

            b.Dispose();

            //e.DrawFocusRectangle();
        }
    }
}