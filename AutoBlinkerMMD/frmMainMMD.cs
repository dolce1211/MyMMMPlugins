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
            //MMDはモーフに補間曲線は付けられない
            this.chkHokan.Visible = false;

            //MMDはボーンレイヤーを使えない
            this.lblEyeBoneNotification.Visible = true;
            this.chkEyeMotionLayer.Visible = false;

            this.mmdSelectorControl1.Visible = true;

            this.FormBorderStyle = FormBorderStyle.Sizable;
            this.MinimizeBox = true;
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();
            //
            // mmdSelectorControl1
            //
            this.mmdSelectorControl1.Location = new System.Drawing.Point(0, 405);
            this.mmdSelectorControl1.Size = new System.Drawing.Size(301, 52);
            //
            // lblModel
            //
            this.lblModel.Size = new System.Drawing.Size(0, 20);
            this.lblModel.Text = "";
            //
            // frmMainMMD
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.ClientSize = new System.Drawing.Size(452, 660);
            this.Name = "frmMainMMD";
            this.Shown += new System.EventHandler(this.frmMainMMD_Shown);
            this.ResumeLayout(false);
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
                this.lblWait.Parent = this;
                this.lblWait.Size = new System.Drawing.Size(this.Width, 300);
                this.lblWait.Location = new System.Drawing.Point(0, (int)(this.Height - 300) / 2);
                this.lblWait.Text = text;
                this.lblWait.Visible = true;
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
            var mmd = this.Invoke((Func<Process>)(() =>
            {
                try
                {
                    return this.mmdSelectorControl1.SelectMMD();
                }
                catch (Exception ex)
                {
                    return null;
                }
            })) as Process;

            if (mmd == null)
            {
                //MMD紐づけ無し
                return;
            }

            Args setting = e.Args;
            if (setting == null)
                return;
            if (string.IsNullOrWhiteSpace(setting.ModelInfo.BlinkingMorphName))
                return;

            //現在のモーフ適用状態を取得する
            ModelItem currentModelWithWeight = null;
            this.Cursor = Cursors.WaitCursor;
            try
            {
                currentModelWithWeight = this.TryApplyCurrentWeight();
                if (currentModelWithWeight == null)
                    return;
            }
            finally
            {
                this.Cursor = Cursors.Default;
            }

            var blinkMorph = currentModelWithWeight.EyeMorphItems.Where(n => n.MorphName == setting.ModelInfo.BlinkingMorphName).FirstOrDefault();
            var bikkuriMorph = currentModelWithWeight.EyeMorphItems.Where(n => n.MorphName == setting.ModelInfo.BikkuriMorphName).FirstOrDefault();

            var vmd = new VmdDocument() { ModelName = currentModelWithWeight.ModelName };
            //対象外モーフの反転モーフを生成します。
            foreach (var morph in currentModelWithWeight.EyeMorphItems
                                .Where(n => n.MorphName != setting.ModelInfo.BlinkingMorphName &&
                                            n.MorphName != setting.ModelInfo.BikkuriMorphName &&
                                            n.MorphType == MorphType.Eye && n.Weight > 0)
                                )
            {
                if (setting.Exceptions.IndexOf(morph.MorphName) < 0)
                    this.AddCenterFrames(vmd, setting, morph, 0);
            }

            //まばたきモーフを生成する
            this.AddCenterFrames(vmd, setting, blinkMorph, 1f, true);

            //反動を付ける
            if (bikkuriMorph != null)
            {
                this.AddOuterFrames(vmd, setting, bikkuriMorph, setting.ModelInfo.BikkuriMorphValue);
            }

            if (setting.DoEyebrowSync)
            {
                //まゆ連動

                var mayuDownMorph = currentModelWithWeight.BrowMorphItems.Where(n => n.MorphName == setting.ModelInfo.EyebrowDownMorphName).FirstOrDefault();
                var mayuUpMorph = currentModelWithWeight.BrowMorphItems.Where(n => n.MorphName == setting.ModelInfo.EyebrowUpMorphName).FirstOrDefault();

                //まゆ下連動
                this.AddCenterFrames(vmd, setting, mayuDownMorph, setting.ModelInfo.EyebrowDownSyncValue, true);

                //まゆ上連動
                this.AddOuterFrames(vmd, setting, mayuUpMorph, setting.ModelInfo.EyebrowUpSyncValue);
            }

            //目連動を行う
            if (setting.DoEyeSync)
            {
                this.AddEyeBone(vmd, setting);
            }

            //ゆっくり戻す
            if (setting.DoYuruyaka && blinkMorph != null)
            {
                uint fr = 0;
                if (setting.DoHandouStart) fr += (uint)setting.HandouFramesStart;
                fr += (uint)setting.EnterFrames;
                fr += (uint)setting.BlinkingFrames;
                fr += (uint)setting.ExitFrames;
                if (setting.DoHandouEnd) fr += (uint)setting.HandouFramesEnd;

                vmd.MorphFrames.Add(new VmdMorphFrame() { Name = blinkMorph.MorphName, Weight = setting.YuruyakaValue * 0.01f, FrameTime = fr });
                fr += (uint)setting.YuruyakaFrame;
                vmd.MorphFrames.Add(new VmdMorphFrame() { Name = blinkMorph.MorphName, Weight = 0, FrameTime = fr });
            }

            //vmdデータをMMDに投げる
            using (var vmdStream = new MemoryStream())
            {
                vmd.Write(vmdStream);
                vmdStream.Seek(0, SeekOrigin.Begin);

                var rtn = MmdDrop.DropFile(mmd.MainWindowHandle, new MmdDropFile("TempMotion" + mmd.Id + ".vmd", vmdStream)
                {
                    Timeout = 500,
                });
                if (!rtn)
                    return;
            }

            //ダイアログを潰す
            MMDUtilility.PressOKToDialog(mmd.MainWindowHandle, new string[] { "モーションチェック", "MMPlus", "モーションデータ読込" });

            //MMDにフォーカスを当てる
            MMDUtilility.SetForegroundWindow(mmd.MainWindowHandle);
        }

        /// <summary>
        /// this._currentModelに現在のモーフ状態を取得して返します。
        /// </summary>
        /// <returns></returns>
        private ModelItem TryApplyCurrentWeight()
        {
            var mmd = this.mmdSelectorControl1.SelectMMD();
            if (mmd == null)
            {
                //MMD紐づけ無し
                return null;
            }
            if (this._currentModel == null || string.IsNullOrWhiteSpace(this._currentModel.ModelName))
            {
                //アクティブモデル無し
                return null;
            }
            this.Invoke((Action)(() => this.Cursor = Cursors.WaitCursor));

            var retHash = MMDUtilility.TryGetAllMorphValue(mmd.MainWindowHandle, new List<MorphType> { MorphType.Eye });
            foreach (var kvp in retHash)
            {
                var tuple = kvp.Key;
                var weight = kvp.Value;
                List<MorphItem> lst = null;

                if (tuple.Item1 == MorphType.Eye)
                    lst = _currentModel.EyeMorphItems;
                else if (tuple.Item1 == MorphType.Brow)
                    lst = _currentModel.BrowMorphItems;

                if (lst != null)
                {
                    var morph = lst.Where(n => n.ComboBoxIndex == tuple.Item2).FirstOrDefault();
                    if (morph == null)
                    {
                    }
                    else
                    {
                        morph.Weight = weight;
                    }
                }
            }
            this.BeginInvoke((Action)(() => this.Cursor = Cursors.Default));
            return this._currentModel;
        }

        /// <summary>
        /// 主モーフを適用/キャンセルするモーフフレームを適用します。
        /// </summary>
        /// <param name=""></param>
        /// <param name=""></param>
        private void AddCenterFrames(VmdDocument vmd, Args setting, MorphItem morph, float weight, bool executeOnZeroWeight = false)
        {
            if (morph == null)
                return;
            if (morph.Weight == 0 && !executeOnZeroWeight)
                return;

            uint fr = 0;
            if (setting.DoHandouStart && setting.HandouFramesStart > 0)
                fr = (uint)setting.HandouFramesStart;

            vmd.MorphFrames.Add(new VmdMorphFrame() { Name = morph.MorphName, Weight = morph.Weight, FrameTime = fr });
            fr += (uint)setting.EnterFrames;
            vmd.MorphFrames.Add(new VmdMorphFrame() { Name = morph.MorphName, Weight = weight, FrameTime = fr });
            fr += (uint)setting.BlinkingFrames;
            vmd.MorphFrames.Add(new VmdMorphFrame() { Name = morph.MorphName, Weight = weight, FrameTime = fr });
            fr += (uint)setting.ExitFrames;
            vmd.MorphFrames.Add(new VmdMorphFrame() { Name = morph.MorphName, Weight = morph.Weight, FrameTime = fr });
        }

        /// <summary>
        /// 主モーフを適用/キャンセルするモーフフレームを適用します。
        /// </summary>
        /// <param name=""></param>
        /// <param name=""></param>
        private void AddOuterFrames(VmdDocument vmd, Args setting, MorphItem morph, float weight)
        {
            if (morph == null)
                return;

            uint fr = 0;
            if (setting.DoHandouStart && setting.HandouFramesStart > 0)
            {
                vmd.MorphFrames.Add(new VmdMorphFrame() { Name = morph.MorphName, Weight = morph.Weight, FrameTime = fr });
                fr = (uint)setting.HandouFramesStart;
                vmd.MorphFrames.Add(new VmdMorphFrame() { Name = morph.MorphName, Weight = weight, FrameTime = fr });
                fr += (uint)setting.EnterFrames;
                vmd.MorphFrames.Add(new VmdMorphFrame() { Name = morph.MorphName, Weight = 0, FrameTime = fr });
                fr += (uint)setting.BlinkingFrames;
            }

            if (setting.DoHandouEnd && setting.HandouFramesEnd > 0)
            {
                vmd.MorphFrames.Add(new VmdMorphFrame() { Name = morph.MorphName, Weight = 0, FrameTime = fr });
                fr += (uint)setting.ExitFrames;
                vmd.MorphFrames.Add(new VmdMorphFrame() { Name = morph.MorphName, Weight = weight, FrameTime = fr });

                fr += (uint)setting.HandouFramesEnd;
                vmd.MorphFrames.Add(new VmdMorphFrame() { Name = morph.MorphName, Weight = morph.Weight, FrameTime = fr });
            }
        }

        /// <summary>
        /// 主モーフを適用/キャンセルするモーフフレームを適用します。
        /// </summary>
        /// <param name=""></param>
        /// <param name=""></param>
        private void AddEyeBone(VmdDocument vmd, Args setting)
        {
            if (!setting.DoEyeSync)
                return;
            uint fr = 0;
            var downq = new Vector3D(setting.ModelInfo.EyeSyncValueDown * -1, 0, 0).ToQuatanion().ToFloatArray();
            var upq = new Vector3D(setting.ModelInfo.EyeSyncValueUp, 0, 0).ToQuatanion().ToFloatArray();
            var emptyq = new Vector3D(0, 0, 0).ToQuatanion().ToFloatArray();

            vmd.BoneFrames.Add(new VmdBoneFrame() { Name = setting.ModelInfo.EyeSyncBoneName, FrameTime = fr, Quaternion = emptyq });
            if (setting.DoHandouStart)
            {
                fr += (uint)setting.HandouFramesStart;
                vmd.BoneFrames.Add(new VmdBoneFrame() { Name = setting.ModelInfo.EyeSyncBoneName, FrameTime = fr, Quaternion = upq });
            }

            fr += (uint)setting.EnterFrames;
            vmd.BoneFrames.Add(new VmdBoneFrame() { Name = setting.ModelInfo.EyeSyncBoneName, FrameTime = fr, Quaternion = downq });
            fr += (uint)setting.BlinkingFrames;
            vmd.BoneFrames.Add(new VmdBoneFrame() { Name = setting.ModelInfo.EyeSyncBoneName, FrameTime = fr, Quaternion = downq });
            fr += (uint)setting.ExitFrames;
            if (setting.DoHandouEnd)
            {
                vmd.BoneFrames.Add(new VmdBoneFrame() { Name = setting.ModelInfo.EyeSyncBoneName, FrameTime = fr, Quaternion = upq });
                fr += (uint)setting.HandouFramesEnd;
            }
            vmd.BoneFrames.Add(new VmdBoneFrame() { Name = setting.ModelInfo.EyeSyncBoneName, FrameTime = fr, Quaternion = emptyq });
        }
    }
}