using MikuMikuPlugin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using MyUtility;

namespace AutoBlinkerPlugin
{
    /// <summary>
    /// モデル毎に管理する情報
    /// </summary>
    public class ModelInfoEntity
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

        public ModelInfoEntity()
        {
        }
    }

    public class Entity : RawEntity
    {
        public ModelInfoEntity ModelInfo { get; set; } = new ModelInfoEntity();

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

        /// <summary>
        /// 例外
        /// </summary>
        public string Exceptions { get; set; }

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
    /// 保存状態
    /// </summary>
    public class SavedState : RawEntity
    {
        /// <summary>
        /// Version
        /// </summary>
        public string Version { get; set; }

        public List<ModelInfoEntity> ModelInfos { get; set; } = new List<ModelInfoEntity>();

        /// <summary>
        /// お気に入り開いてる？
        /// </summary>
        public bool IsFavOpen { get; set; }

        public List<FavEntity> Favorites { get; set; } = null;

        public SavedState()
        {
            base.SetBaseValue();
        }
    }
}