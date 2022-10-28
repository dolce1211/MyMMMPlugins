using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace MMDUtil
{
    public partial class frmMMDSelect : Form
    {
        private IList<Process> _mmds = null;

        public frmMMDSelect(Process selectedMmd, IList<Process> mmds)
        {
            InitializeComponent();
            this.ShowInTaskbar = false;
            mmdComboBox.Items.Clear();
            mmdComboBox.Items.AddRange(mmds.ToArray());
            mmdComboBox.SelectedIndex = 0;
            if (selectedMmd != null)
            {
                foreach (var item in mmdComboBox.Items)
                {
                    Process tmpMmd = (Process)item;
                    if (tmpMmd.Id == selectedMmd.Id)
                    {
                        mmdComboBox.SelectedItem = item;
                        break;
                    }
                }
            }

            this._mmds = mmds;
        }

        public Process SelectedMmd
        {
            get
            {
                return _mmds[this.mmdComboBox.SelectedIndex];
            }
        }

        private void mmdComboBox_Format(object sender, ListControlConvertEventArgs e)
        {
            Process mmd = (Process)e.Value;
            var windowName = string.Empty;
            if (mmd.MainWindowTitle.Contains(" ["))
            {
                windowName = mmd.MainWindowTitle;
            }

            if (windowName != null && windowName.Contains(" ["))
                windowName = windowName.Split(new[] { " [" }, 2, StringSplitOptions.None).Last().TrimEnd(']');
            else
                windowName = "(無題のプロジェクト)";

            e.Value = "[PID: " + mmd.Id + "] " + windowName;
        }
    }
}