using MyUtility;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MMDUtil
{
    internal class FixedPanel : System.Windows.Forms.Panel
    {
        protected override Point ScrollToControl(Control activeControl)
        {
            return this.AutoScrollPosition;
        }

        protected override void OnScroll(ScrollEventArgs se)
        {
            //普通にスクロールするとクシャってなるのに対応
            this.BeginUpdate();

            base.OnScroll(se);

            //this.BeginInvoke(new Action(async () =>
            //{
            //    await Task.Delay(2);
            this.EndUpdate();
            this.AdjustFormScrollbars(true);
            //}
            //));
        }

        protected override void OnMouseWheel(MouseEventArgs e)
        {
            //普通にスクロールするとクシャってなるのに対応
            this.BeginUpdate();
            this.BeginInvoke(new Action(async () =>
            {
                await Task.Delay(2);
                this.EndUpdate();
                this.AdjustFormScrollbars(true);
            }
            ));

            base.OnMouseWheel(e);
        }

        public FixedPanel()
        {
            this.DoubleBuffered = true;
        }
    }
}