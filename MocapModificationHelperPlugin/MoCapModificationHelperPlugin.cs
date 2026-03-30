using DxMath;
using MikuMikuPlugin;
using MMDUtil;
using MyUtility;
using MoCapModificationHelperPlugin;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.Eventing.Reader;
using System.IO;
using System.Linq;
using System.Reflection.Emit;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MoCapModificationHelperPlugin.service;

namespace MoCapModificationHelperPlugin
{
    public class MoCapModificationHelperPlugin : IResidentPlugin
    {
        private KeyboardMessageFilter _keyboardFilter;
        private MouseMessageFilter _mouseMessageFilter;
        //　任意のGUIDを生成する

        public Guid GUID => new Guid("7F3E8A91-2B4C-4D56-9E12-A8F7C3B091E4");

        public string Description => "かゆいところヘルパーR";

        public IWin32Window ApplicationForm { get; set; }

        public string Text => "かゆいところヘルパーR";

        public string EnglishText => "MoCapModificationHelperPlugin";

        public System.Drawing.Image Image => Properties.Resources._32;

        public System.Drawing.Image SmallImage => Properties.Resources._22;

        private Configs _configs = new Configs();
        private frmMain _frm = null;

        public Scene Scene { get; set; }

        public void Disabled()
        {
            _frm?.Close();
            var mainform = (Form)ApplicationForm;
            mainform.KeyDown -= KeydownHandler;
            _frm.KeyDown -= KeydownHandler;

            // メッセージフィルターを削除
            if (_keyboardFilter != null)
            {
                Application.RemoveMessageFilter(_keyboardFilter);
                _keyboardFilter = null;
            }
            if (_mouseMessageFilter != null)
            {
                Application.RemoveMessageFilter(_mouseMessageFilter);
                _mouseMessageFilter = null;
            }
        }

        public void Dispose()
        {
        }

        public void Enabled()
        {
            _configs = LoadConfig();
            _frm = new frmMain((Form)ApplicationForm, this.Scene, _configs);
            _frm.Show();

            var mainform = (Form)ApplicationForm;

            // IMessageFilterを使用してグローバルにキーイベントをキャプチャ
            // (otherWindowはKeyDownイベントをキャッチできなかったので)
            if (_keyboardFilter == null)
            {
                _keyboardFilter = new KeyboardMessageFilter(KeydownHandler);
                Application.AddMessageFilter(_keyboardFilter);
            }

            // var otherWindow = MMDUtil.MMMUtilility.TryGetOtherWindowForm();
        }

        private Keys _prevPressedKeys = Keys.None;
        private DateTime _prevPressedTime = DateTime.MinValue;

        private void KeydownHandler(object sender, KeyEventArgs e)
        {
            ConfigItem config = CreateConfig(e);
            if (e.KeyCode == Keys.W)
            {
                //R
                config = _configs.Services.FirstOrDefault(n => n.ServiceType == ServiceType.EnableReverseMorphService);
            }

            if (config != null)
            {
                // 該当するサービスを実行
                e.Handled = _frm?.ExecuteService(config, e.KeyCode) == true;
            }
            if (e.Shift && e.KeyCode == Keys.Enter)
            {
                //shift+Enterでオフセット付加ボタン押下
                e.Handled = _frm?.TryClickOffsetButton() == true;
            }
        }

        /// <summary>
        /// キー押下からの一定時間内に同じキーが押された場合に対応するConfigItemを返す
        /// </summary>
        /// <param name="e"></param>
        /// <returns></returns>
        private ConfigItem CreateConfig(KeyEventArgs e)
        {
            DateTime now = DateTime.Now;
            var doublePressed = false;
            if ((now - _prevPressedTime).TotalSeconds <= 0.3)
            {
                if (_prevPressedKeys == e.KeyCode)
                {
                    _prevPressedTime = DateTime.MinValue;
                    //素早く二度押しされた
                    doublePressed = true;
                }
            }
            if (doublePressed)
            {
                if (ServiceFactory.IsBusy)
                    return null;

                if (IsModalFormOpen())
                    //MMMのフォームにモーダルフォームが開いている場合は無視
                    return null;

                //二度押しされたキーに対応する処理を実行
                _prevPressedKeys = Keys.None;
                _prevPressedTime = DateTime.MinValue;

                if (e.KeyCode == Keys.W)
                {
                }
                else
                {
                    if (e.Shift || e.Control || e.Alt)
                    {
                        if (e.KeyValue == (int)Keys.ShiftKey ||
                           e.KeyValue == (int)Keys.ControlKey ||
                           e.KeyValue == (int)Keys.Menu)
                        {
                            //修飾キー単体の二度押しは許容
                        }
                        else
                        {
                            //修飾キー併用の二度押しは無視
                            return null;
                        }
                    }
                }

                if (_configs.Services.Any(n => n.Keys == e.KeyCode) ||
                        _configs.Services.Any(n => n.KeysList != null && n.KeysList.Contains(e.KeyCode)))
                {
                    return _configs?.Services.FirstOrDefault(n => n.Keys == e.KeyCode ||
                                           (n.KeysList != null && n.KeysList.Contains(e.KeyCode)));
                }
            }
            else
            {
                _prevPressedKeys = e.KeyCode;
                _prevPressedTime = now;
            }
            return null;
        }

