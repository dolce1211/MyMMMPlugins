using FaceExpressionHelper.UI;
using MikuMikuPlugin;
using MMDUtil;
using MyUtility;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FaceExpressionHelper
{
    public class FaceExpressionHelperPlugin : IResidentPlugin, ICanSavePlugin //
    {
        private Args _memo = null;
        private frmMainBase _frmMain = null;

        public Guid GUID => new Guid("{9005F13A-A53B-4BC3-B2C5-70903ADEDB4F}");

        public string Description => "表情を選択を助けるプラグインです。";

        public IWin32Window ApplicationForm { get; set; }

        /// <summary>
        /// ボタンに表示するテキスト
        /// 日本語環境で表示されるテキストです。改行する場合は Environment.NewLineを使用してください。
        /// </summary>
        public string Text => "表情選択";

        public string EnglishText => "FaceExpressionHelper";

        public System.Drawing.Image Image => Properties.Resources._32;

        public System.Drawing.Image SmallImage => Properties.Resources._22;

        public Scene Scene { get; set; }

        private async Task Run(Args arg)
        {
        }

        public void Initialize()
        {
        }

        public void Enabled()
        {
            MMMUtilility.Initialize(this.ApplicationForm as Form, this.Scene);

            if (this._frmMain != null)
            {
                this._frmMain.Dispose();
                this._frmMain = null;
            }
            this._frmMain = new frmMainMMM(this.Scene, this.ApplicationForm);
            this._frmMain.Show(this.ApplicationForm);
        }

        public void Disabled()
        {
            Dispose();
        }

        public void Dispose()
        {
            if (this._frmMain != null)
            {
                this._frmMain.Close();
                this._frmMain.Dispose();
                this._frmMain = null;
            }
        }

        private string _prevActiveModelName = String.Empty;

        public void Update(float Frame, float ElapsedTime)
        {
            if (this.Scene.State != SceneState.Editing)
                return;

            if (this._frmMain != null && !this._frmMain.IsBusy)
            {
                var activeModelName = string.Empty;

                if (this.Scene.ActiveModel != null)
                    activeModelName = this.Scene.ActiveModel.Name;
                if (this._prevActiveModelName != activeModelName)
                {
                    //アクティブモデルが変わった
                    this._frmMain.ActiveModelChangedEventHandler?.Invoke(null, new ActiveModelChangedEventArgs(activeModelName));
                    this._prevActiveModelName = activeModelName;
                }
            }

            //if (_frmMain != null && _frmMain.Visible)
            //    if (!_frmMain.IsBusy)
            //        this._frmMain.ApplyFrame(Frame);
        }

        public Stream OnSaveProject()
        {
            MemoryStream stream = new MemoryStream();
            //if (this._frmMain != null)
            //{
            //    _memo = this._frmMain.GetArgs();
            //}
            //if (_memo != null)
            //{
            //    //テキストをbyte配列にして保存
            //    var filepath = _memo.FolderPath;
            //    int toggle = _memo.Toggle ? 1 : 0;
            //    int addframe = _memo.AddFrameNameToFileName ? 1 : 0;
            //    int topmost = _memo.TopMost ? 1 : 0;
            //    string fmt = _memo.PictureFormat.ToString();
            //    int lockmotion = _memo.LockMotion ? 1 : 0;
            //    var sb = new System.Text.StringBuilder();
            //    sb.AppendLine($"\"{filepath}\",{toggle},{addframe},{topmost},{fmt},{lockmotion}");

            //    byte[] buffer = System.Text.Encoding.Unicode.GetBytes(sb.ToString());
            //    stream.Write(buffer, 0, buffer.Length);
            //}

            return stream;
        }

        public void OnLoadProject(Stream stream)
        {
            byte[] buffer;

            //やらなくてもいいですが、まあ一応。
            stream.Seek(0, SeekOrigin.Begin);

            //保存されたテキストをStreamから取得
            buffer = new byte[stream.Length];
            int count = stream.Read(buffer, 0, buffer.Length);
            var folderPath = string.Empty;
            int toggle = 0;
            int addrame = 0;
            int topmost = 0;
            int lockMotion = 0;
            //PictureFormat fmt = PictureFormat.png;
            //if (count == buffer.Length)
            //{
            //    var tmp = System.Text.Encoding.Unicode.GetString(buffer);
            //    var array = tmp.Split(',');
            //    if (array.Length > 0)
            //        folderPath = array[0].Replace($"\"", "");
            //    if (array.Length > 1)
            //        toggle = array[1].ToInt();
            //    if (array.Length > 2)
            //        addrame = array[2].ToInt();
            //    if (array.Length > 3)
            //        topmost = array[3].ToInt();
            //    if (array.Length > 4)
            //    {
            //        var buf = array[4].Trim().ToLower();
            //        foreach (PictureFormat xfmt in Enum.GetValues(typeof(PictureFormat)).Cast<PictureFormat>())
            //        {
            //            if (buf == xfmt.ToString().ToLower())
            //            {
            //                fmt = xfmt;
            //                break;
            //            }
            //        }
            //    }
            //    if (array.Length > 5)
            //        lockMotion = array[5].ToInt();

            //    this._memo = new Args()
            //    {
            //        FolderPath = folderPath,
            //        Toggle = (toggle > 0),
            //        AddFrameNameToFileName = (addrame > 0),
            //        TopMost = (topmost > 0),
            //        PictureFormat = fmt,
            //        LockMotion = (lockMotion > 0)
            //    };
            //}
            //if (this._frmMain != null)
            //{
            //    this._frmMain.SetArgs(this._memo);
            //}
        }
    }
}