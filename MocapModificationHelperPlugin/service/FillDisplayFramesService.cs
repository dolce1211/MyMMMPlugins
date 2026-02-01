using Linearstar.Keystone.IO.MikuMikuDance;
using MikuMikuPlugin;
using MMDUtil;
using System.Collections.Generic;
using System.Linq;

namespace MoCapModificationHelperPlugin.service
{
    /// <summary>
    /// 表示枠内のキーフレームを操作する機能を提供するサービスクラス
    /// </summary>
    internal class FillDisplayFramesService : BaseService
    {
        public override bool ExecuteInternal(ConfigItem config)
        {
            if (this.Scene.ActiveModel == null)
                return false;

            // まずモーフを扱う
            var selectedMorphs = this.Scene.ActiveModel.Morphs.Where(m => m.Selected)
                .Select(morph =>
                {
                    var df = this.Scene.ActiveModel.FindDisplayFramesFromMorph(morph);
                    return df;
                });
            selectedMorphs.ForEach(f => f.Morphs.ForEach(m => m.Selected = true));

            var selectedLayers = this.Scene.ActiveModel.Bones
                .SelectMany(b =>
                {
                    return b.SelectedLayers.Select(layer => (name: b.Name, bone: b, layer: layer));
                });
            if (selectedLayers == null)
                return false;

            List<DisplayFrame> displayFrames = selectedLayers.Select(tuple =>
            {
                return this.Scene.ActiveModel.FindDisplayFramesFromBone(tuple.bone);
            }).Distinct().ToList();
            if (displayFrames?.Count == 0)
                return false;

            // いったんキー全解除
            this.Scene.ActiveModel.Bones.ForEach(b =>
            {
                b.SelectedLayers.ForEach(l =>
                {
                    l.SelectedFrames.ForEach(f => f.Selected = false);
                });
            });
            // カレントポジションの表示枠内の全キーを選択
            foreach (var displayFrame in displayFrames)
            {
                var layers = displayFrame.Bones.SelectMany(b => b.Layers);

                layers.Select(l => l.Frames.FirstOrDefault(f => f.FrameNumber == this.Scene.MarkerPosition))
                    .Where(k => k != null)
                    ?.ForEach(f => f.Selected = true);

                layers.ForEach(l => l.Selected = true);
            }
            return true;
        }
    }
}