        private Configs LoadConfig()
        {
            Configs ret = null;
            try
            {
                var configPath = Configs.GetConfigFilePath();
                if (File.Exists(configPath))
                {
                    ret = MyUtility.Serializer.Deserialize<Configs>(configPath);
                }
            }
            catch (Exception)
            {
                // ログ出力やエラーハンドリングが必要に応じて追加
            }
            if (ret == null)
                ret = new Configs();
            if (ret.Services.Count < 10)
                ret.KeepAndInitialize();
            if (!ret.Services.Any(n => n.ServiceType == ServiceType.InterpolateSetterService))
            {
                ret.Services.Add(Configs.CreateInterpolateSetterService());
                MyUtility.Serializer.Serialize(ret, Configs.GetConfigFilePath());
            }
            return ret;
        }

        /// <summary>
        /// モーダルフォームが開いているかを判定します（簡易版）
        /// </summary>
        /// <returns>モーダルフォームが開いている場合true</returns>
        private bool IsModalFormOpen()
        {
            var mainForm = ApplicationForm as Form;
            if (mainForm == null)
                return false;

            // Application.OpenFormsから全てのフォームをチェック
            foreach (Form form in Application.OpenForms)
            {
                // Form.Modalプロパティでモーダル表示されているか確認
                if (form != mainForm && form != _frm && form.Modal)
                {
                    return true;
                }
            }
            return false;
        }

        public void Initialize()
        {
            MMMUtilility.Initialize(this.ApplicationForm as Form, this.Scene);
        }

        public void Update(float Frame, float ElapsedTime)
        {
            SetMouseMessageFilter();
            if (ServiceFactory.IsBusy)
                return;
            _frm?.Update(Frame, ElapsedTime);
        }

        private Model _activeModel = null;
        private long _prevFrame = -1;

        private void SetMouseMessageFilter()
        {
            var doReset = false;

            if (_activeModel?.ID != Scene.ActiveModel?.ID)
            {
                doReset = true;
            }
            if (_prevFrame != Scene.MarkerPosition)
            {
                doReset = true;
            }
            _prevFrame = Scene.MarkerPosition;
            _activeModel = Scene.ActiveModel;
            if (doReset && _activeModel != null)
            {
                Application.RemoveMessageFilter(_mouseMessageFilter);
                _mouseMessageFilter = new MouseMessageFilter(this.ApplicationForm as Form, this.Scene, _activeModel);
                Application.AddMessageFilter(_mouseMessageFilter);
            }
            if (_activeModel == null)
            {
                if (_mouseMessageFilter != null)
                {
                    Application.RemoveMessageFilter(_mouseMessageFilter);
                }
                _mouseMessageFilter = null;
            }
        }
    }

    // キーボードメッセージフィルタークラス
    public class KeyboardMessageFilter : IMessageFilter
    {
        private readonly KeyEventHandler _keyDownHandler;
        private const int WM_KEYDOWN = 0x0100;
        private const int WM_SYSKEYDOWN = 0x0104;

        public KeyboardMessageFilter(KeyEventHandler keyDownHandler)
        {
            _keyDownHandler = keyDownHandler;
        }

        public bool PreFilterMessage(ref Message m)
        {
            if (m.Msg == WM_KEYDOWN || m.Msg == WM_SYSKEYDOWN)
            {
                // MMMにフォーカスがある場合のみ処理
                // キーコードを取得
                Keys keyCode = (Keys)(int)m.WParam & Keys.KeyCode;

                // 修飾キーを取得
                Keys modifiers = Control.ModifierKeys;

                // KeyEventArgsを作成
                KeyEventArgs e = new KeyEventArgs(keyCode | modifiers);

                // ハンドラーを呼び出す
                _keyDownHandler?.Invoke(null, e);

                // メッセージを他のコントロールにも伝播させる
                return e.Handled;
            }
            return false;
        }
    }

    // マウスメッセージフィルタークラス
    public class MouseMessageFilter : IMessageFilter
    {
        private Form _applicationForm { get; set; }
        private Scene _scene;
        private Model _activeModel;
        private IEnumerable<(Bone bone, MotionLayer layer)> _activeTargetLayerTuples = null;
        private Dictionary<string, MotionData> _currentFrameDataDic;
        private const int WM_MOUSEMOVE = 0x0200;
        private const int WM_KEYDOWN = 0x0100;
        private const int WM_KEYUP = 0x0101;
        private const int WM_SYSKEYDOWN = 0x0104;
        private const int WM_SYSKEYUP = 0x0105;

