using AutoBlinkerPlugin;
using LibMMD.Pmx;
using LibMMD.Vmd;
using MMDUtil;
using MyUtility;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static MMDUtil.MMDUtilility;

namespace AutoBlinkerMMD
{
    public class BlinkModelFinder : ModelFinder<ModelItem>
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

        protected override ModelItem CreateInstance()
        {
            return new ModelItem();
        }

        protected override ModelItem PmxModel2ActiveModelInfo(PmxModel pmxmdls)
        {
            var ret = new ModelItem() { ModelName = pmxmdls.ModelNameLocal };

            var allMorphs = new Dictionary<MorphType, List<MorphItem>>();
            var hash = new Dictionary<MorphType, int>();
            hash.Add(MorphType.Eye, 0);
            hash.Add(MorphType.Brow, 0);
            var bones = new List<string>();
            foreach (var mrph in pmxmdls.Morphs.OrderBy(n => n.Index))
            {
                MorphType morphtype = base.PmxPnlType2Morphtype(mrph.PanelType);
                if (hash.ContainsKey(morphtype))
                {
                    var morphitem = new MorphItem() { MorphName = mrph.NameLocal, MorphType = morphtype, ComboBoxIndex = hash[morphtype] };
                    var morphlist = new List<MorphItem>();
                    if (!allMorphs.ContainsKey(morphtype))
                        allMorphs.Add(morphtype, morphlist);
                    else
                        morphlist = allMorphs[morphtype];
                    morphlist.Add(morphitem);
                    hash[morphtype]++;
                }
            }
            foreach (var morph in pmxmdls.Bones)
            {
                bones.Add(morph.NameLocal);
            }

            var eyeMorphs = new List<MorphItem>();
            var browMorphs = new List<MorphItem>();
            if (allMorphs.ContainsKey(MorphType.Eye))
                eyeMorphs = allMorphs[MorphType.Eye];
            if (allMorphs.ContainsKey(MorphType.Brow))
                browMorphs = allMorphs[MorphType.Brow];

            return new ModelItem() { ModelName = pmxmdls.ModelNameLocal, EyeMorphItems = eyeMorphs, BrowMorphItems = browMorphs, Bones = bones };
        }

        ///// <summary>
        ///// モーフのキーフレームを打ちます。
        ///// </summary>
        ///// <param name="entity"></param>
        ///// <param name="morph"></param>
        ///// <param name="pos"></param>
        ///// <param name="weight"></param>
        ///// <param name="hokan"></param>
        //private void AddMorphKeyFrame(Args entity, Morph morph, long pos, float weight, HokanType hokan = HokanType.Default)
        //{
        //    morph.Frames.RemoveKeyFrame(pos);

        //    var newframe = new MorphFrameData(pos, weight);

        //    HokanTemplate.ApplyHokan(entity, newframe, hokan);

        //    morph.Frames.AddKeyFrame(newframe);
        //}

        ///// <summary>
        ///// まばたきモーフを作成します。
        ///// </summary>
        ///// <param name="entity"></param>
        ///// <param name="morph"></param>
        ///// <param name="argvalue"></param>
        //private void AddBlinkMorphKey(Args entity, Morph morph, float argvalue)
        //{
        //    if (morph == null)
        //        return;
        //    if (argvalue == 0)
        //        return;

        //    float currentWeight = morph.CurrentWeight;
        //    var value = Math.Min(currentWeight + argvalue, 1f);
        //    var pos = Scene.MarkerPosition;
        //    pos += entity.HandouFramesStart;

        //    //1
        //    this.AddMorphKeyFrame(entity, morph, pos, currentWeight);

        //    //2
        //    pos += entity.EnterFrames;
        //    this.AddMorphKeyFrame(entity, morph, pos, value, HokanType.Enter);

        //    //3
        //    pos += entity.BlinkingFrames;
        //    this.AddMorphKeyFrame(entity, morph, pos, value);

        //    //4
        //    pos += entity.ExitFrames;
        //    this.AddMorphKeyFrame(entity, morph, pos, currentWeight, HokanType.Exit);
        //}

        ///// <summary>
        ///// まばたきと反対の動作をするモーフを作成します。
        ///// </summary>
        ///// <param name="entity"></param>
        ///// <param name="morph"></param>
        //private void AddInvertMorphKey(Args entity, Morph morph)
        //{
        //    if (morph == null)
        //        return;
        //    if (morph.CurrentWeight == 0)
        //        return;

        //    var pos = Scene.MarkerPosition;
        //    pos += entity.HandouFramesStart;
        //    float currentWeight = morph.CurrentWeight;

        //    //1
        //    this.AddMorphKeyFrame(entity, morph, pos, currentWeight);

