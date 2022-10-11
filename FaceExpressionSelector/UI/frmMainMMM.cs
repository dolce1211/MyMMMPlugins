using MikuMikuPlugin;
using MyUtility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

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

        private long _prevAppliedFrame = -1;

        /// <summary>
        /// 現在のモーフ情報を表示します。
        /// </summary>
        public override void ApplyCurrentMorph()
        {
            if (_scene.MarkerPosition != _prevAppliedFrame)
            {
                //描画停止
                this.BeginUpdate();
                try
                {
                    base.lstValue.Items.Clear();
                    var currentMorphs = this.CreateCurrentMorphList(false);
                    if (currentMorphs != null)
                    {
                        base.lstValue.Items.AddRange(currentMorphs.ToArray());
                    }
                }
                finally
                {
                    //描画再開
                    this.EndUpdate();
                }
            }
            _prevAppliedFrame = _scene.MarkerPosition;
        }

        /// <summary>
        /// モーフ一覧情報を返します。
        /// </summary>
        /// <returns>null:無効な状態</returns>
        protected override List<MorphItem> CreateCurrentMorphList(bool showErrorMsgbox)
        {
            var activeModel = this._scene.ActiveModel;
            if (activeModel == null)
            {
                if (showErrorMsgbox)
                    MessageBox.Show("モデルを選択してください");
                return null;
            }
            var ret = new List<MorphItem>();

            foreach (Morph morph in activeModel.Morphs)
            {
                if (morph.CurrentWeight != 0)
                {
                    var morphitem = new MorphItem()
                    {
                        MorphName = morph.DisplayName,
                        Weight = morph.CurrentWeight,
                    };
                    ret.Add(morphitem);
                }
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

            //操作対象モーフ一覧
            var applyingMorphs = activeModel.Morphs.ToList();

            //無いモーフが無いかチェック
            var missingMorphs = new List<MorphItem>();
            foreach (MorphItem mi in item.MorphItems)
            {
                if (!applyingMorphs.Any(n => n.Name == mi.MorphName))
                    missingMorphs.Add(mi);
            }
            if (missingMorphs.Count > 0)
            {
                var msg = $@"{string.Join("\r\n", missingMorphs.Select(n => n.MorphName).ToArray())}

{activeModel.Name}には以上のモーフがありません。
続行しますか？";
                if (MessageBox.Show(msg, "確認", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.No)
                    return false;
            }

            //適用
            foreach (Morph morph in applyingMorphs)
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
                    if (morph.PanelType != PanelType.Etc && morph.PanelType != PanelType.None)//「その他」パネル、パネル外のモーフは初期化対象外
                    {
                        if (morph.CurrentWeight != 0)
                        {
                            framelist.Add(new MorphFrameData(this._scene.MarkerPosition, morph.CurrentWeight));
                            framelist.Add(new MorphFrameData(this._scene.MarkerPosition + bufferFrames, 0));
                        }
                    }
                }
                if (framelist.Count > 0)
                    morph.Frames.AddKeyFrame(framelist);
            }
            //画面のリフレッシュ
            this._scene.MarkerPosition += bufferFrames;

            var frm = this._applicationForm as Form;
            if (frm != null)
                frm.Refresh();
            return true;
        }

        private void _frmPic_Load(object sender, EventArgs e)
        {
        }
    }
}