using MikuMikuPlugin;
using MoCapModificationHelperPlugin.service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Net.Mime.MediaTypeNames;
using MyUtility;
using System.Diagnostics;

namespace MoCapModificationHelperPlugin.offsetAdder
{
    internal class OffsetAdderUtil
    {
        /// <summary>
        /// 選択中の全てのフレーム情報を返します。
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<IGrouping<string, (string name, Bone bone, MotionLayer layer, IMotionFrameData frame)>> TryGetAllSelectedLayerGroups(Scene scene)
        {
            if (scene?.ActiveModel == null)
            {
                return default;
            }

            var ret = scene.ActiveModel.Bones.SelectMany(b =>
            {
                return b.SelectedLayers.Select(layer => (name: b.Name, bone: b, layer: layer));
            })
            .SelectMany(tuple =>
            {
                return tuple.layer.SelectedFrames
                        .Select(frame => (key: $"{tuple.name}{tuple.layer.Name ?? ""}", bone: tuple.bone, layer: tuple.layer, frame));
            })
            .GroupBy(tuple => tuple.key).ToList();

            return ret;
        }

        /// <summary>
        /// 現在の状態からの変更量を計算して返します。
        /// </summary>
        /// <param name="key"></param>
        /// <param name="currentState"></param>
        /// <param name="_previousStates"></param>
        /// <returns></returns>
        public static (DxMath.Vector3 positionOffset, DxMath.Quaternion rotationOffset) TryGetOffset(string key, IMotionFrameData currentState, Dictionary<string, IMotionFrameData> _previousStates)
        {
            if (!_previousStates.ContainsKey(key))
                return (new DxMath.Vector3(0, 0, 0), new DxMath.Quaternion(0, 0, 0, 1));
            IMotionFrameData previousState = _previousStates[key];
            try
            {
                // previousFrameを現在の基準状態として扱い、currentFrameとの差分を計算
                var previousPosition = previousState.Position;
                var previousRotation = previousState.Quaternion;

                // currentFrameがMotionFrameDataの場合、その位置と回転を取得
                DxMath.Vector3 currentPosition = currentState.Position;  // デフォルトは基準位置
                DxMath.Quaternion currentRotation = currentState.Quaternion; // デフォルトは基準回転

                // 位置の差分を計算 (current - previous)
                var positionOffset = new DxMath.Vector3(
                    currentPosition.X - previousPosition.X,
                    currentPosition.Y - previousPosition.Y,
                    currentPosition.Z - previousPosition.Z
                );

                // 回転の差分を計算 (current - previous)
                // クォータニオンの差分は current * previous^-1 で計算
                var previousRotationInverse = new DxMath.Quaternion(
                    -previousRotation.X,
                    -previousRotation.Y,
                    -previousRotation.Z,
                    previousRotation.W
                );

                var rotationOffset = new DxMath.Quaternion(
                    currentRotation.W * previousRotationInverse.X + currentRotation.X * previousRotationInverse.W +
                    currentRotation.Y * previousRotationInverse.Z - currentRotation.Z * previousRotationInverse.Y,

                    currentRotation.W * previousRotationInverse.Y - currentRotation.X * previousRotationInverse.Z +
                    currentRotation.Y * previousRotationInverse.W + currentRotation.Z * previousRotationInverse.X,

                    currentRotation.W * previousRotationInverse.Z + currentRotation.X * previousRotationInverse.Y -
                    currentRotation.Y * previousRotationInverse.X + currentRotation.Z * previousRotationInverse.W,

                    currentRotation.W * previousRotationInverse.W - currentRotation.X * previousRotationInverse.X -
                    currentRotation.Y * previousRotationInverse.Y - currentRotation.Z * previousRotationInverse.Z
                );

                return (positionOffset.RoundVector3(3), rotationOffset.RoundQuaternion(3));
            }
            catch
            {
                // エラーの場合はゼロオフセットを返す
                return (new DxMath.Vector3(0, 0, 0), new DxMath.Quaternion(0, 0, 0, 1));
            }
        }

        /// <summary>
        /// 現在のフレーム位置における、移動・回転状態を取得します。
        /// </summary>
        /// <param name="scene"></param>
        /// <param name="mode">0:全てのフレームが対象 1:現在選択中のフレームが対象</param>
        /// <returns></returns>
        public static Dictionary<string, IMotionFrameData> TryGetCurrentState(Scene scene, int mode)
        {
            if (scene?.ActiveModel == null)
            {
                return default;
            }

            var selectedLayers = scene.ActiveModel.Bones.SelectMany(b =>
            {
                if (mode == 0)
                    return b.Layers.Select(layer => (name: b.Name, bone: b, layer: layer));
                else
                    return b.SelectedLayers.Select(layer => (name: b.Name, bone: b, layer: layer));
            }).ToList();

            var ret = new Dictionary<string, IMotionFrameData>();

            foreach (var tuple in selectedLayers)
            {
                var currentState = tuple.layer.CurrentLocalMotion;
                var motionData = new MotionData(currentState.Move, currentState.Rotation);
                var f = new MotionFrameData(scene.MarkerPosition, motionData.Move, motionData.Rotation);
                ret[$"{tuple.name}{tuple.layer.Name ?? ""}"] = f;
            }
            return ret;
        }

