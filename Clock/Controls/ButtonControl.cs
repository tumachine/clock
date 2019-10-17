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
    public partial class ButtonControl : UserControl
    {
        public event Action ButtonClicked;
        public Label PreviewLabel { get => previewLabel; set => previewLabel = value; }

        public ButtonControl(string title, string buttonName)
        {
            InitializeComponent();

            titleLabel.Text = title;
            button.Text = buttonName;
        }

        private void button_Click(object sender, EventArgs e)
        {
            ButtonClicked?.Invoke();
        }

        public Color SetLabelColor()
        {
            ColorDialog colorD = new ColorDialog();
            if (colorD.ShowDialog() == DialogResult.OK)
                previewLabel.BackColor = colorD.Color;
            return colorD.Color;
        }

        public Font SetLabelFont()
        {
            FontDialog fontD = new FontDialog();
            if (fontD.ShowDialog() == DialogResult.OK)
                previewLabel.Font = fontD.Font;
            return fontD.Font;
        }
    }
}
