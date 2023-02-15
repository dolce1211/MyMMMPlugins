using LibMMD.Pmx;
using MMDUtil;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using static MMDUtil.MMDUtilility;

namespace MyUtility

{
    public abstract class ModelFinder<T> : IDisposable
        where T : IMMDModel
    {
        /// <summary>
        /// アクティブモデルが変更された時に走るイベントハンドら
        /// </summary>
        public EventHandler<ActiveModelChangedEventArgs> ActiveModelChangedEventHandler { get; set; }

        protected MMDSelectorControl _mmdSelector = null;

        protected T _currentModel;
        protected Form _frm = null;

        public bool IsBusy { get; set; }

        protected Dictionary<string, T> _modelCache = new Dictionary<string, T>();

        protected DateTime _prevpmmSavedTime = new DateTime();

        protected Action<string> _showWaitAction = null;

        protected Action _hideWaitAction = null;

        private System.Threading.Timer _timer = null;

        public ModelFinder(Form frm, MMDSelectorControl mmdselector, Action<string> showWaitAction = null, Action hideWaitAction = null)
        {
            this._frm = frm;
            this._mmdSelector = mmdselector;
            this._showWaitAction = showWaitAction;
            this._hideWaitAction = hideWaitAction;

            _timer = new System.Threading.Timer(new System.Threading.TimerCallback(
               (Action<object>)(async x =>
               {
                   _timer.Change(int.MaxValue, int.MaxValue);

                   await this.TryApplyActiveModel();

                   _timer.Change(1000, 0);
               }))
               , null, 10, 1000);
        }

        protected bool _isBusyGettingActiveModel;

        protected int _prevmmdID = -1;

        protected abstract T CreateInstance();

        /// <summary>
        /// 現在のアクティブモデルの情報をキャッシュします。
        /// </summary>
        /// <returns></returns>
        protected virtual Task<T> TryApplyActiveModel()
        {
            return Task<T>.Run(async () =>
            {
                if (this.IsBusy)
                    return this._currentModel;
                if (this._mmdSelector.IsBusy)
                    return this._currentModel;
                if (_isBusyGettingActiveModel)
                    return this._currentModel;
                _isBusyGettingActiveModel = true;
                Process mmd = null;
                var activeModelName = string.Empty;
                var isModelChanged = false;
                if (this._frm == null || this._frm.IsDisposed || this._mmdSelector.IsDisposed)
                    return default;
                try
                {
                    try
                    {
                        mmd = this._frm.Invoke((Func<Process>)(() => this._mmdSelector.SelectMMD())) as Process;
                    }
                    catch (Exception)
                    {
                    }

                    if (mmd == null)
                    {
                        //MMD紐づけ無し。キャッシュクリア
                        _modelCache = new Dictionary<string, T>();
                        _prevpmmSavedTime = new DateTime();
                        _currentModel = this.CreateInstance();
                        return _currentModel;
                    }

                    if (_prevmmdID != mmd.Id)
                    {
                        //監視してるMMDが切り替わった。キャッシュクリア
                        _modelCache = new Dictionary<string, T>();
                        _prevpmmSavedTime = new DateTime();
                        this._currentModel = this.CreateInstance();
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
                        if (activeModelName == MMDUtil.MMDUtilility._MMD_CAMERAMODE_CAPTION)
                        {
                            if (!string.IsNullOrEmpty(_currentModel.ModelName))
                                isModelChanged = true;
                            //カメラモードになってる
                            _currentModel = this.CreateInstance();
                            activeModelName = "";
                        }
                        else
                        {
                            isModelChanged = true;
                            if (_modelCache.ContainsKey(activeModelName))
                            {
                                _currentModel = _modelCache[activeModelName];
                                this._hideWaitAction?.Invoke();
                            }
                            else
                            {
                                //前回キャッシュ時には居なかったモデルだ。再度キャッシュしてみる。
                                var tmpcache = await this.TryCachePmx(mmd);
                                if (tmpcache != null)
                                    _modelCache = tmpcache;

                                var errmsg = string.Empty;

                                if (string.IsNullOrEmpty(this._mmdSelector.MMEPath))
                                {
                                    //MMEが入ってないとemmファイルが作られないのでpmxファイルの場所を特定できない
                                    errmsg = $"MMEffectが導入されていないようです。\r\n本ツールを使用するにはMMEffectの導入が必須となります。";
                                }
                                else if (!_modelCache.ContainsKey(activeModelName))
                                {
                                    //おそらく後から追加されてまだ保存されていないモデルだ
                                    errmsg = $"「{activeModelName}」の情報は\r\nまだpmmに保存されていません。 \r\n\r\n pmmを保存してください。";
                                }
                                if (!string.IsNullOrEmpty(errmsg))
                                {
                                    this._showWaitAction?.Invoke(errmsg);
                                    _currentModel = this.CreateInstance();
                                    activeModelName = "";
                                    isModelChanged = false;
                                }
                            }
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
                        this._frm.BeginInvoke((Action)(() =>
                        {
                            //アクティブモデルが変わったイベントを起こす
                            this.ActiveModelChangedEventHandler?.Invoke(this, new ActiveModelChangedEventArgs(this._currentModel));
                        }));
                    }
                }
            });
        }

        /// <summary>
        /// mmdのプロセスからpmxファイルを引っ張って中身をキャッシュします。
        /// </summary>
        private async Task<Dictionary<string, T>> TryCachePmx(Process mmd)
        {
            var ret = new Dictionary<string, T>();
            if (mmd == null || mmd.HasExited)
                return null;

            var pmminfo = this.GetPmmInfoFromProcess(mmd);
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
            this._showWaitAction?.Invoke(msg);
            try
            {
                ret = this.CreateActiveModelInfoHashFromProcess(mmd);
                //直近のpmm保存日時を保持しておく
                _prevpmmSavedTime = pmminfo.LastWriteTime;
            }
            finally
            {
                this._hideWaitAction?.Invoke();
            }
            return ret;
        }

        //protected abstract Dictionary<string, T> CreateActiveModelInfoHashFromProcess(Process mmd);

        protected abstract T PmxModel2ActiveModelInfo(PmxModel pmxmdls);

        /// <summary>
        /// pmxPnlTypeからMMDUtilility.MorphTypeへ変換して返します。
        /// </summary>
        /// <param name="pmxpnltype"></param>
        /// <returns></returns>
        protected MorphType PmxPnlType2Morphtype(byte pmxpnltype)
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

        protected Dictionary<string, T> CreateActiveModelInfoHashFromProcess(Process mmd)
        {
            var ret = new Dictionary<string, T>();
            var pmxmodels = GetPmxFromProcess(mmd);
            foreach (var pmxmdls in pmxmodels)
            {
                var model = this.PmxModel2ActiveModelInfo(pmxmdls);
                if (!ret.ContainsKey(model.ModelName))
                    ret.Add(model.ModelName, model);
            }
            return ret;
        }

        private FileInfo GetPmmInfoFromProcess(Process mmd)
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
        public List<PmxModel> GetPmxFromProcess(Process mmd)
        {
            var ret = new List<PmxModel>();
            if (mmd == null)
                return ret;
            var pmminfo = GetPmmInfoFromProcess(mmd);
            if (pmminfo == null)
                return ret;

            var mmdexedir = MMDUtilility.GetProgramPathFromProcess(mmd);
            if (string.IsNullOrEmpty(mmdexedir))
                return ret;
            mmdexedir = System.IO.Path.GetDirectoryName(mmdexedir);

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

        /// <summary>
        /// pmxあるいはpmdのファイルパスからPmxModelのインスタンスを生成して返します。
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        private PmxModel FilePath2PmxModel(string filePath)
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

        public void Dispose()
        {
            this._timer.Change(int.MaxValue, int.MaxValue);
        }
    }
}