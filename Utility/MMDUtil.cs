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

        [DllImport("USER32.DLL")]
        private static extern int GetMenuItemID(IntPtr hWnd, int nPos);

        [DllImport("USER32.DLL")]
        private static extern long GetMenuString(IntPtr hMenu, int wIDItem, StringBuilder lpString, int nMaxCount, int wFlag);

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

        private const int _EDIT_MORPH_EYE = 511;
        private const int _BUTTON_MORPH_EYE = 524;

        /// <summary>
        /// 眉モーフ
        /// </summary>
        private const int _Combo_MORPH_BROW = 504;

        private const int _EDIT_MORPH_BROW = 506;

        private const int _BUTTON_MORPH_BROW = 525;

        /// <summary>
        /// リップモーフ
        /// </summary>
        private const int _Combo_MORPH_LIP = 514;

        private const int _EDIT_MORPH_LIP = 516;
        private const int _BUTTON_MORPH_LIP = 527;

        /// <summary>
        /// その他モーフ
        /// </summary>
        private const int _Combo_MORPH_OTHER = 519;

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

            return modelCombo.Text;
            int textLen = GetWindowTextLength(modelCombo.hWnd);
            string windowText = null;
            if (0 < textLen)
            {
                // ウィンドウのタイトルを取得する
                StringBuilder windowTextBuffer = new StringBuilder(textLen + 1);
                GetWindowText(modelCombo.hWnd, windowTextBuffer, windowTextBuffer.Capacity);
                windowText = windowTextBuffer.ToString();
            }

            return windowText;

            if (modelCombo == null)
                return String.Empty;
            var selectedIndex = SendMessage(modelCombo.hWnd, CB_GETCURSEL, 0, "");

            var textlength = SendMessage(modelCombo.hWnd, CB_GETLBTEXTLEN, selectedIndex, "");
            var sb = new StringBuilder(textlength);
            SendMessage(modelCombo.hWnd, CB_GETLBTEXT, selectedIndex, sb);

            return sb.ToString();
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

                    PostMessage(hWnd, 0x0100, 0x0D, 0);

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

        /// <summary>
        /// アクティブモデルのモーフをすべて取得します。
        /// </summary>
        /// <param name="parentHandle"></param>
        /// <param name="morphtype">モーフ種類</param>
        /// <returns></returns>
        public static Dictionary<MorphType, List<string>> TryGetAllMorphValue(IntPtr parentHandle)
        {
            var ret = new Dictionary<MorphType, List<string>>();
            foreach (MorphType morphtype in Enum.GetValues(typeof(MorphType)))
            {
                if (morphtype == MorphType.none)
                    continue;
                var morphwindow = TryGetMorphWindows(parentHandle, morphtype);
                if (morphwindow == null || morphwindow.ComboBox == null)
                    return ret;

                var lst = GetAllComboItems(morphwindow.ComboBox);
                ret.Add(morphtype, lst);
            }
            return ret;
        }

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
                        ret.Edit = mmdhash[_EDIT_MORPH_EYE];
                        ret.Button = mmdhash[_BUTTON_MORPH_EYE];
                        break;

                    case MorphType.Lip:
                        //リップ
                        ret.ComboBox = mmdhash[_Combo_MORPH_LIP];
                        ret.Edit = mmdhash[_EDIT_MORPH_LIP];
                        ret.Button = mmdhash[_BUTTON_MORPH_LIP];
                        break;

                    case MorphType.Brow:
                        //まゆ
                        ret.ComboBox = mmdhash[_Combo_MORPH_BROW];
                        ret.Edit = mmdhash[_EDIT_MORPH_BROW];
                        ret.Button = mmdhash[_BUTTON_MORPH_BROW];
                        break;

                    case MorphType.Other:
                        //その他
                        ret.ComboBox = mmdhash[_Combo_MORPH_OTHER];
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
        /// 指定した名称のモーフを指定した値にセットします。
        /// </summary>
        /// <param name="parentHandle"></param>
        /// <param name="morphtype">モーフ種類</param>
        /// <param name="morphName">モーフ名</param>
        /// <param name="value">値</param>
        /// <param name="determine">確定するならtrue</param>
        /// <returns></returns>
        public static bool TrySetMorphValue(IntPtr parentHandle, MorphType morphtype, string morphName, float value, bool determine = true)
        {
            var morphwindow = TryGetMorphWindows(parentHandle, morphtype);
            if (morphwindow == null)
                return false;

            //コンボの件数
            var count = SendMessage(morphwindow.ComboBox.hWnd, CB_GETCOUNT, null, null);

            //var selectedIndex = SendMessage(morphwindow.ComboBox.hWnd, CB_GETCURSEL, 0, "");
            //morphNameのバイト数を取得
            int byteCount = Encoding.GetEncoding("Shift_JIS").GetByteCount(morphName);
            for (int i = 0; i < count; i++)
            {
                var textlength = SendMessage(morphwindow.ComboBox.hWnd, CB_GETLBTEXTLEN, i, "");

                if (textlength == byteCount)
                {
                    var sb = new StringBuilder(textlength);
                    SendMessage(morphwindow.ComboBox.hWnd, CB_GETLBTEXT, i, sb);
                    if (sb.ToString() == morphName)
                    {
                        //モーフ名が一致した。
                        //コンボのインデックスをそれに合わせる
                        var selectedIndex = SendMessage(morphwindow.ComboBox.hWnd, CB_SETCURSEL, i, "");
                        //↑だけでは必要なEventが発生しないので、こちらから強制的にCBN_SELCHANGEを発生させる
                        int send_cbn_selchange = MakeWParam(morphwindow.ComboBox.ID, CBN_SELCHANGE);
                        SendMessage(morphwindow.Parent.hWnd, WM_COMMAND, send_cbn_selchange, morphwindow.ComboBox.hWnd.ToInt32());

                        //値を入れる
                        TrySetText(morphwindow.Edit, value.ToString(), true);
                        if (determine)
                            //確定までやるなら、登録ボタンを押す
                            PostMessage(morphwindow.Button.hWnd, BM_CLICK, 0, 0);

                        return true;
                    }
                }
            }
            return false;
        }

        /// <summary>
        /// 指定した名称のモーフを指定した値にセットします。
        /// </summary>
        /// <param name="parentHandle"></param>
        /// <param name="morphtype">モーフ種類</param>
        /// <param name="morphIndex">いくつめのモーフ？</param>
        /// <param name="value">値</param>
        /// <param name="determine">確定するならtrue</param>
        /// <returns></returns>
        public static bool TrySetMorphValue(IntPtr parentHandle, MorphType morphtype, int morphIndex, float value, bool determine = true)
        {
            var morphwindow = TryGetMorphWindows(parentHandle, morphtype);
            if (morphwindow == null)
                return false;

            //コンボのインデックスをそれに合わせる
            var selectedIndex = SendMessage(morphwindow.ComboBox.hWnd, CB_SETCURSEL, morphIndex, "");
            //↑だけでは必要なEventが発生しないので、こちらから強制的にCBN_SELCHANGEを発生させる
            int send_cbn_selchange = MakeWParam(morphwindow.ComboBox.ID, CBN_SELCHANGE);
            SendMessage(morphwindow.Parent.hWnd, WM_COMMAND, send_cbn_selchange, morphwindow.ComboBox.hWnd.ToInt32());

            //値を入れる
            System.Threading.Thread.Sleep(10);
            TrySetText(morphwindow.Edit, value.ToString(), true);
            System.Threading.Thread.Sleep(10);
            if (determine)
            {
                //確定までやるなら、登録ボタンを押す
                PostMessage(morphwindow.Button.hWnd, BM_CLICK, 0, 0);
                System.Threading.Thread.Sleep(10);
            }

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
            System.Threading.Thread.Sleep(10);

            return true;
        }

        /// <summary>
        /// 指定した名称のモーフを指定した値にセットします。
        /// </summary>
        /// <param name="parentHandle"></param>
        /// <param name="morphtype">モーフ種類</param>
        /// <param name="morphIndex">いくつめのモーフ？</param>
        /// <param name="value">値</param>
        /// <param name="determine">確定するならtrue</param>
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
            SendMessage(morphwindow.Parent.hWnd, WM_COMMAND, send_cbn_selchange, morphwindow.ComboBox.hWnd.ToInt32());

            return Convert.ToSingle(morphwindow.Edit.Text);
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

                    PostMessage(hWnd, 0x0100, 0x0D, 0);
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
                int textLen = GetWindowTextLength(win.hWnd);
                string windowText = null;
                if (0 < textLen)
                {
                    // ウィンドウのタイトルを取得する
                    StringBuilder windowTextBuffer = new StringBuilder(textLen + 1);
                    GetWindowText(win.hWnd, windowTextBuffer, windowTextBuffer.Capacity);
                    windowText = windowTextBuffer.ToString();
                }
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
                PostMessage(win.hWnd, 0x0100, 0x0D, 0);
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
                var mmds = Process.GetProcessesByName("MikuMikuDance");

                if (this._currentmmd != null && this._currentmmd.HasExited)
                {
                    this._currentmmd.Dispose();
                    this._currentmmd = null;
                }

                if (this._currentmmd != null)
                {
                    if (!forceUpdate)
                    {
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
                        mmd = this._mmdSelector.TrySelectMMD(this._currentmmd, mmds);
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
                        if (this._lblMMD != null && this._lblMMPlus != null)
                        {
                            this._lblMMPlus.Visible = false;
                            this._lblMMD.Text = "MikuMikuDanceが起動していません";
                        }

                        if (mmd != null)
                        {
                            var mmdwindow = MMDUtil.MMDUtilility.GetWindow(mmd.MainWindowHandle);

                            var title = "(無題のプロジェクト)";
                            if (mmdwindow != null && mmdwindow.Title.TrimSafe().Contains(" ["))
                                title = mmdwindow.Title.Split(new[] { " [" }, 2, StringSplitOptions.None).Last().TrimEnd(']');

                            if (this._lblMMD != null)
                                this._lblMMD.Text = title;//WindowFinder.FindMMDTitle(mmd);

                            //mmPlusが入っているか確認
                            var mmdPath = GetMMDPath(mmd);
                            var mmdDir = System.IO.Path.GetDirectoryName(mmdPath);
                            if (System.IO.Directory.Exists(mmdDir))
                            {
                                var mmPlusPath = System.IO.Path.Combine(mmdDir, "MMPlus.dll");
                                var mmPlusIniPath = System.IO.Path.Combine(mmdDir, "MMPlus.ini");
                                if (System.IO.File.Exists(mmPlusPath))
                                {
                                    this._mmPlusPath = mmPlusPath;
                                    if (this._lblMMPlus != null)
                                        this._lblMMPlus.Visible = true;
                                }

                                if (System.IO.File.Exists(mmPlusIniPath))
                                    this._mmPlusIniPath = mmPlusIniPath;
                            }
                        }
                    }));
                }
                _isBusy = false;
            }
        }

        /// <summary>
        /// MMDの実行ファイルのパスを取得します。
        /// </summary>
        /// <param name="mmd"></param>
        /// <returns></returns>
        private string GetMMDPath(Process mmd)
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
    }

    #endregion "プロセス検索"

    public class WindowFinder
    {
        private delegate bool EnumWindowsDelegate(IntPtr hWnd, IntPtr lparam);

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool EnumWindows(EnumWindowsDelegate lpEnumFunc,
            IntPtr lparam);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern int GetWindowText(IntPtr hWnd,
            StringBuilder lpString, int nMaxCount);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern int GetWindowTextLength(IntPtr hWnd);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern int GetClassName(IntPtr hWnd,
            StringBuilder lpClassName, int nMaxCount);

        private const int GW_HWNDNEXT = 2;

        [DllImport("user32")]
        private static extern int GetParent(int hwnd);

        [DllImport("user32")]
        private static extern int GetWindow(int hwnd, int wCmd);

        [DllImport("user32")]
        private static extern int FindWindow(
            String lpClassName, String lpWindowName);

        [DllImport("user32")]
        private static extern int GetWindowThreadProcessId(
            IntPtr hwnd, out int lpdwprocessid);

        [DllImport("user32")]
        private static extern int IsWindowVisible(int hwnd);

        private static object _lockObj = new object();
        private static Process _mmd = null;
        private static string _windowName = null;

        /// <summary>
        /// エントリポイント
        /// </summary>
        public static string FindMMDTitle(Process mmd)
        {
            if (mmd.MainWindowTitle.Contains(" ["))
            {
                _windowName = mmd.MainWindowTitle;
            }
            else
            {
                lock (_lockObj)
                {
                    _windowName = null;
                    _mmd = mmd;
                    //ウィンドウを列挙する
                    EnumWindows(new EnumWindowsDelegate(EnumWindowCallBack), IntPtr.Zero);

                    Console.ReadLine();
                }
            }
            if (_windowName != null && _windowName.Contains(" ["))
                return _windowName.Split(new[] { " [" }, 2, StringSplitOptions.None).Last().TrimEnd(']');
            else
                return "(無題のプロジェクト)";
        }

        private static bool EnumWindowCallBack(IntPtr hWnd, IntPtr lparam)
        {
            //ウィンドウのタイトルの長さを取得する
            int textLen = GetWindowTextLength(hWnd);
            if (0 < textLen)
            {
                //ウィンドウのタイトルを取得する
                StringBuilder tsb = new StringBuilder(textLen + 1);
                GetWindowText(hWnd, tsb, tsb.Capacity);

                //ウィンドウのクラス名を取得する
                StringBuilder csb = new StringBuilder(256);
                GetClassName(hWnd, csb, csb.Capacity);
                if (csb.ToString() == "Polygon Movie Maker")
                {
                    int pid;
                    GetWindowThreadProcessId(hWnd, out pid);
                    if (pid == _mmd.Id)
                    {
                        _windowName = tsb.ToString();
                    }
                }
                ////結果を表示する
                //Console.WriteLine("ハンドル:" + hWnd);
                //Console.WriteLine("クラス名:" + csb.ToString());
                //Console.WriteLine("タイトル:" + tsb.ToString());
            }

            //すべてのウィンドウを列挙する
            return true;
        }
    }
}