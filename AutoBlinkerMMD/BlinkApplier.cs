using AutoBlinkerPlugin;
using Linearstar.Keystone.IO.MikuMikuDance;
using MMDUtil;
using MyUtility;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Media.Media3D;
using static MMDUtil.MMDUtilility;

namespace AutoBlinkerMMD
{
    /// <summary>
    /// まばたきを適用する機能を提供するクラスです。
    /// </summary>
    internal class BlinkApplier
    {
        private MMDSelectorControl _mmdSelectorControl = null;
        private ModelItem _currentModel = null;
        private Form _frm = null;
        private Args _setting = null;

        public BlinkApplier(Form frm, MMDSelectorControl mmdselectorControl, Args setting, ModelItem currentModel)
        {
            this._frm = frm;
            this._mmdSelectorControl = mmdselectorControl;
            this._setting = setting;
            this._currentModel = currentModel;
        }

        public bool Execute()
        {
            var mmd = this._mmdSelectorControl.SelectMMD();

            if (mmd == null)
            {
                //MMD紐づけ無し
                return false;
            }

            if (this._setting == null)
                return false;
            if (string.IsNullOrWhiteSpace(this._setting.ModelInfo.BlinkingMorphName))
                return false;

            //現在のモーフ適用状態を取得する
            ModelItem currentModelWithWeight = null;
            this._frm.Cursor = Cursors.WaitCursor;

            try
            {
                currentModelWithWeight = this.TryApplyCurrentWeight();
                if (currentModelWithWeight == null)
                    return false;
            }
            finally
            {
                this._frm.Cursor = Cursors.Default;
            }

            var blinkMorph = currentModelWithWeight.EyeMorphItems.Where(n => n.MorphName == this._setting.ModelInfo.BlinkingMorphName).FirstOrDefault();
            var bikkuriMorph = currentModelWithWeight.EyeMorphItems.Where(n => n.MorphName == this._setting.ModelInfo.BikkuriMorphName).FirstOrDefault();

            var vmd = new VmdDocument() { ModelName = currentModelWithWeight.ModelName };
            //対象外モーフの反転モーフを生成します。
            foreach (var morph in currentModelWithWeight.EyeMorphItems
                                .Where(n => n.MorphName != this._setting.ModelInfo.BlinkingMorphName &&
                                            n.MorphName != this._setting.ModelInfo.BikkuriMorphName &&
                                            n.MorphType == MorphType.Eye && n.Weight > 0)
                                )
            {
                if (this._setting.Exceptions.IndexOf(morph.MorphName) < 0)
                    this.AddCenterFrames(vmd, this._setting, morph, 0, true, false);
            }

            //まばたきモーフを生成する
            this.AddCenterFrames(vmd, this._setting, blinkMorph, 1f, false, false);

            //反動を付ける
            if (bikkuriMorph != null)
            {
                this.AddOuterFrames(vmd, this._setting, bikkuriMorph, this._setting.ModelInfo.BikkuriMorphValue);
            }

            if (this._setting.DoEyebrowSync)
            {
                //まゆ連動

                var mayuDownMorph = currentModelWithWeight.BrowMorphItems.Where(n => n.MorphName == this._setting.ModelInfo.EyebrowDownMorphName).FirstOrDefault();
                var mayuUpMorph = currentModelWithWeight.BrowMorphItems.Where(n => n.MorphName == this._setting.ModelInfo.EyebrowUpMorphName).FirstOrDefault();

                //まゆ下連動
                this.AddCenterFrames(vmd, this._setting, mayuDownMorph, this._setting.ModelInfo.EyebrowDownSyncValue, false, true);

                //まゆ上連動
                this.AddOuterFrames(vmd, this._setting, mayuUpMorph, this._setting.ModelInfo.EyebrowUpSyncValue);
            }

            //目連動を行う
            if (this._setting.DoEyeSync)
            {
                this.AddEyeBone(vmd, this._setting);
            }

            //ゆっくり戻す
            if (this._setting.DoYuruyaka && blinkMorph != null)
            {
                uint fr = 0;
                if (this._setting.DoHandouStart) fr += (uint)this._setting.HandouFramesStart;
                fr += (uint)this._setting.EnterFrames;
                fr += (uint)this._setting.BlinkingFrames;
                fr += (uint)this._setting.ExitFrames;
                if (this._setting.DoHandouEnd) fr += (uint)this._setting.HandouFramesEnd;

                var delmorph = vmd.MorphFrames.Where(n => n.FrameTime == fr && n.Name == blinkMorph.MorphName).FirstOrDefault();
                if (delmorph != null)
                    //最後のキーを消して入れ直す
                    vmd.MorphFrames.Remove(delmorph);
                vmd.MorphFrames.Add(new VmdMorphFrame() { Name = blinkMorph.MorphName, Weight = blinkMorph.Weight+ this._setting.YuruyakaValue * 0.01f, FrameTime = fr });
                fr += (uint)this._setting.YuruyakaFrame;
                vmd.MorphFrames.Add(new VmdMorphFrame() { Name = blinkMorph.MorphName, Weight = blinkMorph.Weight, FrameTime = fr });
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
                    return false;
            }

            //MMDにフォーカスを当てる
            MMDUtilility.SetForegroundWindow(mmd.MainWindowHandle);

            return true;
        }

        /// <summary>
        /// this._currentModelに現在のモーフ状態を取得して返します。
        /// </summary>
        /// <returns></returns>
        private ModelItem TryApplyCurrentWeight()
        {
            var mmd = this._mmdSelectorControl.SelectMMD();
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

            var retHash = MMDUtilility.TryGetAllMorphValue(mmd.MainWindowHandle, new List<MorphType> { MorphType.Eye, MorphType.Brow });
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

            return this._currentModel;
        }

        /// <summary>
        /// 主モーフを適用/キャンセルするモーフフレームを適用します。
        /// </summary>
        /// <param name="vmd"></param>
        /// <param name="setting"></param>
        /// <param name="morph"></param>
        /// <param name="weight">適用ウェイト</param>
        /// <param name="ignoreZeroWeight">ウェイト0は無視するならtrue</param>
        /// <param name="addCurrentWeight">今のウェイト適用状態にweightを足し合わせて適用するならtrue</param>
        private void AddCenterFrames(VmdDocument vmd, Args setting, MorphItem morph, float weight, bool ignoreZeroWeight, bool addCurrentWeight)
        {
            if (morph == null)
                return;
            if (morph.Weight == 0 && ignoreZeroWeight)
                return;

            uint fr = 0;
            if (setting.DoHandouStart && setting.HandouFramesStart > 0)
                fr = (uint)setting.HandouFramesStart;

            var applyingWeight = weight;
            if (addCurrentWeight)
                applyingWeight += morph.Weight;

            vmd.MorphFrames.Add(new VmdMorphFrame() { Name = morph.MorphName, Weight = morph.Weight, FrameTime = fr });
            fr += (uint)setting.EnterFrames;
            vmd.MorphFrames.Add(new VmdMorphFrame() { Name = morph.MorphName, Weight = applyingWeight, FrameTime = fr });
            fr += (uint)setting.BlinkingFrames;
            vmd.MorphFrames.Add(new VmdMorphFrame() { Name = morph.MorphName, Weight = applyingWeight, FrameTime = fr });

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