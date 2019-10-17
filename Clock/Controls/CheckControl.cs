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
    public partial class CheckControl : UserControl
    {
        public event Action OnChecked;
        public event Action OffChecked;

        public CheckControl(string title, string checkName)
        {
            InitializeComponent();
            titleLabel.Text = title;
            checkBox.Text = checkName;
        }

        private void checkBox_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox.Checked)
                OnChecked?.Invoke();
            else
                OffChecked?.Invoke();
        }
    }
}
