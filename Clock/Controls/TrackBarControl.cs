using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Clock
{
    public partial class TrackBarControl : UserControl
    {
        public event Action ValueChanged;
        public int Value { get => trackBar.Value; }

        public TrackBarControl(string title, int min, int max, int value)
        {
            InitializeComponent();

            titleLabel.Text = title;
            trackBar.Minimum = min;
            trackBar.Maximum = max;
            trackBar.Value = value;
            valueLabel.Text = value.ToString();
        }

        private void trackBar_ValueChanged(object sender, EventArgs e)
        {
            valueLabel.Text = trackBar.Value.ToString();
            ValueChanged?.Invoke();
        }
    }
}
