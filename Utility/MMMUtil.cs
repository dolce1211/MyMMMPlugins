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
            if (frm == null)
            {
                Debugger.Break();
                return false;
            }

            try
            {
                // ���S�ȃL�[������V�~�����[�g�FKEYDOWN��KEYUP�̗����𑗐M
                bool keyDownResult = PostMessage(frm.Handle, WM_KEYDOWN, VK_RETURN, 0);

                // �Z���ԑҋ@���Ă���KEYUP�𑗐M�i��莩�R�ȃL�[����̃V�~�����[�g�j
                System.Threading.Thread.Sleep(10);

                bool keyUpResult = PostMessage(frm.Handle, WM_KEYUP, VK_RETURN, 0);

                // �����̃��b�Z�[�W������ɑ��M���ꂽ�ꍇ�̂ݐ����Ƃ���
                return keyDownResult && keyUpResult;
            }
            catch
            {
                return false;
            }
        }

        [DllImport("user32.dll", SetLastError = true)]
        private static extern bool PostMessage(IntPtr hWnd, int Msg, int wParam, int lParam);

        private const int WM_KEYDOWN = 0x0100;
        private const int WM_KEYUP = 0x0101;
        private const int VK_RETURN = 0x0D;
    }
}