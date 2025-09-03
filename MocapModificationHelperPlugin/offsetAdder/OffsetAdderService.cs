using Linearstar.Keystone.IO.MikuMikuDance;
using MikuMikuPlugin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MyUtility;
using MMDUtil;
using MoCapModificationHelperPlugin.offsetAdder;
using System.Numerics;
using System.Diagnostics;

namespace MoCapModificationHelperPlugin.service
{
    /// <summary>
    /// 選択されたキーにたいしてオフセットを加える
    /// </summary>
    internal class OffsetAdderService : BaseService
    {
        private Dictionary<string, IMotionFrameData> _previousStates = null;
        public Dictionary<string, IMotionFrameData> PreviousStates => _previousStates;
        private long _frameNumber = 0;
        private ProgressBar _progressBar = null;
        private Form _frmMain = null;

        public void Update()
        {
            if (this.Scene.MarkerPosition != _frameNumber)
            {
                //フレーム位置が変わった
                this.SetPrevisouState();
            }
        }

        public void ExecuteUndo()
        {
        }

        public void Initialize(Scene scene, IWin32Window applicationForm, Form frmMain, ProgressBar progressBar)
        {
            _progressBar = progressBar;
            _frmMain = frmMain;
            Initialize(scene, applicationForm);
        }

        public override void Initialize(Scene scene, IWin32Window applicationForm)
        {
            base.Initialize(scene, applicationForm);
            this.SetPrevisouState();
        }

        private void SetPrevisouState()
        {
            MMDUtil.MMDUtilility.BeginAndEndUpdate(this.ApplicationForm.Handle, false);
            var otherWindow = MMDUtil.MMMUtilility.TryGetOtherWindow(true);
            if (otherWindow != null)
                MMDUtil.MMDUtilility.BeginAndEndUpdate(otherWindow.hWnd, false);

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
                return ExecuteOffsetAsync(processingItems);
            }
            finally
            {
                //操作前の状態に戻す
                currentStates.ForEach(kvp =>
                {
                    var currentLayer = this.Scene.ActiveModel.Bones.SelectMany(b =>
                    {
                        return b.Layers.Select(layer => (name: b.Name, bone: b, layer: layer));
                    }).FirstOrDefault(tuple => kvp.Key == $"{tuple.bone.Name}{tuple.layer.Name ?? ""}");
                    currentLayer.layer.CurrentLocalMotion = new MotionData(kvp.Value.Position, kvp.Value.Quaternion);
                });
                this.Scene.MarkerPosition = currentPosition;
                SetPrevisouState();
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

                            //// 回転のオフセット適用（クォータニオンの乗算による合成）
                            //var newRotation = new DxMath.Quaternion(
                            //    motionFrame.Quaternion.W * offset.rotationOffset.X + motionFrame.Quaternion.X * offset.rotationOffset.W +
                            //    motionFrame.Quaternion.Y * offset.rotationOffset.Z - motionFrame.Quaternion.Z * offset.rotationOffset.Y,

                            //    motionFrame.Quaternion.W * offset.rotationOffset.Y - motionFrame.Quaternion.X * offset.rotationOffset.Z +
                            //    motionFrame.Quaternion.Y * offset.rotationOffset.W + motionFrame.Quaternion.Z * offset.rotationOffset.X,

                            //    motionFrame.Quaternion.W * offset.rotationOffset.Z + motionFrame.Quaternion.X * offset.rotationOffset.Y -
                            //    motionFrame.Quaternion.Y * offset.rotationOffset.X + motionFrame.Quaternion.Z * offset.rotationOffset.W,

                            //    motionFrame.Quaternion.W * offset.rotationOffset.W - motionFrame.Quaternion.X * offset.rotationOffset.X -
                            //    motionFrame.Quaternion.Y * offset.rotationOffset.Y - motionFrame.Quaternion.Z * offset.rotationOffset.Z
                            //).RoundQuaternion(3);

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

        private bool ExecuteOffsetAsync(List<(string layerName, MotionLayer layer, MotionFrameData frameData)> processingItems)
        {
            _progressBar.Visible = true;
            _progressBar.Maximum = processingItems.Count;
            _progressBar.Value = 0;
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

                    //this.ApplicationForm.Refresh();
                    _progressBar.Value++;
                }
            }
            catch (Exception ex)
            {
                // エラーログ出力（処理は継続）
                System.Diagnostics.Debug.WriteLine($"フレーム処理エラー");
            }
            finally
            {
                this._progressBar.Visible = false;
            }
            return true;
        }
    }
}