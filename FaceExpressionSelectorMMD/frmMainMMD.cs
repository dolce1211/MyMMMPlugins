using FaceExpressionHelper;
using FaceExpressionHelper.UI;
using LibMMD.Pmx;
using MMDUtil;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
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
        private Dictionary<string, ActiveModelInfo> _modelCache = new Dictionary<string, ActiveModelInfo>();

        /// <summary>
        /// テストキャッシュ
        /// </summary>
        private Dictionary<string, ActiveModelInfo> _testCache = new Dictionary<string, ActiveModelInfo>();

        private int? _prevmmdID = -1;

        private System.Threading.Timer _timer = null;

        public frmMainMMD() : base()
        {
            mmdSelector.Visible = true;
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
            _timer = new System.Threading.Timer(new System.Threading.TimerCallback(
                (Action<object>)(async x =>
                {
                    _timer.Change(int.MaxValue, int.MaxValue);

                    await this.TryApplyActiveModel();

                    _timer.Change(1000, 0);
                }))
                , null, 10, 1000);

            base.OnShown(e);
        }

        private void frmMainMMD_Shown(object sender, EventArgs e)
        {
            //var mmd = this.mmdSelector.SelectMMD();
        }

        private void InitializeComponent()
        {
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).BeginInit();
            this.pnlTop.SuspendLayout();
            this.pnlBottom.SuspendLayout();
            this.SuspendLayout();
            //
            // mmdSelector
            //
            this.mmdSelector.Location = new System.Drawing.Point(0, 578);
            //
            // pnlBottom
            //
            this.pnlBottom.Location = new System.Drawing.Point(0, 542);
            //
            // frmMainMMD
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.ClientSize = new System.Drawing.Size(287, 624);
            this.Name = "frmMainMMD";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmMainMMD_FormClosing);
            this.Shown += new System.EventHandler(this.frmMainMMD_Shown);
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).EndInit();
            this.pnlTop.ResumeLayout(false);
            this.pnlTop.PerformLayout();
            this.pnlBottom.ResumeLayout(false);
            this.pnlBottom.PerformLayout();
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
                this.pnlTop.Enabled = false;
                this.pnlBottom.Enabled = false;
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

            foreach (var kvp in _currentModel.AllMorphs)
            {
                var index = 0;
                foreach (MorphItem morph in kvp.Value)
                {
                    morph.Weight = MMDUtilility.TryGetMorphValue(mmd.MainWindowHandle, kvp.Key, index);
                    //MMDUtilility.TrySetMorphValueAsIs(mmd.MainWindowHandle, kvp.Key, index);
                    index++;
                }
            }
            this.BeginInvoke((Action)(() => this.Cursor = Cursors.Default));
            return this._currentModel;
        }

        /// <summary>
        /// 現在のアクティブモデルの情報をキャッシュします。
        /// </summary>
        /// <returns></returns>
        private Task<ActiveModelInfo> TryApplyActiveModel()
        {
            return Task<ActiveModelInfo>.Run(() =>
            {
                if (base.IsBusy)
                    return this._currentModel;
                if (this.mmdSelector.IsBusy)
                    return this._currentModel;
                if (_isBusyGettingActiveModel)
                    return this._currentModel;
                _isBusyGettingActiveModel = true;
                Process mmd = null;
                var activeModelName = string.Empty;
                var isModelChanged = false;
                if (this.IsDisposed)
                    return null;
                try
                {
                    this.Invoke((Action)(() => mmd = this.mmdSelector.SelectMMD()));

                    if (mmd == null)
                    {
                        //MMD紐づけ無し
                        _modelCache = new Dictionary<string, ActiveModelInfo>();
                        _testCache = new Dictionary<string, ActiveModelInfo>();
                        _currentModel = new ActiveModelInfo();
                        return _currentModel;
                    }

                    Console.WriteLine($"{_prevmmdID},{mmd?.Id}");
                    if (_prevmmdID != mmd?.Id)
                    {
                        //監視してるMMDが切り替わった。モデルキャッシュを初期化する
                        _modelCache = new Dictionary<string, ActiveModelInfo>();
                        _testCache = new Dictionary<string, ActiveModelInfo>();
                        this._currentModel = new ActiveModelInfo();
                        isModelChanged = true;
                    }

                    if (_testCache == null || _testCache.Count == 0)
                    {
                        var sw = new Stopwatch();
                        sw.Start();
                        _testCache = LibMMDUtil.CreateActiveModelInfoHashFromProcess(mmd);
                        Console.WriteLine("読み取り:" + sw.ElapsedMilliseconds);
                    }

                    activeModelName = MMDUtilility.TryGetActiveModelName(mmd.MainWindowHandle);
                    if (activeModelName != _currentModel.ModelName)
                    {
                        isModelChanged = true;
                        if (activeModelName == _MMD_CAMERAMODE_CAPTION)
                        {
                            //カメラモードになってる
                            _currentModel = new ActiveModelInfo();
                        }
                        else
                        {
                            //モデルが変わった
                            this.Invoke((Action)(() =>
                            {
                                if (!string.IsNullOrEmpty(activeModelName))
                                    this.lblActiveModel.Text = activeModelName;
                                else
                                    this.lblActiveModel.Text = "モデルを選択してください";
                            }));

                            if (_modelCache.ContainsKey(activeModelName))
                            {
                                _currentModel = _modelCache[activeModelName];
                            }
                            else
                            {
                                var msg = $"「{activeModelName}」\r\nのモーフ情報を取得しています。\r\n(初回選択時のみ)\r\n\r\nしばらくお待ち下さい。";
                                this.ShowlblWait(msg);

                                var isGetAllMorphCanceled = false;
                                var allmorphsHash = MMDUtilility.TryGetAllMorphValue(mmd.MainWindowHandle);

                                if (isGetAllMorphCanceled || allmorphsHash == null)
                                {
                                    //↑のFuncのキャンセル条件にひっかかった
                                    return _currentModel;
                                }

                                //MorphItemWithIndexのコレクションを作成して_currentModelへ
                                var allMorphs = new Dictionary<MorphType, List<MorphItemWithIndex>>();
                                foreach (var kvp in allmorphsHash)
                                {
                                    var morphtypeList = new List<MorphItemWithIndex>();
                                    for (int i = 0; i < kvp.Value.Count; i++)
                                    {
                                        var morph = new MorphItemWithIndex()
                                        {
                                            MorphName = kvp.Value[i],
                                            MorphType = kvp.Key,
                                            Weight = 0f,
                                            ComboBoxIndex = i,
                                        };
                                        morphtypeList.Add(morph);
                                    }
                                    allMorphs.Add(kvp.Key, morphtypeList);
                                }
                                var tmpactivemodelName = MMDUtilility.TryGetActiveModelName(mmd.MainWindowHandle);
                                if (_prevmmdID >= 0 && _prevmmdID != mmd?.Id)
                                {
                                    //処理している間に監視してるMMDが切り替わった。モデルキャッシュを初期化する
                                    _modelCache = new Dictionary<string, ActiveModelInfo>();
                                    this._currentModel = new ActiveModelInfo();
                                }
                                else if (activeModelName != tmpactivemodelName)
                                {
                                    //なんかおかしい。もう一回やりなおす
                                    this._currentModel = new ActiveModelInfo();
                                }
                                else
                                {
                                    _currentModel = new ActiveModelInfo(activeModelName, allMorphs);

                                    if (!_modelCache.ContainsKey(activeModelName))
                                    {
                                        if (_currentModel.AllMorphs.Count == 0)
                                            Debugger.Break();
                                        //if (activeModelName.IndexOf("沙花叉クロヱ") >= 0 && _currentModel.AllMorphs.Values.FirstOrDefault().Any(n => n.MorphName == "恐ろしい子！"))
                                        //    Debugger.Break();
                                        //if (activeModelName.IndexOf("Tda式改変亞北ネル") >= 0 && _currentModel.AllMorphs.Values.FirstOrDefault().Any(n => n.MorphName == "白目"))
                                        //    Debugger.Break();

                                        if (_testCache.ContainsKey(_currentModel.ModelName))
                                        {
                                            var testmodel = _testCache[_currentModel.ModelName];
                                            if (!LibMMDUtil.CompareActiveModel(_currentModel, testmodel))
                                                Debugger.Break();
                                            Console.WriteLine($"CompareActiveModel合格:{_currentModel.ModelName}");
                                        }
                                        else
                                        {
                                            Debugger.Break();
                                        }
                                        _modelCache.Add(activeModelName, _currentModel);
                                    }
                                }
                            }
                        }
                    }

                    return this._currentModel;
                }
                finally
                {
                    _isBusyGettingActiveModel = false;
                    this._prevmmdID = mmd?.Id;

                    if (isModelChanged)
                    {
                        this.BeginInvoke((Action)(() =>
                        {
                            //アクティブモデルが変わったイベントを起こす
                            this.ActiveModelChangedEventHandler?.Invoke(this, new ActiveModelChangedEventArgs(activeModelName));
                        }));
                    }
                }
            });
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

            try
            {
                var msg = $"「{item.Name}」を適用中です。\r\nMMDに触らないでください。";
                this.ShowlblWait(msg);
                var model = this.TryApplyCurrentWeight();
                if (model == null || string.IsNullOrEmpty(model.ModelName))
                {
                    return false;
                }

                var currentframe = Convert.ToInt64(MMDUtilility.TryGetFrameNumber(mmd.MainWindowHandle));
                var newframe = currentframe + Convert.ToInt64(this.numericUpDown1.Value);

                var targetMorphs = new List<(float, MorphItemWithIndex)>();
                var notTargetMorphs = new List<MorphItemWithIndex>();

                //適用
                MMDUtilility.TrySetFrameNumber(mmd.MainWindowHandle, currentframe);
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
                        MMDUtilility.TrySetMorphValueAsIs(mmd.MainWindowHandle, morph.MorphType, morph.ComboBoxIndex);
                        targetMorphs.Add((applyingMI.Weight, morph));
                    }
                    else
                    {
                        if (morph.Weight != 0)
                            //対象外のモーフでウェイトが乗っている
                            MMDUtilility.TrySetMorphValue(mmd.MainWindowHandle, morph.MorphType, morph.ComboBoxIndex, morph.Weight, true);
                        notTargetMorphs.Add(morph);
                    }
                }
                MMDUtilility.TrySetFrameNumber(mmd.MainWindowHandle, newframe);
                foreach ((float, MorphItemWithIndex) tuple in targetMorphs)
                {
                    var weight = tuple.Item1;
                    var morph = tuple.Item2;
                    MMDUtilility.TrySetMorphValue(mmd.MainWindowHandle, morph.MorphType, morph.ComboBoxIndex, weight, true);
                    var afterValue = MMDUtilility.TryGetMorphValue(mmd.MainWindowHandle, morph.MorphType, morph.ComboBoxIndex);
                    if (Math.Round(afterValue, 3) != Math.Round(weight, 3))
                        Debugger.Break();
                }
                foreach (MorphItemWithIndex morph in notTargetMorphs)
                {
                    var value = MMDUtilility.TryGetMorphValue(mmd.MainWindowHandle, morph.MorphType, morph.ComboBoxIndex);
                    if (value != 0)
                        MMDUtilility.TrySetMorphValue(mmd.MainWindowHandle, morph.MorphType, morph.ComboBoxIndex, 0, true);
                }

                return true;
            }
            finally
            {
                this.HidelblWait();
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

        /// <summary>
        /// アクティブモデルが変更された
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected override void OnActiveModelChanged(object sender, ActiveModelChangedEventArgs e)
        {
            this.HidelblWait();
            if (!string.IsNullOrEmpty(e.CurrentActiveModelName))
                this.lblActiveModel.Text = e.CurrentActiveModelName;
            else
                this.lblActiveModel.Text = "モデルを選択してください";
            base.OnActiveModelChanged(sender, e);
        }

        #endregion "override"

        private void frmMainMMD_FormClosing(object sender, FormClosingEventArgs e)
        {
            this._timer.Change(int.MaxValue, int.MaxValue);
        }
    }

    /// <summary>
    /// モデル情報をキャッシュするエンティティ
    /// </summary>
    public class ActiveModelInfo
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

    /// <summary>
    /// コンボボックス内の何個目のモーフかの情報を保持できるMorphItemです。
    /// </summary>
    public class MorphItemWithIndex : MorphItem
    {
        /// <summary>
        /// コンボボックス内のindex
        /// </summary>
        public int ComboBoxIndex { get; set; } = -1;
    }
}