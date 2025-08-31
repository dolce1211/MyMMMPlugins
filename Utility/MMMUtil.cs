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
        /// �������g�̃v���Z�X���擾���ĕێ����܂��B
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
        /// Scene.Controller��bool�̃v���p�e�B�̒l��Set���܂��B
        /// </summary>
        /// <param name="propertyName">�v���p�e�B��</param>
        /// <param name="value">�l</param>
        /// <returns></returns>
        public static bool SetControllerValue(string propertyName, bool value)
        {
            if (_scene == null)
            {
                //Initialize�𑖂点�ĂȂ��Ǝv����
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
        /// Scene.Controller��bool�̃v���p�e�B�̒l��Get���܂��B
        /// </summary>
        /// <param name="propertyName"></param>
        /// <returns></returns>
        public static bool GetControllerValue(string propertyName)
        {
            if (_scene == null)
            {
                //Initialize�𑖂点�ĂȂ��Ǝv����
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
        /// �ʃE�B���h�E�����Ă���΂��̏���Ԃ��܂��B
        /// </summary>
        /// <param name="usecache">BeginAndStopUpdate�ŕ`���~���ĊJ���g���ۂɎg�p�B�`���~�����GetAllChildWindows�����Ȃ��Ȃ邽��</param>
        /// <returns></returns>
        public static Window TryGetOtherWindow(bool usecache = false)
        {
            if (_currentMMMProcess == null)
            {
                //Initialize�𑖂点�ĂȂ��Ǝv����
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
                        //�q�E�B���h�E��5�ŁA�S��ClassName��windowclassName�ŁA[1]��Title��View�Ȃ�ʃE�B���h�E�Ƃ݂Ȃ�
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
        /// ���C����ʂ̕`����~�߂܂��B
        /// </summary>
        /// <param name="flg">
        /// false:�`����~�߂�
        /// true:�`����ĊJ����
        /// </param>
        public static void BeginAndEndUpdate(bool flg)
        {
            if (_currentMMMProcess == null)
            {
                //Initialize�𑖂点�ĂȂ��Ǝv����
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
        /// ApplicationForm��Enter�L�[�𑗐M���܂��iMMMUtility�̃w���p�[���\�b�h�j
        /// </summary>
        /// <returns>���������ꍇtrue</returns>
        public static bool SendEnterToApplicationForm(Form frm)
        {
            return SendKeyToApplicationFormInternal(frm, VK_RETURN, false, false, false);
        }

        /// <summary>
        /// ApplicationForm��Space�L�[�𑗐M���܂��iMMMUtility�̃w���p�[���\�b�h�j
        /// </summary>
        /// <returns>���������ꍇtrue</returns>
        public static bool SendSpaceToApplicationForm(Form frm)
        {
            return SendKeyToApplicationFormInternal(frm, VK_SPACE, false, false, false);
        }

        /// <summary>
        /// ApplicationForm�ɔC�ӂ̃L�[�ƏC���L�[�̑g�ݍ��킹�𑗐M���܂�
        /// </summary>
        /// <param name="frm">�Ώۃt�H�[��</param>
        /// <param name="key">���M����L�[�iVK_*�j</param>
        /// <param name="ctrl">Ctrl�L�[�𓯎��ɉ������ǂ���</param>
        /// <param name="shift">Shift�L�[�𓯎��ɉ������ǂ���</param>
        /// <param name="alt">Alt�L�[�𓯎��ɉ������ǂ���</param>
        /// <returns>���������ꍇtrue</returns>
        public static bool SendKeyWithModifiersToApplicationForm(Form frm, int key, bool ctrl = false, bool shift = false, bool alt = false)
        {
            return SendKeyToApplicationFormInternal(frm, key, ctrl, shift, alt);
        }

        /// <summary>
        /// ApplicationForm�ɃL�[���b�Z�[�W�𑗐M���܂��i�������\�b�h�j
        /// </summary>
        /// <param name="frm">�Ώۃt�H�[��</param>
        /// <param name="key">���M����L�[�iVK_*�j</param>
        /// <param name="ctrl">Ctrl�L�[�𓯎��ɉ������ǂ���</param>
        /// <param name="shift">Shift�L�[�𓯎��ɉ������ǂ���</param>
        /// <param name="alt">Alt�L�[�𓯎��ɉ������ǂ���</param>
        /// <returns>���������ꍇtrue</returns>
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

                // �C���L�[�����Ԃɉ���
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

                // ���C���L�[������
                results.Add(PostMessage(frm.Handle, WM_KEYDOWN, key, 0));
                System.Threading.Thread.Sleep(10);

                // ���C���L�[�𗣂�
                results.Add(PostMessage(frm.Handle, WM_KEYUP, key, 0));
                System.Threading.Thread.Sleep(10);

                // �C���L�[���t���ŗ���
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

                // �S�Ẵ��b�Z�[�W������ɑ��M���ꂽ�ꍇ�̂ݐ����Ƃ���
                return results.All(r => r);
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// ApplicationForm��Ctrl�L�[�Ƒg�ݍ��킹���L�[�𑗐M���܂��i�����݊����̂��ߕێ��j
        /// </summary>
        /// <param name="frm">�Ώۃt�H�[��</param>
        /// <param name="key">���M����L�[�iVK_Z, VK_Y�Ȃǁj</param>
        /// <returns>���������ꍇtrue</returns>
        private static bool SendCtrlKeyToApplicationFormInternal(Form frm, int key)
        {
            return SendKeyToApplicationFormInternal(frm, key, true, false, false);
        }

        /// <summary>
        /// ����̃t�H�[���ɑ΂���keybd_event��p����Ctrl+Z�𑗐M���܂�
        /// </summary>
        /// <param name="targetForm">�Ώۂ̃t�H�[��</param>
        /// <returns>���������ꍇtrue</returns>
        public static bool SendCtrlZToFormWithKeyboardEvent(Form targetForm)
        {
            return SendKeyToFormWithKeyboardEvent(targetForm, VK_Z, true, false, false);
        }

        /// <summary>
        /// ����̃t�H�[���ɑ΂���keybd_event��p����Ctrl+Y�𑗐M���܂�
        /// </summary>
        /// <param name="targetForm">�Ώۂ̃t�H�[��</param>
        /// <returns>���������ꍇtrue</returns>
        public static bool SendCtrlYToFormWithKeyboardEvent(Form targetForm)
        {
            return SendKeyToFormWithKeyboardEvent(targetForm, VK_Y, true, false, false);
        }

        /// <summary>
        /// ����̃t�H�[���ɑ΂���keybd_event��p���ăL�[�̑g�ݍ��킹�𑗐M���܂�
        /// </summary>
        /// <param name="targetForm">�Ώۂ̃t�H�[��</param>
        /// <param name="key">���M����L�[�iVK_*�j</param>
        /// <param name="ctrl">Ctrl�L�[�𓯎��ɉ������ǂ���</param>
        /// <param name="shift">Shift�L�[�𓯎��ɉ������ǂ���</param>
        /// <param name="alt">Alt�L�[�𓯎��ɉ������ǂ���</param>
        /// <returns>���������ꍇtrue</returns>
        public static bool SendKeyToFormWithKeyboardEvent(Form targetForm, int key, bool ctrl = false, bool shift = false, bool alt = false)
        {
            if (targetForm == null || targetForm.IsDisposed || !targetForm.IsHandleCreated)
            {
                return false;
            }

            try
            {
                // �t�H�[�����A�N�e�B�u�ɂ���
                if (targetForm.WindowState == FormWindowState.Minimized)
                {
                    targetForm.WindowState = FormWindowState.Normal;
                }

                // �t�H�[����O�ʂɎ����Ă��ăA�N�e�B�u�ɂ���
                SetForegroundWindow(targetForm.Handle);
                targetForm.Activate();
                targetForm.BringToFront();

                // �t�H�[�����A�N�e�B�u�ɂȂ�܂ŏ����ҋ@
                System.Threading.Thread.Sleep(100);

                // �C���L�[�����Ԃɉ���
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

                // ���C���L�[������
                keybd_event((byte)key, 0, 0, UIntPtr.Zero);
                System.Threading.Thread.Sleep(10);

                // ���C���L�[�𗣂�
                keybd_event((byte)key, 0, KEYEVENTF_KEYUP, UIntPtr.Zero);
                System.Threading.Thread.Sleep(10);

                // �C���L�[���t���ŗ���
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

        // keybd_event API�֘A�̒�`
        [DllImport("user32.dll", SetLastError = true)]
        private static extern void keybd_event(byte bVk, byte bScan, uint dwFlags, UIntPtr dwExtraInfo);

        [DllImport("user32.dll", SetLastError = true)]
        private static extern bool SetForegroundWindow(IntPtr hWnd);

        // keybd_event�p�̒萔
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