        /// <summary>
        /// DataGridViewのカラムを設定する
        /// </summary>
        public static void ConfigureDataGridViewColumns(DataGridView dataGridView)
        {
            dataGridView.DataSource = new List<OffsetGridItem>();
            try
            {
                // 属性によって列の表示/非表示やヘッダー名が自動設定されるため、
                // 基本的な設定のみを行う

                // 読み取り専用に設定
                dataGridView.ReadOnly = true;

                // 行の高さを調整
                dataGridView.RowTemplate.Height = 20;

                // カラム幅を中身に合わせる設定
                dataGridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.DisplayedCells;

                // ヘッダーのスタイル設定
                dataGridView.ColumnHeadersDefaultCellStyle.BackColor = System.Drawing.SystemColors.Control;
                dataGridView.ColumnHeadersDefaultCellStyle.Font = new System.Drawing.Font("MS UI Gothic", 9F, System.Drawing.FontStyle.Bold);

                // 選択モードを行全体に設定
                dataGridView.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
                dataGridView.MultiSelect = false;

                // 行ヘッダーを非表示
                dataGridView.RowHeadersVisible = false;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"DataGridViewカラム設定エラー: {ex.Message}");
            }
        }

        /// <summary>
        /// オフセット情報をDataGridViewに反映する
        /// </summary>
        /// <param name="itemsHash">OffsetStateItemのリスト</param>
        public static List<OffsetGridItem> UpdateDataGridView(Scene scene, DataGridView dataGridView, Dictionary<string, IMotionFrameData> previousStates)
        {
            // 現在の各ボーンレイヤーのオフセット値を取得
            var offsetState = OffsetAdderUtil.CreateOffsetState(scene);
            try
            {
                // DataGridViewの描画を一時停止
                dataGridView.SuspendLayout();

                // DataGridView用のデータを作成
                var gridData = new List<OffsetGridItem>();
                var currentStates = OffsetAdderUtil.TryGetCurrentState(scene, 0);

                var offsetsTuplesDic = new Dictionary<string, (DxMath.Vector3 positionOffset, DxMath.Quaternion rotationOffset)>();
                foreach (var kvp in currentStates)
                {
                    (DxMath.Vector3 positionOffset, DxMath.Quaternion rotationOffset) offsetTuple =
                                OffsetAdderUtil.TryGetOffset(kvp.Key, kvp.Value, previousStates);
                    if (offsetTuple.rotationOffset == new DxMath.Quaternion(0, 0, 0, 1) && offsetTuple.positionOffset == new DxMath.Vector3())
                        continue;

                    offsetsTuplesDic.Add(kvp.Key, offsetTuple);
                }

                foreach (var kvp in offsetsTuplesDic)
                {
                    var layerName = kvp.Key;
                    OffsetStateItem item = null;

                    if (offsetState.ItemsHash.ContainsKey(layerName))
                        item = offsetState.ItemsHash[layerName];

                    var currentState = currentStates.ContainsKey(layerName) ? currentStates[layerName] : null;
                    if (currentState == null)
                        continue;

                    var offsetTuple = OffsetAdderUtil.TryGetOffset(layerName, currentState, previousStates);

                    gridData.Add(new OffsetGridItem
                    {
                        LayerName = layerName,
                        FrameCount = item != null ? item.Frames.Count : 0,
                        RotationValue = offsetTuple.rotationOffset,
                        MoveValue = offsetTuple.positionOffset,
                    });
                }

                // DataSourceに設定
                var prevGridData = dataGridView.DataSource as List<OffsetGridItem>;
                if (!OffsetGridItemEqualsTo(gridData, prevGridData))
                    dataGridView.DataSource = gridData;

                return gridData;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"DataGridView更新エラー: {ex.Message}");
            }
            finally
            {
                // DataGridViewの描画を再開
                dataGridView.ResumeLayout();
            }
            return new List<OffsetGridItem>();
        }

        private static OffsetState CreateOffsetState(Scene scene)
        {
            OffsetState state = new OffsetState();
            long minFrame = long.MaxValue;
            long maxFrame = 0;

            // ActiveModel内の全ボーンのLayersを調べ、選択されているLayerの総数を取得
            int totalSelectedLayers = 0;

            foreach (var layer in scene.ActiveModel.Bones.SelectMany(b => b.SelectedLayers))
            {
                totalSelectedLayers += layer.SelectedFrames.Count();
                if (totalSelectedLayers == 0)
                    continue;
            }
            var allSelectedLayergroup = OffsetAdderUtil.TryGetAllSelectedLayerGroups(scene);
            if (allSelectedLayergroup == null)
                return null;

            totalSelectedLayers = 0;
            var itemsHash = new Dictionary<string, OffsetStateItem>();
            foreach (var grp in allSelectedLayergroup)
            {
                totalSelectedLayers += grp.Count();

                itemsHash.Add(grp.FirstOrDefault().name, new OffsetStateItem()
                {
                    LayerName = grp.FirstOrDefault().name,
                    Bone = grp.FirstOrDefault().bone,
                    Layer = grp.FirstOrDefault().layer,
                    Frames = grp.Select(n => n.frame).ToList()
                });
            }
            // 結果を返す
            return new OffsetState()
            {
                MaxFrame = maxFrame,
                MinFrame = minFrame,
                TotalSelectedLayers = totalSelectedLayers,
                ItemsHash = itemsHash,
            };
        }

        private static bool OffsetGridItemEqualsTo(List<OffsetGridItem> prev, List<OffsetGridItem> current)
        {
            if (prev == null && current == null)
                return true;
            if (prev == null || current == null)
                return false;
            if (prev.Count != current.Count)
                return false;
            for (int i = 0; i < prev.Count; i++)
            {
                var p = prev[i];
                var c = current[i];
                if (p.LayerName != c.LayerName)
                    return false;
                if (p.FrameCount != c.FrameCount)
                    return false;
                if (p.MoveValue != c.MoveValue)
                    return false;
                if (p.RotationValue != c.RotationValue)
                    return false;
            }
            return true;
        }

        public class OffsetState
        {
            public long MinFrame { get; set; } = long.MaxValue;
            public long MaxFrame { get; set; } = 0;
            public int TotalSelectedLayers { get; set; } = 0;

            public Dictionary<string, OffsetStateItem> ItemsHash { get; set; } = new Dictionary<string, OffsetStateItem>();

            public string StateText
            {
                get
                {
                    if (TotalSelectedLayers > 0)
                        return $"フレーム間隔:{MaxFrame - MinFrame + 1}   ({MinFrame}fr ～ {MaxFrame}fr)\r\n選択キー数: {TotalSelectedLayers}";
                    else
                        return "";
                }
            }
        }

        public class OffsetStateItem
        {
            public string LayerName { get; set; }
            public Bone Bone { get; set; }
            public MotionLayer Layer { get; set; }
            public List<IMotionFrameData> Frames { get; set; } = new List<IMotionFrameData>();
        }

        /// <summary>
        /// DataGridView表示用のデータクラス
        /// </summary>
        public class OffsetGridItem
        {
            [System.ComponentModel.DisplayName("ボーン")]
            public string LayerName { get; set; }

            [System.ComponentModel.DisplayName("選択\r\nkey数")]
            public int FrameCount { get; set; }

            [System.ComponentModel.DisplayName("オフセット\r\n移動量")]
            public string Move
            {
                get
                {
                    return $"X:{MoveValue.X:0.000}  Y:{MoveValue.Y:0.000}  Z:{MoveValue.Z:0.000}";
                }
            }

            [System.ComponentModel.DisplayName("オフセット\r\n回転量")]
            public string Rotation
            {
                get
                {
                    try
                    {
                        return $"X:{this._rotationEular.X:0.000}  Y:{this._rotationEular.Y:0.000}  Z:{this._rotationEular.Z:0.000}";
                    }
                    catch
                    {
                        return "X:0.00 Y:0.00 Z:0.00";
                    }
                }
            }

            [System.ComponentModel.Browsable(false)]
            public DxMath.Vector3 MoveValue { get; set; }

            private DxMath.Vector3 _rotationEular = new DxMath.Vector3(0, 0, 0);

            [System.ComponentModel.Browsable(false)]
            private DxMath.Quaternion _rotationValue = new DxMath.Quaternion(0, 0, 0, 1);

            [System.ComponentModel.Browsable(false)]
            public DxMath.Quaternion RotationValue
            {
                get => _rotationValue;
                set
                {
                    _rotationValue = value;
                    _rotationEular = MyUtility.ControlHelper_DxMath.ToEularDxMath(_rotationValue);
                }
            }
        }
    }
}