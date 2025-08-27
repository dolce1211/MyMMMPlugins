using DxMath;
using MikuMikuPlugin;
using MyUtility;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MoCapModificationHelperPlugin.service
{
    /// <summary>
    /// カレントポジション時点で何かしらの変更が加えられているレイヤーを選択する機能を提供するクラス
    /// </summary>
    /// <returns></returns>
    internal class ModifiedLayerSelectorService : BaseService
    {
        public override bool ExecuteInternal(int mode)
        {
            if (this.Scene.ActiveModel == null)
                return false;

            return ExecuteSpace();
        }

        // フレーム検索の高速化のためのキャッシュ
        private static readonly Vector3 ZeroVector = Vector3.Zero;

        private static readonly Quaternion IdentityQuaternion = Quaternion.Identity;

        private bool ExecuteSpace()
        {
            var ret = false;
            var currentPosition = Scene.MarkerPosition;

            foreach (var tuple in this.Scene.ActiveModel.Bones.SelectMany(b => b.Layers.Select(l => (bone: b, layer: l))))
            {
                if (tuple.layer.Name != null)
                    Debug.WriteLine("Center");
                //if (tuple.layer.Frames.Count == 0)
                //{
                //    tuple.layer.Selected = false;
                //    continue;
                //}

                //// 現行フレーム直前のフレームを取得
                //IMotionFrameData lastFrame = null;
                //long maxFrameNumber = -1;

                //foreach (var frame in tuple.layer.Frames)
                //{
                //    if (frame.FrameNumber <= currentPosition && frame.FrameNumber > maxFrameNumber)
                //    {
                //        maxFrameNumber = frame.FrameNumber;
                //        lastFrame = frame;
                //    }
                //}

                //if (lastFrame == null)
                //{
                //    tuple.layer.Selected = false;

                //    continue;
                //}

                // 値の比較を効率化
                var selected = tuple.layer.CurrentLocalMotion.Move.RoundVector3(4) != ZeroVector || tuple.layer.CurrentLocalMotion.Rotation.RoundQuaternion(4) != IdentityQuaternion;
                tuple.layer.Selected = selected;
                tuple.layer.Frames.FirstOrDefault().Selected = selected;
                if (selected)
                    ret = true;
            }

            return ret;
        }

    }
}