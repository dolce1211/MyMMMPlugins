﻿using DxMath;
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
        public override bool ExecuteInternal(ConfigItem config)
        {
            if (this.Scene.ActiveModel == null)
                return false;

            return ExecuteSpace(config);
        }

        // フレーム検索の高速化のためのキャッシュ
        private static readonly Vector3 ZeroVector = Vector3.Zero;

        private static readonly Quaternion IdentityQuaternion = Quaternion.Identity;

        private bool ExecuteSpace(ConfigItem config)
        {
            var ret = false;
            var currentPosition = Scene.MarkerPosition;
            var inverse = config.Inverse;
            // キーボードでshiftが押下されている場合は一時的に反転
            if (Control.ModifierKeys.HasFlag(Keys.Shift))
                inverse = !inverse;

            foreach (var tuple in this.Scene.ActiveModel.Bones.SelectMany(b => b.Layers.Select(l => (bone: b, layer: l))))
            {
                var selected = tuple.layer.CurrentLocalMotion.Move.RoundVector3(4) != ZeroVector || tuple.layer.CurrentLocalMotion.Rotation.RoundQuaternion(4) != IdentityQuaternion;
                if (inverse)
                    selected = !selected;

                tuple.layer.Selected = selected;

                tuple.layer.Frames.FirstOrDefault().Selected = selected;
                if (selected)
                    ret = true;
            }

            return ret;
        }
    }
}