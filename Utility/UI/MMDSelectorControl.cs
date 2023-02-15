using MMDUtil;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MMDUtil
{
    public partial class MMDSelectorControl : UserControl
    {
        public MMDSelectorControl()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 選択処理中はtrueを返します。
        /// </summary>
        public bool IsBusy { get; private set; } = false;

        /// <summary>
        /// 選択中のMMDのプロセス
        /// </summary>
        public Process MMDProcess { get; private set; } = null;

        /// <summary>
        /// MMEffect.dllのパス
        /// </summary>
        public string MMEPath { get; private set; } = string.Empty;

        private void btnSelectMMD_Click(object sender, EventArgs e)
        {
            this.MMDProcess = this.SelectMMD(true, true);
        }

        /// <summary>
        /// 動いているMMDのプロセスから対象のものを選択します。
        /// </summary>
        /// <param name="showmsg">true:見つからなかった時にメッセージを出す</param>
        /// <param name="forceUpdate">true:現在選択中のMMDがあっても選択フォームを出す</param>
        /// <returns></returns>
        public Process SelectMMD(bool showmsg = false, bool forceUpdate = false)
        {
            this.IsBusy = true;
            var selector = new MMDUtil.MMDFinder(this.ParentForm, this.MMDProcess, this.lblMMD, this.lblMMPlus, new MMDSelector(this.ParentForm));
            this.MMDProcess = selector.TryFindMMDProcess(showmsg, forceUpdate);
            this.IsBusy = false;

            this.MMEPath = selector.MMEPath;
            return this.MMDProcess;
        }
    }

    public class MMDSelector : MMDUtil.IMMDSelector
    {
        private Form _parentForm = null;

        public MMDSelector(Form parentForm)
        {
            this._parentForm = parentForm;
        }

        /// <summary>
        /// 複数あるMMDを選択します。
        /// </summary>
        /// <param name="currentMMD"></param>
        /// <param name="allMMDs"></param>
        /// <returns></returns>
        public Process TrySelectMMD(Process currentMMD, Process[] allMMDs)
        {
            using (var f = new frmMMDSelect(currentMMD, allMMDs))
            {
                if (f.ShowDialog(this._parentForm) != DialogResult.OK)
                    return null;

                var ret = f.SelectedMmd;
                if (ret != null && !ret.HasExited)
                    MMDUtilility.SetForegroundWindow(ret.MainWindowHandle);
                return f.SelectedMmd;
            }
        }
    }
}