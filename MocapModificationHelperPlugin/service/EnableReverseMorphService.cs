using MyUtility;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MikuMikuPlugin;
using MMDUtil;
using Linearstar.Keystone.IO.MikuMikuDance;

namespace MoCapModificationHelperPlugin.service
{
    internal class EnableReverseMorphService : BaseService
    {
        public override bool ExecuteInternal(ConfigItem config)
        {
            if (this.Scene?.ActiveModel == null)
                return false;

            if (Control.ModifierKeys.HasFlag(Keys.Control))
            {
                // Ctrl+Wは反転ペースト
                var switchedDataTuple = TrySwitchMorphLRFromClipboard();
                if (switchedDataTuple.Item1 == null)
                    return false;
                Clipboard.SetData("MMM_MotionFrameData", switchedDataTuple.Item2);
                // Ctrl+V押下をシミュレート
                MMDUtil.MMMUtilility.SendKeyToFormWithKeyboardEvent(
                        this.ApplicationForm as Form,
                                0x56,  // VK_V
                        true, false, false);

                // ペースト後クリップボードを戻す
                Clipboard.SetData("MMM_MotionFrameData", switchedDataTuple.Item2);
                return true;
            }
            else
            {
                // 全モーフの選択を解除
                if (this.Scene.ActiveModel.Bones.SelectMany(b => b.Layers.SelectMany(l => l.SelectedFrames)).Count() > 0)
                    //選択ボーンがある場合は処理しない
                    return false;
                var flg = false;
                try
                {
                    // Wキー単体の場合はモーフ左右反転
                    var prevSelectedMorphs = new List<Morph>();
                    foreach (var selectedMorph in this.Scene.ActiveModel.Morphs.Where(m => m.SelectedFrames.Count() > 0))
                    {
                        var currentFrame = selectedMorph.SelectedFrames.FirstOrDefault(f => f.FrameNumber == this.Scene.MarkerPosition);
                        if (currentFrame == null)
                            continue;
                        var partnerMorph = FindPartnerMorph(this.Scene.ActiveModel, selectedMorph.Name);
                        if (partnerMorph != null)
                        {
                            partnerMorph.CurrentWeight = currentFrame.Weight;
                            partnerMorph.Selected = true;
                            flg = true;
                        }
                        prevSelectedMorphs.Add(selectedMorph);
                    }
                    if (flg)
                    {
                        foreach (var morph in prevSelectedMorphs)
                        {
                            morph.Selected = false;
                        }
                    }
                }
                finally
                {
                    if (flg)
                    {
                        // アンドゥを効かせられるよう、partnerMorph.Frames.AddKeyFrameは使わず仮想的にEnterキーを送る
                        MMDUtil.MMMUtilility.SendKeyToFormWithKeyboardEvent(
                            this.ApplicationForm as Form,
                                    0x0D,  // VK_RETURN
                            false, false, false);
                    }
                }
            }

            return false;// 普通にWも仕事させる
        }

        /// <summary>
        /// 左右逆のモーフを探して返す
        /// </summary>
        /// <param name="model"></param>
        /// <param name="morph"></param>
        /// <returns></returns>
        private Morph FindPartnerMorph(Model model, string morphName)
        {
            var lr = new string[] { "右", "左" };
            if (!lr.Any(x => morphName.StartsWith(x)))
                return null;
            var currentLRJp = lr.FirstOrDefault(x => morphName.StartsWith(x) || morphName.EndsWith(x)); //現在の接頭辞
            if (currentLRJp == null)
                return null;
            var partnerLRJp = lr.FirstOrDefault(x => !morphName.StartsWith(x) || morphName.EndsWith(x)); //反対側の接頭辞

            var lrTail = partnerLRJp == "右" ? "_lf" : "_rf";
            var partnerLrTail = partnerLRJp == "右" ? "_rf" : "_lf";
            var partnerMorphName = morphName.Replace(currentLRJp, partnerLRJp); //反対側のモーフ名

            partnerMorphName = partnerMorphName.Replace(lrTail, partnerLrTail);
            return model.Morphs.FirstOrDefault(m => m.Name == partnerMorphName);
        }

