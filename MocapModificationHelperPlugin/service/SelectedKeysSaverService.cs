using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Linearstar.Keystone.IO.MikuMikuDance;
using MikuMikuPlugin;
using MMDUtil;

namespace MoCapModificationHelperPlugin.service
{
    internal class KeySaverHistory
    {
        public DateTime DateTime = DateTime.MinValue;
        public List<string> SelectedBones = null;
        public List<string> SelectedMorphs = null;
    }

    /// <summary>
    /// 現在のキー選択状態をコピーする機能を提供するクラス
    /// </summary>
    internal class SelectedKeysSaverService : BaseService
    {
        public static List<KeySaverHistory> Histories = new List<KeySaverHistory>();

        public override bool ExecuteInternal(ConfigItem config)
        {
            if (this.Scene.ActiveModel == null)
                return false;

            var tmpSelectedKeys = new List<string>();
            foreach (var tuple in this.Scene.ActiveModel.Bones.SelectMany(b => b.Layers.Select(l => (bone: b, layer: l))))
            {
                if (this.Scene.ActiveModel.FindDisplayFramesFromBone(tuple.bone) != null)
                {
                    var selectedFrames = tuple.layer.SelectedFrames?

                        .Select(f =>
                        {
                            return (bone: tuple.bone, layer: tuple.layer, frame: f);
                        }
                        ).ToList();
                    if (selectedFrames?.Count > 0)
                    {
                        if (tmpSelectedKeys != null)
                            tmpSelectedKeys.AddRange(selectedFrames.Select(t => $"{t.bone.Name}|{t.layer.Name ?? ""}|{t.frame.FrameNumber}"));
                    }
                }
            }

            var tmpSelectedMortphs = new List<string>();
            foreach (var morph in this.Scene.ActiveModel.Morphs)
            {
                if (this.Scene.ActiveModel.FindDisplayFramesFromMorph(morph) != null)
                {
                    if (morph.SelectedFrames == null)
                        continue;
                    tmpSelectedMortphs.AddRange(morph.SelectedFrames.Select(f => $"{morph.Name}|{f.FrameNumber}"));
                }
            }
            if (tmpSelectedKeys.Count + tmpSelectedMortphs.Count == 0)
            {
                return false;
            }
            do
            {
                if (Histories.Count <= 9)
                    break;
                Histories.RemoveAt(Histories.Count - 1);
            } while (true);
            var history = new KeySaverHistory()
            {
                DateTime = DateTime.Now,
                SelectedBones = tmpSelectedKeys,
                SelectedMorphs = tmpSelectedMortphs
            };
            Histories.Insert(0, history);
            return true;
        }
    }

    /// <summary>
    /// SelectedKeysSaverServiceでコピーされたキー選択状態を復元する機能を提供するクラス
    /// </summary>
    internal class SelectedKeysLoaderService : BaseService
    {
        public int HistoryIndex { get; set; } = -1;

        public override bool ExecuteInternal(ConfigItem config)
        {
            if (this.Scene.ActiveModel == null)
                return false;
            if (SelectedKeysSaverService.Histories.Count == 0)
                return false;
            if (HistoryIndex < 0 || HistoryIndex >= SelectedKeysSaverService.Histories.Count)
                return false;
            var history = SelectedKeysSaverService.Histories[this.HistoryIndex];
            //if (SelectedKeysSaverService.SelectedBones?.Count() > 0)
            //{
            foreach (var tuple in this.Scene.ActiveModel.Bones.SelectMany(b => b.Layers.Select(l => (bone: b, layer: l))))
            {
                tuple.layer.Frames.ForEach(f => f.Selected = false);
                foreach (var f in tuple.layer.Frames)
                {
                    if (this.Scene.ActiveModel.FindDisplayFramesFromBone(tuple.bone) != null)
                    {
                        var t = (bone: tuple.bone, layer: tuple.layer, frame: f);
                        var keyString = $"{t.bone.Name}|{t.layer.Name ?? ""}|{t.frame.FrameNumber}";
                        if (history.SelectedBones != null && history.SelectedBones.Contains(keyString))
                        {
                            f.Selected = true;
                            if (!tuple.layer.Selected)
                                tuple.layer.Selected = true;
                        }
                        else
                            f.Selected = false;
                    }
                }
            }
            //}
            //if (SelectedKeysSaverService.SelectedMorphs?.Count() > 0)
            //{
            foreach (var morph in this.Scene.ActiveModel.Morphs)
            {
                if (this.Scene.ActiveModel.FindDisplayFramesFromMorph(morph) != null)
                {
                    foreach (var f in morph.Frames)
                    {
                        var keyString = $"{morph.Name}|{f.FrameNumber}";
                        if (history.SelectedMorphs != null && history.SelectedMorphs.Contains(keyString))
                            f.Selected = true;
                        else
                            f.Selected = false;
                    }
                }
            }
            //}

            return true;
        }

        public override void PostExecute()
        {
            this.HistoryIndex = 0;
            base.PostExecute();
        }
    }
}