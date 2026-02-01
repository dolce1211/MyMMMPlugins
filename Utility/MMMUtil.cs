using MikuMikuPlugin;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

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
        /// 別ウィンドウ化されている場合、そのFormを取得します
        /// </summary>
        /// <param name="usecache">キャッシュを使用するかどうか</param>
        /// <returns>見つかったForm、または null</returns>
        public static Form TryGetOtherWindowForm()
        {
            var window = TryGetOtherWindow(false);
            if (window == null)
                return null;

            return TryGetFormFromWindowHandle(window.hWnd);
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
        /// 指定したハンドルを持つControlを再帰的に検索します
        /// </summary>
        /// <param name="parent">検索開始コントロール</param>
        /// <param name="handle">検索するハンドル</param>
        /// <returns>見つかったControl、または null</returns>
        private static Control FindControlByHandle(Control parent, IntPtr handle)
        {
            if (parent.Handle == handle)
                return parent;

            foreach (Control child in parent.Controls)
            {
                var found = FindControlByHandle(child, handle);
                if (found != null)
                    return found;
            }

            return null;
        }

        /// <summary>
        /// TryGetOtherWindowで取得したウィンドウハンドルから対応するFormを取得します
        /// </summary>
        /// <param name="windowHandle">ウィンドウハンドル（TryGetOtherWindowの戻り値のhWnd）</param>
        /// <returns>見つかったForm、または null</returns>
        private static Form TryGetFormFromWindowHandle(IntPtr windowHandle)
        {
            if (windowHandle == IntPtr.Zero)
                return null;

            try
            {
                // Control.FromHandleを使用してハンドルからControlを取得
                var control = Control.FromHandle(windowHandle);

                // ControlがFormの場合はそのまま返す
                if (control is Form form)
                {
                    return form;
                }

                // Controlの親を辿ってFormを探す
                while (control != null)
                {
                    if (control is Form parentForm)
                    {
                        return parentForm;
                    }
                    control = control.Parent;
                }

                // Application.OpenFormsから一致するハンドルを持つFormを探す
                foreach (Form openForm in Application.OpenForms)
                {
                    if (openForm.Handle == windowHandle)
                    {
                        return openForm;
                    }

                    // 子コントロールも確認
                    var foundControl = FindControlByHandle(openForm, windowHandle);
                    if (foundControl != null && foundControl.TopLevelControl is Form topForm)
                    {
                        return topForm;
                    }
                }
            }
            catch (Exception ex)
            {
                // エラーが発生した場合はnullを返す
                Debug.WriteLine($"TryGetFormFromWindowHandle error: {ex.Message}");
            }

            return null;
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
        /// クリップボードからモーフフレームデータを取得します
        /// </summary>
        /// <returns>モーフフレームデータのリスト、取得できない場合はnull</returns>
        public static Dictionary<string, List<IMorphFrameData>> GetMorphFrameDataFromClipboard()
        {
            try
            {
                var dataObject = Clipboard.GetDataObject();
                if (dataObject == null || !dataObject.GetDataPresent("MMM_MotionFrameData"))
                    return null;

                var data = Clipboard.GetData("MMM_MotionFrameData");
                if (data == null)
                    return null;

                // motiondata.data まで取得
                var motiondataField = data.GetType().GetField("motiondata");
                if (motiondataField == null)
                    return null;

                var motiondata = motiondataField.GetValue(data);
                if (motiondata == null)
                    return null;

                var dataField = motiondata.GetType().GetField("data");
                if (dataField == null)
                    return null;

                var outerDict = dataField.GetValue(motiondata) as System.Collections.IDictionary;
                if (outerDict == null)
                    return null;

                var result = new Dictionary<string, List<IMorphFrameData>>();

                // Dictionary<int, Dictionary<FrameItemType, ...>> を走査
                foreach (System.Collections.DictionaryEntry modelEntry in outerDict)
                {
                    // Dictionary<FrameItemType, Dictionary<string, ...>> を取得
                    if (modelEntry.Value is System.Collections.IDictionary frameTypeDict)
                    {
                        foreach (System.Collections.DictionaryEntry frameTypeEntry in frameTypeDict)
                        {
                            // FrameItemType が Morph の場合のみ処理
                            if (frameTypeEntry.Key.ToString() == "Morph")
                            {
                                // Dictionary<string, Dictionary<int, List<FrameData>>> を取得
                                if (frameTypeEntry.Value is System.Collections.IDictionary morphDict)
                                {
                                    foreach (System.Collections.DictionaryEntry morphEntry in morphDict)
                                    {
                                        string morphName = morphEntry.Key as string;

                                        // Dictionary<int, List<FrameData>> を取得
                                        if (morphEntry.Value is System.Collections.IDictionary frameDict)
                                        {
                                            foreach (System.Collections.DictionaryEntry frameEntry in frameDict)
                                            {
                                                // List<FrameData> を取得
                                                if (frameEntry.Value is System.Collections.IEnumerable frameDataList)
                                                {
                                                    foreach (var frameData in frameDataList)
                                                    {
                                                        if (frameData == null)
                                                            continue;

                                                        // リフレクションで値を取得
                                                        var frameDataType = frameData.GetType();

                                                        var frameNumberField = frameDataType.GetField("frameNumber");
                                                        var weightField = frameDataType.GetField("weight");
                                                        var interpolAField = frameDataType.GetField("interpolA");
                                                        var interpolBField = frameDataType.GetField("interpolB");

                                                        if (frameNumberField != null && weightField != null)
                                                        {
                                                            var frameNumber = Convert.ToInt64(frameNumberField.GetValue(frameData));
                                                            var weight = Convert.ToSingle(weightField.GetValue(frameData));
                                                            var morphFrameData = new MorphFrameData(frameNumber, weight);

                                                            // 補間曲線を設定
                                                            if (interpolAField != null)
                                                            {
                                                                var interpolA = interpolAField.GetValue(frameData);
                                                                if (interpolA != null)
                                                                {
                                                                    var xProp = interpolA.GetType().GetProperty("X");
                                                                    var yProp = interpolA.GetType().GetProperty("Y");
                                                                    if (xProp != null && yProp != null)
                                                                    {
                                                                        var x = Convert.ToInt32(xProp.GetValue(interpolA));
                                                                        var y = Convert.ToInt32(yProp.GetValue(interpolA));
                                                                        morphFrameData.InterpolA = new InterpolatePoint(x, y);
                                                                    }
                                                                }
                                                            }

                                                            if (interpolBField != null)
                                                            {
                                                                var interpolB = interpolBField.GetValue(frameData);
                                                                if (interpolB != null)
                                                                {
                                                                    var xProp = interpolB.GetType().GetProperty("X");
                                                                    var yProp = interpolB.GetType().GetProperty("Y");
                                                                    if (xProp != null && yProp != null)
                                                                    {
                                                                        var x = Convert.ToInt32(xProp.GetValue(interpolB));
                                                                        var y = Convert.ToInt32(yProp.GetValue(interpolB));
                                                                        morphFrameData.InterpolB = new InterpolatePoint(x, y);
                                                                    }
                                                                }
                                                            }
                                                            if(!result.ContainsKey(morphName))                                                           
                                                                result[morphName] = new List<IMorphFrameData>();
                                                            
                                                            result[morphName].Add(morphFrameData);
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                return result.Count > 0 ? result : null;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"GetMorphFrameDataFromClipboard failed: {ex.Message}");
                return null;
            }
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

        /// <summary>
        /// MikuMikuMovingの1～6の補間曲線パレットボタンを押下します
        /// </summary>
        /// <param name="applicationForm">アプリケーションフォーム</param>
        /// <param name="buttonNumber">1～6の番号</param>
        /// <returns>成功した場合true</returns>
        public static bool ClickInterpolateButton(Form applicationForm, int buttonNumber)
        {
            if (buttonNumber < 1 || buttonNumber > 6)
                return false;

            try
            {
                // アプリケーションフォームのハンドルを取得
                IntPtr mainWindowHandle = applicationForm.Handle;

                // ボタンのテキストとして探すべき文字列
                string buttonText = buttonNumber.ToString();

                // 子ウィンドウを再帰的に探索してボタンを見つける
                List<IntPtr> buttonHandles = FindButtonByText(mainWindowHandle, buttonText);

                if (buttonHandles?.Count > 0)
                {
                    foreach (var buttonHandle in buttonHandles)
                    {
                        ClickButtonWM_LBUTTONDOWN_UP(buttonHandle);
                    }
                }

                return false;
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// Form.Controlsを使って補間曲線パレットボタンを検索・クリックします
        /// </summary>
        /// <param name="applicationForm">アプリケーションフォーム</param>
        /// <param name="buttonNumber">1～6の番号</param>
        /// <returns>成功した場合true</returns>
        public static bool ClickInterpolateButtonByControls(Form applicationForm, int buttonNumber)
        {
            if (buttonNumber < 1 || buttonNumber > 6)
                return false;

            try
            {
                string buttonText = buttonNumber.ToString();
                var paletteButtons = Get_InterpolatePalletteButtons(applicationForm);

                var button = paletteButtons.FirstOrDefault(b => (b as Control).Text.Trim() == buttonText);

                if (button != null && button is IButtonControl btn)
                {
                    btn.PerformClick();
                    return true;
                }

                return false;
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// Form.Controlsを使って補間曲線パレットボタンを検索・クリックします
        /// </summary>
        /// <param name="applicationForm">アプリケーションフォーム</param>
        /// <param name="tabNumber">1～6の番号</param>
        /// <returns>成功した場合true</returns>
        public static bool SelectInterpolateTabByControls(Form applicationForm, string tabCaption)
        {
            try
            {
                var paletteTabs = Get_InterpolatePalletteTabs(applicationForm);
                if (paletteTabs.ContainsKey(tabCaption))
                {
                    var tab = paletteTabs[tabCaption];
                    Control tabparent = tab?.Parent;
                    if (tabparent != null)
                    {
                        var p = tabparent.GetType().GetProperty("SelectedTabPage", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public);
                        if (p != null)
                        {
                            p.SetValue(tabparent, tab);
                        }
                    }
                    return true;
                }

                return false;
            }
            catch (Exception)
            {
                return false;
            }
        }

        private static readonly List<object> _topButtons = new List<object>();
        private static readonly Dictionary<string, Control> _interpolatePalletteTabs = new Dictionary<string, Control>();
        private static readonly List<Control> _interpolatePalletteButtons = new List<Control>();
        private static readonly List<Control> _interpolatePalletteButtons_Camera = new List<Control>();

        private static Dictionary<string, Control> Get_InterpolatePalletteTabs(Form applicationForm)
        {
            if (_interpolatePalletteTabs.Count == 5)
                return _interpolatePalletteTabs;
            _interpolatePalletteTabs.Clear();
            var captions = new string[] { "R", "X", "Y", "Z", "M", "移動", "回転", "距離", "Fov" };
            foreach (var c in captions)
            {
                var tab = FindButtonByTextInControls(applicationForm, "XtraTabPage", c, 0, false);
                _interpolatePalletteTabs.Add(c, tab);
            }
            return _interpolatePalletteTabs;
        }

        private static List<Control> Get_InterpolatePalletteButtons(Form applicationForm)
        {
            var cache = _interpolatePalletteButtons;
            if (_scene?.ActiveCamera != null)
            {
                // カメラモード
                cache = _interpolatePalletteButtons_Camera;
            }
            if (cache.Count == 6)
                return cache;
            cache.Clear();
            for (int i = 1; i <= 6; i++)
            {
                var button = FindButtonByTextInControls(applicationForm, "SimpleButton", i.ToString(), 0, true);
                if (button != null)
                {
                    cache.Add(button);
                }
            }

            return cache;
        }

        private static List<object> Get_TopRibbonButtons(Form applicationForm)
        {
            if (_topButtons.Count > 0)
                return _topButtons;

            _topButtons.Clear();
            foreach (Control control in applicationForm.Controls)
            {
                if (control.GetType().Name == "RibbonControl")
                {
                    var pi = control.GetType().GetProperty("Items");
                    var items = pi?.GetValue(control) as IEnumerable;

                    var itemsPi = items.GetType().GetProperty("InnerList", BindingFlags.NonPublic | BindingFlags.Instance);
                    var ribbonItems = itemsPi?.GetValue(items) as ArrayList;
                    var type = ribbonItems.GetType();

                    _topButtons.AddRange(ribbonItems.OfType<object>());
                }
            }

            return _topButtons;
        }

        /// <summary>
        /// 画面上部のリボンのボタンをテキストで検索してクリックします
        /// </summary>
        /// <param name="applicationForm"></param>
        /// <param name="buttonText"></param>
        /// <returns></returns>
        public static bool ClickTopRibbonButtonByText(Form applicationForm, string buttonText)
        {
            try
            {
                var btns = Get_TopRibbonButtons(applicationForm);
                var targetBtn = btns.FirstOrDefault(b =>
                {
                    var pi = b.GetType().GetProperty("Caption");
                    var text = pi?.GetValue(b) as string;
                    return text == buttonText;
                });
                if (targetBtn != null)
                {
                    //DevExpress.XtraBars.BarButtonItem bb = targetBtn as DevExpress.XtraBars.BarButtonItem;
                    //bb.PerformClick();

                    var method = targetBtn.GetType().GetMethod("PerformClick", Type.EmptyTypes);
                    method?.Invoke(targetBtn, null);
                    return true;
                }
            }
            catch (Exception)
            {
            }
            return false;
        }

        /// <summary>
        /// Form.Controlsを使って「すべてにコピー」ボタンを検索・クリックします
        /// </summary>
        /// <param name="applicationForm">アプリケーションフォーム</param>
        /// <returns>成功した場合true</returns>
        public static bool ClickInterpolatePalleteButtonByToolTip(Form applicationForm, string targetToolTip)
        {
            try
            {
                // まず補間曲線パレットボタン（1-6）を探して、その親コンテナを特定
                var paletteButtons = Get_InterpolatePalletteButtons(applicationForm);
                var btns = Get_TopRibbonButtons(applicationForm);

                //var topButtons = Get_TopButtons(applicationForm);
                if (paletteButtons.Count > 0)
                {
                    // パレットボタンの親コンテナ内で「すべてにコピー」ボタンを探す
                    var copyButton = FindCopyToAllButtonInSameContainer(paletteButtons, targetToolTip);
                    if (copyButton != null && copyButton is IButtonControl copyBtn)
                    {
                        copyBtn.PerformClick();
                        return true;
                    }
                }

                // フォールバック：全体から小さなボタンを探す
                return FindAndClickSmallButtonInControls(applicationForm);
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// Form.Controls階層を再帰的に検索してテキストが一致するボタンを探します
        /// </summary>
        /// <param name="parent">検索開始コントロール</param>
        /// <param name="buttonText">探すボタンのテキスト</param>
        /// <returns>見つかったボタン、または null</returns>
        private static Control FindButtonByTextInControls(Control parent, string typeName, string text, int mode, bool forceVisible)
        {
            try
            {
                // 直接の子コントロールを検索
                IEnumerable controls = parent.Controls;
                //if (parent.GetType().Name == "RibbonControl")
                //{
                //    var pi = parent.GetType().GetProperty("Items");
                //    var items = pi?.GetValue(parent) as IEnumerable;

                //    var itemsPi = items.GetType().GetProperty("InnerList", BindingFlags.NonPublic | BindingFlags.Instance);
                //    controls = itemsPi?.GetValue(items) as IEnumerable;
                //}
                foreach (Control control in controls)
                {
                    if (string.IsNullOrEmpty(typeName) || control.GetType().Name == typeName)
                    {
                        if (mode == 0)
                        {
                            if (control.Text.Trim() == text)
                                if (!forceVisible || control.Visible)
                                    return control;
                        }
                        else
                        {
                            if (control.Text.Contains(text))
                                if (!forceVisible || control.Visible)
                                    return control;
                            var pi = control.GetType().GetProperty("ToolTip");
                            var toolTip = pi?.GetValue(control) as string;
                            if (toolTip == text)
                                if (!forceVisible || control.Visible)
                                    return control;
                        }
                    }

                    // 再帰的に子コントロールを検索
                    var result = FindButtonByTextInControls(control, typeName, text, mode, forceVisible);
                    if (result != null)
                    {
                        return result;
                    }
                }
            }
            catch (Exception)
            {
            }

            return null;
        }

        /// <summary>
        /// パレットボタンと同じコンテナ内で「すべてにコピー」ボタンを探します
        /// </summary>
        /// <param name="paletteButtons">パレットボタンのリスト</param>
        /// <returns>見つかったコピーボタン、または null</returns>
        private static Control FindCopyToAllButtonInSameContainer(List<Control> paletteButtons, string targetTooltip)
        {
            try
            {
                // 各パレットボタンの親コンテナを調べる
                var paletteButton = paletteButtons.FirstOrDefault();
                if (paletteButton != null)
                {
                    var container = paletteButton.Parent;
                    if (container != null)
                    {
                        // 同一コンテナ内のすべてのボタンを調べる

                        foreach (Control control in container.Controls)
                        {
                            if (control.GetType().Name == "SimpleButton")
                            {
                                var pi = control.GetType().GetProperty("ToolTip");
                                var toolTip = pi?.GetValue(control) as string;
                                if (toolTip == targetTooltip)
                                    return control;
                            }
                        }
                    }
                }
            }
            catch (Exception)
            {
            }

            return null;
        }

        /// <summary>
        /// フォールバック：小さなボタンを全体から探してクリック
        /// </summary>
        /// <param name="applicationForm">アプリケーションフォーム</param>
        /// <returns>成功した場合 true</returns>
        private static bool FindAndClickSmallButtonInControls(Form applicationForm)
        {
            try
            {
                var smallButtons = new List<Button>();
                FindSmallButtonsInControls(applicationForm, smallButtons);

                // 候補が少数の場合のみクリック（安全性のため）
                if (smallButtons.Count > 0 && smallButtons.Count <= 3)
                {
                    foreach (var button in smallButtons)
                    {
                        button.PerformClick();
                    }
                    return true;
                }

                return false;
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// 小さなボタンを再帰的に探します
        /// </summary>
        /// <param name="parent">検索開始コントロール</param>
        /// <param name="smallButtons">見つかったボタンのリスト</param>
        private static void FindSmallButtonsInControls(Control parent, List<Button> smallButtons)
        {
            try
            {
                foreach (Control control in parent.Controls)
                {
                    if (control is Button button &&
                        string.IsNullOrEmpty(button.Text.Trim()) &&
                        button.Size.Width <= 25 && button.Size.Height <= 25 &&
                        button.Size.Width > 12 && button.Size.Height > 12)
                    {
                        smallButtons.Add(button);
                    }

                    // 再帰的に子コントロールも検索
                    FindSmallButtonsInControls(control, smallButtons);
                }
            }
            catch (Exception)
            {
            }
        }

        /// <summary>
        /// WM_LBUTTONDOWN + WM_LBUTTONUP メッセージでボタンをクリックします
        /// </summary>
        /// <param name="buttonHandle">ボタンのハンドル</param>
        /// <returns>成功した場合true</returns>
        private static bool ClickButtonWM_LBUTTONDOWN_UP(IntPtr buttonHandle)
        {
            try
            {
                const int WM_LBUTTONDOWN = 0x0201;
                const int WM_LBUTTONUP = 0x0202;
                SendMessage(buttonHandle, WM_LBUTTONDOWN, (IntPtr)1, IntPtr.Zero);
                System.Threading.Thread.Sleep(10);
                SendMessage(buttonHandle, WM_LBUTTONUP, IntPtr.Zero, IntPtr.Zero);
                System.Threading.Thread.Sleep(50);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// 指定されたテキストを持つボタンを子ウィンドウから探します
        /// </summary>
        /// <param name="parentHandle">親ウィンドウのハンドル</param>
        /// <param name="buttonText">探すボタンのテキスト</param>
        /// <returns>見つかったボタンのハンドルリスト</returns>
        private static List<IntPtr> FindButtonByText(IntPtr parentHandle, string buttonText)
        {
            var ret = new List<IntPtr>();
            List<IntPtr> childWindows = new List<IntPtr>();

            // 全ての子ウィンドウを列挙
            EnumChildWindows(parentHandle, (hWnd, lParam) =>
            {
                childWindows.Add(hWnd);
                return true;
            }, IntPtr.Zero);

            foreach (IntPtr childHandle in childWindows)
            {
                try
                {
                    // ウィンドウクラス名を取得
                    StringBuilder className = new StringBuilder(256);
                    GetClassName(childHandle, className, className.Capacity);

                    // ウィンドウテキストを取得
                    StringBuilder windowText = new StringBuilder(256);
                    GetWindowText(childHandle, windowText, windowText.Capacity);

                    string classNameStr = className.ToString();
                    string windowTextStr = windowText.ToString();

                    // ボタンかどうか判定
                    bool isButton = classNameStr.Contains("Button") ||
                                  classNameStr.Contains("WindowsForms") ||
                                  classNameStr.StartsWith("Button") ||
                                  classNameStr == "Button";

                    bool textMatches = windowTextStr.Trim() == buttonText;

                    if (isButton && textMatches)
                    {
                        ret.Add(childHandle);
                    }
                }
                catch (Exception)
                {
                    // エラーが発生しても継続
                    continue;
                }
            }

            return ret;
        }

        // Win32 API宣言
        [DllImport("user32.dll")]
        private static extern bool EnumChildWindows(IntPtr hWndParent, EnumWindowsProc lpEnumFunc, IntPtr lParam);

        [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        private static extern int GetWindowText(IntPtr hWnd, StringBuilder lpString, int nMaxCount);

        [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        private static extern int GetClassName(IntPtr hWnd, StringBuilder lpClassName, int nMaxCount);

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        private static extern IntPtr SendMessage(IntPtr hWnd, int Msg, IntPtr wParam, IntPtr lParam);

        [DllImport("user32.dll", SetLastError = true)]
        private static extern void keybd_event(byte bVk, byte bScan, uint dwFlags, UIntPtr dwExtraInfo);

        [DllImport("user32.dll", SetLastError = true)]
        private static extern bool SetForegroundWindow(IntPtr hWnd);

        [DllImport("user32.dll", SetLastError = true)]
        private static extern bool PostMessage(IntPtr hWnd, int Msg, int wParam, int lParam);

        private delegate bool EnumWindowsProc(IntPtr hWnd, IntPtr lParam);

        // 定数定義
        private const uint KEYEVENTF_KEYUP = 0x0002;

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

    public static class MMMExtensions
    {
        private static Dictionary<int, Dictionary<int, DisplayFrame>> _displayFrameCacheBone = new Dictionary<int, Dictionary<int, DisplayFrame>>();
        private static Dictionary<int, Dictionary<int, DisplayFrame>> _displayFrameCacheMorph = new Dictionary<int, Dictionary<int, DisplayFrame>>();

        public static DisplayFrame FindDisplayFramesFromBone(this Model model, Bone bone)
        {
            return FindDisplayFramesInternal<Bone>(model, bone, _displayFrameCacheBone);
        }

        public static DisplayFrame FindDisplayFramesFromMorph(this Model model, Morph morph)
        {
            return FindDisplayFramesInternal<Morph>(model, morph, _displayFrameCacheMorph);
        }

        private static int TryGetId<T>(T item)
        {
            if (item is Bone bone)
            {
                return bone.BoneID;
            }
            else if (item is Morph morph)
            {
                return morph.MorphID;
            }
            return -1;
        }

        private static DisplayFrame FindDisplayFramesInternal<T>(this Model model, T item, Dictionary<int, Dictionary<int, DisplayFrame>> cache)
        {
            var id = TryGetId(item);
            if (cache.TryGetValue(model.ID, out var dict))
            {
                if (dict.TryGetValue(id, out var df))
                {
                    return df;
                }
            }
            else
            {
                dict = new Dictionary<int, DisplayFrame>();
                cache[model.ID] = dict;
            }
            if (dict.TryGetValue(id, out var ret))
            {
                return ret;
            }
            if (model == null || item == null)
            {
                return null;
            }

            ret = model.DisplayFrame.Cast<DisplayFrame>().FirstOrDefault(df =>
            {
                if (item is Bone bone)
                {
                    return df.Bones.Any(b => b.BoneID == bone.BoneID);
                }
                else if (item is Morph morph)
                {
                    return df.Morphs.Any(m => m.MorphID == morph.MorphID);
                }
                else
                    return false;
            });
            if (!dict.ContainsKey(id))
            {
                dict[id] = ret;
            }
            return ret;
        }
    }
}