using MikuMikuPlugin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DxMath;
using MMDUtil;

namespace MoCapModificationHelperPlugin.service
{
    /// <summary>
    /// 選択されたレイヤーに対して、カレントポジションの一つ前から穴が空いている所まで選択する機能を提供するクラス
    /// </summary>
    /// <returns></returns>
    internal class GapSelectorService : BaseService
    {
        public override bool ExecuteInternal(int mode)
        {
            if (this.Scene.ActiveModel == null)
                return false;

            var selectedLayers = TryGetSubjectBones();

            if (selectedLayers != null)
            {
                return SelectGap(selectedLayers);
            }
            return false;
        }

        private bool SelectGap(List<MotionLayer> selectedLayers)
        {
            if (selectedLayers.Count == 0)
                return false;

            var deletingFrames = new List<IMotionFrameData>();

            var flg = false;
            foreach (var layer in selectedLayers)
            {
                var goal = long.MaxValue;
                //1000フレーム以内に穴があるか確認
                for (long i = Scene.MarkerPosition - 1; i > Scene.MarkerPosition - 1000; i--)
                {
                    if (i <= 1)
                        break;
                    var frame = layer.Frames.FirstOrDefault(f => f.FrameNumber == i);
                    if (frame == null)
                    {
                        if (i < Scene.MarkerPosition - 2)
                        {
                            //穴の二つ前まで選択する
                            goal = i + 2;
                            break;
                        }
                    }
                }
                if (goal < long.MaxValue)
                {
                    //穴直前まで選択
                    flg = true;
                    layer.Frames.Where(f => f.FrameNumber >= goal && f.FrameNumber < Scene.MarkerPosition).ToList()
                                    .ForEach(f => f.Selected = true);
                }
            }
            if (flg)
            {
                //現行フレームはすべて選択解除
                var currentFrames = this.Scene.ActiveModel.Bones
                                                    .SelectMany(n => n.Layers)
                                                    .Select(l => l.Frames.FirstOrDefault(f => f.Selected && f.FrameNumber == Scene.MarkerPosition))
                                                    .Where(f => f != null)
                                                    .ToList();
                currentFrames.ForEach(n => n.Selected = false);
                return true;
            }
            return false;
        }

        /// <summary>
        /// 目的のボーンを取得
        /// </summary>
        /// <param name="bone"></param>
        /// <returns></returns>
        private List<MotionLayer> TryGetSubjectBones()
        {
            var ret = new List<MotionLayer>();
            if (Scene.ActiveModel == null)
                return ret;

            //メインレイヤーのみ抽出
            var selectedMainLayers = (Scene.ActiveModel.Bones.SelectMany(b => b.Layers))
                                        .Where(n => n.Selected).ToList();

            foreach (var selectedLayer in selectedMainLayers)
            {
                var flg = false;
                if (selectedLayer != null)
                {
                    if (selectedLayer.SelectedFrames.Any(f => f.FrameNumber == Scene.MarkerPosition))
                    {
                        //カレントポジションに選択キーがあり
                        if (selectedLayer.Frames.Any(f => f.FrameNumber == Scene.MarkerPosition - 1))
                        {
                            //カレントポジション-1にキーがあるなら対象
                            ret.Add(selectedLayer);
                            flg = true;
                        }
                    }
                }

                //}
                //if (!flg)
                //    return null;
            }

            if (ret.Count == 0)
                return null;
            return ret;
        }
    }
}