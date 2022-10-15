using FaceExpressionHelper;
using MMDUtil;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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
        public frmMainMMD() : base()
        {
            mmdSelector.Visible = true;
        }

        /// <summary>
        /// 取得済みのActiveModelInfoのキャッシュ。モデル名をキーにしてるので、同名かつモーフ状態が異なるモデルが同じプロジェクトに複数読み込まれるとおかしくなるかも
        /// </summary>
        private Dictionary<string, ActiveModelInfo> _modelCache = new Dictionary<string, ActiveModelInfo>();

        private ActiveModelInfo _currentModel = new ActiveModelInfo();

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

        private bool _isBusyGettingActiveModel;

        /// <summary>
        /// 現在のアクティブモデルの情報をキャッシュします。
        /// </summary>
        /// <returns></returns>
        private Task<ActiveModelInfo> TryFindActiveModel()
        {
            return Task<ActiveModelInfo>.Run(() =>
            {
                if (_isBusyGettingActiveModel)
                    return this._currentModel;

                _isBusyGettingActiveModel = true;
                try
                {
                    var prevmmd = this.mmdSelector.MMDProcess;
                    var mmd = this.mmdSelector.SelectMMD();
                    if (mmd == null)
                    {
                        //MMD紐づけ無し
                        _modelCache = new Dictionary<string, ActiveModelInfo>();
                        _currentModel = new ActiveModelInfo();
                        return _currentModel;
                    }
                    if (prevmmd != mmd)
                    {
                        //監視してるMMDが切り替わった。モデルキャッシュを初期化する
                        _modelCache = new Dictionary<string, ActiveModelInfo>();
                        this._currentModel = new ActiveModelInfo();
                    }

                    var activeModelName = MMDUtilility.TryGetActiveModelName(mmd.MainWindowHandle);
                    if (activeModelName != _currentModel.ModelName)
                    {
                        if (activeModelName == "ｶﾒﾗ･照明･ｱｸｾｻﾘ")
                        {
                            //カメラモードになってる
                            this.Invoke((Action)(() =>
                            {
                                this.pnlBody.Enabled = false;
                                this.pnlBottom.Enabled = false;
                            }));
                            _currentModel = new ActiveModelInfo();
                        }
                        else
                        {
                            //モデルが変わった
                            if (_modelCache.ContainsKey(activeModelName))
                            {
                                _currentModel = _modelCache[activeModelName];
                            }
                            else
                            {
                                var msg = $"「{activeModelName}」\r\nのモーフ情報を取得しています。\r\nしばらくお待ち下さい。";
                                this.ShowlblWait(msg);

                                var allmorphsHash = MMDUtilility.TryGetAllMorphValue(mmd.MainWindowHandle);
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
                                _currentModel = new ActiveModelInfo(activeModelName, allMorphs);
                                if (!_modelCache.ContainsKey(activeModelName))
                                    _modelCache.Add(activeModelName, _currentModel);
                            }
                        }
                    }
                    return this._currentModel;
                }
                finally
                {
                    _isBusyGettingActiveModel = false;
                    this.BeginInvoke((Action)(() =>
                    {
                        //this.Enabled = false;

                        this.HidelblWait();
                        if (!string.IsNullOrEmpty(this._currentModel.ModelName))
                        {
                            this.lblActiveModel.Text = this._currentModel.ModelName;
                        }
                        else
                        {
                            this.lblActiveModel.Text = "モデルを選択してください";
                            this.pnlBody.Enabled = true;
                            this.pnlBottom.Enabled = true;
                        }
                    }));
                }
            });
        }

        private void ShowlblWait(string text)
        {
            this.Invoke((Action)(() =>
            {
                this.lblWait.Text = text;
                this.lblWait.Visible = true;
                this.pnlBody.Enabled = false;
                this.pnlBottom.Enabled = false;
                this.listBox1.Enabled = false;
            }));
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
                this.pnlBody.Enabled = true;
                this.pnlBottom.Enabled = true;
                this.listBox1.Enabled = true;

            }));
        }

        /// <summary>
        /// this._currentModelに現在のモーフ状態を取得して返します。
        /// </summary>
        /// <returns></returns>
        private async Task<ActiveModelInfo> TryApplyCurrentWeight()
        {
            var mmd = this.mmdSelector.SelectMMD();
            if (mmd == null)
            {
                //MMD紐づけ無し
                return null;
            }

            var model = await this.TryFindActiveModel().ConfigureAwait(false);
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
            return model;
        }

        private async Task XX()
        {
            var mmd = this.mmdSelector.SelectMMD();
            if (mmd == null)
            {
                //MMD紐づけ無し
                return;
            }
            var model = await this.TryApplyCurrentWeight().ConfigureAwait(false);
            var frame = Convert.ToInt64(MMDUtilility.TryGetFrameNumber(mmd.MainWindowHandle));
            var newframe = frame + Convert.ToInt64(this.numericUpDown1.Value);
            var lst = new List<string>();
            foreach (var kvp in model.AllMorphs)
            {
                var index = 0;
                foreach (MorphItem morph in kvp.Value.Take(4444))
                {
                    morph.Weight = MMDUtilility.TryGetMorphValue(mmd.MainWindowHandle, kvp.Key, index);
                    MMDUtilility.TrySetMorphValueAsIs(mmd.MainWindowHandle, kvp.Key, index);
                    index++;
                }
            }
            foreach (var kvp in model.AllMorphs)
            {
                var index = 0;
                MMDUtilility.TrySetFrameNumber(mmd.MainWindowHandle, newframe);
                foreach (MorphItem morph in kvp.Value.Take(4444))
                {
                    MMDUtilility.TrySetMorphValue(mmd.MainWindowHandle, kvp.Key, index, 0.777f, true);

                    index++;
                }
            }
            MMDUtilility.SetForegroundWindow(mmd.MainWindowHandle);
        }

        #region "override"

        /// <summary>
        /// 今のアクティブモデルの有効なモーフをすべて返します。
        /// </summary>
        /// <returns></returns>
        protected override List<MorphItem> GetAllMorphsForActiveModel()
        {
            this.TryFindActiveModel();
            var mmd = this.mmdSelector.SelectMMD();
            if (mmd == null)
                //MMD紐づけ無し
                return null;

            var ret = new List<MorphItem>();

            return ret;
        }

        /// <summary>
        /// アクティブモデルの今のフレームのモーフ適用状態を返します。
        /// </summary>
        /// <returns>null:アクティブモデルなし</returns>
        protected override List<MorphItem> GetCurrentMorphState()
        {
            var model = TryApplyCurrentWeight().Result;
            if (model == null || string.IsNullOrWhiteSpace(model.ModelName))
            {
                return null;
            }

            var ret = new List<MorphItem>();

            foreach (var kvp in model.AllMorphs)
            {
                foreach (var morph in kvp.Value)
                {
                    if (morph.Weight != 0)
                        ret.Add(morph);
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
            var ret = new List<MorphItem>();

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

            var msg = $"「{item.Name}」を適用中です。\r\nMMDに触らないでください。";
            this.ShowlblWait(msg);
            try
            {
                var model = this.TryApplyCurrentWeight().ConfigureAwait(false).GetAwaiter().GetResult();
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
                    var applyingMI = item.MorphItems.Where(n => n.MorphName == morph.MorphName).FirstOrDefault();
                    if (applyingMI != null)
                    {
                        //対象のモーフ
                        MMDUtilility.TrySetMorphValueAsIs(mmd.MainWindowHandle, morph.MorphType, morph.ComboBoxIndex);
                        targetMorphs.Add((applyingMI.Weight, morph));
                    }
                    else if (morph.Weight != 0)
                    {
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
                        weight = weight;
                }
                foreach (MorphItemWithIndex morph in notTargetMorphs)
                {
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
        /// アクティブモデルが変更された
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected override void OnActiveModelChanged(object sender, ActiveModelChangedEventArgs e)
        {
            this.lblActiveModel.Text = e.CurrentActiveModelName;
            base.OnActiveModelChanged(sender, e);
        }

        #endregion "override"

        private void button1_Click(object sender, EventArgs e)
        {
            using (var frm = new frmMMDSelect(null, new List<Process>()))
            {
                frm.ShowDialog();
            }
        }

        private System.Threading.Timer _timer = null;

        protected override void OnShown(EventArgs e)
        {
            _timer = new System.Threading.Timer(new System.Threading.TimerCallback(
                (Action<object>)(x =>
                {
                    _ = Task.Run(() => this.TryFindActiveModel());
                }))
                , null, 10, 1000);

            base.OnShown(e);
        }

        private void InitializeComponent()
        {
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).BeginInit();
            this.pnlBottom.SuspendLayout();
            this.SuspendLayout();
            //
            // frmMainMMD
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.ClientSize = new System.Drawing.Size(287, 624);
            this.Name = "frmMainMMD";
            this.Shown += new System.EventHandler(this.frmMainMMD_Shown);
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).EndInit();
            this.pnlBottom.ResumeLayout(false);
            this.pnlBottom.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        private void frmMainMMD_Shown(object sender, EventArgs e)
        {
            var mmd = this.mmdSelector.SelectMMD();
        }
    }

    /// <summary>
    /// モデル情報をキャッシュするエンティティ
    /// </summary>
    public class ActiveModelInfo
    {
        /// <summary>
        /// モデル名
        /// </summary>
        public string ModelName { get; }

        /// <summary>
        /// モーフ情報
        /// </summary>
        public Dictionary<MorphType, List<MorphItemWithIndex>> AllMorphs { get; }

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