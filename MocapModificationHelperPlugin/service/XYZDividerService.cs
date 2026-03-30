using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using MikuMikuPlugin;

namespace MoCapModificationHelperPlugin.service
{
    internal class XYZDividerService : BaseService
    {
        public override bool ExecuteInternal(ConfigItem config)
        {
            if (this.Scene?.ActiveModel == null)
                return false;

            var selectedFrames = this.Scene.ActiveModel.Bones.SelectMany(b =>
                                b.SelectedLayers.Select(l => (bone: b, layer: l, frames: l.SelectedFrames))).ToList();
            var grp = selectedFrames.GroupBy(t => t.layer);
            if (grp.Count() > 1)
                // 選択レイヤーが複数ある場合は無視
                return false;

            var tuple = grp.FirstOrDefault().FirstOrDefault();
            if (tuple.frames.All(f => f.Position == new DxMath.Vector3()))
                // 全く移動が無い場合は無視
                return false;

            var yFrames = new List<IMotionFrameData>();
            for (int i = 0; i < 3; i++)
            {
                var name = i == 0 ? "X" : i == 1 ? "Y" : "Z";
                var layerName = $"{tuple.bone.Name}_{name}";
                var layer = tuple.bone.Layers.FirstOrDefault(l => l.Name == layerName);
                if (layer == null)
                {
                    tuple.bone.AddLayer(layerName);
                    layer = tuple.bone.Layers.FirstOrDefault(l => l.Name == layerName);
                }
                var sign = 0;//プラス、マイナス
                var prevValue = 0f;
                var prevAddedValue = 0f;
                IMotionFrameData prevFrame = null;

                foreach (var frame in tuple.frames)
                {
                    var add = false;

                    var p = typeof(DxMath.Vector3).GetField(name);
                    var value = (float)p.GetValue(frame.Position);

                    if (prevFrame != null)
                    {
                        if (value * sign < prevValue * sign)
                            add = true;
                        if (prevFrame.FrameNumber == tuple.frames.FirstOrDefault().FrameNumber)
                            add = true;
                        if (add)
                        {
                            //あまり変化量が小さいようなら無視する
                            if (Math.Abs(value - prevAddedValue) > 0.08)
                            {
                                // 山か谷に遭遇。フレームを打ったうえで上下入れ替える
                                var pos = i == 0 ? new DxMath.Vector3(prevValue, 0, 0) :
                                                          i == 1 ? new DxMath.Vector3(0, prevValue, 0) :
                                                                   new DxMath.Vector3(0, 0, prevValue);
                                var addFrame = new MotionFrameData(prevFrame.FrameNumber, pos, prevFrame.Quaternion);
                                layer.Frames.AddKeyFrame(addFrame);
                                if (name == "Y")
                                    yFrames.Add(addFrame);
                                prevAddedValue = prevValue;
                            }

                            if (prevFrame.FrameNumber != tuple.frames.FirstOrDefault().FrameNumber)
                                sign *= -1;
                        }
                    }
                    if (sign == 0 && prevFrame != null)
                    {
                        //初回の方向を調べる
                        if (value >= prevValue)
                            //上っている
                            sign = 1;
                        else
                            //下っている
                            sign = -1;
                    }
                    prevFrame = frame;
                    prevValue = value;
                }
                layer.Frames.AddKeyFrame(new MotionFrameData(prevFrame.FrameNumber, prevFrame.Position, prevFrame.Quaternion));
            }

            if (tuple.bone.Name == "センター")
            {
                // 下半身をセンターのY軸に合わせて肉抜きする
                var lowerBody = this.Scene.ActiveModel.Bones.FirstOrDefault(b => b.Name == "下半身");
                if (lowerBody != null)
                {
                    var noExistCount = 0;
                    var lowerBodyLayer = lowerBody.Layers.FirstOrDefault();
                    for (long fr = 0; fr < 99999; fr++)
                    {
                        if (lowerBodyLayer.Frames.Any(f => f.FrameNumber == fr))
                        {
                            if (!yFrames.Any(f => f.FrameNumber == fr))
                            {
                                lowerBodyLayer.Frames.RemoveKeyFrame(fr);
                            }
                        }
                        else
                        {
                            noExistCount++;
                            if (noExistCount > 5)
                                // 連続キーが途絶えたなら抜ける
                                break;
                        }
                    }
                }
            }

            return true;
        }
    }
}