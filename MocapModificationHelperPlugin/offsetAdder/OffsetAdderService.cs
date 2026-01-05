using Linearstar.Keystone.IO.MikuMikuDance;
using MikuMikuPlugin;
using MMDUtil;
using MoCapModificationHelperPlugin.offsetAdder;
using MyUtility;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MoCapModificationHelperPlugin.service
{
    /// <summary>
    /// 選択されたキーにたいしてオフセットを加える
    /// </summary>
    internal class OffsetAdderService : BaseService
    {
        public EventHandler<ProgressChangedEventArgs> ProgressChanged;
        private Dictionary<string, IMotionFrameData> _previousStates = null;
        public Dictionary<string, IMotionFrameData> PreviousStates => _previousStates;
        private long _frameNumber = -1;
        private frmMain _frmMain = null;
        public long FrameNumber => _frameNumber;

        public OffsetAdderService(frmMain frmMain)
        {
            this._frmMain = frmMain;
        }

        public override void Initialize(Scene scene, IWin32Window applicationForm)
        {
            base.Initialize(scene, applicationForm);
            this.SaveCurrentState();
        }

        /// <summary>
        /// 現在の状態を保存する
        /// </summary>
        private void SaveCurrentState()
        {
            MMDUtil.MMDUtilility.BeginAndEndUpdate(this.ApplicationForm.Handle, false);
            var otherWindow = MMDUtil.MMMUtilility.TryGetOtherWindow(true);
            if (otherWindow != null)
                MMDUtil.MMDUtilility.BeginAndEndUpdate(otherWindow.hWnd, false);
            this._frmMain.Enabled = false;
            try
            {
                this.Scene.MarkerPosition += 1;
                this.Scene.MarkerPosition -= 1;
                //現時点の無変更の状態を保存
                this._previousStates = OffsetAdderUtil.TryGetCurrentState(this.Scene, 0);
                _frameNumber = this.Scene.MarkerPosition;
            }
            finally
            {
                this._frmMain.Enabled = true;
                MMDUtil.MMDUtilility.BeginAndEndUpdate(this.ApplicationForm.Handle, true);
                if (otherWindow != null)
                    MMDUtil.MMDUtilility.BeginAndEndUpdate(otherWindow.hWnd, true);
            }
        }

        public override bool ExecuteInternal(ConfigItem config)
        {
            //全ての選択されているフレームを取得
            var currentPosition = this.Scene.MarkerPosition;
            //現時点の変更状態を保持
            var currentStates = OffsetAdderUtil.TryGetCurrentState(this.Scene, 1);

            var processingItems = this.PrepareOffset(currentStates);
            if (processingItems == null || processingItems.Count == 0)
            {
                Debug.WriteLine("オフセットを適用するフレームなし");
                return false;
            }

            try
            {
                return ExecuteAddOffsetAsync(processingItems);
            }
            finally
            {
                //操作前の状態に戻す
                this.ApplicationForm.Refresh();
                this.Scene.MarkerPosition = currentPosition;
                foreach (var tuple in Scene.ActiveModel.Bones
                                        .SelectMany(bone => (bone.Layers.Select(layer => (bone, layer))))
                                        .Where(tuple => processingItems.Any(p => p.layerName == $"{tuple.bone.Name}{tuple.layer.Name ?? ""}")))

                {
                    foreach (var frame in tuple.layer.Frames
                                            .Where(f => processingItems.Any(p => tuple.layer.LayerID == p.layer.LayerID && p.frameData.FrameNumber == f.FrameNumber)))
                    {
                        frame.Selected = true;
                    }
                }
                this.SaveCurrentState();
            }
        }

        private List<(string layerName, MotionLayer layer, MotionFrameData frameData)> PrepareOffset(Dictionary<string, IMotionFrameData> currentStates)
        {
            var allSelectedLayergroup = OffsetAdderUtil.TryGetAllSelectedLayerGroups(this.Scene);

            var previousStates = this._previousStates.Where(n => currentStates.ContainsKey(n.Key))
                            .ToDictionary(n => n.Key, n => n.Value);
            if (currentStates?.Count == 0 || previousStates?.Count == 0 || !previousStates.All(n => currentStates.ContainsKey(n.Key)))
            {
                return default;
            }
            var offsets = new Dictionary<string, (DxMath.Vector3 positionOffset, DxMath.Quaternion rotationOffset)>();
            foreach (var kvp in currentStates)
            {
                var layerName = kvp.Key;
                var currentState = kvp.Value;

                var offsetTuple = OffsetAdderUtil.TryGetOffset(layerName, currentState, this._previousStates);
                if (!(offsetTuple.positionOffset == default && offsetTuple.rotationOffset == default))
                {
                    //変更あり
                    offsets.Add(kvp.Key, offsetTuple);
                }
            }
            var newFramesHash = new Dictionary<string, List<(MotionLayer, MotionFrameData)>>();
            foreach (var kvp in offsets)
            {
                var layerName = kvp.Key;
                var offset = kvp.Value;

                var targetLayerFrames = allSelectedLayergroup.FirstOrDefault(g => g.Key == layerName);
                if (targetLayerFrames != null)
                {
                    foreach (var frameTuple in targetLayerFrames)
                    {
                        if (frameTuple.frame is IMotionFrameData motionFrame)
                        {
                            // 位置のオフセット適用（単純な加算）

                            var newPosition = motionFrame.Position + offset.positionOffset;

                            var newRotation = DxMath.Vector3.Add(motionFrame.Quaternion.ToEularDxMath(), offset.rotationOffset.ToEularDxMath());

                            // フレームデータを更新
                            var newMotionData = new MotionData(newPosition, newRotation.ToQuatanionDxMath());
                            var f = new MotionFrameData(frameTuple.frame.FrameNumber, newMotionData.Move, newMotionData.Rotation);
                            if (!newFramesHash.ContainsKey(layerName))
                                newFramesHash[layerName] = new List<(MotionLayer, MotionFrameData)>();
                            newFramesHash[layerName].Add((frameTuple.layer, f));
                        }
                    }
                }
            }

            // シーケンス処理：一件ずつ確実に処理を完了
            var processingItems = new List<(string layerName, MotionLayer layer, MotionFrameData frameData)>();

            // 処理対象を整理
            foreach (var kvp in newFramesHash)
            {
                var layerName = kvp.Key;
                foreach (var tuple in kvp.Value)
                {
                    if (tuple.Item2.Quaternion == new DxMath.Quaternion(0, 0, 0, 1) && tuple.Item2.Position == new DxMath.Vector3())
                    {
                        //未変更
                        continue;
                    }
                    processingItems.Add((layerName, tuple.Item1, tuple.Item2));
                }
            }

            MMDUtilility.SetForegroundWindow(this.ApplicationForm.Handle);
            // 全てのフレームの選択を解除
            foreach (var f in Scene.ActiveModel.Bones.SelectMany(b => b.Layers).SelectMany(l => l.SelectedFrames))
            {
                f.Selected = false;
            }
            foreach (var l in Scene.ActiveModel.Bones.SelectMany(b => b.SelectedLayers))
            {
                l.Selected = false;
            }
            return processingItems;
        }

        private bool ExecuteAddOffsetAsync(List<(string layerName, MotionLayer layer, MotionFrameData frameData)> processingItems)
        {
            var value = 0;
            ProgressChanged?.Invoke(this, new ProgressChangedEventArgs(value, processingItems.Count));
            try
            {
                MMDUtil.MMMUtilility.SendSpaceToApplicationForm(this.ApplicationForm);
                System.Windows.Forms.Application.DoEvents();

                // 一件ずつ同期的に処理
                foreach (var layerFrameTuples in processingItems.OrderBy(p => p.frameData.FrameNumber))
                {
                    // フレーム位置を移動
                    this.Scene.MarkerPosition = layerFrameTuples.frameData.FrameNumber;

                    // フレームを選択
                    layerFrameTuples.layer.Selected = true;
                    layerFrameTuples.frameData.Selected = true;

                    // モーションデータを適用
                    layerFrameTuples.layer.CurrentLocalMotion = new MotionData(layerFrameTuples.frameData.Position, layerFrameTuples.frameData.Quaternion);
                    // UI更新のためにEnterキー送信
                    MMDUtil.MMMUtilility.SendEnterToApplicationForm(this.ApplicationForm);
                    System.Windows.Forms.Application.DoEvents();

                    System.Threading.Thread.Sleep(10);

                    // フレームを選択解除
                    layerFrameTuples.frameData.Selected = false;
                    layerFrameTuples.layer.Selected = false;

                    value++;
                    ProgressChanged?.Invoke(this, new ProgressChangedEventArgs(value, processingItems.Count));
                }
            }
            catch (Exception)
            {
                // エラーログ出力（処理は継続）
                System.Diagnostics.Debug.WriteLine($"フレーム処理エラー");
            }
            finally
            {
                ProgressChanged?.Invoke(this, new ProgressChangedEventArgs(processingItems.Count, processingItems.Count));
            }
            return true;
        }
    }

    internal class ProgressChangedEventArgs : EventArgs
    {
        public int Value { get; set; }
        public int Maximum { get; set; }

        public ProgressChangedEventArgs(int value, int maximum)
        {
            this.Value = value;
            this.Maximum = maximum;
        }
    }
}