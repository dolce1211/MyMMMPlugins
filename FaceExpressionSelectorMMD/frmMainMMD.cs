﻿using FaceExpressionHelper;
using FaceExpressionHelper.UI;
using LibMMD.Pmx;
using LibMMD.Vmd;
using Linearstar.Keystone.IO.MikuMikuDance;
using MMDUtil;
using MyUtility;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;
using System.Xml;
using static MMDUtil.MMDUtilility;
using static System.Net.Mime.MediaTypeNames;

namespace FaceExpressionSelectorMMD
{
    public partial class frmMainMMD : frmMainBase
    {
        private ActiveModelInfo _currentModel = new ActiveModelInfo();

        private bool _isBusyGettingActiveModel;

        /// <summary>
        /// 取得済みのActiveModelInfoのキャッシュ。モデル名をキーにしてるので、同名かつモーフ状態が異なるモデルが同じプロジェクトに複数読み込まれるとおかしくなるかも
        /// </summary>
        //private Dictionary<string, ActiveModelInfo> _oldCache = new Dictionary<string, ActiveModelInfo>();

        /// <summary>
        /// テストキャッシュ
        /// </summary>
        private Dictionary<string, ActiveModelInfo> _modelCache = new Dictionary<string, ActiveModelInfo>();

        /// <summary>
        /// 監視対象のpmmが前回保存された日時
        /// </summary>
        private DateTime _prevpmmSavedTime = new DateTime();

        private int _prevmmdID = -1;

        private ActiveMorphModelFinder _modelFinder = null;
        //private System.Threading.Timer _timer = null;

        public frmMainMMD() : base()
        {
            mmdSelector.Visible = true;

            this.オプションtoolStripMenuItem1.Visible = true;
            this.処理中にモデルを非表示にするToolStripMenuItem.Checked = base._args.HideModelsWhileProcessing;
            //MMDで動いている
            OperationgMode = OperatingMode.OnMMD;
        }

        private bool _isBusy = false;

        public override bool IsBusy
        {
            get => this._isBusy;
            protected set
            {
                this._isBusy = value;
                if (this._modelFinder != null)
                    this._modelFinder.IsBusy = value;
            }
        }

        /// <summary>
        /// 現在のフレームを取得します。
        /// </summary>
        /// <returns></returns>
        protected override long GetCurrentFrame()
        {
            var mmd = this.mmdSelector.SelectMMD();
            if (mmd == null)
                //MMD紐づけ無し
                return 0;

            return MMDUtilility.TryGetFrameNumber(mmd.MainWindowHandle);
        }

        /// <summary>
        /// 現在のアクティブモデルの名前を返します。
        /// </summary>
        protected override string ActiveModelName
        {
            get
            {
                if (this._currentModel == null)
                    return string.Empty;

                return this._currentModel.ModelName;
            }
        }

        protected override void OnShown(EventArgs e)
        {
            this._modelFinder = new ActiveMorphModelFinder(this, this.mmdSelector, this.ShowlblWait, this.HidelblWait);
            this._modelFinder.ActiveModelChangedEventHandler += this.OnActiveModelChanged;

            base.OnShown(e);
        }

        protected override void OnActiveModelChanged(object sender, ActiveModelChangedEventArgs e)
        {
            var newactivemodel = e.CurrentActiveModel as ActiveModelInfo;
            if (newactivemodel != null)
                this._currentModel = newactivemodel;
            base.OnActiveModelChanged(sender, e);
        }

        /// <summary>
        /// 選択されたモーフを探してMMD上で選択します。(MMDでのみ使用)
        /// </summary>
        /// <param name="sender">アクティブモデル</param>
        /// <param name="e"></param>
        /// <returns></returns>
        protected override void OnMorphSelected(object sender, MorphSelectedEventArgs e)
        {
            var mmd = this.mmdSelector.SelectMMD();
            if (mmd == null)
                //MMD紐づけ無し
                return;
            MMDUtil.MMDUtilility.BeginAndEndUpdate(mmd.MainWindowHandle, true);

            if (this._currentModel == null)
                return;
            if (this._currentModel.ModelName != e.ActiveModelName)
                //アクティブモデルじゃないっぽい
                return;

            if (string.IsNullOrEmpty(e.MorphName))
            {
                //モーフ名未指定ならフレームリセットして抜ける
                MMDUtil.MMDUtilility.TryResetFrameNumber(mmd.MainWindowHandle);
            }
            else
            {
                foreach (var morph in this._currentModel.AllMorphs.Values.SelectMany(n => n))
                {
                    if (morph.MorphName == e.MorphName)
                    {
                        if (e.Value == float.MinValue)
                            //valueの指定がないならコンボのindexを変更するだけ
                            MMDUtil.MMDUtilility.TrySetMorphIndex(mmd.MainWindowHandle, morph.MorphType, morph.ComboBoxIndex);
                        else
                        {
                            //valueの指定をするなら値を入れる
                            if (e.ResetFrame)
                                MMDUtil.MMDUtilility.TryResetFrameNumber(mmd.MainWindowHandle);

                            MMDUtil.MMDUtilility.TrySetMorphValue(mmd.MainWindowHandle, morph.MorphType, morph.ComboBoxIndex, e.Value, false);
                        }

                        break;
                    }
                }
            }
            base.OnMorphSelected(sender, e);
        }

