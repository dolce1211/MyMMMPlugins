using FaceExpressionHelper;
using FaceExpressionHelper.UI;
using LibMMD.Pmx;
using LibMMD.Vmd;
using MMDUtil;
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

        private System.Threading.Timer _timer = null;

        public frmMainMMD() : base()
        {
            mmdSelector.Visible = true;

            //MMDで動いている
            OperationgMode = OperatingMode.OnMMD;
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
            //var mmd = this.mmdSelector.SelectMMD();
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

            foreach (var kvp in _currentModel.AllMorphs)
            {
                var index = 0;
                foreach (MorphItem morph in kvp.Value)
                {
                    morph.Weight = MMDUtilility.TryGetMorphValue(mmd.MainWindowHandle, kvp.Key, index);

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
            return Task<ActiveModelInfo>.Run(async () =>
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
                    mmd = this.Invoke((Func<Process>)(() => this.mmdSelector.SelectMMD())) as Process;

                    if (mmd == null)
                    {
                        //MMD紐づけ無し。キャッシュクリア
                        _modelCache = new Dictionary<string, ActiveModelInfo>();
                        _prevpmmSavedTime = new DateTime();
                        _currentModel = new ActiveModelInfo();
                        return _currentModel;
                    }

                    if (_prevmmdID != mmd.Id)
                    {
                        //監視してるMMDが切り替わった。キャッシュクリア
                        _modelCache = new Dictionary<string, ActiveModelInfo>();
                        _prevpmmSavedTime = new DateTime();
                        this._currentModel = new ActiveModelInfo();
                        isModelChanged = true;
                    }

                    if (_modelCache == null || _modelCache.Count == 0)
                    {
                        var tmpcache = await this.TryCachePmx(mmd);
                        if (tmpcache != null)
                            _modelCache = tmpcache;
                    }

                    activeModelName = MMDUtilility.TryGetActiveModelName(mmd.MainWindowHandle);
                    if (activeModelName != _currentModel.ModelName)
                    {
                        isModelChanged = true;
                        if (activeModelName == _MMD_CAMERAMODE_CAPTION)
                        {
                            //カメラモードになってる
                            _currentModel = new ActiveModelInfo();
                            activeModelName = "";
                        }
                        else
                        {
                            if (_modelCache.ContainsKey(activeModelName))
                            {
                                _currentModel = _modelCache[activeModelName];
                                this.HidelblWait();
                            }
                            else
                            {
                                //前回キャッシュ時には居なかったモデルだ。再度キャッシュしてみる。
                                var tmpcache = await this.TryCachePmx(mmd);
                                if (tmpcache != null)
                                    _modelCache = tmpcache;

                                if (!_modelCache.ContainsKey(activeModelName))
                                {
                                    //おそらく後から追加されてまだ保存されていないモデルだ
                                    var msg = $"「{activeModelName}」の情報は\r\nまだpmmに保存されていません。 \r\n\r\n pmmを保存してください。";

                                    this.ShowlblWait(msg);
                                    _currentModel = new ActiveModelInfo();
                                    activeModelName = "";
                                    isModelChanged = false;
                                }
                            }

                            //モデルが変わった
                            this.Invoke((Action)(() =>
                            {
                                if (!string.IsNullOrEmpty(activeModelName))
                                    this.lblActiveModel.Text = activeModelName;
                                else
                                    this.lblActiveModel.Text = "モデルを選択してください";
                            }));
                        }
                    }

                    return this._currentModel;
                }
                finally
                {
                    _isBusyGettingActiveModel = false;
                    if (mmd != null)
                        this._prevmmdID = mmd.Id;

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
        /// mmdのプロセスからpmxファイルを引っ張って中身をキャッシュします。
        /// </summary>
        private async Task<Dictionary<string, ActiveModelInfo>> TryCachePmx(Process mmd)
        {
            var ret = new Dictionary<string, ActiveModelInfo>();
            if (mmd == null || mmd.HasExited)
                return null;

            var pmminfo = LibMMDUtil.GetPmmInfoFromProcess(mmd);
            if (pmminfo == null)
                return null;
            if (pmminfo.LastWriteTime <= _prevpmmSavedTime)
            {
                //前回キャッシュ時からまだ保存されていないので抜ける
                return this._modelCache;
            }
            if (_prevpmmSavedTime != new DateTime())
                await Task.Delay(2000);
            var msg = $"pmmファイルから\r\nモデル情報をキャッシュしています。\r\nしばらくお待ち下さい。";
            this.ShowlblWait(msg);
            try
            {
                ret = LibMMDUtil.CreateActiveModelInfoHashFromProcess(mmd);
                //直近のpmm保存日時を保持しておく
                _prevpmmSavedTime = pmminfo.LastWriteTime;
            }
            finally
            {
                this.HidelblWait();
            }
            return ret;
        }

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

            var sw = new Stopwatch();
            sw.Start();

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

                long currentframe;
                long newframe;
                if (bufferFrames >= 0)
                {
                    currentframe = Convert.ToInt64(MMDUtilility.TryGetFrameNumber(mmd.MainWindowHandle));
                    newframe = currentframe + Convert.ToInt64(bufferFrames);
                }
                else
                {
                    newframe = Convert.ToInt64(MMDUtilility.TryGetFrameNumber(mmd.MainWindowHandle));
                    currentframe = newframe + Convert.ToInt64(bufferFrames);
                }
                MMDUtilility.TrySetFrameNumber(mmd.MainWindowHandle, currentframe);

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
                        MMDUtilility.TrySetMorphValueAsIs(mmd.MainWindowHandle, morph.MorphType, morph.ComboBoxIndex);
                        targetMorphs.Add((applyingMI.Weight, morph));
                    }
                    else
                    {
                        if (morph.Weight != 0)
                        {
                            //対象外のモーフでウェイトが乗っている
                            var ret = MMDUtilility.TrySetMorphValue(mmd.MainWindowHandle, morph.MorphType, morph.ComboBoxIndex, morph.Weight, true, 3);
                            if (!ret)
                                failed.Add($"{morph.MorphName},{currentframe}fr");
                        }

                        notTargetMorphs.Add(morph);
                    }
                }

                MMDUtilility.TrySetFrameNumber(mmd.MainWindowHandle, newframe);
                foreach ((float, MorphItemWithIndex) tuple in targetMorphs)
                {
                    var weight = tuple.Item1;
                    var morph = tuple.Item2;
                    var ret = MMDUtilility.TrySetMorphValue(mmd.MainWindowHandle, morph.MorphType, morph.ComboBoxIndex, weight, true, 3);
                    if (!ret)
                        failed.Add($"{morph.MorphName},{newframe}fr");
                }
                foreach (MorphItemWithIndex morph in notTargetMorphs)
                {
                    var value = MMDUtilility.TryGetMorphValue(mmd.MainWindowHandle, morph.MorphType, morph.ComboBoxIndex);

                    if (value != 0)
                    {
                        var ret = MMDUtilility.TrySetMorphValue(mmd.MainWindowHandle, morph.MorphType, morph.ComboBoxIndex, 0, true, 3);
                        if (!ret)
                            failed.Add($"{morph.MorphName},{newframe}fr");
                    }
                }

                if (failed.Count > 0)
                {
                    var errmsg = $"{string.Join("\r\n", failed)}\r\n\r\n以上のモーフの適用に失敗した可能性があります。\r\n確認してください。";
                    MessageBox.Show(errmsg, "失敗?", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }

                //モーフのスライダーの表示がバグることがあるのでMMD画面をリフレッシュする
                MMDUtilility.BeginAndEndUpdate(mmd.MainWindowHandle, false);
                MMDUtilility.BeginAndEndUpdate(mmd.MainWindowHandle, true);

                //MMDにフォーカスを当てる
                MMDUtilility.SetForegroundWindow(mmd.MainWindowHandle);
                return true;
            }
            finally
            {
                this.HidelblWait();
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

        /// <summary>
        /// アクティブモデルが変更された
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected override void OnActiveModelChanged(object sender, ActiveModelChangedEventArgs e)
        {
            //this.HidelblWait();
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
    [DebuggerDisplay("{MorphName},{ComboBoxIndex}")]
    public class MorphItemWithIndex : MorphItem
    {
        /// <summary>
        /// コンボボックス内のindex
        /// </summary>
        public int ComboBoxIndex { get; set; } = -1;
    }
}