using MikuMikuPlugin;
using MMDUtil;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Management;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MoCapModificationHelperPlugin.service
{
    public class InterpolateService : BaseService
    {
        private static InterpolatePoint DefaultInterpolateA => new InterpolatePoint(20, 20);
        private static InterpolatePoint DefaultInterpolateB => new InterpolatePoint(107, 107);

        private Settings _settings = null;
        private InterpolateType _type = InterpolateType.none;

        private int _pallette = 0;

        public void SetTypeAndPallette(int pallette, InterpolateType type)
        {
            _pallette = pallette;
            _type = type;
        }

        public override bool PreExecute()
        {
            if (Scene.ActiveModel == null)
                if (Scene.ActiveCamera == null)
                    return false;

            // Mikumikumoving.exeと同じパスにあるSettings.xmlを探す
            string assemblyPath = System.Reflection.Assembly.GetEntryAssembly().Location;
            string assemblyDirectory = System.IO.Path.GetDirectoryName(assemblyPath);
            string settingFilePath = System.IO.Path.Combine(assemblyDirectory, "settings.xml");

            // setting.xmlを読み込み
            Settings settings = null;
            if (System.IO.File.Exists(settingFilePath))
            {
                settings = MyUtility.Serializer.Deserialize<Settings>(settingFilePath);
            }
            if (settings?.InterpolPalette?.Count < 6)
            {
                // 6個入ってないなら初期値を設定
                settings = new Settings();
            }
            _settings = settings;
            _affectedFrames = new List<IMotionFrameData>();
            return true;
        }

        private List<IMotionFrameData> _affectedFrames = new List<IMotionFrameData>();

        public override bool ExecuteInternal(ConfigItem config)
        {
            // プラグイン機能を使うと補完曲線の見た目が更新されないのでPostExecuteで実際のボタン押下シミュレートで対応する
            //if (false)
            //{
            //    if (_pallette < -1 || _pallette > 5) // -1はデフォルト値
            //        return false;
            //    if (_type == InterpolateType.none)
            //        return false;

            //    var interpolateA = InterpolateService.DefaultInterpolateA;
            //    var interpolateB = InterpolateService.DefaultInterpolateB;
            //    if (_pallette >= 0 && _pallette < 6)
            //    {
            //        interpolateA = _settings.InterpolPalette[_pallette].A;
            //        interpolateB = _settings.InterpolPalette[_pallette].B;
            //    }

            //    foreach (var tuple in this.Scene.ActiveModel.Bones.SelectMany(b => b.Layers.Select(l => (bone: b, layer: l)))
            //                                                        .SelectMany(t => t.layer.SelectedFrames.Select(f => (Bone: t.bone, layer: t.layer, frame: f))))
            //    {
            //        tuple.frame.InterpolRA = (_type == InterpolateType.ALL || _type == InterpolateType.R) ? interpolateA : tuple.frame.InterpolRA;
            //        tuple.frame.InterpolRB = (_type == InterpolateType.ALL || _type == InterpolateType.R) ? interpolateB : tuple.frame.InterpolRB;
            //        tuple.frame.InterpolXA = ((tuple.Bone.BoneFlags & BoneType.XYZ) == BoneType.XYZ) &&
            //                                    (_type == InterpolateType.ALL || _type == InterpolateType.X) ? interpolateA : tuple.frame.InterpolXA;
            //        tuple.frame.InterpolXB = ((tuple.Bone.BoneFlags & BoneType.XYZ) == BoneType.XYZ) &&
            //                                    (_type == InterpolateType.ALL || _type == InterpolateType.X) ? interpolateB : tuple.frame.InterpolXA;
            //        tuple.frame.InterpolYA = ((tuple.Bone.BoneFlags & BoneType.XYZ) == BoneType.XYZ) &&
            //                                    (_type == InterpolateType.ALL || _type == InterpolateType.Y) ? interpolateA : tuple.frame.InterpolYA;
            //        tuple.frame.InterpolYB = ((tuple.Bone.BoneFlags & BoneType.XYZ) == BoneType.XYZ) &&
            //                                    (_type == InterpolateType.ALL || _type == InterpolateType.Y) ? interpolateB : tuple.frame.InterpolYB;
            //        tuple.frame.InterpolZA = ((tuple.Bone.BoneFlags & BoneType.XYZ) == BoneType.XYZ) &&
            //                                    (_type == InterpolateType.ALL || _type == InterpolateType.Z) ? interpolateA : tuple.frame.InterpolZA;
            //        tuple.frame.InterpolZB = ((tuple.Bone.BoneFlags & BoneType.XYZ) == BoneType.XYZ) &&
            //                                    (_type == InterpolateType.ALL || _type == InterpolateType.Z) ? interpolateB : tuple.frame.InterpolZB;

            //        var fr = tuple.frame.FrameNumber;

            //        var affected = tuple.layer.Frames.FirstOrDefault(f => f.FrameNumber == fr);
            //        if (affected != null)
            //        {
            //            var flg = true;
            //            if ((tuple.Bone.BoneFlags & BoneType.XYZ) == 0)
            //            {
            //                // 回転専用
            //                if (_type == InterpolateType.X || _type == InterpolateType.Y || _type == InterpolateType.Z)
            //                    flg = false;
            //            }
            //            if (flg)
            //            {
            //                _affectedFrames.Add(affected);
            //                affected.Selected = false;
            //            }
            //        }
            //    }

            //    // モーフの補間も設定
            //    foreach (var morph in this.Scene.ActiveModel.Morphs.SelectMany(m => m.SelectedFrames))
            //    {
            //        morph.InterpolA = new InterpolatePoint(interpolateA.X, interpolateA.Y);
            //        morph.InterpolB = new InterpolatePoint(interpolateB.X, interpolateB.Y);
            //    }
            //}

            return true;
        }

        public override void PostExecute()
        {
            base.PostExecute();
            this.ApplicationForm.BeginInvoke(new Action(() =>
            {
                // RXYZタブを切り替え
                var tabCaption = "";

                switch (_type)
                {
                    case InterpolateType.none:
                        break;

                    case InterpolateType.R:
                    case InterpolateType.ALL:
                        tabCaption = "R";
                        if (this.Scene.ActiveCamera != null)
                            tabCaption = "移動";

                        break;

                    case InterpolateType.X:
                        tabCaption = "X";
                        if (this.Scene.ActiveCamera != null)
                            tabCaption = "回転";

                        break;

                    case InterpolateType.Y:
                        tabCaption = "Y";
                        if (this.Scene.ActiveCamera != null)
                            tabCaption = "距離";

                        break;

                    case InterpolateType.Z:
                        tabCaption = "X";
                        if (this.Scene.ActiveCamera != null)
                            tabCaption = "Fov";
                        break;

                    default:
                        break;
                }

                if (!string.IsNullOrEmpty(tabCaption))
                {
                    // RXYZタブを切り替え
                    MMMUtilility.SelectInterpolateTabByControls(this.ApplicationForm, tabCaption);
                }
                if (this.Scene.ActiveModel?.Morphs.Any(m => m.SelectedFrames.Count() > 0) == true)
                {
                    // モーフが選択されている場合は強制的にMタブに切り替えて実行
                    tabCaption = "M";
                    MMMUtilility.SelectInterpolateTabByControls(this.ApplicationForm, "M");
                }

                // 補間曲線パレットボタンを押下して見た目を更新
                if (_pallette >= 0 && _pallette <= 5)
                {
                    // パレット番号は0ベースだが、ボタンは1ベースなので+1する
                    int buttonNumber = _pallette + 1;
                    bool success = MMDUtil.MMMUtilility.ClickInterpolateButtonByControls(this.ApplicationForm, buttonNumber);

                    if (success)
                    {
                        if (_type == InterpolateType.ALL)
                        {
                            // 「すべてにコピーする」ボタンをクリック
                            try
                            {
                                var targetToolTip = "すべてにコピーする";
                                if (this.Scene.ActiveCamera != null)
                                {
                                    targetToolTip = "すべてにコピー";
                                }
                                bool copySuccess = MMMUtilility.ClickInterpolatePalleteButtonByToolTip(this.ApplicationForm, targetToolTip);
                            }
                            catch (Exception)
                            {
                            }
                        }
                    }
                }
                else if (_pallette == -1)
                {
                    // 初期化ボタンをクリック
                    try
                    {
                        // Form.Controlsベースで初期化ボタンを探す
                        var initButton = MMMUtilility.ClickInterpolatePalleteButtonByToolTip(this.ApplicationForm, "初期化");
                    }
                    catch (Exception ex)
                    {
                        System.Diagnostics.Debug.WriteLine($"Init button click failed: {ex.Message}");
                    }
                }

                this.ApplicationForm.Refresh();
                //    }
            }));
        }
    }

    public enum InterpolateType
    {
        none = 0,
        R, X, Y, Z, ALL
    }

    public class Settings
    {
        public Settings()
        {
            // 6個のデフォルト値を設定
            InterpolPalette = new List<InterpolatePointPair>()
                {
                    new InterpolatePointPair()
                    {
                        A = new InterpolatePoint(64,0),
                        B = new InterpolatePoint(64,127),
                    },
                    new InterpolatePointPair()
                    {
                        A = new InterpolatePoint(48,5),
                        B = new InterpolatePoint(80,122),
                    },
                    new InterpolatePointPair()
                    {
                        A = new InterpolatePoint(32,10),
                        B = new InterpolatePoint(96,117),
                    },
                    new InterpolatePointPair()
                    {
                        A = new InterpolatePoint(96,0),
                        B = new InterpolatePoint(32,127),
                    },
                    new InterpolatePointPair()
                    {
                        A = new InterpolatePoint(0,64),
                        B = new InterpolatePoint(64,127),
                    },
                    new InterpolatePointPair()
                    {
                        A = new InterpolatePoint(64,0),
                        B = new InterpolatePoint(127,64),
                    },
                    new InterpolatePointPair()
                };
        }

        public List<InterpolatePointPair> InterpolPalette { get; set; } = new List<InterpolatePointPair>();
    }

    public class InterpolatePointPair
    {
        public InterpolatePoint A { get; set; }
        public InterpolatePoint B { get; set; }
    }
}