        /// <summary>
        /// クリップボードにMMM_MotionFrameDataが存在し、モーフ情報が含まれている場合、
        /// モーフの左右を反転させたデータを返す
        /// </summary>
        /// <returns>
        /// item1: 反転前のデータ（失敗時null）
        /// item2: 反転後のデータ（失敗時null）
        /// </returns>
        private (object, object) TrySwitchMorphLRFromClipboard()
        {
            try
            {
                var dataObject = Clipboard.GetDataObject();
                var formats = dataObject?.GetFormats();
                if (dataObject == null || !dataObject.GetDataPresent("MMM_MotionFrameData"))
                    return (null, null);

                var data = Clipboard.GetData("MMM_MotionFrameData");
                if (data == null)
                    return (null, null);

                // dataをディープコピーして更新前の状態を保持する
                var clonedData = DeepCopyBySerialization(data);
                if (clonedData == null)
                    return (null, null);

                // motiondata.data まで取得（コピーしたオブジェクトから）
                var motiondataField = data.GetType().GetField("motiondata");
                if (motiondataField == null)
                    return (null, null);

                var motiondata = motiondataField.GetValue(data);
                if (motiondata == null)
                    return (null, null);

                var dataField = motiondata.GetType().GetField("data");
                if (dataField == null)
                    return (null, null);

                var outerDict = dataField.GetValue(motiondata) as System.Collections.IDictionary;
                if (outerDict == null)
                    return (null, null);

                var flg = false;
                // Dictionary<int, Dictionary<FrameItemType, ...>> を走査
                foreach (System.Collections.DictionaryEntry modelEntry in outerDict)
                {
                    // Dictionary<FrameItemType, Dictionary<string, ...>> を取得
                    if (modelEntry.Value is System.Collections.IDictionary frameTypeDict)
                    {
                        foreach (System.Collections.DictionaryEntry frameTypeEntry in frameTypeDict)
                        {
                            // FrameItemType が Morph の場合のみ処理
                            if (frameTypeEntry.Key.ToString() == "Morph")
                            {
                                // Dictionary<string, Dictionary<int, List<FrameData>>> を取得
                                if (frameTypeEntry.Value is System.Collections.IDictionary morphDict)
                                {
                                    var addMorphs = new List<(string, object)>();
                                    var delMorphs = new List<string>();
                                    foreach (System.Collections.DictionaryEntry morphEntry in morphDict)
                                    {
                                        string morphName = morphEntry.Key as string;
                                        var partner = FindPartnerMorph(this.Scene.ActiveModel, morphName);
                                        if (partner != null)
                                        {
                                            addMorphs.Add((partner.Name, morphEntry.Value));
                                            delMorphs.Add(morphName);
                                            flg = true;
                                        }
                                    }
                                    foreach (var morphName in delMorphs)
                                    {
                                        morphDict.Remove(morphName);
                                    }
                                    foreach (var addMorph in addMorphs)
                                    {
                                        morphDict.Add(addMorph.Item1, addMorph.Item2);
                                    }
                                }
                            }
                        }
                    }
                }
                return flg ? (clonedData, data) : (null, null);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"TrySwitchMorphLRFromClipboard failed: {ex.Message}");
                Debug.WriteLine($"StackTrace: {ex.StackTrace}");
                return (null, null);
            }
        }

        /// <summary>
        /// バイナリシリアライゼーションを使用してオブジェクトをディープコピーします
        /// </summary>
        /// <param name="obj">コピー元オブジェクト</param>
        /// <returns>ディープコピーされたオブジェクト</returns>
        private object DeepCopyBySerialization(object obj)
        {
            if (obj == null)
                return null;

            try
            {
                using (var ms = new System.IO.MemoryStream())
                {
                    var formatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
                    formatter.Serialize(ms, obj);
                    ms.Position = 0;
                    return formatter.Deserialize(ms);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"DeepCopyBySerialization failed: {ex.Message}");
                return null;
            }
        }
    }
}