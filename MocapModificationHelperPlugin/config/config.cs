using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MoCapModificationHelperPlugin
{
    public enum ServiceType
    {
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
        /// 選択されたキーにたいしてオフセットを加える
        /// </summary>
        OffsetAdderService,
    }

    public class Config
    {
        public List<ConfigItem> Services = new List<ConfigItem>();

        public Config()
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
        }

        public static string GetConfigFilePath()
        {
            var assemblyPath = Assembly.GetExecutingAssembly().Location;
            var directory = Path.GetDirectoryName(assemblyPath);
            return Path.Combine(directory, "MoCapModificationHelperPluginSetting.xml");
        }
    }

    public class ConfigItem
    {
        public Keys Keys { get; set; }
        public ServiceType ServiceType { get; set; }
    }
}