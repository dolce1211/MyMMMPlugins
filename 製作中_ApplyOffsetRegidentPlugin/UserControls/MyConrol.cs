using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Input;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace ApplyOffsetRegidentPlugin
{
    public partial class MyConrol : UserControl
    {
        public EventHandler MyControlScrolled = null;

        [Browsable(true)]
        public string Caption
        {
            get { return this.lblCaption.Text; }
            set { this.lblCaption.Text = value; }
        }

        [Browsable(true)]
        public int Minimum
        {
            get { return (int)this.numericUpDown1.Minimum; }
            set
            {
                this.numericUpDown1.Minimum = value;
                this.trackBar1.Minimum = (int)(value * _keisuu);
            }
        }

        [Browsable(true)]
        public int Maximum
        {
            get { return (int)this.numericUpDown1.Maximum; }
            set
            {
                this.numericUpDown1.Maximum = value;
                this.trackBar1.Maximum = (int)(value * _keisuu);
            }
        }

        private float _keisuu = 0.1f;

        [Browsable(true)]
        public float Keisuu
        {
            get { return _keisuu; }
            set
            {
                _keisuu = value;
            }
        }

        [Browsable(true)]
        public float Value
        {
            get
            {
                return (float)this.numericUpDown1.Value;
            }
        }

        public MyConrol()
        {
            InitializeComponent();
        }

        public void Initialize()
        {
            this.trackBar1.Value = 0;
            this.numericUpDown1.Value = 0;
            this.MyControlScrolled.Invoke(this, new EventArgs());
        }

        private bool _trackbarBusy = false;

        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            decimal value = (decimal)((float)this.trackBar1.Value * this._keisuu);

            this._trackbarBusy = true;
            try
            {
                this.numericUpDown1.Value = value;
                this.MyControlScrolled.Invoke(this, new EventArgs());
            }
            finally
            {
                this._trackbarBusy = false;
            }
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            if (_trackbarBusy)
                return;
            var value = ((float)this.numericUpDown1.Value / _keisuu);
            if (value > this.trackBar1.Maximum)
                this.trackBar1.Value = this.trackBar1.Maximum;
            else if (value < this.trackBar1.Minimum)
                this.trackBar1.Value = this.trackBar1.Minimum;
            else
                this.trackBar1.Value = (int)value;
        }

        private void numericUpDown1_Enter(object sender, EventArgs e)
        {
            this.numericUpDown1.Select();
            this.numericUpDown1.Select(0, this.numericUpDown1.Text.Length);
        }
    }
}