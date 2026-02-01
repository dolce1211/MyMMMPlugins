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
            if (ret.Services.Count < 9)
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
            if (ServiceFactory.IsBusy)
                return;
            _frm?.Update(Frame, ElapsedTime);
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
}