        private bool _ctrlShiftPressed = false;
        private int _origin = 0;
        private int _delta = 0;

        public MouseMessageFilter(Form applicationForm, Scene scene, Model activeModel)
        {
            this._applicationForm = applicationForm;
            this._scene = scene;
            this._activeModel = activeModel;
            var ctrlShiftPressed = (Control.ModifierKeys & Keys.Control) != 0 && (Control.ModifierKeys & Keys.Shift) != 0;
            if (ctrlShiftPressed)
                onCtrlShiftPressed();
        }

        public bool TryReset()
        {
            Reset();
            return true;
        }

        private void Reset()
        {
            _origin = 0;
            _delta = 0;
            _ctrlShiftPressed = false;
            _activeTargetLayerTuples = null;
            _currentFrameDataDic = default;
        }

        private void onCtrlShiftPressed()
        {
            _ctrlShiftPressed = true;
            _origin = Cursor.Position.Y;

            if (_activeModel != null)
            {
                var activeLayers = _activeModel.Bones.Where(b =>
                            new string[] { "センター", "グルーブ", "左足ＩＫ", "右足ＩＫ" }.Contains(b.Name))
                        .SelectMany(b => b.Layers.Where(l => l.Selected).Select(l => (bone: b, layer: l)));
                if (activeLayers.Count() > 0)
                {
                    _activeTargetLayerTuples = activeLayers.ToList();
                    _currentFrameDataDic = _activeTargetLayerTuples.ToDictionary(l => $"{l.bone.Name}{l.layer.LayerID}", l => l.layer.CurrentLocalMotion);
                }
            }
        }

        public bool PreFilterMessage(ref Message m)
        {
            if (m.Msg == WM_KEYDOWN || m.Msg == WM_SYSKEYDOWN)
            {
                // Ctrl+Shiftが押された
                Keys keyCode = (Keys)(int)m.WParam & Keys.KeyCode;
                bool ctrlPressed = (Control.ModifierKeys & Keys.Control) != 0;
                bool shiftPressed = (Control.ModifierKeys & Keys.Shift) != 0;

                if (ctrlPressed && shiftPressed && !_ctrlShiftPressed)
                {
                    Reset();
                    Debug.WriteLine($"Ctrl+Shift pressed. Origin Y: {_origin}");
                    onCtrlShiftPressed();
                }

                if (_ctrlShiftPressed && keyCode == Keys.Space)
                {
                    Console.WriteLine("space");
                    this._applicationForm.BeginInvoke(new Action(async () =>
                    {
                        await Task.Delay(200);
                        _scene.MarkerPosition += 1;
                        _scene.MarkerPosition -= 1;

                    }));
                    
                }
            }
            else if (m.Msg == WM_KEYUP || m.Msg == WM_SYSKEYUP)
            {
                // CtrlまたはShiftが離された
                Keys keyCode = (Keys)(int)m.WParam & Keys.KeyCode;
                if (keyCode == Keys.ControlKey || keyCode == Keys.ShiftKey)
                {
                    Debug.WriteLine("Ctrl or Shift released.");
                    Reset();
                }
            }
            else if (m.Msg == WM_MOUSEMOVE && _ctrlShiftPressed)
            {
                int currentY = Cursor.Position.Y;
                int delta = currentY - _origin;
                if (delta == _delta)
                    return false;
                _delta = delta;
                if (_activeModel != null && _activeTargetLayerTuples != null)
                {
                    foreach (var tuple in _activeTargetLayerTuples)
                    {
                        var currentFrameData = _currentFrameDataDic[$"{tuple.bone.Name}{tuple.layer.LayerID}"];
                        var _activeLayer = _activeModel.Bones.Where(b => b.Name == tuple.bone.Name)
                                            .Select(b => (bone: b, layer: b.Layers.FirstOrDefault(l => l.LayerID == tuple.layer.LayerID))).FirstOrDefault();
                        if (_activeLayer.layer != null)
                        {
                            var activeCenterLayer = _activeLayer;
                            var options = new string[] { "x", "y", "z" };
                            var selectedLayer = options.FirstOrDefault(o => !string.IsNullOrWhiteSpace(activeCenterLayer.layer.Name) && activeCenterLayer.layer.Name.ToLower().Contains(o));
                            if (selectedLayer == null)
                            {
                                selectedLayer = "y";
                            }
                            var v = new Vector3(0, 0, 0);
                            var f = typeof(Vector3).GetField(selectedLayer.ToUpper());
                            // 構造体をobjectにボックス化
                            object boxed = v;
                            f.SetValue(boxed, delta * 0.025f);
                            // ボックス化されたものを構造体に戻す
                            v = (Vector3)boxed;
                            Debug.WriteLine($"Mouse Y delta: {v}");
                            activeCenterLayer.layer.CurrentLocalMotion = new MotionData(currentFrameData.Move - v, currentFrameData.Rotation);
                            
                        }
                    }
                }
            }

            return false;
        }
    }
}