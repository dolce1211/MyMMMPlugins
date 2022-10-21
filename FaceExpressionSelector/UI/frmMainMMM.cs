using MikuMikuPlugin;
using MMDUtil;
using MyUtility;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Windows.Forms;
using static MMDUtil.MMDUtilility;
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

            //MMMで動いている
            OperationgMode = OperatingMode.OnMMM;
        }

        /// <summary>
        /// 現在のフレームを取得します。
        /// </summary>
        /// <returns></returns>
        protected override long GetCurrentFrame()
        {
            if (this._scene != null)
                return this._scene.MarkerPosition;

            return 0;
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
                var morphitem = new MorphItem()
                {
                    MorphName = morph.DisplayName,
                    Weight = morph.CurrentWeight,
                    MorphType = morph.PanelType.ToMyPanelType(),
                };

                if (!this._args.IsTargetMorph(morphitem))
                    //対象外モーフ
                    continue;

                if (morph.CurrentWeight != 0)
                    ret.Add(morphitem);
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
            var allMorphs = activeModel.Morphs.Where(n =>
            {
                var morphitem = new MorphItem()
                {
                    MorphName = n.DisplayName,
                    Weight = n.CurrentWeight,
                    MorphType = n.PanelType.ToMyPanelType(),
                };
                return this._args.IsTargetMorph(morphitem);
            }).ToList();

            //処理対象のモーフ情報を取得
            var applyingMorphs = this.GetApplyingMorphs(item);
            //無いモーフチェック
            var missingMorphs = this.GetMissingMorphs(item);
            if (missingMorphs.Count > 0)
            {
                using (var frmMissing = new frmShowMissingMorphs(activeModel.Name, missingMorphs.Select(n => n.MorphName).ToList()))
                {
                    if (frmMissing.ShowDialog(this) != DialogResult.OK)
                    {
                        if (frmMissing.OpenReplace)
                            this.btnReplace.PerformClick();
                        return false;
                    }
                }
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
                    var applyingMI = applyingMorphs.Where(n => n.DspMorphName == morph.Name).FirstOrDefault();
                    if (applyingMI != null && applyingMI.Ignore)
                        //無視するモーフだ
                        continue;

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