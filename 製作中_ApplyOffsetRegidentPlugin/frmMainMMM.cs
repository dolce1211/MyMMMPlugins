using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ApplyOffsetRegidentPlugin;
using MikuMikuPlugin;

namespace ApplyOffsetRegidentPlugin
{
    public partial class frmMainMMM : Form
    {
        private Scene _scene;

        public event EventHandler<ExecutedEventArgs> Executed = null;

        public frmMainMMM(Scene scene)
        {
            InitializeComponent();

            this._scene = scene;

            foreach (var ctrl in this.Controls)
            {
                if (ctrl is MyConrol mc)
                {
                    mc.MyControlScrolled += MyControlScrolled;
                }
            }
        }

        private void rb01_CheckedChanged(object sender, EventArgs e)
        {
            float keisuu = 0.1f;
            if (sender == this.rb001)
                keisuu = 0.01f;
            else if (sender == this.rb01)
                keisuu = 0.1f;
            else if (sender == this.rb1)
                keisuu = 1f;

            this.myConrolX.Keisuu = keisuu;
            this.myConrolY.Keisuu = keisuu;
            this.myConrolZ.Keisuu = keisuu;
            this.btnReset.PerformClick();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            var args = this.CreateArgs();
            this.Executed?.Invoke(this, new ExecutedEventArgs(true, args));
            this.btnReset.PerformClick();
        }

        private void MyControlScrolled(object sender, EventArgs e)
        {
            var args = this.CreateArgs();
            this.Executed?.Invoke(this, new ExecutedEventArgs(false, args));
        }

        private Args CreateArgs()
        {
            return new Args()
            {
                Position = new DxMath.Vector3() { X = this.myConrolX.Value, Y = this.myConrolY.Value, Z = this.myConrolZ.Value },
                IsLocalL = this.chkLocalL.Checked,
                Rotation = new DxMath.Vector3() { X = this.myConrolRX.Value, Y = this.myConrolRY.Value, Z = this.myConrolRZ.Value },
                IsLocalR = this.chkLocalR.Checked,
            };
        }

        public void ApplyCount(int selectedBones, int selectedKeys)
        {
            this.lblCount.Text = $"{selectedBones}/{selectedKeys}";
        }

        private void frmMainMMM_FormClosed(object sender, FormClosedEventArgs e)
        {
            foreach (var ctrl in this.Controls)
            {
                if (ctrl is MyConrol mc)
                {
                    mc.MyControlScrolled -= MyControlScrolled;
                }
            }
        }

        private void btnReset_Click(object sender, EventArgs e)
        {
            foreach (var ctrl in this.Controls)
            {
                if (ctrl is MyConrol mc)
                {
                    mc.Initialize();
                }
            }
        }
    }
}

public class ExecutedEventArgs : EventArgs
{
    public bool Execute { get; }
    public Args Args { get; }

    public ExecutedEventArgs(bool execute, Args args)
    {
        this.Execute = execute;
        this.Args = args;
    }
}