using AutoBlinkerMMD.Properties;
using AutoBlinkerPlugin;
using LibMMD.Pmx;
using LibMMD.Vmd;
using Linearstar.Keystone.IO.MikuMikuDance;
using MMDUtil;
using MyUtility;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Media.Media3D;
using static MMDUtil.MMDUtilility;

namespace AutoBlinkerMMD
{
    public class frmMainMMD : frmMainBase
    {
        private BlinkModelFinder _blinkModelFinder = null;

        public frmMainMMD()
        {
            this.InitializeComponent();
            frmMainBase.OperationgMode = MyUtility.OperatingMode.OnMMD;

            this.ControlBox = true;
            this.MinimizeBox = false;
            this.MaximizeBox = false;

            //MMDはモーフに補間曲線は付けられない
            this.chkHokan.Visible = false;

            //MMDはボーンレイヤーを使えない
            this.chkEyeMotionLayer.Visible = false;

            this.mmdSelectorControl1.Visible = true;
        }

        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmMainMMD));
            this.SuspendLayout();
            //
            // mmdSelectorControl1
            //
            this.mmdSelectorControl1.Location = new System.Drawing.Point(0, 628);
            //this.mmdSelectorControl1.Size = new System.Drawing.Size(488, 52);
            //
            // lblModel
            //
            this.lblModel.Size = new System.Drawing.Size(0, 20);
            this.lblModel.Text = "";
            //
            // frmMainMMD
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "frmMainMMD";
            this.Shown += new System.EventHandler(this.frmMainMMD_Shown);
            this.ResumeLayout(false);
        }

        protected override void cboEyeBone_SelectedIndexChanged(object sender, EventArgs e)
        {
            base.cboEyeBone_SelectedIndexChanged(sender, e);

            this.lblEyeBoneNotification.Visible = false;
            if (base.cboEyeBone.SelectedItem.ToString() == "両目")
            {
                this.lblEyeBoneNotification.Visible = true;
            }
        }

        /// <summary>
        /// lblWaitを隠す
        /// </summary>
        private void HidelblWait()
        {
            this.Invoke((Action)(() =>
            {
                this.lblWait.Text = String.Empty;
                this.lblWait.Visible = false;
                this.ApplyModel(this._currentModel);
            }));
        }

        /// <summary>
        /// lblWaitを出す
        /// </summary>
        /// <param name="text"></param>
        private void ShowlblWait(string text)
        {
            this.Invoke((Action)(() =>
            {
                if (this._isMinimized)
                {
                    //最小化時には邪魔なので出さない
                    this.lblWait.Visible = false;
                }
                else
                {
                    this.lblWait.Parent = this;
                    this.lblWait.Size = new System.Drawing.Size(this.Width, 300);
                    this.lblWait.Location = new System.Drawing.Point(0, (int)(this.Height - 300) / 2);
                    this.lblWait.Text = text;
                    this.lblWait.Visible = true;
                }

                this.lblWait.BringToFront();
                this.lblWait.Refresh();

                this.ApplyModel(null);
            }));
        }

        private void frmMainMMD_Shown(object sender, EventArgs e)
        {
            this._blinkModelFinder = new BlinkModelFinder(this, this.mmdSelectorControl1, this.ShowlblWait, this.HidelblWait);
            this._blinkModelFinder.ActiveModelChangedEventHandler += (object ss, ActiveModelChangedEventArgs ee) =>
            {
                if (ee.CurrentActiveModel != null)
                    this.lblModel.Text = ee.CurrentActiveModel.ModelName.TrimSafe();
            };
            this._blinkModelFinder.ActiveModelChangedEventHandler += (ss, ee) =>
            {
                var model = ee.CurrentActiveModel as ModelItem;
                base.ApplyModel(model);
            };

            this.Executed -= this.OnExecute;
            this.Executed += this.OnExecute;
        }

        public void OnExecute(object sender, ExecutedEventArgs e)
        {
            if (e.Args == null)
                return;

            //まばたきを適用する
            var blinkApplier = new BlinkApplier(this, this.mmdSelectorControl1, e.Args, this._currentModel);
            blinkApplier.Execute();
        }
    }
}