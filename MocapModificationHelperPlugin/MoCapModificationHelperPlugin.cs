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

namespace MoCapModificationHelperPlugin
{
    public class MoCapModificationHelperPlugin : IResidentPlugin
    {
        //　任意のGUIDを生成する

        public Guid GUID => new Guid("7F3E8A91-2B4C-4D56-9E12-A8F7C3B091E4");

        public string Description => "モーキャプ修正作業ヘルパー";

        public IWin32Window ApplicationForm { get; set; }

        public string Text => "MoCapModificationHelperPlugin";

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

            mainform.KeyDown -= KeydownHandler;
            mainform.KeyDown += KeydownHandler;
            _frm.KeyDown -= KeydownHandler;
            _frm.KeyDown += KeydownHandler;
        }

        private Keys _prevPressedKeys = Keys.None;
        private DateTime _prevPressedTime = DateTime.MinValue;

        private void KeydownHandler(object sender, KeyEventArgs e)
        {
            if (Scene.ActiveModel == null)
                return;

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
                    return;
                //二度押しされたキーに対応する処理を実行
                _prevPressedKeys = Keys.None;
                _prevPressedTime = DateTime.MinValue;
                if (_configs.Services.Any(n => n.Keys == e.KeyCode))
                {
                    var serviceItem = _configs.Services.FirstOrDefault(n => n.Keys == e.KeyCode);
                    // 該当するサービスを実行
                    _frm.ExecuteService(serviceItem);
                }
            }
            else
            {
                _prevPressedKeys = e.KeyCode;
                _prevPressedTime = now;
            }
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
            catch (Exception ex)
            {
                // ログ出力やエラーハンドリングが必要に応じて追加
            }
            if (ret == null)
                ret = new Configs();
            if (ret.Services.Count < 5)
                ret.Initialize();

            return ret;
        }

        public void Initialize()
        {
            MMMUtilility.Initialize(this.ApplicationForm as Form, this.Scene);
        }

        public void Update(float Frame, float ElapsedTime)
        {
            if (this.Scene.Mode != EditMode.ModelMode)
                return;

            if (ServiceFactory.IsBusy)
                return;
            _frm?.Update(Frame, ElapsedTime);
        }
    }
}