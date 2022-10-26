using AutoBlinkerPlugin;
using LibMMD.Pmx;
using MMDUtil;
using MyUtility;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AutoBlinkerMMD
{
    public class frmMainMMD : frmMain
    {
        private BlinkModelFinder _blinkModelFinder = null;

        public frmMainMMD()
        {
            this.InitializeComponent();
            frmMain.OperationgMode = MyUtility.OperatingMode.OnMMD;

            this.ControlBox = true;
            //MMDはモーフに補間曲線は付けられない
            this.chkHokan.Visible = false;

            this.MinimizeBox = true;
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();
            //
            // frmMainMMD
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.ClientSize = new System.Drawing.Size(301, 404);
            this.Location = new System.Drawing.Point(0, 0);
            this.Name = "frmMainMMD";
            this.Shown += new System.EventHandler(this.frmMainMMD_Shown);
            this.ResumeLayout(false);
        }

        private void frmMainMMD_Shown(object sender, EventArgs e)
        {
            this._blinkModelFinder = new BlinkModelFinder(this, this.mmdSelectorControl1);
            this._blinkModelFinder.ActiveModelChangedEventHandler += (object ss, ActiveModelChangedEventArgs ee) =>
            {
                if (ee.CurrentActiveModel != null)
                    this.lblModel.Text = ee.CurrentActiveModel.ModelName.TrimSafe();
            };
        }
    }

    public class BlinkModelFinder : ModelFinder<SimpleMMDModel>
    {
        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="frm"></param>
        /// <param name="mmdselector"></param>
        /// <param name="showWaitAction"></param>
        /// <param name="hideWaitAction"></param>
        public BlinkModelFinder(Form frm, MMDSelectorControl mmdselector, Action<string> showWaitAction = null, Action hideWaitAction = null) : base(frm, mmdselector, showWaitAction, hideWaitAction)
        {
        }

        protected override SimpleMMDModel CreateInstance()
        {
            return new SimpleMMDModel();
        }

        protected override SimpleMMDModel PmxModel2ActiveModelInfo(PmxModel pmxmdls)
        {
            return new SimpleMMDModel() { ModelName = pmxmdls.ModelNameLocal };
        }
    }
}