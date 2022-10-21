using LibMMD.Pmx;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static MMDUtil.MMDUtilility;
using static System.Net.WebRequestMethods;

namespace FaceExpressionSelectorMMD
{
    internal class LibMMDUtil
    {
        public static Dictionary<string, ActiveModelInfo> CreateActiveModelInfoHashFromProcess(Process mmd)
        {
            var ret = new Dictionary<string, ActiveModelInfo>();
            var pmxmodels = LibMMDUtil.GetPmxFromProcess(mmd);
            foreach (var pmxmdls in pmxmodels)
            {
                var model = PmxModel2ActiveModelInfo(pmxmdls);
                if (!ret.ContainsKey(model.ModelName))
                    ret.Add(model.ModelName, model);
            }
            return ret;
        }

        public static FileInfo GetPmmInfoFromProcess(Process mmd)
        {
            var ret = new List<PmxModel>();
            if (mmd == null)
                return null;

            if (mmd.MainWindowTitle.Length > 16)
            {
                var pmmfilepath = mmd.MainWindowTitle.Substring(15, mmd.MainWindowTitle.Length - 16);
                if (System.IO.File.Exists(pmmfilepath))
                {
                    return new FileInfo(pmmfilepath);
                }
            }
            return null;
        }

        /// <summary>
        /// MMDのプロセスで読み込まれているモデルの一覧を返します。
        /// </summary>
        /// <param name="mmd"></param>
        /// <returns>null:MMDプロセスからモデルデータを取得できなかった</returns>
        public static List<PmxModel> GetPmxFromProcess(Process mmd)
        {
            var ret = new List<PmxModel>();
            if (mmd == null)
                return ret;
            var pmminfo = GetPmmInfoFromProcess(mmd);
            if (pmminfo == null)
                return ret;

            var mmdexedir = string.Empty;
            try
            {
                //32bit/64bitが揃っていないMMDだとProcessからMainModule情報を取れない
                mmdexedir = System.IO.Path.GetDirectoryName(mmd.MainModule.FileName);
            }
            catch (Win32Exception)
            {
                return null;
            }
            if (System.IO.File.Exists(pmminfo.FullName))
            {
                var pmxfiles = new List<string>();
                //pmmからemmファイルを取得
                var emmfilepath = pmminfo.FullName.ToLower().Replace(".pmm", ".emm");
                if (System.IO.File.Exists(emmfilepath))
                {
                    var emmlines = System.IO.File.ReadAllLines(emmfilepath, System.Text.Encoding.GetEncoding("shift_jis"));
                    var start = false;

                    foreach (var line in emmlines)
                    {
                        if (start)
                        {
                            var array = line.Split('=');
                            if (array[0].ToLower().Trim().IndexOf("pmd") == 0)
                            {
                                var pmxpath = System.IO.Path.Combine(mmdexedir.Trim(), array[1].Trim());
                                if (System.IO.File.Exists(pmxpath))
                                    pmxfiles.Add(pmxpath);
                            }
                        }
                        if (start && line.IndexOf("[") == 0)
                            break;
                        if (line.ToLower() == "[object]")
                            start = true;
                    }

                    foreach (var pmxpath in pmxfiles)
                    {
                        var pmxmodel = FilePath2PmxModel(pmxpath);
                        if (pmxmodel != null)
                            ret.Add(pmxmodel);
                    }
                }
            }

            return ret;
        }

        public static bool CompareActiveModel(ActiveModelInfo model1, ActiveModelInfo model2)
        {
            if (model1.ModelName != model2.ModelName)
                return false;

            foreach (var morphtype in Enum.GetValues(typeof(MorphType)).Cast<MorphType>())
            {
                if (model1.AllMorphs.ContainsKey(morphtype))
                {
                    var mrph1List = model1.AllMorphs[morphtype];
                    var mrph2List = model2.AllMorphs[morphtype];
                    if (mrph1List.Count != mrph2List.Count)
                        return false;

                    for (int i = 0; i < mrph1List.Count; i++)
                    {
                        var mrph1 = mrph1List[i];
                        var mrph2 = mrph2List[i];
                        if (mrph1.MorphName != mrph2.MorphName)
                            return false;

                        if (mrph1.ComboBoxIndex != mrph2.ComboBoxIndex)
                            return false;
                    }
                }
            }

            return true;
        }

        /// <summary>
        /// pmxあるいはpmdのファイルパスからPmxModelのインスタンスを生成して返します。
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        private static PmxModel FilePath2PmxModel(string filePath)
        {
            if (!System.IO.File.Exists(filePath))
                return null;

            var file = new FileInfo(filePath);
            using (var stream = file.OpenRead())
            {
                try
                {
                    var model = PmxParser.Parse(stream);
                    return model;
                }
                catch (Exception)
                {
                }
            }
            return null;
        }

        /// <summary>
        /// LibMMDUtilから取得したPmxModelをActiveModelInfoの形に変換して返します。
        /// </summary>
        /// <param name="pmx"></param>
        /// <returns></returns>
        private static ActiveModelInfo PmxModel2ActiveModelInfo(PmxModel pmx)
        {
            var allMorphs = new Dictionary<MorphType, List<MorphItemWithIndex>>();
            var hash = new Dictionary<MorphType, int>();
            hash.Add(MorphType.Eye, 0);
            hash.Add(MorphType.Lip, 0);
            hash.Add(MorphType.Brow, 0);
            hash.Add(MorphType.Other, 0);

            foreach (var mrph in pmx.Morphs.OrderBy(n => n.Index))
            {
                MorphType morphtype = PmxPnlType2Morphtype(mrph.PanelType);
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

        private static MorphType PmxPnlType2Morphtype(byte pmxpnltype)
        {
            switch (pmxpnltype)
            {
                case 1:
                    return MorphType.Brow;

                case 2:
                    return MorphType.Eye;

                case 3:
                    return MorphType.Lip;

                case 4:
                    return MorphType.Other;

                default:
                    break;
            }
            return MorphType.none;
        }
    }
}