using LibMMD.Pmx;
using MMDUtil;
using MyUtility;
using System;
using System.Collections.Generic;
using System.IO;
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

        /// <returns></returns>
        protected override ActiveModelInfo PmxModel2ActiveModelInfo(string pmxFilePath)
        {
            var allMorphs = new Dictionary<MorphType, List<MorphItemWithIndex>>();
            var hash = new Dictionary<MorphType, int>();
            hash.Add(MorphType.Eye, 0);
            hash.Add(MorphType.Lip, 0);
            hash.Add(MorphType.Brow, 0);
            hash.Add(MorphType.Other, 0);

            var extension = "";
            if (System.IO.File.Exists(pmxFilePath))
                extension = System.IO.Path.GetExtension(pmxFilePath).ToLower();
            if (extension == ".pmx")
            {
                try
                {
                    PmxModel pmx = FilePath2PmxModel(pmxFilePath);
                    //pmxファイルと思われる
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
                    return new ActiveModelInfo(pmx.ModelNameLocal, extension, allMorphs);
                }
                catch (System.Exception)
                {
                }
            }
            else if (extension == ".pmd")
            {
                //pmdファイルと思われる。とりあえず自力でモデル名だけ取得する
                var file = new FileInfo(pmxFilePath);
                using (var stream = file.OpenRead())
                {
                    try
                    {
                        using (BinaryReader reader = new BinaryReader(stream, Encoding.Default, leaveOpen: true))
                        {
                            //8バイト目から20バイトがモデル名
                            reader.BaseStream.Seek(7, SeekOrigin.Begin);
                            Encoding textEncoding = System.Text.Encoding.GetEncoding("shift_jis");
                            byte[] bytes = reader.ReadBytes(20);
                            var modelNameLocal = textEncoding.GetString(bytes);
                            int nullIndex = modelNameLocal.IndexOf('\x00');
                            if (nullIndex >= 0)
                                modelNameLocal = modelNameLocal.Substring(0, nullIndex);

                            return new ActiveModelInfo(modelNameLocal, extension, allMorphs);
                        }
                    }
                    catch (Exception)
                    {
                    }
                }
            }
            return null;
        }
    }

    /// <summary>
    /// モデル情報をキャッシュするエンティティ
    /// </summary>
    public class ActiveModelInfo : IMMDModelWithExtension
    {
        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="modelName">モデル名</param>
        /// <param name="morphHash">モーフ情報</param>
        public ActiveModelInfo(string modelName, string extension, Dictionary<MorphType, List<MorphItemWithIndex>> allMorphs)
        {
            this.ModelName = modelName;
            this.Extension = extension;
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

        /// <summary>
        /// モデルの拡張子(pmx or pmd)
        /// </summary>
        public string Extension { get; }
    }
}