using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using DxMath;
using MikuMikuPlugin;
using Mocopi.Receiver.Core;
using MyUtility;
using Sony.SMF;
using static Mocopi.Receiver.Core.MocopiUdpReceiver;

namespace MocopiReceiverPlugin
{
    public partial class frmMain : Form
    {
        public EventHandler<BoneInfoEventArgs> BoneInfoReceived;
        private Form _applicationForm = null;
        private Scene _scene = null;
        private MocopiUdpReceiver _receiver;

        private System.Threading.Timer _timer = null;
        private long _seconds = 0;
        private long _count = 0;

        public frmMain(Scene scene, Form applicationForm)
        {
            InitializeComponent();
            this._applicationForm = applicationForm;
            this._scene = scene;

            this.txtPort.LimitInputToNum(false, false);

            _timer = new System.Threading.Timer(x =>
            {
                this.Invoke(new Action(() =>
                {
                    this._seconds++;

                    this.label1.Text = $"{_count.ToString()}";
                    _count = 0;
                }
                ));
            }
            , null, 1000, 1000);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (_receiver?.IsRuning == true)
            {
                _receiver.OnReceiveSkeletonDefinition = null;
                _receiver.OnReceiveFrameData = null;
                _receiver.UdpStop();
            }
            else
            {
                if (_receiver != null)
                {
                    _receiver.UdpStop();
                    _receiver = null;
                }
                _receiver = new MocopiUdpReceiver(Convert.ToInt32(this.txtPort.Text));
                _receiver.OnReceiveSkeletonDefinition = this.OnReceiveSkeletonDefinition;
                _receiver.OnReceiveFrameData = this.OnReceiveFrameData;
                _receiver.OnReceiveFrameData2 = (boneList) =>
                {
                    _count++;

                    if (boneList?.Count > 0)
                    {
                        if (radioButton1.Checked)
                            this.BoneInfoReceived?.Invoke(this, new BoneInfoEventArgs(0, boneList));
                    }
                };
                _receiver.UdpStart();
            }
        }

        private void OnReceiveSkeletonDefinition(
            int[] boneIds, int[] parentBoneIds,
            float[] rotationsX, float[] rotationsY, float[] rotationsZ, float[] rotationsW,
            float[] positionsX, float[] positionsY, float[] positionsZ
        )
        {
            var boneList = new List<MocopiBone>();
            for (int i = 0; i < boneIds.Length; i++)
            {
                var bone = new MocopiBone(boneIds[i], rotationsX[i], rotationsY[i], rotationsZ[i], rotationsW[i],
                        positionsX[i], positionsY[i], positionsZ[i]);
                boneList.Add(bone);
            }
            if (boneList.Count > 0)
            {
                this.BoneInfoReceived?.Invoke(this, new BoneInfoEventArgs(1, boneList));
            }
        }

        private void OnReceiveFrameData
           (
               int[] boneIds,
               float[] rotationsX, float[] rotationsY, float[] rotationsZ, float[] rotationsW,
               float[] positionsX, float[] positionsY, float[] positionsZ
           )
        {
            var boneList = new List<MocopiBone>();
            for (int i = 0; i < boneIds.Length; i++)
            {
                var bone = new MocopiBone(boneIds[i], rotationsX[i], rotationsY[i], rotationsZ[i], rotationsW[i],
                        positionsX[i], positionsY[i], positionsZ[i]);
                boneList.Add(bone);
            }
            if (boneList.Count > 0)
            {
                if (this.radioButton2.Checked)
                    this.BoneInfoReceived?.Invoke(this, new BoneInfoEventArgs(0, boneList));
            }
        }
    }

    public class BoneInfoEventArgs : EventArgs
    {
        /// <summary>
        /// 0:OnReceiveFrameDataから
        /// 1:OnReceiveSkeletonDefinitionから
        /// </summary>
        public int Mode { get; }

        public List<MocopiBone> BoneList { get; }

        public BoneInfoEventArgs(int mode, List<MocopiBone> boneList)
        {
            this.Mode = mode;
            this.BoneList = boneList;
        }
    }
}