using LibMMD.Pmx;
using MMDUtil;
using MyUtility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static MMDUtil.MMDUtilility;

namespace FaceExpressionSelectorMMD
{
    public class ActiveMorphModelFinder : ModelFinder<ActiveModelInfo>
    {
        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="frm"></param>
        /// <param name="mmdselector"></param>
        public ActiveMorphModelFinder(Form frm, MMDSelectorControl mmdselector, Action<string> showWaitAction = null, Action hideWaitAction = null) : base(frm, mmdselector, showWaitAction, hideWaitAction)
        {
        }

        protected override ActiveModelInfo CreateInstance() => new ActiveModelInfo();

        /// <summary>
        /// LibMMDUtilから取得したPmxModelをActiveModelInfoの形に変換して返します。
        /// </summary>
        /// <param name="pmx"></param>
        /// <returns></returns>
        protected override ActiveModelInfo PmxModel2ActiveModelInfo(PmxModel pmx)
        {
            var allMorphs = new Dictionary<MorphType, List<MorphItemWithIndex>>();
            var hash = new Dictionary<MorphType, int>();
            hash.Add(MorphType.Eye, 0);
            hash.Add(MorphType.Lip, 0);
            hash.Add(MorphType.Brow, 0);
            hash.Add(MorphType.Other, 0);

            foreach (var mrph in pmx.Morphs.OrderBy(n => n.Index))
            {
                MorphType morphtype = base.PmxPnlType2Morphtype(mrph.PanelType);
                if (hash.ContainsKey(morphtype))
                {
                    var morphitem = new MorphItemWithIndex() { MorphName = mrph.NameLocal, MorphType = morphtype, ComboBoxIndex = hash[morphtype] };
                    var morphlist = new List<MorphItemWithIndex>();
                    if (!allMorphs.ContainsKey(morphtype))
                        allMorphs.Add(morphtype, morphlist);
                    else
                        morphlist = allMorphs[morphtype];
                    morphlist.Add(morphitem);
                    hash[morphtype]++;
                }
            }

            return new ActiveModelInfo(pmx.ModelNameLocal, allMorphs);
        }
    }

    /// <summary>
    /// モデル情報をキャッシュするエンティティ
    /// </summary>
    public class ActiveModelInfo : IMMDModel
    {
        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="modelName">モデル名</param>
        /// <param name="morphHash">モーフ情報</param>
        public ActiveModelInfo(string modelName, Dictionary<MorphType, List<MorphItemWithIndex>> allMorphs)
        {
            this.ModelName = modelName;
            this.AllMorphs = allMorphs;
            if (this.AllMorphs == null)
                this.AllMorphs = new Dictionary<MorphType, List<MorphItemWithIndex>>();
        }

        public ActiveModelInfo()
        {
            this.ModelName = String.Empty;
            this.AllMorphs = new Dictionary<MorphType, List<MorphItemWithIndex>>();
        }

        /// <summary>
        /// モーフ情報
        /// </summary>
        public Dictionary<MorphType, List<MorphItemWithIndex>> AllMorphs { get; }

        /// <summary>
        /// モデル名
        /// </summary>
        public string ModelName { get; }
    }
}