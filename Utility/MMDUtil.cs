using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Diagnostics;
using System.Windows.Forms;
using System.Threading.Tasks;
using System.Management;
using System.Runtime.CompilerServices;
using MyUtility;
using System.Windows;
using System.ComponentModel;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Window;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using System.Threading;
using static System.Windows.Forms.AxHost;
using System.Windows.Media.Animation;

namespace MMDUtil
{
    // Windowsクラス
    public class Window
    {
        [DllImport("User32.dll")]
        private static extern int SendMessage(IntPtr hWnd, uint Msg, int wParam, StringBuilder lParam);

        private const int WM_GETTEXT = 0x000d;
        public string ClassName { get; set; }
        public string Title { get; set; }
        public IntPtr hWnd { get; set; }
        public int ID { get; set; }

        private string _text = null;

        public string Text
        {
            get
            {
                if (_text == null || ClassName == "ComboBox" || ClassName == "Edit")
                {
                    StringBuilder sbb = new StringBuilder(256);
                    SendMessage(hWnd, WM_GETTEXT, 255, sbb);
                    _text = sbb.ToString();
                }

                return _text;
            }
        }
    }

    public class MorphWindows
    {
        /// <summary>
        /// MMDウィンドウ
        /// </summary>
        public Window Parent { get; set; }

        /// <summary>
        /// コンボボックス
        /// </summary>
        public Window ComboBox { get; set; }

        /// <summary>
        /// トラックバー
        /// </summary>
        public Window TrackBar { get; set; }

        /// <summary>
        /// 値用Edit
        /// </summary>
        public Window Edit { get; set; }

        /// <summary>
        /// 登録用Button
        /// </summary>
        public Window Button { get; set; }
    }

    public static class MMDUtilility
    {
        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool EnableWindow(IntPtr hWnd, bool bEnable);

        [DllImport("user32.dll")]
        private static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

        [DllImport("user32.dll")]
        private static extern IntPtr FindWindowEx(IntPtr hWnd, IntPtr hwndChildAfter, string lpszClass, string lpszWindow);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern int GetWindowText(IntPtr hWnd, StringBuilder lpString, int nMaxCount);

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool SetForegroundWindow(IntPtr hWnd);

        [DllImport("user32.dll")]
        private static extern uint keybd_event(byte bVk, byte bScan, uint dwFlags, UIntPtr dwExtraInfo);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern int GetClassName(IntPtr hWnd, StringBuilder lpClassName, int nMaxCount);

        [DllImport("user32.dll")]
        private static extern int GetWindowThreadProcessId(IntPtr hWnd, out int lpdwProcessId);

        [DllImport("user32.dll")]
        private static extern IntPtr GetForegroundWindow();

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern int GetWindowTextLength(IntPtr hWnd);

        [DllImport("user32.dll")]
        private static extern int GetWindowLong(IntPtr hWnd, int nIndex);

        [DllImport("user32.dll", EntryPoint = "GetWindowLong")]
        private static extern IntPtr GetWindowLongPtr32(IntPtr hWnd, int nIndex);

        [DllImport("user32.dll", EntryPoint = "GetWindowLongPtr")]
        private static extern IntPtr GetWindowLongPtr64(IntPtr hWnd, int nIndex);

        public static IntPtr GetWindowLongPtr(IntPtr hWnd, int nIndex)
        {
            if (IntPtr.Size == 8)
                return GetWindowLongPtr64(hWnd, nIndex);
            else
                return GetWindowLongPtr32(hWnd, nIndex);
        }