        //    //2
        //    pos += entity.EnterFrames;
        //    this.AddMorphKeyFrame(entity, morph, pos, 0, HokanType.Enter);

        //    //3
        //    pos += entity.BlinkingFrames;
        //    this.AddMorphKeyFrame(entity, morph, pos, 0);

        //    //4
        //    pos += entity.ExitFrames;
        //    this.AddMorphKeyFrame(entity, morph, pos, currentWeight, HokanType.Exit);
        //}

        ///// <summary>
        ///// ゆっくり戻す動作をするモーフを作成します。
        ///// </summary>
        ///// <param name="entity"></param>
        ///// <param name="morph"></param>
        //private void AddYuruyakaMorphKey(Args entity, Morph morph)
        //{
        //    if (morph == null)
        //        return;
        //    if (!entity.DoYuruyaka)
        //        return;

        //    //まばたきモーフにゆるやか適用
        //    var pos = Scene.MarkerPosition;
        //    pos += entity.HandouFramesStart + entity.EnterFrames + entity.BlinkingFrames + entity.ExitFrames + entity.HandouFramesEnd;

        //    float currentWeight = morph.CurrentWeight;

        //    var weight = currentWeight + (float)(entity.YuruyakaValue * 0.01);
        //    //1
        //    var hokantype = HokanType.Exit;
        //    if (entity.DoHandouEnd)
        //        hokantype = HokanType.Final;
        //    this.AddMorphKeyFrame(entity, morph, pos, weight, hokantype);

        //    pos += entity.YuruyakaFrame;
        //    //2
        //    this.AddMorphKeyFrame(entity, morph, pos, currentWeight, HokanType.Yuruyaka);

        //    //ボーンもゆるやかを適用
        //    pos = Scene.MarkerPosition;
        //    pos += entity.HandouFramesStart + entity.EnterFrames + entity.BlinkingFrames + entity.ExitFrames + entity.HandouFramesEnd;

        //    var eyeBone = this.Scene.ActiveModel.Bones.Where(n => n.Name == entity.ModelInfo.EyeSyncBoneName.TrimSafe()).FirstOrDefault();
        //    MotionLayer layer = eyeBone.Layers.FirstOrDefault();
        //    if (entity.CreateEyeMotionLayer)
        //    {
        //        if (!eyeBone.Layers.Any(n => n.Name == "まばたき連動"))
        //            eyeBone.AddLayer("まばたき連動");
        //        layer = eyeBone.Layers.Where(n => n.Name == "まばたき連動").FirstOrDefault();
        //    }

        //    float downermovment = entity.ModelInfo.EyeSyncValueDown * (float)(entity.YuruyakaValue * -0.005);
        //    MotionData currentstate = layer.CurrentLocalMotion;

        //    var frame = layer.Frames.Where(n => n.FrameNumber == pos).FirstOrDefault();
        //    if (frame != null)
        //    {
        //        frame.Quaternion = frame.Quaternion.AddEular((new DxMath.Vector3(downermovment, 0, 0)));
        //    }

        //    pos += entity.YuruyakaFrame;
        //    frame = new MotionFrameData(pos, currentstate.Move, currentstate.Rotation);
        //    frame.FrameNumber = pos;

        //    HokanTemplate.ApplyHokan(entity, frame, HokanType.Yuruyaka);
        //    layer.Frames.AddKeyFrame((MotionFrameData)frame);
        //}

        ///// <summary>
        ///// 反動モーフを作成します。
        ///// </summary>
        ///// <param name="entity"></param>
        ///// <param name="morph"></param>
        ///// <param name="argvalue"></param>
        //private void AddHandouMorphKey(Args entity, Morph morph, float argvalue)
        //{
        //    if (morph == null)
        //        return;
        //    if (argvalue == 0)
        //        return;

        //    if (entity.DoHandouStart && entity.HandouFramesStart > 0)
        //    {
        //        //始反動を付ける
        //        var currentWeight = morph.CurrentWeight;
        //        var pos = Scene.MarkerPosition;

        //        //1
        //        this.AddMorphKeyFrame(entity, morph, pos, currentWeight);

        //        pos += entity.HandouFramesStart;
        //        var value = Math.Min(currentWeight + argvalue, 1f);
        //        //2
        //        this.AddMorphKeyFrame(entity, morph, pos, value, HokanType.Exit);

        //        pos += entity.EnterFrames;
        //        //3
        //        this.AddMorphKeyFrame(entity, morph, pos, currentWeight, HokanType.Enter);
        //    }

