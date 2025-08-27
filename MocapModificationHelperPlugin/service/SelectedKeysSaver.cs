using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Linearstar.Keystone.IO.MikuMikuDance;
using MikuMikuPlugin;

namespace MoCapModificationHelperPlugin.service
{
    /// <summary>
    /// 現在のキー選択状態をコピーする機能を提供するクラス
    /// </summary>
    internal class SelectedKeysSaverService : BaseService
    {
        public static List<string> SelectedKeys = null;

        public static string Tuple2String((Bone bone, MotionLayer layer, IMotionFrameData frame) tuple)
        {
            return $"{tuple.bone.Name}|{tuple.layer.Name ?? ""}|{tuple.frame.FrameNumber}";
        }

        public override bool ExecuteInternal(int mode)
        {
            if (this.Scene.ActiveModel == null)
                return false;
            var tmpSelectedKeys = new List<string>();
            foreach (var tuple in this.Scene.ActiveModel.Bones.SelectMany(b => b.Layers.Select(l => (bone: b, layer: l))))
            {
                var selectedFrames = tuple.layer.SelectedFrames?.Select(f =>
                {
                    return (bone: tuple.bone, layer: tuple.layer, frame: f);
                }
                ).ToList();
                if (selectedFrames?.Count > 0)
                {
                    if (tmpSelectedKeys != null)
                        tmpSelectedKeys.AddRange(selectedFrames.Select(t => Tuple2String(t)));
                }
            }
            if (tmpSelectedKeys.Count > 0)
                SelectedKeys = tmpSelectedKeys;

            return true;
        }
    }

    /// <summary>
    /// SelectedKeysSaverServiceでコピーされたキー選択状態を復元する機能を提供するクラス
    /// </summary>
    internal class SelectedKeysLoaderService : BaseService
    {
        public override bool ExecuteInternal(int mode)
        {
            if (this.Scene.ActiveModel == null)
                return false;
            if (SelectedKeysSaverService.SelectedKeys?.Count == 0)
                return false;

            foreach (var tuple in this.Scene.ActiveModel.Bones.SelectMany(b => b.Layers.Select(l => (bone: b, layer: l))))
            {
                tuple.layer.Frames.ForEach(f => f.Selected = false);
                foreach (var f in tuple.layer.Frames)
                {
                    var keyTuple = (bone: tuple.bone, layer: tuple.layer, frame: f);
                    var keyString = SelectedKeysSaverService.Tuple2String(keyTuple);
                    if (SelectedKeysSaverService.SelectedKeys.Contains(keyString))
                        f.Selected = true;
                    else
                        f.Selected = false;
                }
            }
            return true;
        }
    }
}