        [DllImport("user32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool SetWindowPos(IntPtr hWnd, int hWndInsertAfter, int x, int y, int cx, int cy, int uFlags);

        [DllImport("user32.dll")]
        private static extern bool IsWindowVisible(IntPtr hWnd);

        private delegate bool EnumWindowsDelegate(IntPtr hWnd, IntPtr lparam);

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool EnumWindows(EnumWindowsDelegate lpEnumFunc, IntPtr lparam);

        [DllImport("user32.dll", SetLastError = true)]
        private static extern bool PostMessage(IntPtr hWnd, int Msg, int wParam, int lParam);

        [DllImport("User32.dll")]
        private static extern int SendMessage(IntPtr hWnd, uint Msg, int wParam, string lParam);

        [DllImport("User32.dll", EntryPoint = "SendMessage")]
        private static extern int SendMessage(IntPtr hwnd, int msg, int wParam, int lParam);

        [DllImport("User32.dll")]
        private static extern int SendMessage(IntPtr hWnd, uint Msg, int wParam, StringBuilder lParam);

        [DllImport("User32.dll")]
        private static extern int SendMessage(IntPtr hWnd, uint Msg, string wParam, string lParam);

        [DllImport("user32.dll", SetLastError = true)]
        private static extern uint GetDlgItemText(IntPtr hDlg, int nIDDlgItem, [Out] StringBuilder lpString, int nMaxCount);

        [DllImport("user32.dll")]
        private static extern IntPtr GetMenu(int hwnd);

        [DllImport("user32.dll")]
        private static extern IntPtr GetSubMenu(IntPtr hMenu, int nPos);

        [DllImport("user32.dll")]
        private static extern IntPtr GetMenuState(IntPtr hMenu, int uItem, int uFlags);

        private const int MF_CHECKED = 0x08;

        [DllImport("USER32.DLL")]
        private static extern int GetMenuItemID(IntPtr hWnd, int nPos);

        [DllImport("USER32.DLL")]
        private static extern long GetMenuString(IntPtr hMenu, int wIDItem, StringBuilder lpString, int nMaxCount, int wFlag);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern bool GetMenuItemInfo(IntPtr hMenu, uint uItem, bool fByPosition, ref MENUITEMINFO lpmii);

        [StructLayout(LayoutKind.Sequential)]
        public struct MENUITEMINFO
        {
            public uint cbSize;
            public uint fMask;
            public uint fType;
            public uint fState;
            public uint wID;
            public IntPtr hSubMenu;
            public IntPtr hbmpChecked;
            public IntPtr hbmpUnchecked;
            public IntPtr dwItemData;
            public String dwTypeData;
            public uint cch;
            public IntPtr hbmpItem;

            // Return the size of the structure
            public static uint sizeOf
            {
                get { return (uint)Marshal.SizeOf(typeof(MENUITEMINFO)); }
            }
        }

        private const int WM_COMMAND = 0x0111;
        private const int WM_SYSCOMMAND = 0x112;
        private const int MF_BYCOMMAND = 0x00000000;
        private const int MF_BYPOSITION = 0x00000400;

        private const int WM_GETTEXT = 0x000d;
        private const int WM_GETTEXTLENGTH = 0x000e;
        private const int WM_SETREDRAW = 0x000B;
        private const int WM_SETTEXT = 0x000c;
        private const int WM_IME_KEYDOWN = 0x00290;
        private const int WM_IME_KEYUP = 0x00291;
        private const int WM_KEYDOWN = 0x0100;
        private const int WM_KEYUP = 0x0101;
        private const int BM_CLICK = 0x00F5;

        private const int VK_RETURN = 0x0D;
        private const int VK_MENU = 0x12;
        private const int VK_LEFT = 0x25;
        private const int VK_UP = 0x26;
        private const int VK_RIGHT = 0x27;
        private const int VK_DOWN = 0x28;

        private const int WM_LBUTTONDOWN = 0x201;           //マウスダウン
        private const int WM_LBUTTONUP = 0x202;           //マウスアップ

        private const int CB_GETCOUNT = 0x146;               //コンボボックスのリストボックスの項目数を取得する
        private const int CB_GETCURSEL = 0x147;             //コンボボックスの現在の選択インデックスを取得する
        private const int CB_GETLBTEXT = 0x148;              //コンボボックスのリストボックスから文字列を取得する
        private const int CB_GETLBTEXTLEN = 0x149;           //コンボボックスのリストボックス文字列の長さを取得する
        private const int CB_SETCURSEL = 0x14e;               //コンボボックスに指定のインデックをセットする
        private const int CB_SHOWDROPDOWN = 0x14f;          //コンボボックスのリストボックスの表示または非表示を切り替える
        private const int CBN_SELCHANGE = 0x0001;           //コンボボックスの選択が変わったよ状態を送る

        // EnumWindowsから呼び出されるコールバック関数WNDENUMPROCのデリゲート
        private delegate bool WNDENUMPROC(IntPtr hWnd, IntPtr lParam);

        private const int _GWL_ID = -12;

        #region "MMD内部定数"

        /// <summary>
        /// モーフタイプ
        /// </summary>
        public enum MorphType
        {
            Eye,
            Lip,
            Brow,
            Other,
            none
        }

        /// <summary>
        /// カメラモード時のキャプション
        /// </summary>
        public const string _MMD_CAMERAMODE_CAPTION = "ｶﾒﾗ･照明･ｱｸｾｻﾘ";

        /// <summary>
        /// 「フレーム操作」テキストボックス
        /// </summary>
        private const int _FRAMESOUSA = 417;

        /// <summary>
        /// 「モデル操作」コンボ
        /// </summary>
        private const int _Combo_Model = 436;

        /// <summary>
        /// 目モーフ
        /// </summary>
        private const int _Combo_MORPH_EYE = 509;

        private const int _TRACKBAR_MORPH_EYE = 510;
        private const int _EDIT_MORPH_EYE = 511;
        private const int _BUTTON_MORPH_EYE = 524;

        /// <summary>
        /// 眉モーフ
        /// </summary>
        private const int _Combo_MORPH_BROW = 504;

        private const int _TRACKBAR_MORPH_BROW = 505;
        private const int _EDIT_MORPH_BROW = 506;
        private const int _BUTTON_MORPH_BROW = 525;

        /// <summary>
        /// リップモーフ
        /// </summary>
        private const int _Combo_MORPH_LIP = 514;

        private const int _TRACKBAR_MORPH_LIP = 515;
        private const int _EDIT_MORPH_LIP = 516;
        private const int _BUTTON_MORPH_LIP = 527;

        /// <summary>
        /// その他モーフ
        /// </summary>
        private const int _Combo_MORPH_OTHER = 519;

        private const int _TRACKBAR_MORPH_OTHER = 520;
        private const int _EDIT_MORPH_OTHER = 521;
        private const int _BUTTON_MORPH_OTHER = 526;

        #endregion "MMD内部定数"

        /// <summary>
        /// アクティブモデル名称を取得します。
        /// </summary>
        /// <param name="parentHandle"></param>
        /// <returns></returns>
        public static string TryGetActiveModelName(IntPtr parentHandle)
        {
            Dictionary<int, Window> mmdhash = GetAllMMDWindows(parentHandle);
            if (mmdhash == null || !mmdhash.ContainsKey(_Combo_Model))
                return string.Empty;
            var modelCombo = mmdhash[_Combo_Model];
            if (modelCombo == null)
                return String.Empty;

            return modelCombo.Text;
            //var textLen = GetWindowTextLength(modelCombo.hWnd);
            //string windowText = null;
            //if (0 < textLen)
            //{
            //    // ウィンドウのタイトルを取得する
            //    StringBuilder windowTextBuffer = new StringBuilder(textLen + 1);
            //    GetWindowText(modelCombo.hWnd, windowTextBuffer, windowTextBuffer.Capacity);
            //    windowText = windowTextBuffer.ToString();
            //}

            //var ret1 = string.Empty;

            //var selectedIndex = SendMessage(modelCombo.hWnd, CB_GETCURSEL, 1, "");
            ////Console.WriteLine($"CB_GETCURSEL:{selectedIndex} windowsText:{modelCombo.Text}");
            //var textlength = SendMessage(modelCombo.hWnd, CB_GETLBTEXTLEN, selectedIndex, "");
            //if (textlength > 0)
            //{
            //    var sb = new StringBuilder(textlength);
            //    SendMessage(modelCombo.hWnd, CB_GETLBTEXT, selectedIndex, sb);
            //    ret1 = sb.ToString();
            //}

            //return ret1;
        }

        /// <summary>
        /// フレーム数を1進めて2戻し、未確定変更をリセットします。
        /// </summary>
        /// <param name="parentHandle"></param>
        /// <param name="action">再描画する前に何か仕事をさせたいならここで定義します。</param>
        /// <returns></returns>
        public static bool TryResetFrameNumber(IntPtr parentHandle, Action action = null)
        {
            var currentFrame = MMDUtil.MMDUtilility.TryGetFrameNumber(parentHandle);
            if (currentFrame < 0)
                //現在のフレーム番号が取れなかった
                return false;

            BeginAndEndUpdate(parentHandle, false);
            try
            {
                MMDUtil.MMDUtilility.TrySetFrameNumber(parentHandle, currentFrame + 1);
                MMDUtil.MMDUtilility.TrySetFrameNumber(parentHandle, currentFrame);
                if (action != null)
                    action.Invoke();

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"{ex.Message}\r\n\r\n{ex.StackTrace}");
            }
            finally
            {
                MMDUtil.MMDUtilility.BeginAndEndUpdate(parentHandle, true);
            }
            return false;
        }

        /// <summary>
        /// フレーム数をセットします。
        /// </summary>
        /// <param name="parentHandle"></param>
        /// <param name="frame"></param>
        /// <returns></returns>
        public static bool TrySetFrameNumber(IntPtr parentHandle, long frame)
        {
            //フレーム操作欄に値を追加
            Dictionary<int, Window> mmdhash = GetAllMMDWindows(parentHandle);
            if (mmdhash == null)
                return false;
            if (!mmdhash.ContainsKey(_FRAMESOUSA))
                return false;
            var frameEditor = mmdhash[_FRAMESOUSA];

            var ret = TrySetText(frameEditor, frame.ToString(), true);
            return ret;
        }

        /// <summary>
        /// フレーム数を指定の数だけ増減させます。
        /// </summary>
        /// <param name="parentHandle"></param>
        /// <param name="addFrame"></param>
        /// <returns></returns>
        public static bool TryAddFrameNumber(IntPtr parentHandle, long addFrame)
        {
            var currentFrame = TryGetFrameNumber(parentHandle);
            return TrySetFrameNumber(parentHandle, currentFrame + addFrame);
        }

        /// <summary>
        /// フレーム数を取得します。
        /// </summary>
        /// <param name="parentHandle"></param>
        /// <param name="frame"></param>
        /// <returns></returns>
        public static int TryGetFrameNumber(IntPtr parentHandle)
        {
            //_ = TrySetMorphValue(parentHandle, MorphType.Eye, "まばたき", 2.5f, true);

            //フレーム操作欄に値を追加
            Dictionary<int, Window> mmdhash = GetAllMMDWindows(parentHandle);
            if (mmdhash == null)
                return -1;
            if (!mmdhash.ContainsKey(_FRAMESOUSA))
                return -1;
            var frameEditor = mmdhash[_FRAMESOUSA];
            var text = TryGetText(frameEditor);
            if (string.IsNullOrEmpty(text))
                return 0;
            var ret = Convert.ToInt32(text);
            return ret;
        }

        /// <summary>
        /// メニューのチェック状態を取得します。
        /// </summary>
        /// <param name="parentHandle"></param>
        /// <param name="mainMenuCaption">親メニューのキャプション</param>
        /// <param name="subMenuCaption">子メニューのキャプション</param>
        /// <returns></returns>
        public static int TryGetMenuChecked(IntPtr parentHandle, string mainMenuCaption, string subMenuCaption)
        {
            IntPtr subMenuhWnd = TryGetMenuHandleByCaption(parentHandle, mainMenuCaption);
            if (subMenuhWnd != IntPtr.Zero)
            {
                //選択したメニュー項目のIDをゲットする。
                for (int i = 0; i <= 100; i++)
                {
                    int intIDx = GetMenuItemID(subMenuhWnd, i);
                    StringBuilder ssb = new StringBuilder(100);
                    var retx = GetMenuString(subMenuhWnd, i, ssb, ssb.Capacity, MF_BYPOSITION);
                    if (ssb.ToString().IndexOf(subMenuCaption) == 0)
                    {
                        var state = GetMenuState(subMenuhWnd, intIDx, 0);
                        if ((int)state == MF_CHECKED)
                            //チェックあり。現在エフェクト有効である
                            return 1;
                        else
                            return 0;
                    }
                }
            }
            return -1;
        }

        /// <summary>
        /// メニューのチェック状態をセットします。
        /// </summary>
        /// <param name="parentHandle"></param>
        /// <param name="mainMenuCaption">親メニューのキャプション</param>
        /// <param name="subMenuCaption">子メニューのキャプション</param>
        /// <param name="chk">セットするチェック状態</param>
        /// <returns></returns>
        public static bool TrySetMenuChecked(IntPtr parentHandle, string mainMenuCaption, string subMenuCaption, bool chk)
        {
            var currentstate = (TryGetMenuChecked(parentHandle, mainMenuCaption, subMenuCaption) == 1);
            if (currentstate == chk)
                return true;

            IntPtr subMenuhWnd = TryGetMenuHandleByCaption(parentHandle, mainMenuCaption);
            if (subMenuhWnd != IntPtr.Zero)
            {
                //選択したメニュー項目のIDをゲットする。
                for (int i = 0; i <= 100; i++)
                {
                    int intIDx = GetMenuItemID(subMenuhWnd, i);
                    StringBuilder ssb = new StringBuilder(100);
                    var retx = GetMenuString(subMenuhWnd, i, ssb, ssb.Capacity, MF_BYPOSITION);
                    if (ssb.ToString().IndexOf(subMenuCaption) == 0)
                    {
                        //メニューをクリック
                        var ret = SendMessage(parentHandle, WM_COMMAND, intIDx, "0");

                        return true;
                    }
                }
            }
            return false;
        }

        /// <summary>
        /// 現在の物理演算状態を取得します。
        /// </summary>
        /// <param name="parentHandle"></param>
        /// <returns>
        /// -1:取得失敗
        /// 0:オン/オフモード
        /// 1:常に演算
        /// 2:トレースモード
        /// 3:演算しない
        /// </returns>
        public static int TryGetPhysicsState(IntPtr parentHandle)
        {
            if (parentHandle == IntPtr.Zero)
                return -1;

            //物理演算メニューのハンドルをゲット
            IntPtr subMenuhWnd = TryGetMenuHandleByCaption(parentHandle, "物理演算(&P)");
            if (subMenuhWnd != IntPtr.Zero)
            {
                //選択したメニュー項目のIDをゲットする。
                for (int i = 0; i <= 3; i++)
                {
                    int intIDx = GetMenuItemID(subMenuhWnd, i);
                    var state = GetMenuState(subMenuhWnd, intIDx, 0);
                    StringBuilder tsbb = new StringBuilder(100);
                    var retx = GetMenuString(subMenuhWnd, i, tsbb, tsbb.Capacity, MF_BYPOSITION);
                    if ((int)state == MF_CHECKED)
                        //チェックあり
                        return i;
                }
            }

            return -1;
        }

        /// <summary>
        /// エフェクトのオンオフ状態を取得します。
        /// </summary>
        /// <param name="parentHandle"></param>
        /// <returns></returns>
        public static bool TryGetEffectState(IntPtr parentHandle)
        {
            if (parentHandle == IntPtr.Zero)
                return false;

            //MMEメニューのハンドルをゲット
            IntPtr subMenuhWnd = TryGetMenuHandleByCaption(parentHandle, "MMEffect");
            if (subMenuhWnd != IntPtr.Zero)
            {
                //選択したメニュー項目のIDをゲットする。
                for (int i = 0; i <= 10; i++)
                {
                    int intIDx = GetMenuItemID(subMenuhWnd, i);
                    StringBuilder ssb = new StringBuilder(100);
                    var retx = GetMenuString(subMenuhWnd, i, ssb, ssb.Capacity, MF_BYPOSITION);
                    if (ssb.ToString().IndexOf("エフェクト使用(&E)\tCtrl+Shift+E") == 0)
                    {
                        var state = GetMenuState(subMenuhWnd, intIDx, 0);
                        if ((int)state == MF_CHECKED)
                            //チェックあり。現在エフェクト有効である
                            return true;
                    }
                }
            }

            return false;
        }

        /// <summary>
        /// 物理演算状態をセットします。
        /// </summary>
        /// <param name="parentHandle"></param>
        /// <param name="state">
        /// 0:オン/オフモード
        /// 1:常に演算
        /// 2:トレースモード
        /// 3:演算しない
        /// </param>
        /// <returns></returns>
        public static bool TrySetPhysicsState(IntPtr parentHandle, int state)
        {
            if (parentHandle == IntPtr.Zero)
                return false;
            if (state < 0 || state > 3)
                return true;

            IntPtr subMenuhWnd = TryGetMenuHandleByCaption(parentHandle, "物理演算(&P)");
            if (subMenuhWnd != IntPtr.Zero)
            {
                //選択したメニュー項目のIDをゲットする。
                int intIDx = GetMenuItemID(subMenuhWnd, state);

                //メニューをクリック
                var ret = SendMessage(parentHandle, WM_COMMAND, intIDx, "0");

                return true;
            }
            return false;
        }

        /// <summary>
        /// エフェクトのオンオフ状態を取得します。
        /// </summary>
        /// <param name="parentHandle"></param>
        /// <returns></returns>
        public static bool TrySetEffectState(IntPtr parentHandle, bool state)
        {
            if (parentHandle == IntPtr.Zero)
                return false;

            var currentstate = TryGetEffectState(parentHandle);
            if (currentstate == state)
                return true;

            //MMEメニューのハンドルをゲット
            IntPtr subMenuhWnd = TryGetMenuHandleByCaption(parentHandle, "MMEffect");
            if (subMenuhWnd != IntPtr.Zero)
            {
                //選択したメニュー項目のIDをゲットする。
                for (int i = 0; i <= 10; i++)
                {
                    int intIDx = GetMenuItemID(subMenuhWnd, i);
                    StringBuilder ssb = new StringBuilder(100);
                    var retx = GetMenuString(subMenuhWnd, i, ssb, ssb.Capacity, MF_BYPOSITION);
                    if (ssb.ToString().IndexOf("エフェクト使用(&E)\tCtrl+Shift+E") == 0)
                    {
                        //メニューをクリック
                        var ret = SendMessage(parentHandle, WM_COMMAND, intIDx, "0");
                        return true;
                    }
                }
            }
            return false;
        }

        /// <summary>
        /// メニューのハンドルを取得します。
        /// </summary>
        /// <param name="parentHandle"></param>
        /// <returns></returns>
        private static IntPtr TryGetMenuHandleByCaption(IntPtr parentHandle, string caption)
        {
            if (parentHandle == IntPtr.Zero)
                return IntPtr.Zero;

            //メニューハンドルをゲット
            IntPtr menuhWnd = GetMenu(parentHandle.ToInt32());
            if (menuhWnd != IntPtr.Zero)
            {
                for (int i = 0; i < 20; i++)
                {
                    StringBuilder sb = new StringBuilder(100);
                    var retxx = GetMenuString(menuhWnd, i, sb, sb.Capacity, MF_BYPOSITION);
                    if (sb.Length == 0)
                        break;
                    if (sb.ToString().IndexOf(caption) == 0)
                    {
                        //サブメニューのハンドルをゲット　第2引数はインデックス番号
                        return GetSubMenu(menuhWnd, i);
                    }
                }
            }
            return IntPtr.Zero;
        }

        /// <summary>
        /// ダイアログがなくなるまで「はい」を押し続けます。
        /// </summary>
        /// <param name="parentHandle"></param>
        /// <returns></returns>
        public static bool PressOKToDialog(IntPtr parentHandle, IEnumerable<string> captions)
        {
            foreach (var caption in captions)
            {
                var flg = true;
                for (int i = 0; i < 5; i++)
                {
                    System.Threading.Thread.Sleep(50);
                    IntPtr mmplushWnd = FindWindow("#32770", caption);
                    if (mmplushWnd != IntPtr.Zero)
                    {
                        flg = false;
                        int processId;
                        GetWindowThreadProcessId(mmplushWnd, out processId);

                        // プロセスIDからProcessクラスのインスタンスを取得
                        var parentProcess = Process.GetProcessById(processId);
                        if (parentProcess.MainWindowHandle == parentHandle)
                        {
                            var mmpluswindows = GetAllChildWindows(mmplushWnd, new List<Window>(), false);
                            foreach (Window window in mmpluswindows)
                            {
                                if (window.Title == "はい(&Y)" || window.Title == "OK")
                                {
                                    //むりやり「はい」を押す
                                    PostMessage(window.hWnd, BM_CLICK, 0, 0);
                                    flg = true;
                                    break;
                                }
                            }
                        }
                        if (flg)
                            break;
                    }
                }
                if (!flg)
                    return false;
            }
            return true;
        }

        /// <summary>
        /// MMDを用いて静画の保存を試みます。
        /// </summary>
        /// <param name="parentHandle">MMD親ウィンドウのハンドル</param>
        /// <param name="frame">何フレーム目？</param>
        /// <param name="filePath">ファイルパス</param>
        /// <param name="sleepms"></param>
        /// <returns></returns>
        public static async Task<bool> TrySavePictureByMMD(IntPtr parentHandle, long frame, string filePath, int sleepms = 50)
        {
            if (System.IO.File.Exists(filePath))
                //上書きはしない
                return false;

            //フレームを反映
            TrySetFrameNumber(parentHandle, frame);

            if (!TryPressMenu(parentHandle, 0, "画像ファイルに出力"))
                return false;

            await Task.Delay(200);
            //MMPlusの警告が出た場合はぶっちぎる
            IntPtr mmplushWnd = FindWindow("#32770", "MMPlus");
            if (mmplushWnd != IntPtr.Zero)
            {
                int processId;
                GetWindowThreadProcessId(mmplushWnd, out processId);
                var flg = false;

                //2022/9/11 重い環境だとMMPlusのエラーメッセージが認識されるまでに時間がかかることがあるので500ms待ちを5回まで試行する
                for (int i = 0; i < 5; i++)
                {
                    // プロセスIDからProcessクラスのインスタンスを取得
                    var parentProcess = Process.GetProcessById(processId);
                    if (parentProcess.MainWindowHandle == parentHandle)
                    {
                        var mmpluswindows = GetAllChildWindows(mmplushWnd, new List<Window>(), false);
                        foreach (Window window in mmpluswindows)
                        {
                            if (window.Title == "はい(&Y)")
                            {
                                //むりやり「はい」を押す
                                PostMessage(window.hWnd, BM_CLICK, 0, 0);
                                flg = true;
                                break;
                            }
                        }
                    }
                    if (flg)
                        break;
                    await Task.Delay(500);
                }
            }
            await Task.Delay(800);

            IntPtr hWnd = FindWindow("#32770", "画像ファイル出力");

            if (hWnd != IntPtr.Zero)
            {
                int processId;
                GetWindowThreadProcessId(hWnd, out processId);
                // プロセスIDからProcessクラスのインスタンスを取得
                var parentProcess = Process.GetProcessById(processId);
                if (parentProcess.MainWindowHandle == parentHandle)
                {
                    if (!string.IsNullOrWhiteSpace(filePath))
                    {
                        var children = GetAllChildWindows(hWnd, new List<Window>(), false);
                        var edit = children.Where(n => n.ClassName.ToLower() == "edit").FirstOrDefault();
                        if (edit != null)
                        {
                            SendMessage(edit.hWnd, WM_SETTEXT, 0, filePath);
                            await Task.Delay(sleepms);
                        }
                    }

                    ////メッセージボックスにEnterを入力
                    //SendMessage(hWnd, WM_IME_KEYDOWN, VK_RETURN, "1");
                    //await Task.Delay(sleepms);
                    //// エンターキー(離す)を送る
                    //SendMessage(hWnd, WM_IME_KEYUP, VK_RETURN, "0");

                    PostMessage(hWnd, WM_KEYDOWN, 0x0D, 0);

                    var recordingWindowFound = false;
                    for (int i = 0; i < 200; i++)
                    {
                        IntPtr recordhWnd = FindWindow(null, "録画中(0 frame)");
                        if (recordhWnd != IntPtr.Zero)
                            //録画中ウィンドウが見つかった
                            recordingWindowFound = true;
                        else if (recordingWindowFound)
                        {
                            //録画中ウィンドウがきえれば準備完了
                            await Task.Delay(i * 30);
                            return true;
                        }

                        await Task.Delay(200);
                        if (!recordingWindowFound && System.IO.File.Exists(filePath))
                            if (i > 5)
                                //超速で保存が終わったと思われる
                                return true;
                    }
                    return false;
                }
            }
            return false;
        }

        ///// <summary>
        ///// アクティブモデルのモーフをすべて取得します。
        ///// </summary>
        ///// <param name="parentHandle"></param>
        ///// <param name="morphtype">モーフ種類</param>
        ///// <param name="onActionModelChanged">この処理中にアクティブモデルが変わってしまった場合に実行するメソッド。帰り値でfalseが返ったら処理中断してnullを返す</param>
        ///// <returns></returns>
        //public static Dictionary<MorphType, List<string>> TryGetAllMorphValue(IntPtr parentHandle, Func<string, bool> onActiveModelChanged = null)
        //{
        //    var activemodel = TryGetActiveModelName(parentHandle);

        //    Dictionary<int, Window> mmdhash = GetAllMMDWindows(parentHandle);
        //    if (mmdhash == null)
        //        return null;

        //    //モデル選択コンボ
        //    var modelCombo = mmdhash[_Combo_Model];
        //    //最後の手段。MMDのモデル選択コンボをいじれなくする**//
        //    EnableWindow(modelCombo.hWnd, false);
        //    //最後の手段//

        //    try
        //    {
        //    tryagain:
        //        var ret = new Dictionary<MorphType, List<string>>();
        //        var retrycount = 0;
        //        foreach (MorphType morphtype in Enum.GetValues(typeof(MorphType)))
        //        {
        //            if (morphtype == MorphType.none)
        //                continue;
        //            var morphwindow = TryGetMorphWindows(parentHandle, morphtype);
        //            if (morphwindow == null || morphwindow.ComboBox == null)
        //                return ret;

        //            var lst = GetAllComboItems(morphwindow.ComboBox);
        //            ret.Add(morphtype, lst);

        //            var tmpactivemodel = TryGetActiveModelName(parentHandle);
        //            if (activemodel != tmpactivemodel)
        //            {
        //                //処理途中でアクティブモデルが切り替わったっぽい。
        //                //このまま進めると不正データになるのでもう一回最初からやりなおす
        //                activemodel = tmpactivemodel;
        //                retrycount++;
        //                if (onActiveModelChanged != null && retrycount <= 5)
        //                {
        //                    if (!onActiveModelChanged.Invoke(tmpactivemodel))
        //                        return null;

        //                    //最初からやりなおす
        //                    goto tryagain;
        //                }
        //            }
        //        }
        //        return ret;
        //    }
        //    catch (Exception ex)
        //    {
        //        //何かエラーが出たら握りつぶす
        //        return new Dictionary<MorphType, List<string>>();
        //    }
        //    finally
        //    {
        //        EnableWindow(modelCombo.hWnd, true);
        //    }
        //}

        /// <summary>
        /// 各モーフタイプ用の操作コントロール群を取得します。
        /// </summary>
        /// <param name="morphtype">モーフ種類</param>
        /// <returns></returns>
        public static MorphWindows TryGetMorphWindows(IntPtr parentHandle, MorphType morphtype)
        {
            Dictionary<int, Window> mmdhash = GetAllMMDWindows(parentHandle);
            if (mmdhash == null)
                return null;

            try
            {
                var ret = new MorphWindows();
                ret.Parent = mmdhash.FirstOrDefault().Value;

                switch (morphtype)
                {
                    case MorphType.Eye:
                        //目
                        ret.ComboBox = mmdhash[_Combo_MORPH_EYE];
                        ret.TrackBar = mmdhash[_TRACKBAR_MORPH_EYE];
                        ret.Edit = mmdhash[_EDIT_MORPH_EYE];
                        ret.Button = mmdhash[_BUTTON_MORPH_EYE];
                        break;

                    case MorphType.Lip:
                        //リップ
                        ret.ComboBox = mmdhash[_Combo_MORPH_LIP];
                        ret.TrackBar = mmdhash[_TRACKBAR_MORPH_LIP];
                        ret.Edit = mmdhash[_EDIT_MORPH_LIP];
                        ret.Button = mmdhash[_BUTTON_MORPH_LIP];
                        break;

                    case MorphType.Brow:
                        //まゆ
                        ret.ComboBox = mmdhash[_Combo_MORPH_BROW];
                        ret.TrackBar = mmdhash[_TRACKBAR_MORPH_BROW];
                        ret.Edit = mmdhash[_EDIT_MORPH_BROW];
                        ret.Button = mmdhash[_BUTTON_MORPH_BROW];
                        break;

                    case MorphType.Other:
                        //その他
                        ret.ComboBox = mmdhash[_Combo_MORPH_OTHER];
                        ret.TrackBar = mmdhash[_TRACKBAR_MORPH_OTHER];
                        ret.Edit = mmdhash[_EDIT_MORPH_OTHER];
                        ret.Button = mmdhash[_BUTTON_MORPH_OTHER];
                        break;

                    default:
                        break;
                }
                return ret;
            }
            catch (Exception)
            {
                return null;
            }
        }

        /// <summary>
        /// 指定したindexのモーフを指定した値にセットします。
        /// </summary>
        /// <param name="parentHandle"></param>
        /// <param name="morphtype">モーフ種類</param>
        /// <param name="morphIndex">いくつめのモーフ？</param>
        /// <param name="value">値</param>
        /// <param name="determine">確定するならtrue</param>
        /// <param name="argretrycount">リトライ回数(determine=trueの場合のみ有効)</param>
        /// <returns></returns>
        public static bool TrySetMorphValue(IntPtr parentHandle, MorphType morphtype, int morphIndex, float value, bool determine = true, int argretrycount = 1)
        {
            var morphwindow = TryGetMorphWindows(parentHandle, morphtype);
            if (morphwindow == null)
                return false;

            var retrycount = argretrycount;
            if (retrycount < 1)
                retrycount = 1;
            if (!determine)
                retrycount = 1;

            for (int i = 0; i <= retrycount - 1; i++) //リトライ回数分回す
            {
                //コンボのインデックスをそれに合わせる
                var selectedIndex = SendMessage(morphwindow.ComboBox.hWnd, CB_SETCURSEL, morphIndex, "");
                //↑だけでは必要なEventが発生しないので、こちらから強制的にCBN_SELCHANGEを発生させる
                int send_cbn_selchange = MakeWParam(morphwindow.ComboBox.ID, CBN_SELCHANGE);
                SendMessage(morphwindow.Parent.hWnd, WM_COMMAND, send_cbn_selchange, morphwindow.ComboBox.hWnd.ToInt32());

                //値を入れる
                System.Threading.Thread.Sleep(30);
                TrySetText(morphwindow.Edit, value.ToString(), true);
                System.Threading.Thread.Sleep(30);
                if (determine)
                {
                    //確定までやるなら、登録ボタンを押す
                    PostMessage(morphwindow.Button.hWnd, BM_CLICK, 0, 0);
                    System.Threading.Thread.Sleep(30);

                    var flg = false;
                    for (int j = 0; j < 3; j++)
                    {
                        //ちゃんと値が入ったかチェックする
                        MMDUtilility.TryResetFrameNumber(parentHandle);
                        var afterValue = MMDUtilility.TryGetMorphValue(parentHandle, morphtype, morphIndex);
                        if (Math.Round(afterValue, 3) == Math.Round(value, 3))
                        {
                            flg = true;
                            break;
                        }
                        System.Threading.Thread.Sleep(30);
                    }
                    if (flg)
                        break;

                    System.Threading.Thread.Sleep(30);
                    if (i >= retrycount)
                    {
                        Debugger.Break();
                        return false;
                    }
                }
                else
                    break;
            }

            return true;
        }

        /// <summary>
        /// 指定したモーフコンボを指定したindexにセットします。
        /// </summary>
        /// <param name="parentHandle"></param>
        /// <param name="morphtype">モーフ種類</param>
        /// <param name="morphIndex">いくつめのモーフ？</param>
        /// <param name="value">値</param>
        /// <param name="determine">確定するならtrue</param>
        /// <returns></returns>
        public static bool TrySetMorphIndex(IntPtr parentHandle, MorphType morphtype, int morphIndex)
        {
            var morphwindow = TryGetMorphWindows(parentHandle, morphtype);
            if (morphwindow == null)
                return false;

            //コンボのインデックスをそれに合わせる
            var selectedIndex = SendMessage(morphwindow.ComboBox.hWnd, CB_SETCURSEL, morphIndex, "");
            //↑だけでは必要なEventが発生しないので、こちらから強制的にCBN_SELCHANGEを発生させる
            int send_cbn_selchange = MakeWParam(morphwindow.ComboBox.ID, CBN_SELCHANGE);
            SendMessage(morphwindow.Parent.hWnd, WM_COMMAND, send_cbn_selchange, morphwindow.ComboBox.hWnd.ToInt32());

            return true;
        }

        /// <summary>
        /// 今の状態でキーを打ちます。
        /// </summary>
        /// <param name="parentHandle"></param>
        /// <param name="morphtype"></param>
        /// <param name="morphIndex"></param>
        /// <returns></returns>
        public static bool TrySetMorphValueAsIs(IntPtr parentHandle, MorphType morphtype, int morphIndex)
        {
            var morphwindow = TryGetMorphWindows(parentHandle, morphtype);
            if (morphwindow == null)
                return false;

            //コンボのインデックスをそれに合わせる
            var selectedIndex = SendMessage(morphwindow.ComboBox.hWnd, CB_SETCURSEL, morphIndex, "");
            //↑だけでは必要なEventが発生しないので、こちらから強制的にCBN_SELCHANGEを発生させる
            int send_cbn_selchange = MakeWParam(morphwindow.ComboBox.ID, CBN_SELCHANGE);
            SendMessage(morphwindow.Parent.hWnd, WM_COMMAND, send_cbn_selchange, morphwindow.ComboBox.hWnd.ToInt32());

            //登録ボタンを押す
            PostMessage(morphwindow.Button.hWnd, BM_CLICK, 0, 0);
            System.Threading.Thread.Sleep(30);

            return true;
        }

        /// <summary>
        /// 指定した名称のモーフを指定した値にセットします。
        /// </summary>
        /// <param name="parentHandle"></param>
        /// <param name="morphtype">モーフ種類</param>
        /// <param name="morphIndex">いくつめのモーフ？</param>
        /// <param name="value">値</param>
        /// <returns></returns>
        public static float TryGetMorphValue(IntPtr parentHandle, MorphType morphtype, int morphIndex)
        {
            var morphwindow = TryGetMorphWindows(parentHandle, morphtype);
            if (morphwindow == null)
                return float.MaxValue * -1;

            //コンボのインデックスをそれに合わせる
            var selectedIndex = SendMessage(morphwindow.ComboBox.hWnd, CB_SETCURSEL, morphIndex, "");
            //↑だけでは必要なEventが発生しないので、こちらから強制的にCBN_SELCHANGEを発生させる
            int send_cbn_selchange = MakeWParam(morphwindow.ComboBox.ID, CBN_SELCHANGE);

            if (morphIndex == 0)
                //なんか先頭のコンボの値の取得に失敗することが多い
                System.Threading.Thread.Sleep(10);
            SendMessage(morphwindow.Parent.hWnd, WM_COMMAND, send_cbn_selchange, morphwindow.ComboBox.hWnd.ToInt32());

            return Convert.ToSingle(morphwindow.Edit.Text);
        }

        /// <summary>
        /// 指定した名称のモーフを指定した値にセットします。
        /// </summary>
        /// <param name="parentHandle"></param>
        /// <param name="morphtype">モーフ種類</param>
        /// <param name="morphIndex">いくつめのモーフ？</param>
        /// <param name="value">値</param>
        /// <returns></returns>
        public static Dictionary<(MorphType, int), float> TryGetAllMorphValue(IntPtr parentHandle)
        {
            var ret = new Dictionary<(MorphType, int), float>();
            var morphWindows = new Dictionary<MorphType, MorphWindows>();
            foreach (MorphType morphType in Enum.GetValues(typeof(MorphType)).Cast<MorphType>())
            {
                if (morphType == MorphType.none)
                    continue;
                var morphwindow = TryGetMorphWindows(parentHandle, morphType);
                if (morphwindow != null)
                    morphWindows.Add(morphType, morphwindow);
            }
            foreach (var kvp in morphWindows)
            {
                var morphType = kvp.Key;
                var morphwindow = kvp.Value;
                //BeginAndEndUpdate(morphwindow.Parent.hWnd, false);
                BeginAndEndUpdate(morphwindow.TrackBar.hWnd, false);
                BeginAndEndUpdate(morphwindow.Edit.hWnd, false);

                var count = SendMessage(morphwindow.ComboBox.hWnd, CB_GETCOUNT, null, null);
                for (int i = 0; i < count; i++)
                {
                    //コンボのインデックスをそれに合わせる
                    var selectedIndex = SendMessage(morphwindow.ComboBox.hWnd, CB_SETCURSEL, i, "");
                    //↑だけでは必要なEventが発生しないので、こちらから強制的にCBN_SELCHANGEを発生させる
                    int send_cbn_selchange = MakeWParam(morphwindow.ComboBox.ID, CBN_SELCHANGE);

                    if (i == 0)
                        //なんか先頭のコンボの値の取得に失敗することが多い
                        System.Threading.Thread.Sleep(10);
                    SendMessage(morphwindow.Parent.hWnd, WM_COMMAND, send_cbn_selchange, morphwindow.ComboBox.hWnd.ToInt32());

                    var value = Convert.ToSingle(morphwindow.Edit.Text);
                    ret.Add((morphType, i), value);
                }

                //BeginAndEndUpdate(morphwindow.Parent.hWnd, true);
                BeginAndEndUpdate(morphwindow.TrackBar.hWnd, true);
                BeginAndEndUpdate(morphwindow.Edit.hWnd, true);
            }

            return ret;
        }

        /// <summary>
        /// メッセージボックスにエンターを押します
        /// </summary>
        /// <param name="parentHandle"></param>
        /// <param name="caption"></param>
        /// <returns></returns>
        public static bool PressEnterToMessageBox(IntPtr parentHandle, string caption)
        {
            IntPtr hWnd = FindWindow("#32770", caption);
            if (hWnd != IntPtr.Zero)
            {
                int processId;
                GetWindowThreadProcessId(hWnd, out processId);
                // プロセスIDからProcessクラスのインスタンスを取得
                var parentProcess = Process.GetProcessById(processId);

                int argprocessId = 0;
                GetWindowThreadProcessId(parentHandle, out argprocessId);

                //if (parentProcess.MainWindowHandle == parentHandle) //他の常駐プラグインが立ち上がっているとなぜかメインウィンドウハンドルが一致しないことがあるのでプロセスIDで判定する
                if (processId == argprocessId)
                {
                    //メッセージボックスにEnterを入力
                    //SendMessage(hWnd, WM_IME_KEYDOWN, VK_RETURN, "1");
                    //System.Threading.Thread.Sleep(50);
                    //// エンターキー(離す)を送る
                    //SendMessage(hWnd, WM_IME_KEYUP, VK_RETURN, "0");
                    //System.Threading.Thread.Sleep(50);

                    PostMessage(hWnd, WM_KEYDOWN, 0x0D, 0);
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// コンボボックスの中身を取得します。
        /// </summary>
        /// <param name="win"></param>
        /// <returns></returns>
        private static List<string> GetAllComboItems(Window win)
        {
            var ret = new List<string>();
            if (win == null)
                return ret;
            if (win.ClassName == "ComboBox")
            {
                var count = SendMessage(win.hWnd, CB_GETCOUNT, null, null);
                //var selectedIndex = SendMessage(win.hWnd, CB_GETCURSEL, 0, "");
                for (int i = 0; i < count; i++)
                {
                    var textlength = SendMessage(win.hWnd, CB_GETLBTEXTLEN, i, "");
                    if (textlength > 0)
                    {
                        var sb = new StringBuilder(textlength);
                        SendMessage(win.hWnd, CB_GETLBTEXT, i, sb);
                        ret.Add(sb.ToString());
                    }
                }
            }
            return ret;
        }

        private static Dictionary<IntPtr, Dictionary<int, Window>> _cache = null;

        private static Dictionary<int, Window> GetAllMMDWindows(IntPtr mmdhWnd)
        {
            if (mmdhWnd == IntPtr.Zero)
                return null;
            if (_cache != null && !_cache.ContainsKey(mmdhWnd))
                //MMDが切り替わった
                _cache = null;

            if (_cache == null)
            {
                _cache = new Dictionary<IntPtr, Dictionary<int, Window>>();
                var desc = GetAllChildWindows(mmdhWnd, new List<Window>(), true);

                if (false)
                {
                    foreach (var win in desc)
                    {
                        var addedstr = new StringBuilder();
                        if (win.ClassName == "ComboBox")
                        {
                            var count = SendMessage(win.hWnd, CB_GETCOUNT, null, null);
                            addedstr.Append($"{count}");
                            int textLen = GetWindowTextLength(win.hWnd);
                            string windowText = null;
                            if (0 < textLen)
                            {
                                // ウィンドウのタイトルを取得する
                                StringBuilder windowTextBuffer = new StringBuilder(textLen + 1);
                                GetWindowText(win.hWnd, windowTextBuffer, windowTextBuffer.Capacity);
                                windowText = windowTextBuffer.ToString();
                            }
                            var selectedIndex = SendMessage(win.hWnd, CB_GETCURSEL, 0, "");
                            for (int i = 0; i < Math.Min(5, count); i++)
                            {
                                var textlength = SendMessage(win.hWnd, CB_GETLBTEXTLEN, i, "");
                                var sb = new StringBuilder(textlength);
                                SendMessage(win.hWnd, CB_GETLBTEXT, i, sb);
                                addedstr.Append($",{sb.ToString()}");
                            }
                        }
                        if (win.ClassName == "Button")
                        {
                            StringBuilder sb = new StringBuilder(256);
                            SendMessage(win.hWnd, WM_GETTEXT, 255, sb);
                            addedstr.Append(sb.ToString());
                        }
                        if (win.ClassName == "Edit")
                        {
                            StringBuilder sb = new StringBuilder(256);
                            SendMessage(win.hWnd, WM_GETTEXT, 255, sb);
                            addedstr.Append(sb.ToString());
                        }
                        Console.WriteLine($"{win.ID},{win.ClassName},{win.Text},{addedstr.ToString()}");
                    }
                }

                var dict = desc.ToDictionary(n => n.ID);
                if (!_cache.ContainsKey(mmdhWnd))
                    _cache.Add(mmdhWnd, dict);
            }

            if (_cache.ContainsKey(mmdhWnd))
                return _cache[mmdhWnd];

            return null;
        }

        /// <summary>
        /// 指定されたキャプションのメニューのクリック処理を試みます。
        /// </summary>
        /// <param name="mmdhWnd">mmdのハンドル</param>
        /// <param name="nPos">左からいくつめのメニュー？</param>
        /// <param name="menuCaption"></param>
        private static bool TryPressMenu(IntPtr mmdhWnd, int nPos, string menuCaption)
        {
            if (mmdhWnd != IntPtr.Zero)

            {
                //メニューハンドルをゲット

                IntPtr MenuhWnd = GetMenu(mmdhWnd.ToInt32());

                StringBuilder tsbbb = new StringBuilder(8);
                if (MenuhWnd != IntPtr.Zero)

                {
                    //サブメニューのハンドルをゲット　第2引数はインデックス番号

                    IntPtr SubMenuhWnd = GetSubMenu((IntPtr)MenuhWnd, nPos);

                    //選択したメニュー項目のIDをゲットする。
                    for (int i = 0; i < 20; i++)

                    {
                        StringBuilder tsbb = new StringBuilder(100);

                        int intIDx = GetMenuItemID(SubMenuhWnd, i);
                        var retx = GetMenuString(SubMenuhWnd, intIDx, tsbb, tsbb.Capacity, MF_BYCOMMAND);
                        var menustr = tsbb.ToString();
                        if (menustr.IndexOf(menuCaption) >= 0)
                        {
                            _ = Task.Run(() =>
                            {
                                //メニューを実行する
                                var ret = SendMessage(mmdhWnd, WM_COMMAND, intIDx, "0");
                            });

                            return true;
                            break;
                        }
                    }
                }
            }
            return false;
        }

        #region "汎用"

        private static int _wantedProcessID = -1;
        private static List<Window> _wantedWindowList = null;

        public static void BeginAndEndUpdate(IntPtr hWnd, bool flg)
        {
            int wParam = 0;
            if (flg)
                wParam = 1;

            SendMessage(hWnd,
                WM_SETREDRAW, wParam, 0);
        }

        public static List<Window> TryEnumChildWindows(int processID)
        {
            _wantedProcessID = processID;
            _wantedWindowList = new List<Window>();
            try
            {
                //ウィンドウを列挙する
                EnumWindows(new EnumWindowsDelegate(EnumWindowCallBack), IntPtr.Zero);
                return _wantedWindowList;
            }
            finally
            {
                _wantedProcessID = -1;
                _wantedWindowList = null;
            }
        }

        private static bool EnumWindowCallBack(IntPtr hWnd, IntPtr lparam)
        {
            int processId;
            GetWindowThreadProcessId(hWnd, out processId);
            if (processId == _wantedProcessID)
            {
                var window = GetWindow(hWnd);
                _wantedWindowList.Add(window);
            }

            //すべてのウィンドウを列挙する
            return true;
        }

        #endregion "汎用"

        // 指定したウィンドウの全ての子孫ウィンドウを取得し、リストに追加する
        public static List<Window> GetAllChildWindows(IntPtr hWnd, List<Window> dest, bool isAll)
        {
            // タブコントロールなどで選択タブ以外は取得しない
            if (!isAll && !IsWindowVisible(hWnd)) return dest;

            dest.Add(GetWindow(hWnd));
            EnumChildWindows(hWnd).ToList().ForEach(x => GetAllChildWindows(x.hWnd, dest, isAll));

            IntPtr childhWnd = IntPtr.Zero;
            var xhwnd = FindWindowEx(hWnd, childhWnd, null, null);

            return dest;
        }

        // 与えた親ウィンドウの直下にある子ウィンドウを列挙する(孫ウィンドウは見つけてくれない)
        private static IEnumerable<Window> EnumChildWindows(IntPtr hParentWindow)
        {
            IntPtr hWnd = IntPtr.Zero;
            var ret = new List<Window>();
            while (true)
            {
                hWnd = FindWindowEx(hParentWindow, hWnd, null, null);
                if (hWnd == IntPtr.Zero)
                    break;

                ret.Add(GetWindow(hWnd));
            }
            return ret;
            //while ((hWnd = FindWindowEx(hParentWindow, hWnd, null, null)) != IntPtr.Zero) { yield return GetWindow(hWnd); }
        }

        // ウィンドウハンドルを渡すと、ウィンドウテキスト(ラベルなど)、クラス、スタイルを取得してWindowsクラスに格納して返す
        public static Window GetWindow(IntPtr hWnd)
        {
            int textLen = GetWindowTextLength(hWnd);
            string windowText = null;
            if (0 < textLen)
            {
                // ウィンドウのタイトルを取得する
                StringBuilder windowTextBuffer = new StringBuilder(textLen + 1);
                GetWindowText(hWnd, windowTextBuffer, windowTextBuffer.Capacity);
                windowText = windowTextBuffer.ToString();
            }

            // ウィンドウのクラス名を取得する
            StringBuilder classNameBuffer = new StringBuilder(256);
            GetClassName(hWnd, classNameBuffer, classNameBuffer.Capacity);
            // IDを取得する
            var id = GetWindowLong(hWnd, _GWL_ID);

            StringBuilder sbb = new StringBuilder(256);
            //SendMessage(hWnd, WM_GETTEXT, 255, sbb);

            return new Window() { hWnd = hWnd, Title = windowText, ClassName = classNameBuffer.ToString(), ID = id };
        }

        #region "汎用操作"

        /// <summary>
        /// プロセスの実行プログラムのパスを取得します。
        /// </summary>
        /// <param name="mmd"></param>
        /// <returns></returns>
        public static string GetProgramPathFromProcess(Process mmd)
        {
            using (var mc = new ManagementClass("Win32_Process"))
            using (var moc = mc.GetInstances())
            {
                foreach (ManagementObject mo in moc)
                    using (mo)
                    {
                        if (mo["ProcessId"].ToString() == mmd.Id.ToString())
                        {
                            try
                            {
                                return mo["ExecutablePath"].ToString();
                            }
                            catch (Exception e)
                            {
                                Console.WriteLine(e);
                                throw;
                            }
                        }
                    }
            }
            return null;
        }

        public static void TrySendKey(IntPtr hWnd, Keys keys)
        {
            var wParam = -1;
            switch (keys)
            {
                case Keys.Left:
                    wParam = VK_LEFT;
                    break;

                case Keys.Right:
                    wParam = VK_RIGHT;
                    break;

                case Keys.Up:
                    wParam = VK_UP;
                    break;

                case Keys.Down:
                    wParam = VK_DOWN;
                    break;

                default:
                    break;
            }
            if (wParam > 0)
                PostMessage(hWnd, WM_KEYDOWN, wParam, 0);
        }

        private static int MakeWParam(int loWord, int hiWord)
        {
            return (loWord & 0xFFFF) + ((hiWord & 0xFFFF) << 16);
        }

        /// <summary>
        /// 指定されたウィンドウのテキストを取得します。
        /// </summary>
        /// <param name="win">ウィンドウ</param>
        /// <returns></returns>
        private static string TryGetText(Window win)
        {
            var length = SendMessage(win.hWnd, WM_GETTEXTLENGTH, 0, "");
            StringBuilder sb = new StringBuilder(length);
            SendMessage(win.hWnd, WM_GETTEXT, 255, sb);

            return sb.ToString();
        }

        /// <summary>
        /// 指定されたウィンドウにテキストをセットします。
        /// </summary>
        /// <param name="win">ウィンドウ</param>
        /// <param name="text">セットするテキスト</param>
        /// <param name="sleepms">処理後のスリープ時間(ms)</param>
        /// <param name="addEnter">セットした後エンターキー操作を行うならtrue</param>
        /// <returns></returns>
        private static bool TrySetText(Window win, string text, bool addEnter = false)
        {
            //await Task.Delay(sleepms);
            var ret = SendMessage(win.hWnd, WM_SETTEXT, 0, text);
            //await Task.Delay(sleepms);
            if (addEnter && ret > 0)
            {
                //Enter操作を追加
                PostMessage(win.hWnd, WM_KEYDOWN, 0x0D, 0);
                //await Task.Delay(sleepms);
            }
            return ret > 0;
        }

        #endregion "汎用操作"
    }

    #region "プロセス検索"

    public interface IMMDSelector
    {
        Process TrySelectMMD(Process currentMMD, Process[] allMMDs);
    }

    public class MMDFinder
    {
        private static bool _isBusy = false;
        private Process _currentmmd = null;
        private string _mmPlusPath = string.Empty;
        private string _mmPlusIniPath = string.Empty;

        private Form _parentForm;
        private Label _lblMMPlus = null;
        private Label _lblMMD = null;
        private IMMDSelector _mmdSelector = null;

        public MMDFinder(Form parentForm, Process currentMMD = null, Label lblMMD = null, Label lblMMPlus = null, IMMDSelector mmdSelector = null)
        {
            this._parentForm = parentForm;
            this._currentmmd = currentMMD;

            this._lblMMD = lblMMD;
            this._lblMMPlus = lblMMPlus;
            this._mmdSelector = mmdSelector;
        }

        /// <summary>
        /// 動いているMMDのプロセスから対象のものを選択します。
        /// </summary>
        /// <param name="showmsg">true:見つからなかった時にメッセージを出す</param>
        /// <param name="forceUpdate">true:現在選択中のMMDがあっても選択フォームを出す</param>
        /// <returns></returns>
        public Process TryFindMMDProcess(bool showmsg, bool forceUpdate)
        {
            var unchanged = true;
            _mmPlusPath = null;
            _mmPlusIniPath = null;

            Process mmd = null;
            if (_isBusy)
                return _currentmmd;
            _isBusy = true;
            try
            {
                var mmdarray = Process.GetProcessesByName("MikuMikuDance");

                var mmds = new List<Process>();
                foreach (var tmpmmd in mmdarray)
                {
                    //32bit/64bitが不一致のプロセスを除外する →やっぱやめ
                    try
                    {
                        //var x = tmpmmd.MainModule;
                        mmds.Add(tmpmmd);
                    }
                    catch (Win32Exception ex)
                    {
                        Console.WriteLine($"32bit/64bit{tmpmmd.MainWindowTitle}");
                    }
                }
                if (this._currentmmd != null && this._currentmmd.HasExited)
                {
                    this._currentmmd.Dispose();
                    this._currentmmd = null;
                }

                if (this._currentmmd != null)
                {
                    if (!forceUpdate)
                    {
                        var tmpmmd = mmds.Where(n => n.Id == this._currentmmd.Id).FirstOrDefault();
                        if (tmpmmd != null && string.IsNullOrEmpty(this._currentmmd.MainWindowTitle))
                        {
                            //ロード中のpmmを拾うとタイトルが入ってないことがあるのでそれ対策
                            this._currentmmd = tmpmmd;
                            unchanged = false;
                        }
                        mmd = this._currentmmd;
                        return mmd;
                    }
                }
                unchanged = false;
                if (!mmds.Any())
                {
                    if (showmsg)
                        MessageBox.Show("MikuMikuDanceが起動していません");
                    return null;
                }
                else if (mmds.Count() > 1)
                {
                    if (this._mmdSelector != null)
                    {
                        mmd = this._mmdSelector.TrySelectMMD(this._currentmmd, mmds.ToArray());
                        if (mmd != null)
                            return mmd;
                    }
                    return null;
                    //using (var f = new frmMMDSelect(this._mmd, mmds))
                    //{
                    //    if (f.ShowDialog(this) != DialogResult.OK)
                    //        return null;
                    //    mmd = f.SelectedMmd;
                    //}
                }
                else
                {
                    mmd = mmds.FirstOrDefault();
                    if (mmd == this._currentmmd)
                    {
                    }
                    //MmdImport mmdimp = new MmdImport(mmd);
                    //mmdimp.Dispose();
                }

                return mmd;
            }
            catch
            {
                return null;
            }
            finally
            {
                if (!unchanged)
                {
                    this._parentForm.Invoke((Action)(() =>
                    {
                        var mmplusInstalled = false;
                        if (this._lblMMD != null)
                            this._lblMMD.Text = "MikuMikuDanceが起動していません";

                        if (mmd != null)
                        {
                            var mmdwindow = MMDUtil.MMDUtilility.GetWindow(mmd.MainWindowHandle);

                            var title = "(無題のプロジェクト)";
                            if (mmdwindow != null && mmdwindow.Title.TrimSafe().Contains(" ["))
                                title = mmdwindow.Title.Split(new[] { " [" }, 2, StringSplitOptions.None).Last().TrimEnd(']');

                            if (this._lblMMD != null)
                                this._lblMMD.Text = title;//WindowFinder.FindMMDTitle(mmd);

                            //mmPlusが入っているか確認
                            var mmdPath = MMDUtilility.GetProgramPathFromProcess(mmd);
                            var mmdDir = System.IO.Path.GetDirectoryName(mmdPath);
                            if (System.IO.Directory.Exists(mmdDir))
                            {
                                var mmPlusPath = System.IO.Path.Combine(mmdDir, "MMPlus.dll");
                                var mmPlusIniPath = System.IO.Path.Combine(mmdDir, "MMPlus.ini");
                                if (System.IO.File.Exists(mmPlusPath))
                                {
                                    this._mmPlusPath = mmPlusPath;
                                    mmplusInstalled = true;
                                }

                                if (System.IO.File.Exists(mmPlusIniPath))
                                    this._mmPlusIniPath = mmPlusIniPath;
                            }

                            if (this._lblMMPlus != null)
                                this._lblMMPlus.Visible = mmplusInstalled;
                        }
                    }));
                }
                _isBusy = false;
            }
        }
    }

    #endregion "プロセス検索"
}