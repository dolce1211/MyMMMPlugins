using DxMath;
using MikuMikuPlugin;
using MMDUtil;
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
        public override bool ExecuteInternal(ConfigItem config)
        {
            if (this.Scene.ActiveModel == null)
                return false;

            return ExecuteSpace(config);
        }

        // フレーム検索の高速化のためのキャッシュ
        private static readonly Vector3 _zeroVector = Vector3.Zero;

        private static readonly Quaternion _identityQuaternion = Quaternion.Identity;

        private bool ExecuteSpace(ConfigItem config)
        {
            var ret = false;
            var currentPosition = Scene.MarkerPosition;
            var inverse = config.Inverse;
            // キーボードでshiftが押下されている場合は一時的に反転
            if (Control.ModifierKeys.HasFlag(Keys.Shift))
                inverse = !inverse;

            foreach (var bone in this.Scene.ActiveModel.Bones)
            {
                if (this.Scene.ActiveModel.FindDisplayFramesFromBone(bone) != null) // 表示枠内のボーンのみ処理
                {
                    foreach (var tuple in bone.Layers.Select(l => (bone: bone, layer: l)))
                    {
                        var selected = tuple.layer.CurrentLocalMotion.Move.RoundVector3(4) != _zeroVector || tuple.layer.CurrentLocalMotion.Rotation.RoundQuaternion(4) != _identityQuaternion;
                        if (inverse)
                            selected = !selected;

                        tuple.layer.Selected = selected;

                        tuple.layer.Frames.FirstOrDefault().Selected = selected;
                        if (selected)
                            ret = true;
                    }
                }
                else
                {
                    Debug.WriteLine($"FindDisplayFramesFromBone is null. bone:{bone.Name} ");
                }
            }
            foreach (var morph in this.Scene.ActiveModel.Morphs)
            {
                if (this.Scene.ActiveModel.FindDisplayFramesFromMorph(morph) != null)// 表示枠内のモーフのみ処理
                {
                    morph.Selected = morph.CurrentWeight != 0.0f;
                }
            }

            //var gridHandle = MMMUtilility.FindTimelineGridControl(this.ApplicationForm);
            //if (gridHandle != IntPtr.Zero)
            //{
            //    MMMUtilility.ClickControlAt(gridHandle, 20, 200);
            //}
            return ret;
        }
    }
}