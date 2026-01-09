using DxMath;
using MikuMikuPlugin;
using MMDUtil;
using MyUtility;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MoCapModificationHelperPlugin.service
{
    /// <summary>
    /// まばたきモーフをキャンセルする機能を提供するクラス
    /// </summary>
    /// <returns></returns>
    internal class BlinkCancellerService : BaseService
    {
        private static List<string> _exceptions = null;

        private static List<string> TryCreateExceptions()
        {
            // 拙作AutoBlinkerPluginのユーザーの場合、その設定ファイルを参照して例外リストを取得する
            string basepath = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
            var path = System.IO.Path.Combine(basepath, "AutoBlinkerPluginPatterns.xml");
            if (System.IO.File.Exists(path))
            {
                var savedState = MyUtility.Serializer.Deserialize<SavedState>(path);
                if (!string.IsNullOrEmpty(savedState.Exceptions))
                {
                    return savedState.Exceptions.Split(',').ToList();
                }
            }
            return new List<string>();
        }

        public override bool PreExecute()
        {
            if (!base.PreExecute())
                return false;

            if (BlinkCancellerService._exceptions == null)
            {
                //AutoBlinkerPluginを使っているならその例外リストを取得
                BlinkCancellerService._exceptions = BlinkCancellerService.TryCreateExceptions();
            }
            return true;
        }

        public override bool ExecuteInternal(ConfigItem config)
        {
            if (this.Scene.ActiveModel == null)
                return false;

            var ret = false;
            var currentPosition = Scene.MarkerPosition;
            for (int i = 0; i < 2; i++)
            {
                if (i > 0)
                    if (!config.ForSmile)
                        break;
                var blinkMorph = "";
                if (i == 0)
                    blinkMorph = "まばたき";
                else
                    blinkMorph = "笑い";

                // まばたきモーフを取得
                var blickMorph = this.Scene.ActiveModel.Morphs.Where(m => m.PanelType == PanelType.Eyes)
                                                .FirstOrDefault(m => m.Name.Contains(blinkMorph));
                if (blickMorph == null)
                    //まばたきモーフなし
                    return false;

                // まばたきモーフで選択されているフレームを取得
                var blinkSelectedFrames = blickMorph.SelectedFrames;

                foreach (var morph in this.Scene.ActiveModel.Morphs.Where(m => m.PanelType == PanelType.Eyes))
                {
                    if (morph.Name == blickMorph.Name)
                        continue;
                    if (BlinkCancellerService._exceptions.Contains(morph.Name))
                        continue;
                    var affectedFrames = new List<long>();
                    foreach (var blinkFrame in blinkSelectedFrames)
                    {
                        // blinkFrame.FrameNumberより小さいFrameNumberのうち最大のものを取得
                        var currentFrame = morph.Frames
                            .Where(m => !affectedFrames.Contains(m.FrameNumber))
                            .Where(m => m.FrameNumber <= blinkFrame.FrameNumber)
                            .OrderByDescending(m => m.FrameNumber)
                            .FirstOrDefault();

                        if (currentFrame == null)
                            continue;

                        if (blinkFrame.Weight == 1 && currentFrame.Weight > 0)
                        {
                            // まばたき中に他の目モーフが生きている→キャンセルキーフレーム追加
                            var addingFrame = new MorphFrameData(blinkFrame.FrameNumber, currentFrame.Weight - blinkFrame.Weight < 0 ? 0 : currentFrame.Weight - blinkFrame.Weight)
                            {
                                InterpolA = blinkFrame.InterpolA,
                                InterpolB = blinkFrame.InterpolB
                            };

                            morph.Frames.AddKeyFrame(addingFrame);

                            affectedFrames.Add(blinkFrame.FrameNumber);
                        }
                        else if (blinkFrame.Weight == 0 && currentFrame.Weight > 0)
                        {
                            // まばたき開始、終了時に他の目モーフが生きている→そのままキーフレーム追加
                            var addingFrame = new MorphFrameData(blinkFrame.FrameNumber, currentFrame.Weight)
                            {
                                InterpolA = blinkFrame.InterpolA,
                                InterpolB = blinkFrame.InterpolB
                            };
                            morph.Frames.AddKeyFrame(addingFrame);
                        }
                    }
                }
            }

            // モーフの変更を即座に反映させるために画面を更新する
            this.Scene.MarkerPosition += 1;
            this.Scene.MarkerPosition -= 1;
            this.ApplicationForm.Refresh();
            return ret;
        }

        public override void PostExecute()
        {
            base.PostExecute();
        }
    }

    public class SavedState
    {
        /// <summary>
        /// 例外
        /// </summary>
        public string Exceptions { get; set; } = String.Empty;
    }
}