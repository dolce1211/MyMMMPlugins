using MikuMikuPlugin;
using MyUtility;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AutoBlinkerPlugin
{
    public class AutoBlinkerPlugin : IResidentPlugin
    {
        [DllImport("kernel32.dll")]
        private static extern bool AllocConsole();

        /// <summary>
        /// このプラグインのGUID
        /// </summary>
        public Guid GUID
        {
            get { return new Guid("7dcbbf1b-7b8f-bc51-d7ce-50c32ddf8a7e"); }
        }

        /// <summary>
        /// プラグインの名前や作者名、プラグインの説明
        /// </summary>
        public string Description
        {
            get { return "まばたき生成"; }
        }

        /// <summary>
        /// メインフォーム
        /// MikuMikuMoving側から与えられます。
        /// ダイアログ表示やメッセージ表示に使用してください。
        /// </summary>
        public IWin32Window ApplicationForm { get; set; }

        /// <summary>
        /// ボタンに表示するテキスト
        /// 日本語環境で表示されるテキストです。改行する場合は Environment.NewLineを使用してください。
        /// </summary>
        public string Text
        {
            get { return "まばたき生成"; }
        }

        /// <summary>
        /// ボタンに表示する英語テキスト
        /// 日本以外の環境で表示されるテキストです。
        /// </summary>
        public string EnglishText
        {
            get { return "AutoBlinker"; }
        }

        /// <summary>
        /// ボタンに表示するアイコン画像(32x32)
        /// nullだとデフォルト画像が表示されます。
        /// </summary>
        public Image Image
        {
            get { return Properties.Resources._32i; }
        }

        /// <summary>
        /// 中コマンドバーに表示するアイコン画像(20x20)
        /// nullだとデフォルト画像が表示されます。
        /// </summary>
        public Image SmallImage
        {
            get { return Properties.Resources._22i; }
        }

        /// <summary>
        /// シーンオブジェクト
        /// MikuMikuMoving側から与えられます。
        /// MikuMikuMovingのモデルやアクセサリといったオブジェクトにアクセスできます。
        /// </summary>
        public Scene Scene { get; set; }

        private frmMain _frm = null;

        public void Disabled()
        {
            if (_frm != null)
            {
                _frm.Executed -= EventInvoked;
                _frm?.SaveState();
                _frm?.Dispose();
                _frm = null;
            }
        }

        public void Dispose()
        {
            if (_frm != null)
            {
                _frm.Executed -= EventInvoked;
                _frm?.SaveState();
                _frm?.Dispose();
                _frm = null;
            }
        }

        public void Enabled()
        {
            _selectedModel = null;
            _frm?.Dispose();
            _frm = null;
            _frm = new frmMain();
            _frm.Executed += EventInvoked;
            _frm.Show(ApplicationForm);
        }

        public void Initialize()
        {
#if DEBUG
            AllocConsole();
#endif
        }

        private Model _selectedModel = null;

        public void Update(float Frame, float ElapsedTime)
        {
            if (Scene.ActiveModel?.ID != _selectedModel?.ID)
            {
                _selectedModel = Scene.ActiveModel;

                _frm?.ModelChanged(Scene);
            }
        }

        private void EventInvoked(object sender, EventArgs e)
        {
            Entity entity = _frm.RetEntiy;
            if (entity == null)
                return;
            if (string.IsNullOrWhiteSpace(entity.ModelInfo.BlinkingMorphName))
                return;
            if (this.Scene.ActiveModel == null)
                return;

            var blinkMorph = this.Scene.ActiveModel.Morphs
                                    .Where(n => n.PanelType == PanelType.Eyes && n.Name == entity.ModelInfo.BlinkingMorphName).FirstOrDefault();
            var bikkuriMorph = this.Scene.ActiveModel.Morphs
                                    .Where(n => n.PanelType == PanelType.Eyes && n.Name == entity.ModelInfo.BikkuriMorphName).FirstOrDefault();

            if (blinkMorph != null)
            {
                //その他の反転モーフを生成する
                foreach (var morph in this.Scene.ActiveModel.Morphs
                                    .Where(n => n != blinkMorph &&
                                                n != bikkuriMorph &&
                                                n.PanelType == PanelType.Eyes && n.CurrentWeight > 0)
                                    )
                {
                    if (entity.Exceptions.IndexOf(morph.Name) < 0)
                        this.AddInvertMorphKey(entity, morph);
                }

                //まばたきモーフを生成する
                this.AddBlinkMorphKey(entity, blinkMorph, 1f);

                //反動を付ける
                if (bikkuriMorph != null && entity.ModelInfo.BikkuriMorphValue > 0)
                {
                    this.AddHandouMorphKey(entity, bikkuriMorph, entity.ModelInfo.BikkuriMorphValue);
                }

                if (entity.DoEyebrowSync)
                {
                    //まゆ連動
                    var mayuDownMorph = this.Scene.ActiveModel.Morphs
                            .Where(n => n.PanelType == PanelType.Brow && n.Name == entity.ModelInfo.EyebrowDownMorphName).FirstOrDefault();
                    var mayuUpMorph = this.Scene.ActiveModel.Morphs
                            .Where(n => n.PanelType == PanelType.Brow && n.Name == entity.ModelInfo.EyebrowUpMorphName).FirstOrDefault();

                    //まゆ下連動
                    this.AddBlinkMorphKey(entity, mayuDownMorph, entity.ModelInfo.EyebrowDownSyncValue);

                    //まゆ上連動
                    this.AddHandouMorphKey(entity, mayuUpMorph, entity.ModelInfo.EyebrowUpSyncValue);
                }

                //目連動を行う
                if (entity.DoEyeSync)
                {
                    this.AddEyeBone(entity);
                }

                //ゆっくり戻す
                if (entity.DoYuruyaka)
                {
                    this.AddYuruyakaMorphKey(entity, blinkMorph);
                }

                ((Form)this.ApplicationForm).Refresh();
            }
        }

        private enum HokanType
        {
            /// <summary>
            /// デフォルト値
            /// </summary>
            Default,

            /// <summary>
            /// 閉じようとする時の補完
            /// </summary>
            Enter,

            /// <summary>
            /// 開き切る時の補完
            /// </summary>
            Exit,

            /// <summary>
            /// 後反動の最後の補完
            /// </summary>
            Final,

            /// <summary>
            /// ゆるやかに戻す補完
            /// </summary>
            Yuruyaka
        }

        private class HokanTemplate
        {
            public static void ApplyHokanToLastOrDefault(Entity entity, Morph morph, HokanType hokantype)
            {
                if (morph == null)
                    return;
                ApplyHokan(entity, morph.Frames.LastOrDefault(), hokantype);
            }

            public static void ApplyHokan(Entity entity, IMorphFrameData mf, HokanType hokantype)
            {
                if (!entity.DoHokan)
                    return;
                if (mf == null)
                    return;

                InterpolatePoint[] ip = CreateIP(hokantype);
                if (ip.Length < 2)
                    return;

                mf.InterpolA = ip[0];
                mf.InterpolB = ip[1];
            }

            public static void ApplyHokan(Entity entity, IMotionFrameData mtf, HokanType hokantype)
            {
                if (!entity.DoHokan)
                    return;
                if (mtf == null)
                    return;

                InterpolatePoint[] ip = CreateIP(hokantype);
                if (ip.Length < 2)
                    return;

                mtf.InterpolRA = ip[0];
                mtf.InterpolRB = ip[1];
            }

            private static InterpolatePoint[] CreateIP(HokanType hokanType)
            {
                InterpolatePoint[] ip = new InterpolatePoint[] { };
                switch (hokanType)
                {
                    case HokanType.Default:
                        ip = new InterpolatePoint[]
{
                            new InterpolatePoint(20, 20),
                            new InterpolatePoint(107, 107)
};
                        break;

                    case HokanType.Enter:
                        ip = new InterpolatePoint[]
                        {
                            new InterpolatePoint(30, 0),
                            new InterpolatePoint(127, 100)
                        };
                        break;

                    case HokanType.Exit:
                        ip = new InterpolatePoint[]
                        {
                            new InterpolatePoint(30, 0),
                            new InterpolatePoint(40, 127)
                        };
                        break;

                    case HokanType.Final:
                        ip = new InterpolatePoint[]
                        {
                            new InterpolatePoint(70, 0),
                            new InterpolatePoint(57, 127)
                        };
                        break;

                    case HokanType.Yuruyaka:
                        ip = new InterpolatePoint[]
                        {
                            new InterpolatePoint(00, 30),
                            new InterpolatePoint(97, 127)
                        };
                        break;

                    default:
                        break;
                }
                return ip;
            }
        }

        /// <summary>
        /// モーフのキーフレームを打ちます。
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="morph"></param>
        /// <param name="pos"></param>
        /// <param name="weight"></param>
        /// <param name="hokan"></param>
        private void AddMorphKeyFrame(Entity entity, Morph morph, long pos, float weight, HokanType hokan = HokanType.Default)
        {
            morph.Frames.RemoveKeyFrame(pos);

            var newframe = new MorphFrameData(pos, weight);

            HokanTemplate.ApplyHokan(entity, newframe, hokan);

            morph.Frames.AddKeyFrame(newframe);
        }

        /// <summary>
        /// まばたきモーフを作成します。
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="morph"></param>
        /// <param name="argvalue"></param>
        private void AddBlinkMorphKey(Entity entity, Morph morph, float argvalue)
        {
            if (morph == null)
                return;
            if (argvalue == 0)
                return;

            float currentWeight = morph.CurrentWeight;
            var value = Math.Min(currentWeight + argvalue, 1f);
            var pos = Scene.MarkerPosition;
            pos += entity.HandouFramesStart;

            //1
            this.AddMorphKeyFrame(entity, morph, pos, currentWeight);

            //2
            pos += entity.EnterFrames;
            this.AddMorphKeyFrame(entity, morph, pos, value, HokanType.Enter);

            //3
            pos += entity.BlinkingFrames;
            this.AddMorphKeyFrame(entity, morph, pos, value);

            //4
            pos += entity.ExitFrames;
            this.AddMorphKeyFrame(entity, morph, pos, currentWeight, HokanType.Exit);
        }

        /// <summary>
        /// まばたきと反対の動作をするモーフを作成します。
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="morph"></param>
        private void AddInvertMorphKey(Entity entity, Morph morph)
        {
            if (morph == null)
                return;
            if (morph.CurrentWeight == 0)
                return;

            var pos = Scene.MarkerPosition;
            pos += entity.HandouFramesStart;
            float currentWeight = morph.CurrentWeight;

            //1
            this.AddMorphKeyFrame(entity, morph, pos, currentWeight);

            //2
            pos += entity.EnterFrames;
            this.AddMorphKeyFrame(entity, morph, pos, 0, HokanType.Enter);

            //3
            pos += entity.BlinkingFrames;
            this.AddMorphKeyFrame(entity, morph, pos, 0);

            //4
            pos += entity.ExitFrames;
            this.AddMorphKeyFrame(entity, morph, pos, currentWeight, HokanType.Exit);
        }

        /// <summary>
        /// ゆっくり戻す動作をするモーフを作成します。
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="morph"></param>
        private void AddYuruyakaMorphKey(Entity entity, Morph morph)
        {
            if (morph == null)
                return;
            if (!entity.DoYuruyaka)
                return;

            //まばたきモーフにゆるやか適用
            var pos = Scene.MarkerPosition;
            pos += entity.HandouFramesStart + entity.EnterFrames + entity.BlinkingFrames + entity.ExitFrames + entity.HandouFramesEnd;

            float currentWeight = morph.CurrentWeight;

            var weight = currentWeight + (float)(entity.YuruyakaValue * 0.01);
            //1
            var hokantype = HokanType.Exit;
            if (entity.DoHandouEnd)
                hokantype = HokanType.Final;
            this.AddMorphKeyFrame(entity, morph, pos, weight, hokantype);

            pos += entity.YuruyakaFrame;
            //2
            this.AddMorphKeyFrame(entity, morph, pos, currentWeight, HokanType.Yuruyaka);

            //ボーンもゆるやかを適用
            pos = Scene.MarkerPosition;
            pos += entity.HandouFramesStart + entity.EnterFrames + entity.BlinkingFrames + entity.ExitFrames + entity.HandouFramesEnd;

            var eyeBone = this.Scene.ActiveModel.Bones.Where(n => n.Name == entity.ModelInfo.EyeSyncBoneName.TrimSafe()).FirstOrDefault();
            MotionLayer layer = eyeBone.Layers.FirstOrDefault();
            if (entity.CreateEyeMotionLayer)
            {
                if (!eyeBone.Layers.Any(n => n.Name == "まばたき連動"))
                    eyeBone.AddLayer("まばたき連動");
                layer = eyeBone.Layers.Where(n => n.Name == "まばたき連動").FirstOrDefault();
            }

            float downermovment = entity.ModelInfo.EyeSyncValueDown * (float)(entity.YuruyakaValue * -0.005);
            MotionData currentstate = layer.CurrentLocalMotion;

            var frame = layer.Frames.Where(n => n.FrameNumber == pos).FirstOrDefault();
            if (frame != null)
            {
                frame.Quaternion = frame.Quaternion.AddEular((new DxMath.Vector3(downermovment, 0, 0)));
            }

            pos += entity.YuruyakaFrame;
            frame = new MotionFrameData(pos, currentstate.Move, currentstate.Rotation);
            frame.FrameNumber = pos;

            HokanTemplate.ApplyHokan(entity, frame, HokanType.Yuruyaka);
            layer.Frames.AddKeyFrame((MotionFrameData)frame);
        }

        /// <summary>
        /// 反動モーフを作成します。
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="morph"></param>
        /// <param name="argvalue"></param>
        private void AddHandouMorphKey(Entity entity, Morph morph, float argvalue)
        {
            if (morph == null)
                return;
            if (argvalue == 0)
                return;

            if (entity.DoHandouStart && entity.HandouFramesStart > 0)
            {
                //始反動を付ける
                var currentWeight = morph.CurrentWeight;
                var pos = Scene.MarkerPosition;

                //1
                this.AddMorphKeyFrame(entity, morph, pos, currentWeight);

                pos += entity.HandouFramesStart;
                var value = Math.Min(currentWeight + argvalue, 1f);
                //2
                this.AddMorphKeyFrame(entity, morph, pos, value, HokanType.Exit);

                pos += entity.EnterFrames;
                //3
                this.AddMorphKeyFrame(entity, morph, pos, currentWeight, HokanType.Enter);
            }

            if (entity.DoHandouEnd && entity.HandouFramesEnd > 0)
            {
                //後反動を付ける
                var currentWeight = morph.CurrentWeight;
                var pos = Scene.MarkerPosition;
                pos += entity.HandouFramesStart + entity.EnterFrames + entity.BlinkingFrames;

                //1
                this.AddMorphKeyFrame(entity, morph, pos, currentWeight);

                pos += entity.ExitFrames;
                var value = Math.Min(currentWeight + argvalue, 1f);
                //2
                this.AddMorphKeyFrame(entity, morph, pos, value, HokanType.Exit);

                pos += entity.HandouFramesEnd;
                //3
                this.AddMorphKeyFrame(entity, morph, pos, currentWeight, HokanType.Exit);
            }
        }

        /// <summary>
        /// 両目ボーン連動を作成します。
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="morph"></param>
        /// <param name="argvalue"></param>
        private void AddEyeBone(Entity entity)
        {
            if (entity.ModelInfo.EyeSyncBoneName.TrimSafe() == "")
                return;
            var eyeBone = this.Scene.ActiveModel.Bones.Where(n => n.Name == entity.ModelInfo.EyeSyncBoneName.TrimSafe()).FirstOrDefault();
            if (eyeBone != null)
            {
                float uppermovement = entity.ModelInfo.EyeSyncValueUp;
                float downermovment = entity.ModelInfo.EyeSyncValueDown * -1;

                MotionLayer layer = eyeBone.Layers.FirstOrDefault();
                if (entity.CreateEyeMotionLayer)
                {
                    if (!eyeBone.Layers.Any(n => n.Name == "まばたき連動"))
                        eyeBone.AddLayer("まばたき連動");
                    layer = eyeBone.Layers.Where(n => n.Name == "まばたき連動").FirstOrDefault();
                }

                var pos = Scene.MarkerPosition;
                MotionData currentstate = layer.CurrentLocalMotion;

                var frame = new MotionFrameData(pos, currentstate.Move, currentstate.Rotation);
                layer.Frames.AddKeyFrame(frame);

                if (entity.DoHandouStart && entity.HandouFramesStart > 0)
                {
                    pos += entity.HandouFramesStart;
                    frame = new MotionFrameData(pos, currentstate.Move, currentstate.Rotation.AddEular(new DxMath.Vector3(uppermovement, 0, 0)));

                    frame.FrameNumber = pos;

                    HokanTemplate.ApplyHokan(entity, frame, HokanType.Exit);

                    layer.Frames.AddKeyFrame(frame);
                }
                pos += entity.EnterFrames;
                frame = new MotionFrameData(pos, currentstate.Move, currentstate.Rotation.AddEular(new DxMath.Vector3(downermovment, 0, 0)));
                frame.FrameNumber = pos;

                HokanTemplate.ApplyHokan(entity, frame, HokanType.Enter);

                layer.Frames.AddKeyFrame(frame);

                pos += entity.BlinkingFrames;
                frame = new MotionFrameData(pos, currentstate.Move, currentstate.Rotation.AddEular(new DxMath.Vector3(downermovment, 0, 0)));
                frame.FrameNumber = pos;

                layer.Frames.AddKeyFrame(frame);

                if (entity.DoHandouEnd && entity.HandouFramesEnd > 0)
                {
                    pos += entity.ExitFrames;
                    frame = new MotionFrameData(pos, currentstate.Move, currentstate.Rotation.AddEular(new DxMath.Vector3(uppermovement, 0, 0)));
                    frame.FrameNumber = pos;

                    HokanTemplate.ApplyHokan(entity, frame, HokanType.Exit);

                    layer.Frames.AddKeyFrame(frame);

                    pos += entity.HandouFramesEnd;
                    frame = new MotionFrameData(pos, currentstate.Move, currentstate.Rotation);
                    frame.FrameNumber = pos;

                    HokanTemplate.ApplyHokan(entity, frame, HokanType.Final);

                    layer.Frames.AddKeyFrame(frame);
                }
                else
                {
                    pos += entity.ExitFrames;
                    frame = new MotionFrameData(pos, currentstate.Move, currentstate.Rotation);
                    frame.FrameNumber = pos;
                    HokanTemplate.ApplyHokan(entity, frame, HokanType.Exit);

                    layer.Frames.AddKeyFrame(frame);
                }
            }
        }
    }
}