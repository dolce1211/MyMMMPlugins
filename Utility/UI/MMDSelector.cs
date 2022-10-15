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
        /// MMDのプロセス
        /// </summary>
        public Process MMD { get; private set; } = null;

        /// <summary>
        /// 動いているMMDのプロセスから対象のものを選択します。
        /// </summary>
        /// <param name="nomsg"></param>
        /// <param name="forceUpdate"></param>
        /// <returns></returns>
        private Process SelectMMD(bool nomsg, bool forceUpdate)
        {
            var selector = new MMDUtil.MMDFinder(this.MMD, this.lblMMD, this.lblMMPlus, new MMDSelectorx(this.ParentForm));
            return selector.TryFindMMDProcess(nomsg, forceUpdate);
        }
    }

    public class MMDSelectorx : MMDUtil.IMMDSelector
    {
        private Form _parentForm = null;

        public MMDSelectorx(Form parentForm)
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
                return f.SelectedMmd;
            }
        }
    }
}