        private void frmMainMMD_Shown(object sender, EventArgs e)
        {
            //var mmd = this._mmdSelector.SelectMMD();
        }

        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmMainMMD));
            this.pnlTop.SuspendLayout();
            this.pnlBottom.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar1)).BeginInit();
            this.SuspendLayout();
            //
            // mmdSelector
            //
            this.mmdSelector.Location = new System.Drawing.Point(0, 578);
            //
            // pnlBottom
            //
            this.pnlBottom.Location = new System.Drawing.Point(0, 513);
            //
            // frmMainMMD
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.ClientSize = new System.Drawing.Size(287, 624);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "frmMainMMD";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmMainMMD_FormClosing);
            this.Shown += new System.EventHandler(this.frmMainMMD_Shown);
            this.pnlTop.ResumeLayout(false);
            this.pnlTop.PerformLayout();
            this.pnlBottom.ResumeLayout(false);
            this.pnlBottom.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        /// <summary>
        /// lblWaitを隠す
        /// </summary>
        private void HidelblWait()
        {
            this.Invoke((Action)(() =>
            {
                this.lblWait.Text = String.Empty;
                this.lblWait.Visible = false;
                this.pnlTop.Enabled = true;
                this.pnlBottom.Enabled = true;
                this.btnReset.Enabled = true;
                this.対象制御ToolStripMenuItem.Enabled = true;
            }));
        }

        /// <summary>
        /// lblWaitを出す
        /// </summary>
        /// <param name="text"></param>
        private void ShowlblWait(string text)
        {
            this.Invoke((Action)(() =>
            {
                this.lblWait.Text = text;
                this.lblWait.Visible = true;
                this.lblWait.BringToFront();
                this.lblWait.Refresh();
                this.pnlTop.Enabled = false;
                this.pnlBottom.Enabled = false;
                this.btnReset.Enabled = false;

                this.対象制御ToolStripMenuItem.Enabled = false;
            }));
        }

        /// <summary>
        /// this._currentModelに現在のモーフ状態を取得して返します。
        /// </summary>
        /// <returns></returns>
        private ActiveModelInfo TryApplyCurrentWeight()
        {
            var mmd = this.mmdSelector.SelectMMD();
            if (mmd == null)
            {
                //MMD紐づけ無し
                return null;
            }
            if (this._currentModel == null || string.IsNullOrWhiteSpace(this._currentModel.ModelName))
            {
                //アクティブモデル無し
                return null;
            }
            this.Invoke((Action)(() => this.Cursor = Cursors.WaitCursor));

            var retHash = MMDUtilility.TryGetAllMorphValue(mmd.MainWindowHandle);
            foreach (var kvp in retHash)
            {
                var tuple = kvp.Key;
                var weight = kvp.Value;
                var lst = _currentModel.AllMorphs[tuple.Item1];
                var morph = lst.Where(n => n.ComboBoxIndex == tuple.Item2).FirstOrDefault();
                if (morph == null)
                {
                }
                else
                {
                    morph.Weight = weight;
                }
            }
            this.BeginInvoke((Action)(() => this.Cursor = Cursors.Default));
            return this._currentModel;
        }

        #region "override"

        /// <summary>
        /// モーフ一覧情報を返します。
        /// </summary>
        /// <returns></returns>
        protected override bool ApplyExpression(int bufferFrames, ExpressionItem item)
        {
            var mmd = this.mmdSelector.SelectMMD();
            if (mmd == null)
            {
                //MMD紐づけ無し
                return false;
            }

            if (this._currentModel == null || string.IsNullOrWhiteSpace(this._currentModel.ModelName))
                //アクティブモデル無し
                return false;

            //処理対象のモーフ情報を取得
            var applyingMorphs = this.GetApplyingMorphs(item);
            //無いモーフチェック
            var missingMorphs = this.GetMissingMorphs(item);
            if (missingMorphs.Count > 0)
            {
                using (var frmMissing = new frmShowMissingMorphs(this._currentModel.ModelName, missingMorphs.Select(n => n.MorphName).ToList()))
                {
                    if (frmMissing.ShowDialog(this) != DialogResult.OK)
                    {
                        if (frmMissing.OpenReplace)
                            this.btnReplace.PerformClick();
                        return false;
                    }
                }
            }
            var vmd1 = new VmdDocument() { ModelName = this.ActiveModelName };

            //現在の物理演算状態を取得する
            //-1:取得失敗 0:オン/オフモード  1:常に演算 2:トレースモード 3:演算しない
            var currentphysicsState = MMDUtilility.TryGetPhysicsState(mmd.MainWindowHandle);
            //現在のエフェクトオンオフ
            var currentEffectState = (MMDUtilility.TryGetMenuChecked(mmd.MainWindowHandle, "MMEffect", "エフェクト使用") == 1);
            //現在のモデル表示状態
            var currentModelInvisibleState = (MMDUtilility.TryGetMenuChecked(mmd.MainWindowHandle, "表示(&V)", "モデル非表示(&A)") == 1);

            var sw = new Stopwatch();
            sw.Start();

            var currentFrame = Convert.ToInt64(MMDUtilility.TryGetFrameNumber(mmd.MainWindowHandle));

            try
            {
                var msg = $"「{item.Name}」を適用中です。\r\nMMDに触らないでください。";
                this.ShowlblWait(msg);

                //物理演算をオフにする
                MMDUtilility.TrySetPhysicsState(mmd.MainWindowHandle, 3);
                //エフェクトをオフにする
                MMDUtilility.TrySetMenuChecked(mmd.MainWindowHandle, "MMEffect", "エフェクト使用", false);

                if (this.処理中にモデルを非表示にするToolStripMenuItem.Checked)
                    //モデルを非表示にするc
                    MMDUtilility.TrySetMenuChecked(mmd.MainWindowHandle, "表示(&V)", "モデル非表示(&A)", true);

                //現在の適用状態を反映
                var model = this.TryApplyCurrentWeight();
                if (model == null || string.IsNullOrEmpty(model.ModelName))
                {
                    return false;
                }
                Console.WriteLine($"TryApplyCurrentWeight：{this.ActiveModelName}:{item.Name},{sw.ElapsedMilliseconds}");

                var targetMorphs = new List<(float, MorphItemWithIndex)>();
                var notTargetMorphs = new List<MorphItemWithIndex>();

                //適用
                var failed = new List<string>();
                foreach (MorphItemWithIndex morph in this._currentModel.AllMorphs.Values.SelectMany(n => n))
                {
                    if (!base._args.IsTargetMorph(morph))
                        //処理対象外のモーフ
                        continue;

                    var applyingMI = applyingMorphs.Where(n => n.DspMorphName == morph.MorphName).FirstOrDefault();
                    if (applyingMI != null && applyingMI.Ignore)
                        //無視するモーフだ
                        continue;
                    if (applyingMI != null)
                    {
                        //対象のモーフ
                        if (morph.Weight != applyingMI.Weight && true)
                        {
                            vmd1.MorphFrames.Add(new VmdMorphFrame() { FrameTime = (uint)0, Name = morph.MorphName, Weight = morph.Weight });
                            targetMorphs.Add((applyingMI.Weight, morph));
                        }
                    }
                    else
                    {
                        if (morph.Weight != 0)
                        {
                            //対象外のモーフでウェイトが乗っている
                            vmd1.MorphFrames.Add(new VmdMorphFrame() { FrameTime = (uint)0, Name = morph.MorphName, Weight = morph.Weight });
                        }

                        notTargetMorphs.Add(morph);
                    }
                }

                foreach ((float, MorphItemWithIndex) tuple in targetMorphs)
                {
                    var weight = tuple.Item1;
                    var morph = tuple.Item2;
                    vmd1.MorphFrames.Add(new VmdMorphFrame() { FrameTime = (uint)Math.Abs(bufferFrames), Name = morph.MorphName, Weight = weight });
                }
                foreach (MorphItemWithIndex morph in notTargetMorphs)
                {
                    if (morph.Weight != 0)
                    {
                        vmd1.MorphFrames.Add(new VmdMorphFrame() { FrameTime = (uint)Math.Abs(bufferFrames), Name = morph.MorphName, Weight = 0 });
                    }
                }

                if (bufferFrames < 0)
                    MMDUtilility.TrySetFrameNumber(mmd.MainWindowHandle, currentFrame + Convert.ToInt64(bufferFrames));

                //vmdデータをMMDに投げる
                using (var vmdStream = new MemoryStream())
                {
                    vmd1.Write(vmdStream);
                    vmdStream.Seek(0, SeekOrigin.Begin);

                    var rtn = MmdDrop.DropFile(mmd.MainWindowHandle, new MmdDropFile("TempMotion" + mmd.Id + ".vmd", vmdStream)
                    {
                        Timeout = 500,
                    });
                    if (!rtn)
                        return false;
                }

                //ダイアログを潰す
                MMDUtilility.PressOKToDialog(mmd.MainWindowHandle, new string[] { "モーションチェック", "MMPlus", "モーションデータ読込" });

                //MMDにフォーカスを当てる
                MMDUtilility.SetForegroundWindow(mmd.MainWindowHandle);

                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, $"{ex.Message}\r\n\r\n{ex.StackTrace}");
                return false;
            }
            finally
            {
                this.HidelblWait();
                //物理演算を戻す
                MMDUtilility.TrySetPhysicsState(mmd.MainWindowHandle, currentphysicsState);
                //エフェクト状態を戻す
                MMDUtilility.TrySetMenuChecked(mmd.MainWindowHandle, "MMEffect", "エフェクト使用", currentEffectState);
                //モデル表示状態を戻す
                MMDUtilility.TrySetMenuChecked(mmd.MainWindowHandle, "表示(&V)", "モデル非表示(&A)", currentModelInvisibleState);

                if (bufferFrames >= 0)
                    MMDUtilility.TrySetFrameNumber(mmd.MainWindowHandle, currentFrame + Convert.ToInt64(bufferFrames));
                else
                    MMDUtilility.TrySetFrameNumber(mmd.MainWindowHandle, currentFrame);
                Console.WriteLine($"完了：{this.ActiveModelName}:{item.Name},{sw.ElapsedMilliseconds}");
            }
        }

        /// <summary>
        /// 今のアクティブモデルの有効なモーフをすべて返します。
        /// </summary>
        /// <returns></returns>
        protected override List<MorphItem> GetAllMorphsForActiveModel()
        {
            var ret = new List<MorphItem>();

            if (this._currentModel == null || string.IsNullOrWhiteSpace(this._currentModel.ModelName))
                return ret;

            return this._currentModel.AllMorphs.Values.SelectMany(n => n).Cast<MorphItem>().ToList();
        }

        /// <summary>
        /// アクティブモデルの今のフレームのモーフ適用状態を返します。
        /// </summary>
        /// <returns>null:アクティブモデルなし</returns>
        protected override List<MorphItem> GetCurrentMorphState()
        {
            this.ShowlblWait("現在のモーフ適用状態を取得しています");
            this.Cursor = Cursors.WaitCursor;
            try
            {
                var model = TryApplyCurrentWeight();
                if (model == null || string.IsNullOrWhiteSpace(model.ModelName))
                {
                    return null;
                }

                var ret = new List<MorphItem>();
                foreach (var kvp in model.AllMorphs)
                {
                    foreach (var morph in kvp.Value)
                    {
                        if (base._args.IsTargetMorph(morph))
                        {
                            if (morph.Weight != 0)
                                ret.Add(morph.Clone());
                        }
                    }
                }

                return ret;
            }
            finally
            {
                this.HidelblWait();
                this.Cursor = Cursors.Default;
            }
        }

        #endregion "override"

        private void frmMainMMD_FormClosing(object sender, FormClosingEventArgs e)
        {
            this._modelFinder?.Dispose();
        }
    }

    /// <summary>
    /// コンボボックス内の何個目のモーフかの情報を保持できるMorphItemです。
    /// </summary>
    [DebuggerDisplay("{MorphName},{ComboBoxIndex}")]
    public class MorphItemWithIndex : MorphItem
    {
        /// <summary>
        /// コンボボックス内のindex
        /// </summary>
        public int ComboBoxIndex { get; set; } = -1;
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
                MorphType morphtype = this.PmxPnlType2Morphtype(mrph.PanelType);
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
}