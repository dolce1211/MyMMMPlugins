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
                layers.Where(l => l.Frames.Where(f => f.FrameNumber == this.Scene.MarkerPosition) != null).ForEach(l => l.Selected = true);
                var currentKeys = layers.Select(l => l.Frames.FirstOrDefault(f => f.FrameNumber == this.Scene.MarkerPosition))
                    .Where(k => k != null);
                currentKeys.ToList().ForEach(k => k.Selected = true);
            }
            return true;
        }
    }
}