using AutoBlinkerPlugin;
using LibMMD.Pmx;
using LibMMD.Vmd;
using MMDUtil;
using MyUtility;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static MMDUtil.MMDUtilility;

namespace AutoBlinkerMMD
{
    public class BlinkModelFinder : ModelFinder<ModelItem>
    {
        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="frm"></param>
        /// <param name="mmdselector"></param>
        /// <param name="showWaitAction"></param>
        /// <param name="hideWaitAction"></param>
        public BlinkModelFinder(Form frm, MMDSelectorControl mmdselector, Action<string> showWaitAction = null, Action hideWaitAction = null) : base(frm, mmdselector, showWaitAction, hideWaitAction)
        {
        }

        protected override ModelItem CreateInstance()
        {
            return new ModelItem();
        }

        protected override ModelItem PmxModel2ActiveModelInfo(string pmxFilePath)
        {
            PmxModel pmxmdls = FilePath2PmxModel(pmxFilePath);

            var ret = new ModelItem() { ModelName = pmxmdls.ModelNameLocal };

            var allMorphs = new Dictionary<MorphType, List<MorphItem>>();
            var hash = new Dictionary<MorphType, int>();
            hash.Add(MorphType.Eye, 0);
            hash.Add(MorphType.Brow, 0);
            var bones = new List<string>();
            foreach (var mrph in pmxmdls.Morphs.OrderBy(n => n.Index))
            {
                MorphType morphtype = base.PmxPnlType2Morphtype(mrph.PanelType);
                if (hash.ContainsKey(morphtype))
                {
                    var morphitem = new MorphItem() { MorphName = mrph.NameLocal, MorphType = morphtype, ComboBoxIndex = hash[morphtype] };
                    var morphlist = new List<MorphItem>();
                    if (!allMorphs.ContainsKey(morphtype))
                        allMorphs.Add(morphtype, morphlist);
                    else
                        morphlist = allMorphs[morphtype];
                    morphlist.Add(morphitem);
                    hash[morphtype]++;
                }
            }
            var boneindex = -1;
            foreach (var morph in pmxmdls.Bones.Where(n =>
                                        {
                                            boneindex++;
                                            if ((n.Flags & (BoneFlags.IsVisible | BoneFlags.Enabled)) != 0)
                                            {
                                                //表示かつ操作
                                                if (!pmxmdls.RigidBodies.Any(m => m.PhysicsMode != PmxRigidBodyPhysicsMode.FollowBone && m.RelatedBoneIndex == boneindex))
                                                    //物理ボーンじゃない
                                                    return true;
                                            }
                                            return false;
                                        }))
            {
                bones.Add(morph.NameLocal);
            }

            var eyeMorphs = new List<MorphItem>();
            var browMorphs = new List<MorphItem>();
            if (allMorphs.ContainsKey(MorphType.Eye))
                eyeMorphs = allMorphs[MorphType.Eye];
            if (allMorphs.ContainsKey(MorphType.Brow))
                browMorphs = allMorphs[MorphType.Brow];

            return new ModelItem() { ModelName = pmxmdls.ModelNameLocal, EyeMorphItems = eyeMorphs, BrowMorphItems = browMorphs, Bones = bones };
        }
    }
}