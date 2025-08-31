using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Diagnostics;
using System.Windows.Forms;
using System.Threading.Tasks;
using MikuMikuPlugin;

namespace MMDUtil
{
    public static class MMMUtilility
    {
        private static Process _currentMMMProcess = null;
        private static Form _mmmForm = null;
        private static Scene _scene = null;
        private static Window _otherWindow = null;

        /// <summary>
        /// 自分自身のプロセスを取得して保持します。
        /// </summary>
        /// <param name="mmmForm"></param>
        /// <param name="scene"></param>
        public static void Initialize(Form mmmForm, Scene scene)
        {
            _mmmForm = mmmForm;
            _scene = scene;
            var mmms = Process.GetProcessesByName("MikuMikuMoving");
            if (mmms != null)
            {
                foreach (var mmm in mmms)
                {
                    var mmmchildren = MMDUtil.MMDUtilility.TryEnumChildWindows(mmm.Id);
                    if (mmmchildren.Any(n => n.hWnd == mmmForm.Handle))
                    {
                        _currentMMMProcess = mmm;
                        break;
                    }
                }
            }
        }

        /// <summary>
        /// Scene.Controllerのboolのプロパティの値をSetします。
        /// </summary>
        /// <param name="propertyName">プロパティ名</param>
        /// <param name="value">値</param>
        /// <returns></returns>
        public static bool SetControllerValue(string propertyName, bool value)
        {
            if (_scene == null)
            {
                //Initializeを走らせてないと思われる
                Debugger.Break();
                return false;
            }
            //"EnableEffects"
            //"Controller"
            var t = _scene.GetType();
            System.Reflection.TypeInfo ti = t as System.Reflection.TypeInfo;
            if (ti != null)
            {
                var controllerfi = ti.DeclaredFields.FirstOrDefault();
                var controller = controllerfi.GetValue(_scene);
                var pi = controller.GetType().GetProperties();
                var fi = controller.GetType().GetFields();

                var enableeffectpi = controller.GetType().GetProperty(propertyName);
                if (enableeffectpi != null && enableeffectpi.PropertyType == typeof(bool))
                {
                    enableeffectpi.SetValue(controller, value);
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Scene.Controllerのboolのプロパティの値をGetします。
        /// </summary>
        /// <param name="propertyName"></param>
        /// <returns></returns>
        public static bool GetControllerValue(string propertyName)
        {
            if (_scene == null)
            {
                //Initializeを走らせてないと思われる
                Debugger.Break();
                return false;
            }
            //"EnableEffects"
            //"Controller"
            var t = _scene.GetType();
            System.Reflection.TypeInfo ti = t as System.Reflection.TypeInfo;
            if (ti != null)
            {
                var controllerfi = ti.DeclaredFields.FirstOrDefault();
                var controller = controllerfi.GetValue(_scene);
                var pi = controller.GetType().GetProperties();
                var fi = controller.GetType().GetFields();

                var enableeffectpi = controller.GetType().GetProperty(propertyName);
                if (enableeffectpi != null && enableeffectpi.PropertyType == typeof(bool))
                {
                    return (bool)enableeffectpi.GetValue(controller);
                }
            }
            return false;
        }

        /// <summary>
        /// 別ウィンドウ化していればその情報を返します。
        /// </summary>
        /// <param name="usecache">BeginAndStopUpdateで描画停止→再開を使う際に使用。描画停止するとGetAllChildWindowsが取れなくなるため</param>
        /// <returns></returns>
        public static Window TryGetOtherWindow(bool usecache = false)
        {
            if (_currentMMMProcess == null)
            {
                //Initializeを走らせてないと思われる
                Debugger.Break();
                return null;
            }

            var windowclassName = "WindowsForms10.Window.8.app.0.aec740_r6_ad1";
            var desc = MMDUtil.MMDUtilility.TryEnumChildWindows(_currentMMMProcess.Id);
            if (!usecache)
                _otherWindow = null;
            if (_otherWindow != null)
            {
                var ret = desc.Where(n => n.hWnd == _otherWindow.hWnd).FirstOrDefault();
                if (ret != null)
                    return ret;
            }
            var otherwindow = desc.Where(n => n.ClassName == windowclassName || true)
                    .Where(n =>
                    {
                        //子ウィンドウが5つで、全てClassNameがwindowclassNameで、[1]のTitleがViewなら別ウィンドウとみなす
                        var children = MMDUtil.MMDUtilility.GetAllChildWindows(n.hWnd, new List<MMDUtil.Window>(), false);
                        if (children.Count == 5 && children.Where(m => m.ClassName == windowclassName || true).Count() == 5)
                        {
                            if (children[1].Title == "View")
                                return true;
                        }
                        return false;
                    }).FirstOrDefault();

            if (usecache)
                _otherWindow = otherwindow;
            return otherwindow;
        }

        /// <summary>
        /// メイン画面の描画を止めます。
        /// </summary>
        /// <param name="flg">
        /// false:描画を止める
        /// true:描画を再開する
        /// </param>
        public static void BeginAndEndUpdate(bool flg)
        {
            if (_currentMMMProcess == null)
            {
                //Initializeを走らせてないと思われる
                Debugger.Break();
                return;
            }

            IntPtr handle = _mmmForm.Handle;
            var otherwindow = MMDUtil.MMMUtilility.TryGetOtherWindow(true);
            if (otherwindow != null)
                handle = otherwindow.hWnd;

            MMDUtil.MMDUtilility.BeginAndEndUpdate(handle, flg);
        }

        /// <summary>
        /// ApplicationFormにEnterキーを送信します（MMMUtilityのヘルパーメソッド）
        /// </summary>
        /// <returns>成功した場合true</returns>
        public static bool SendEnterToApplicationForm(Form frm)
        {
            return SendKeyToApplicationFormInternal(frm, VK_RETURN, false, false, false);
        }

        /// <summary>
        /// ApplicationFormにSpaceキーを送信します（MMMUtilityのヘルパーメソッド）
        /// </summary>
        /// <returns>成功した場合true</returns>
        public static bool SendSpaceToApplicationForm(Form frm)
        {
            return SendKeyToApplicationFormInternal(frm, VK_SPACE, false, false, false);
        }

        /// <summary>
        /// ApplicationFormに任意のキーと修飾キーの組み合わせを送信します
        /// </summary>
        /// <param name="frm">対象フォーム</param>
        /// <param name="key">送信するキー（VK_*）</param>
        /// <param name="ctrl">Ctrlキーを同時に押すかどうか</param>
        /// <param name="shift">Shiftキーを同時に押すかどうか</param>
        /// <param name="alt">Altキーを同時に押すかどうか</param>
        /// <returns>成功した場合true</returns>
        public static bool SendKeyWithModifiersToApplicationForm(Form frm, int key, bool ctrl = false, bool shift = false, bool alt = false)
        {
            return SendKeyToApplicationFormInternal(frm, key, ctrl, shift, alt);
        }

        /// <summary>
        /// ApplicationFormにキーメッセージを送信します（内部メソッド）
        /// </summary>
        /// <param name="frm">対象フォーム</param>
        /// <param name="key">送信するキー（VK_*）</param>
        /// <param name="ctrl">Ctrlキーを同時に押すかどうか</param>
        /// <param name="shift">Shiftキーを同時に押すかどうか</param>
        /// <param name="alt">Altキーを同時に押すかどうか</param>
        /// <returns>成功した場合true</returns>
        private static bool SendKeyToApplicationFormInternal(Form frm, int key, bool ctrl = false, bool shift = false, bool alt = false)
        {
            if (frm == null)
            {
                Debugger.Break();
                return false;
            }

            try
            {
                var results = new List<bool>();

                // 修飾キーを順番に押下
                if (ctrl)
                {
                    results.Add(PostMessage(frm.Handle, WM_KEYDOWN, VK_CONTROL, 0));
                    System.Threading.Thread.Sleep(10);
                }
                if (shift)
                {
                    results.Add(PostMessage(frm.Handle, WM_KEYDOWN, VK_SHIFT, 0));
                    System.Threading.Thread.Sleep(10);
                }
                if (alt)
                {
                    results.Add(PostMessage(frm.Handle, WM_KEYDOWN, VK_MENU, 0));
                    System.Threading.Thread.Sleep(10);
                }

                // メインキーを押下
                results.Add(PostMessage(frm.Handle, WM_KEYDOWN, key, 0));
                System.Threading.Thread.Sleep(10);

                // メインキーを離す
                results.Add(PostMessage(frm.Handle, WM_KEYUP, key, 0));
                System.Threading.Thread.Sleep(10);

                // 修飾キーを逆順で離す
                if (alt)
                {
                    results.Add(PostMessage(frm.Handle, WM_KEYUP, VK_MENU, 0));
                    System.Threading.Thread.Sleep(10);
                }
                if (shift)
                {
                    results.Add(PostMessage(frm.Handle, WM_KEYUP, VK_SHIFT, 0));
                    System.Threading.Thread.Sleep(10);
                }
                if (ctrl)
                {
                    results.Add(PostMessage(frm.Handle, WM_KEYUP, VK_CONTROL, 0));
                }

                // 全てのメッセージが正常に送信された場合のみ成功とする
                return results.All(r => r);
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// ApplicationFormにCtrlキーと組み合わせたキーを送信します（既存互換性のため保持）
        /// </summary>
        /// <param name="frm">対象フォーム</param>
        /// <param name="key">送信するキー（VK_Z, VK_Yなど）</param>
        /// <returns>成功した場合true</returns>
        private static bool SendCtrlKeyToApplicationFormInternal(Form frm, int key)
        {
            return SendKeyToApplicationFormInternal(frm, key, true, false, false);
        }

        /// <summary>
        /// 特定のフォームに対してkeybd_eventを用いてCtrl+Zを送信します
        /// </summary>
        /// <param name="targetForm">対象のフォーム</param>
        /// <returns>成功した場合true</returns>
        public static bool SendCtrlZToFormWithKeyboardEvent(Form targetForm)
        {
            return SendKeyToFormWithKeyboardEvent(targetForm, VK_Z, true, false, false);
        }

        /// <summary>
        /// 特定のフォームに対してkeybd_eventを用いてCtrl+Yを送信します
        /// </summary>
        /// <param name="targetForm">対象のフォーム</param>
        /// <returns>成功した場合true</returns>
        public static bool SendCtrlYToFormWithKeyboardEvent(Form targetForm)
        {
            return SendKeyToFormWithKeyboardEvent(targetForm, VK_Y, true, false, false);
        }

        /// <summary>
        /// 特定のフォームに対してkeybd_eventを用いてキーの組み合わせを送信します
        /// </summary>
        /// <param name="targetForm">対象のフォーム</param>
        /// <param name="key">送信するキー（VK_*）</param>
        /// <param name="ctrl">Ctrlキーを同時に押すかどうか</param>
        /// <param name="shift">Shiftキーを同時に押すかどうか</param>
        /// <param name="alt">Altキーを同時に押すかどうか</param>
        /// <returns>成功した場合true</returns>
        public static bool SendKeyToFormWithKeyboardEvent(Form targetForm, int key, bool ctrl = false, bool shift = false, bool alt = false)
        {
            if (targetForm == null || targetForm.IsDisposed || !targetForm.IsHandleCreated)
            {
                return false;
            }

            try
            {
                // フォームをアクティブにする
                if (targetForm.WindowState == FormWindowState.Minimized)
                {
                    targetForm.WindowState = FormWindowState.Normal;
                }

                // フォームを前面に持ってきてアクティブにする
                SetForegroundWindow(targetForm.Handle);
                targetForm.Activate();
                targetForm.BringToFront();

                // フォームがアクティブになるまで少し待機
                System.Threading.Thread.Sleep(100);

                // 修飾キーを順番に押下
                if (ctrl)
                {
                    keybd_event(VK_CONTROL, 0, 0, UIntPtr.Zero);
                    System.Threading.Thread.Sleep(1);
                }
                if (shift)
                {
                    keybd_event(VK_SHIFT, 0, 0, UIntPtr.Zero);
                    System.Threading.Thread.Sleep(1);
                }
                if (alt)
                {
                    keybd_event(VK_MENU, 0, 0, UIntPtr.Zero);
                    System.Threading.Thread.Sleep(1);
                }

                // メインキーを押下
                keybd_event((byte)key, 0, 0, UIntPtr.Zero);
                System.Threading.Thread.Sleep(10);

                // メインキーを離す
                keybd_event((byte)key, 0, KEYEVENTF_KEYUP, UIntPtr.Zero);
                System.Threading.Thread.Sleep(10);

                // 修飾キーを逆順で離す
                if (alt)
                {
                    keybd_event(VK_MENU, 0, KEYEVENTF_KEYUP, UIntPtr.Zero);
                    System.Threading.Thread.Sleep(1);
                }
                if (shift)
                {
                    keybd_event(VK_SHIFT, 0, KEYEVENTF_KEYUP, UIntPtr.Zero);
                    System.Threading.Thread.Sleep(1);
                }
                if (ctrl)
                {
                    keybd_event(VK_CONTROL, 0, KEYEVENTF_KEYUP, UIntPtr.Zero);
                }

                return true;
            }
            catch
            {
                return false;
            }
        }

        // keybd_event API関連の定義
        [DllImport("user32.dll", SetLastError = true)]
        private static extern void keybd_event(byte bVk, byte bScan, uint dwFlags, UIntPtr dwExtraInfo);

        [DllImport("user32.dll", SetLastError = true)]
        private static extern bool SetForegroundWindow(IntPtr hWnd);

        // keybd_event用の定数
        private const uint KEYEVENTF_KEYUP = 0x0002;

        [DllImport("user32.dll", SetLastError = true)]
        private static extern bool PostMessage(IntPtr hWnd, int Msg, int wParam, int lParam);

        private const int WM_KEYDOWN = 0x0100;
        private const int WM_KEYUP = 0x0101;
        private const int VK_RETURN = 0x0D;
        private const int VK_SPACE = 0x20;
        private const int VK_CONTROL = 0x11;
        private const int VK_SHIFT = 0x10;
        private const int VK_MENU = 0x12; // Alt key
        private const int VK_Z = 0x5A;
        private const int VK_Y = 0x59;
    }
}