        //    if (entity.DoHandouEnd && entity.HandouFramesEnd > 0)
        //    {
        //        //後反動を付ける
        //        var currentWeight = morph.CurrentWeight;
        //        var pos = Scene.MarkerPosition;
        //        pos += entity.HandouFramesStart + entity.EnterFrames + entity.BlinkingFrames;

        //        //1
        //        this.AddMorphKeyFrame(entity, morph, pos, currentWeight);

        //        pos += entity.ExitFrames;
        //        var value = Math.Min(currentWeight + argvalue, 1f);
        //        //2
        //        this.AddMorphKeyFrame(entity, morph, pos, value, HokanType.Exit);

        //        pos += entity.HandouFramesEnd;
        //        //3
        //        this.AddMorphKeyFrame(entity, morph, pos, currentWeight, HokanType.Exit);
        //    }
        //}

        ///// <summary>
        ///// 両目ボーン連動を作成します。
        ///// </summary>
        ///// <param name="entity"></param>
        ///// <param name="morph"></param>
        ///// <param name="argvalue"></param>
        //private void AddEyeBone(Args entity)
        //{
        //    if (entity.ModelInfo.EyeSyncBoneName.TrimSafe() == "")
        //        return;
        //    var eyeBone = this.Scene.ActiveModel.Bones.Where(n => n.Name == entity.ModelInfo.EyeSyncBoneName.TrimSafe()).FirstOrDefault();
        //    if (eyeBone != null)
        //    {
        //        float uppermovement = entity.ModelInfo.EyeSyncValueUp;
        //        float downermovment = entity.ModelInfo.EyeSyncValueDown * -1;

        //        MotionLayer layer = eyeBone.Layers.FirstOrDefault();
        //        if (entity.CreateEyeMotionLayer)
        //        {
        //            if (!eyeBone.Layers.Any(n => n.Name == "まばたき連動"))
        //                eyeBone.AddLayer("まばたき連動");
        //            layer = eyeBone.Layers.Where(n => n.Name == "まばたき連動").FirstOrDefault();
        //        }

        //        var pos = Scene.MarkerPosition;
        //        MotionData currentstate = layer.CurrentLocalMotion;

        //        var frame = new MotionFrameData(pos, currentstate.Move, currentstate.Rotation);
        //        layer.Frames.AddKeyFrame(frame);

        //        if (entity.DoHandouStart && entity.HandouFramesStart > 0)
        //        {
        //            pos += entity.HandouFramesStart;
        //            frame = new MotionFrameData(pos, currentstate.Move, currentstate.Rotation.AddEular(new DxMath.Vector3(uppermovement, 0, 0)));

        //            frame.FrameNumber = pos;

        //            HokanTemplate.ApplyHokan(entity, frame, HokanType.Exit);

        //            layer.Frames.AddKeyFrame(frame);
        //        }
        //        pos += entity.EnterFrames;
        //        frame = new MotionFrameData(pos, currentstate.Move, currentstate.Rotation.AddEular(new DxMath.Vector3(downermovment, 0, 0)));
        //        frame.FrameNumber = pos;

        //        HokanTemplate.ApplyHokan(entity, frame, HokanType.Enter);

        //        layer.Frames.AddKeyFrame(frame);

        //        pos += entity.BlinkingFrames;
        //        frame = new MotionFrameData(pos, currentstate.Move, currentstate.Rotation.AddEular(new DxMath.Vector3(downermovment, 0, 0)));
        //        frame.FrameNumber = pos;

        //        layer.Frames.AddKeyFrame(frame);

        //        if (entity.DoHandouEnd && entity.HandouFramesEnd > 0)
        //        {
        //            pos += entity.ExitFrames;
        //            frame = new MotionFrameData(pos, currentstate.Move, currentstate.Rotation.AddEular(new DxMath.Vector3(uppermovement, 0, 0)));
        //            frame.FrameNumber = pos;

        //            HokanTemplate.ApplyHokan(entity, frame, HokanType.Exit);

        //            layer.Frames.AddKeyFrame(frame);

        //            pos += entity.HandouFramesEnd;
        //            frame = new MotionFrameData(pos, currentstate.Move, currentstate.Rotation);
        //            frame.FrameNumber = pos;

        //            HokanTemplate.ApplyHokan(entity, frame, HokanType.Final);

        //            layer.Frames.AddKeyFrame(frame);
        //        }
        //        else
        //        {
        //            pos += entity.ExitFrames;
        //            frame = new MotionFrameData(pos, currentstate.Move, currentstate.Rotation);
        //            frame.FrameNumber = pos;
        //            HokanTemplate.ApplyHokan(entity, frame, HokanType.Exit);

        //            layer.Frames.AddKeyFrame(frame);
        //        }
        //    }
        //}
    }
}