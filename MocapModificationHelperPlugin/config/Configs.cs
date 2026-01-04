using MoCapModificationHelperPlugin.service;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Serialization;

namespace MoCapModificationHelperPlugin
{
    public enum ServiceType
    {
        None = 0,

        /// <summary>
        /// 選択中のキーからレイヤーボーンのみを抽出して選択する
        /// </summary>
        LayerBoneSelectorService,

        /// <summary>
        /// // カレントポジション時点で何かしらの変更が加えられているレイヤーを選択する
        /// </summary>
        ModifiedLayerSelectorService,

        /// <summary>
        /// 選択されたレイヤーに対して、カレントポジションの一つ前から穴が空いている所まで選択する
        /// </summary>
        GapSelectorService,

        /// <summary>
        /// 現在のキー選択状態をコピーする
        /// </summary>
        SelectedKeysSaverService,

        /// <summary>
        /// SelectedKeysSaverServiceでコピーされたキー選択状態を貼り付ける
        /// </summary>
        SelectedKeysLoaderService,

        /// <summary>
        /// 補完曲線パレットをセットする
        /// </summary>
        InterpolateSetterService,

        /// <summary>
        ///選択中の表示枠のキーを全選択
        /// </summary>
        FillDisplayFramesService,

        /// <summary>
        ///選択中のまばたきモーフに対する目モーフをキャンセルする
        /// </summary>
        BlinkCancellerService,

        /// <summary>
        /// 選択されたキーにたいしてオフセットを加える
        /// </summary>
        OffsetAdderService,
    }

    public class Configs
    {
        public List<ConfigItem> Services = new List<ConfigItem>();
        public bool ClickOffsetBtnByShiftEnter { get; set; } = false;

        public Configs()
        {
        }

        public void Initialize()
        {
            Services = new List<ConfigItem>();
            Services.Add(new ConfigItem() { Keys = Keys.Space, ServiceType = ServiceType.ModifiedLayerSelectorService });
            Services.Add(new ConfigItem() { Keys = Keys.L, ServiceType = ServiceType.LayerBoneSelectorService });
            Services.Add(new ConfigItem() { Keys = Keys.Enter, ServiceType = ServiceType.GapSelectorService });
            Services.Add(new ConfigItem() { Keys = Keys.C, ServiceType = ServiceType.SelectedKeysSaverService });
            Services.Add(new ConfigItem() { Keys = Keys.V, ServiceType = ServiceType.SelectedKeysLoaderService });
            Services.Add(new ConfigItem() { Keys = Keys.Z, ServiceType = ServiceType.FillDisplayFramesService });

            Services.Add(new ConfigItem() { Keys = Keys.B, ServiceType = ServiceType.BlinkCancellerService });
            Services.Add(CreateInterpolateSetterService());
        }

        public void KeepAndInitialize()
        {
            var prevServices = new List<ConfigItem>(Services);
            Initialize();
            foreach (var item in Services)
            {
                var prev = prevServices.FirstOrDefault(n => n.ServiceType == item.ServiceType);
                if (prev != null)
                {
                    item.Keys = prev.Keys;
                    item.KeysList = prev.KeysList;
                    item.Inverse = prev.Inverse;
                    item.InterpolateType = prev.InterpolateType;
                }
            }
        }

        public static string GetConfigFilePath()
        {
            var assemblyPath = Assembly.GetExecutingAssembly().Location;
            var directory = Path.GetDirectoryName(assemblyPath);
            return Path.Combine(directory, "MoCapModificationHelperPluginSetting.xml");
        }

        public static ConfigItem CreateInterpolateSetterService()
        {
            return new ConfigItem()
            {
                Keys = Keys.None,
                KeysList = new List<Keys>() {
                    Keys.D0, Keys.D1, Keys.D2, Keys.D3, Keys.D4, Keys.D5, Keys.D6,
                    Keys.NumPad0, Keys.NumPad1, Keys.NumPad2, Keys.NumPad3, Keys.NumPad4, Keys.NumPad5, Keys.NumPad6
                }
                ,
                ServiceType = ServiceType.InterpolateSetterService,
                InterpolateType = InterpolateType.R,
            };
        }
    }

    public class ConfigItem
    {
        public Keys Keys { get; set; } = Keys.None;
        public List<Keys> KeysList { get; set; } = null;
        public ServiceType ServiceType { get; set; }
        public bool Inverse { get; set; }
        public InterpolateType InterpolateType { get; set; } = InterpolateType.R;
    }
}