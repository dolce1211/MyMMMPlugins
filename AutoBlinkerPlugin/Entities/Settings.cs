using MikuMikuPlugin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using MyUtility;
using System.Diagnostics;

namespace AutoBlinkerPlugin
{
    /// <summary>
    /// 適用情報
    /// </summary>
    public class Args : RawEntity
    {
        public ModelSetting ModelInfo { get; set; } = new ModelSetting();

        public SavedState CloneToSavedState()
        {
            var ret = new SavedState();
            var clone = this.MemberwiseClone();
            foreach (var pi in this.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance))
            {
                var value = pi.GetValue(clone);
                if (pi.Name != "ModelInfo")
                    pi.SetValue(ret, value);
            }
            return ret;
        }
    }

    /// <summary>
    /// モデル毎に管理する情報
    /// </summary>
    public class ModelSetting
    {
        /// <summary>
        /// モデル名
        /// </summary>
        public string ModelName { get; set; }

        /// <summary>
        /// まばたきモーフ
        /// </summary>
        public string BlinkingMorphName { get; set; }

        /// <summary>
        /// 見開きモーフ
        /// </summary>
        public string BikkuriMorphName { get; set; }

        /// <summary>
        /// 見開きモーフの効き
        /// </summary>
        public float BikkuriMorphValue { get; set; }

        /// <summary>
        /// まゆ下モーフ
        /// </summary>
        public string EyebrowDownMorphName { get; set; }

        /// <summary>
        /// まゆ上モーフ
        /// </summary>
        public string EyebrowUpMorphName { get; set; }

        /// <summary>
        /// まゆ下の効き
        /// </summary>
        public float EyebrowDownSyncValue { get; set; }

        /// <summary>
        /// まゆ上の効き
        /// </summary>
        public float EyebrowUpSyncValue { get; set; }

        /// <summary>
        /// 目連動で動かすボーン(通常は両目ボーン)
        /// </summary>
        public string EyeSyncBoneName { get; set; }

        /// <summary>
        /// 目連動の効き(上)
        /// </summary>
        public float EyeSyncValueUp { get; set; }

        /// <summary>
        /// 目連動の効き(下)
        /// </summary>
        public float EyeSyncValueDown { get; set; }

        public ModelSetting()
        {
        }
    }

    public class RawEntity
    {
        /// <summary>
        /// 導入フレーム数
        /// </summary>
        public int EnterFrames { get; set; }

        /// <summary>
        /// まばたきフレーム数
        /// </summary>
        public int BlinkingFrames { get; set; }

        /// <summary>
        /// 脱出フレーム数
        /// </summary>
        public int ExitFrames { get; set; }

        /// <summary>
        /// 始反動を付ける
        /// </summary>
        public bool DoHandouStart { get; set; }

        /// <summary>
        /// 終反動を付ける
        /// </summary>
        public bool DoHandouEnd { get; set; }

        /// <summary>
        /// 始反動フレーム数
        /// </summary>
        public int HandouFramesStart { get; set; }

        /// <summary>
        /// 終反動フレーム数
        /// </summary>
        public int HandouFramesEnd { get; set; }

        /// <summary>
        /// ゆるやかに戻す？
        /// </summary>
        public bool DoYuruyaka { get; set; }

        /// <summary>
        /// ゆるやかを開始する値
        /// </summary>
        public float YuruyakaValue { get; set; }

        /// <summary>
        /// ゆるやかに戻すフレーム数
        /// </summary>
        public int YuruyakaFrame { get; set; }

        /// <summary>
        /// まゆ連動する？
        /// </summary>
        public bool DoEyebrowSync { get; set; }

        /// <summary>
        /// 補完曲線付ける？
        /// </summary>
        public bool DoHokan { get; set; }

        /// <summary>
        /// 目連動する？
        /// </summary>
        public bool DoEyeSync { get; set; }

        /// <summary>
        /// 目モーションレイヤー作成する？
        /// </summary>
        public bool CreateEyeMotionLayer { get; set; }

        /// <summary>
        /// 例外
        /// </summary>
        public string Exceptions { get; set; } = String.Empty;

        protected void SetBaseValue()
        {
            EnterFrames = 2;
            BlinkingFrames = 3;
            ExitFrames = 3;
            HandouFramesStart = 2;
            HandouFramesEnd = 3;
            DoYuruyaka = false;
            YuruyakaFrame = 24;
            YuruyakaValue = 10;
            DoEyebrowSync = false;
            DoHandouStart = false;
            DoHandouEnd = false;
            DoEyeSync = false;
            DoHokan = false;
        }

        public void CloneTo<T>(T entity)
            where T : RawEntity
        {
            foreach (var pi in typeof(RawEntity).GetProperties(BindingFlags.Public | BindingFlags.Instance))
            {
                var value = pi.GetValue(this);
                try
                {
                    pi.SetValue(entity, value);
                }
                catch (Exception)
                {
                }
            }
        }
    }

    public class FavEntity : RawEntity
    {
        public string FavName { get; set; }

        public FavEntity()
        {
            base.SetBaseValue();
        }

        public FavEntity Clone()
        {
            return (FavEntity)this.MemberwiseClone();
        }
    }

    /// <summary>
    /// 保存情報
    /// </summary>
    public class SavedState : RawEntity
    {
        /// <summary>
        /// Version
        /// </summary>
        public string Version { get; set; }

        public List<ModelSetting> ModelInfos { get; set; } = new List<ModelSetting>();

        /// <summary>
        /// お気に入り開いてる？
        /// </summary>
        public bool IsFavOpen { get; set; }

        /// <summary>
        /// 常に手前？？
        /// </summary>
        public bool TopMost { get; set; }

        public List<FavEntity> Favorites { get; set; } = null;

        public SavedState()
        {
            base.SetBaseValue();
        }
    }

    /// <summary>
    /// 選択中モデルの状態を表すエンティティ
    /// </summary>
    [DebuggerDisplay("{ModelName}")]
    public class ModelItem : IMMDModel
    {
        public string ModelName { get; set; }

        /// <summary>
        /// 目モーフ
        /// </summary>
        public List<MorphItem> EyeMorphItems { get; set; } = new List<MorphItem>();

        /// <summary>
        /// まゆモーフ
        /// </summary>
        public List<MorphItem> BrowMorphItems { get; set; } = new List<MorphItem>();

        /// <summary>
        /// ボーン
        /// </summary>
        public List<string> Bones { get; set; } = new List<string>();
    }

    /// <summary>
    /// 選択中モデルのモーフ状態を表すエンティティ
    /// </summary>
    [DebuggerDisplay("{MorphName}")]
    public class MorphItem
    {
        /// <summary>
        /// モーフ名
        /// </summary>
        public string MorphName { get; set; }

        /// <summary>
        /// モーフ量
        /// </summary>
        public float Weight { get; set; }

        /// <summary>
        /// コンボボックス内のindex(MMDで使用)
        /// </summary>
        public int ComboBoxIndex { get; set; } = -1;

        /// <summary>
        /// どのパネルのモーフ？
        /// </summary>
        public MMDUtil.MMDUtilility.MorphType MorphType { get; set; }

        /// <summary>
        /// モーフタイプ＋名称
        /// </summary>
        public string MortphNameWithType
        {
            get
            {
                switch (this.MorphType)
                {
                    case MMDUtil.MMDUtilility.MorphType.Eye:
                        return $"目__{this.MorphName}";

                    case MMDUtil.MMDUtilility.MorphType.Lip:
                        return $"口__{this.MorphName}";

                    case MMDUtil.MMDUtilility.MorphType.Brow:
                        return $"眉__{this.MorphName}";

                    case MMDUtil.MMDUtilility.MorphType.Other:
                        return $"他__{this.MorphName}";

                    default:
                        return $"？__{this.MorphName}";
                }
            }
        }

        /// <summary>
        /// クローンを返します。
        /// </summary>
        /// <returns></returns>
        public MorphItem Clone()
        {
            //return this.MemberwiseClone() as MorphItem;
            return new MorphItem()
            {
                MorphName = this.MorphName,
                MorphType = this.MorphType,
                Weight = this.Weight,
            };
        }
    }
}