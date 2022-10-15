using MikuMikuPlugin;
using MMDUtil;
using MyUtility;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Window;

namespace FaceExpressionHelper.UI
{
    public class frmMainMMM : frmMainBase
    {
        private Scene _scene = null;
        private IWin32Window _applicationForm = null;

        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="scene"></param>
        public frmMainMMM(Scene scene, IWin32Window applicationForm) : base()
        {
            this._scene = scene;
            this._applicationForm = applicationForm;
        }

        /// <summary>
        /// 現在のアクティブモデルの名前を返します。
        /// </summary>
        protected override string ActiveModelName
        {
            get
            {
                if (this._scene.ActiveModel == null)
                    return string.Empty;

                return this._scene.ActiveModel.Name;
            }
        }

        /// <summary>
        /// 今のアクティブモデルの有効なモーフをすべて返します。
        /// </summary>
        /// <returns></returns>
        protected override List<MorphItem> GetAllMorphsForActiveModel()
        {
            var activeModel = this._scene.ActiveModel;
            if (activeModel == null)
            {
                return null;
            }

            return activeModel.Morphs.Select(n =>
            {
                return new MorphItem()
                {
                    MorphName = n.Name,
                    Weight = n.CurrentWeight,
                    MorphType = n.PanelType.ToMyPanelType()
                };
            }).ToList();
        }

        /// <summary>
        /// アクティブモデルの今のフレームのモーフ適用状態を返します。
        /// </summary>
        /// <returns>null:アクティブモデルなし</returns>
        protected override List<MorphItem> GetCurrentMorphState()
        {
            var activeModel = this._scene.ActiveModel;
            if (activeModel == null)
            {
                return null;
            }
            var ret = new List<MorphItem>();
            foreach (Morph morph in activeModel.Morphs)
            {
                if (this._args.ExceptionMainMorphs.Contains(morph.Name))
                    //対象外モーフ
                    continue;

                MMDUtil.MMDUtilility.MorphType morphtype = morph.PanelType.ToMyPanelType();

                if (morph.CurrentWeight != 0)
                {
                    var morphitem = new MorphItem()
                    {
                        MorphName = morph.DisplayName,
                        Weight = morph.CurrentWeight,
                        MorphType = morphtype,
                    };
                    ret.Add(morphitem);
                }
            }

            return ret;
        }

        /// <summary>
        /// 現在のアクティブモデルに欠けているモーフ一覧を取得します。
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        protected override List<MorphItem> GetMissingMorphs(ExpressionItem item)
        {
            var activeModel = this._scene.ActiveModel;
            if (activeModel == null)
            {
                return null;
            }
            var ret = new List<MorphItem>();
            var allMorphs = activeModel.Morphs.Where(n => !this._args.ExceptionMainMorphs.Contains(n.Name)).ToList();
            foreach (MorphItem mi in item.MorphItems)
            {
                if (!allMorphs.Any(n => n.Name == mi.MorphName))
                    ret.Add(mi);
            }
            return ret;
        }

        /// <summary>
        /// モーフ一覧情報を返します。
        /// </summary>
        /// <returns></returns>
        protected override bool ApplyExpression(int bufferFrames, ExpressionItem item)
        {
            var activeModel = this._scene.ActiveModel;
            if (activeModel == null)
            {
                MessageBox.Show("モデルを選択してください");
                return false;
            }
            var ret = new List<MorphItem>();

            //操作対象モーフ一覧(対象外モーフを除く)
            var allMorphs = activeModel.Morphs.Where(n => !this._args.ExceptionMainMorphs.Contains(n.Name)).ToList();

            //無いモーフチェック
            var missingMorphs = this.GetMissingMorphs(item);
            if (missingMorphs.Count > 0)
            {
                var msg = $@"{string.Join("\r\n", missingMorphs.Select(n => n.MorphName).ToArray())}

{activeModel.Name}には以上のモーフがありません。
続行しますか？";
                if (MessageBox.Show(msg, "確認", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.No)
                    return false;
            }

            //描画を止める
            MMMUtilility.BeginAndEndUpdate(false);

            try
            {
                //適用
                var notTargetMorphs = new List<(float, Morph)>();
                foreach (Morph morph in allMorphs)
                {
                    var framelist = new List<MorphFrameData>();
                    var applyingMI = item.MorphItems.Where(n => n.MorphName == morph.Name).FirstOrDefault();
                    if (applyingMI != null)
                    {
                        //対象のモーフ
                        framelist.Add(new MorphFrameData(this._scene.MarkerPosition, morph.CurrentWeight));
                        framelist.Add(new MorphFrameData(this._scene.MarkerPosition + bufferFrames, applyingMI.Weight));
                    }
                    else
                    {
                        //対象外のモーフ
                        notTargetMorphs.Add((morph.CurrentWeight, morph));
                    }

                    if (framelist.Count > 0)
                        morph.Frames.AddKeyFrame(framelist);
                }

                //対象外のモーフの処理を行う
                this._scene.MarkerPosition += bufferFrames;
                foreach ((float, Morph) tuple in notTargetMorphs)
                {
                    var prevWeight = tuple.Item1;
                    var morph = tuple.Item2;

                    var framelist = new List<MorphFrameData>();
                    if (morph.CurrentWeight != 0)
                    {
                        framelist.Add(new MorphFrameData(this._scene.MarkerPosition - bufferFrames, prevWeight));
                        framelist.Add(new MorphFrameData(this._scene.MarkerPosition, 0));
                    }
                    if (framelist.Count > 0)
                        morph.Frames.AddKeyFrame(framelist);
                }
                this._scene.MarkerPosition -= bufferFrames;
            }
            catch (Exception)
            {
                Debugger.Break();
            }
            finally
            {
                //描画を再開する
                MMMUtilility.BeginAndEndUpdate(true);
            }

            //画面のリフレッシュ
            this._scene.MarkerPosition += bufferFrames;

            var frm = this._applicationForm as Form;
            if (frm != null)
                frm.Refresh();
            return true;
        }

        /// <summary>
        /// アクティブモデルが変更された
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected override void OnActiveModelChanged(object sender, ActiveModelChangedEventArgs e)
        {
            base.OnActiveModelChanged(sender, e);
        }
    }
}