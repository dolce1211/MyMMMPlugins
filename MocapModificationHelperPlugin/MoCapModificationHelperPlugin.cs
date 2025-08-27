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

        private Config _config = new Config();
        private frmMain _frm = null;

        public Scene Scene { get; set; }

        public void Disabled()
        {
            _frm?.Close();
        }

        public void Dispose()
        {
        }

        public void Enabled()
        {
            _config = LoadConfig();
            _frm = new frmMain((Form)ApplicationForm, this.Scene, _config);
            _frm.Show();

            var mainform = (Form)ApplicationForm;
            Keys prevPressedKeys = Keys.None;
            DateTime prevPressedTime = DateTime.MinValue;
            mainform.KeyDown += (s, e) =>
            {
                if (Scene.ActiveModel == null)
                    return;

                DateTime now = DateTime.Now;

                var doublePressed = false;
                if ((now - prevPressedTime).TotalSeconds <= 0.3)
                {
                    if (prevPressedKeys == e.KeyCode)
                    {
                        prevPressedTime = DateTime.MinValue;
                        //素早く二度押しされた
                        doublePressed = true;
                    }
                }
                if (doublePressed)
                {
                    if (ServiceFactory.IsBusy)
                        return;
                    //二度押しされたキーに対応する処理を実行
                    prevPressedKeys = Keys.None;
                    prevPressedTime = DateTime.MinValue;
                    if (_config.Services.Any(n => n.Keys == e.KeyCode))
                    {
                        var serviceType = _config.Services.FirstOrDefault(n => n.Keys == e.KeyCode).ServiceType;
                        int mode = 0;
                        if (e.Shift) mode |= 1;
                        var service = ServiceFactory.Create(serviceType, this.Scene, this.ApplicationForm);
                        if (service != null)
                            service.Execute(mode);
                    }
                }
                else
                {
                    prevPressedKeys = e.KeyCode;
                    prevPressedTime = now;
                }
            };
        }

        private Config LoadConfig()
        {
            Config ret = null;
            try
            {
                var configPath = Config.GetConfigFilePath();
                if (File.Exists(configPath))
                {
                    ret = MyUtility.Serializer.Deserialize<Config>(configPath);
                }
            }
            catch (Exception ex)
            {
                // ログ出力やエラーハンドリングが必要に応じて追加
            }
            if (ret == null)
                ret = new Config();
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
            _frm?.Update(Frame, ElapsedTime);
        }
    }
}