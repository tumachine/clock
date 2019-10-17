using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Clock
{
    public partial class Form1 : Form
    {
        public Graphics g;
        private Circle circle;

        private Timer timer;

        PictureBox backgroundBox;
        PictureBox frontBox;

        int totalTicks = 0;


        public Form1()
        {
            InitializeComponent();

            circle = new Circle(100, 60);

            timer = new Timer();
            timer.Interval = 1000;
            timer.Tick += timer_Tick;

            SplitterPanel circlePanel = splitContainer.Panel1;
            SplitterPanel settingsPanel = splitContainer.Panel2;
            backgroundBox = GetPictureBox(circlePanel, false);
            backgroundBox.Paint += box_Paint;

            frontBox = GetPictureBox(circlePanel, true);
            frontBox.Paint += box_Paint;
            frontBox.Parent = backgroundBox;

            circlePanel.Controls.Add(backgroundBox);
            
            //
            // Ellipse Panel
            //
            TableLayoutPanel ellipsePanel = new TableLayoutPanel();
            TrackBarControl ellipseTrackBar = new TrackBarControl("Width", 1, 20, 5);
            ButtonControl ellipseButton = new ButtonControl("Color", "Change Color");
            ellipseTrackBar.ValueChanged += () => circle.ellipsePen.Width = ellipseTrackBar.Value;
            ellipseButton.ButtonClicked += () => circle.ellipsePen.Color = ellipseButton.SetLabelColor();
            ellipsePanel.Controls.AddRange(new Control[] { ellipseTrackBar, ellipseButton });
            //
            //  Numbers Panel
            //
            TableLayoutPanel numbersPanel = new TableLayoutPanel();
            ButtonControl numberFontButton = new ButtonControl("Font", "Change font");
            numberFontButton.ButtonClicked += () => circle.numberFont = numberFontButton.SetLabelFont();
            ButtonControl numberFontAccentButton = new ButtonControl("Font Accent", "Change font");
            numberFontAccentButton.ButtonClicked += () => circle.numberAccentFont = numberFontAccentButton.SetLabelFont();
            ButtonControl numberColorButton = new ButtonControl("Color", "Change color");
            numberColorButton.ButtonClicked += () => circle.numberBrush.Color = numberColorButton.SetLabelColor();
            ButtonControl numberColorAccentButton = new ButtonControl("Color Accent", "Change color");
            numberColorAccentButton.ButtonClicked += () => circle.numberAccentBrush.Color = numberColorAccentButton.SetLabelColor();
            numbersPanel.Controls.AddRange(new Control[] { numberFontButton, numberFontAccentButton, numberColorButton, numberColorAccentButton });
            //
            // Other Panel
            //
            TableLayoutPanel otherPanel = new TableLayoutPanel();
            TrackBarControl indentTrackBar = new TrackBarControl("Indent", 1, 200, 40);
            indentTrackBar.ValueChanged += () => circle.Indent = indentTrackBar.Value;

            TrackBarControl everyNumberTrackBar = new TrackBarControl("Every number", 2, 160, 5);
            everyNumberTrackBar.ValueChanged += () => circle.everyNumber = everyNumberTrackBar.Value;

            TrackBarControl ticksTrackBar = new TrackBarControl("Ticks", 2, 360, 60);
            ticksTrackBar.ValueChanged += () => { circle.Ticks = ticksTrackBar.Value; totalTicks = 0; };

            TrackBarControl speedTrackBar = new TrackBarControl("Speed", 1, 20, 10);
            speedTrackBar.ValueChanged += () => this.timer.Interval = speedTrackBar.Value * 100;

            TrackBarControl rotateXTrackBar = new TrackBarControl("Rotate X", -10, 10, 10);
            rotateXTrackBar.ValueChanged += () => circle.rotationX = rotateXTrackBar.Value * 0.1f;

            TrackBarControl rotateYTrackBar = new TrackBarControl("Rotate Y", -10, 10, 10);
            rotateYTrackBar.ValueChanged += () => circle.rotationY = rotateYTrackBar.Value * 0.1f;

            CheckControl xRotationCheckBox = new CheckControl("Rotation X", "rotate x");
            xRotationCheckBox.OnChecked += () => circle.RotateX();
            xRotationCheckBox.OffChecked += () => circle.XCancelRotation();

            CheckControl yRotationCheckBox = new CheckControl("Rotation Y", "rotate y");
            yRotationCheckBox.OnChecked += () => circle.RotateY();
            yRotationCheckBox.OffChecked += () => circle.YCancelRotation();

            otherPanel.Controls.AddRange(new Control[] {
                ticksTrackBar, speedTrackBar, indentTrackBar, everyNumberTrackBar,
                rotateXTrackBar, rotateYTrackBar, xRotationCheckBox, yRotationCheckBox });
            otherPanel.AutoScroll = true;
            //
            //  Helping Panel
            //
            TableLayoutPanel helpingLinePanel = new TableLayoutPanel();
            ButtonControl helpingLineButton = new ButtonControl("Color", "Change color");
            helpingLineButton.ButtonClicked += () => circle.helpingLinePen.Color = helpingLineButton.SetLabelColor();
            TrackBarControl helpingLineWidthTrackBar = new TrackBarControl("Width", 1, 20, 4);
            helpingLineWidthTrackBar.ValueChanged += () => circle.helpingLinePen.Width = helpingLineWidthTrackBar.Value;
            TrackBarControl helpingLineLengthTrackBar = new TrackBarControl("Length", 1, 10, 5);
            helpingLineLengthTrackBar.ValueChanged += () => circle.HelpingLineLength = helpingLineLengthTrackBar.Value * 0.05f;
            helpingLinePanel.Controls.AddRange(new Control[] { helpingLineButton, helpingLineWidthTrackBar, helpingLineLengthTrackBar });
            //
            //  Helping Accent Panel
            //
            TableLayoutPanel helpingLineAccentPanel = new TableLayoutPanel();
            ButtonControl helpingLineAccentButton = new ButtonControl("Color", "Change color");
            helpingLineAccentButton.ButtonClicked += () => circle.helpingLineAccentPen.Color = helpingLineAccentButton.SetLabelColor();
            TrackBarControl helpingLineAccentWidthTrackBar = new TrackBarControl("Width", 1, 20, 4);
            helpingLineAccentWidthTrackBar.ValueChanged += () => circle.helpingLineAccentPen.Width = helpingLineAccentWidthTrackBar.Value;
            TrackBarControl helpingLineAccentLengthTrackBar = new TrackBarControl("Length", 1, 10, 5);
            helpingLineAccentLengthTrackBar.ValueChanged += () => circle.HelpingLineAccentLength = helpingLineAccentLengthTrackBar.Value * 0.05f;
            helpingLineAccentPanel.Controls.AddRange(new Control[] { helpingLineAccentButton, helpingLineAccentWidthTrackBar, helpingLineAccentLengthTrackBar });
            //
            //  Tick Panel
            //
            TableLayoutPanel tickPanel = new TableLayoutPanel();
            ButtonControl tickColorButton = new ButtonControl("Color", "Change color");
            tickColorButton.ButtonClicked += () => circle.tickPen.Color = tickColorButton.SetLabelColor();
            TrackBarControl tickWidthTrackBar = new TrackBarControl("Width", 1, 20, 4);
            tickWidthTrackBar.ValueChanged += () => circle.tickPen.Width = tickWidthTrackBar.Value;
            TrackBarControl tickLengthTrackBar = new TrackBarControl("Length", 1, 10, 5);
            tickLengthTrackBar.ValueChanged += () => circle.tickLength = tickLengthTrackBar.Value * 0.1f;
            tickPanel.Controls.AddRange(new Control[] { tickColorButton, tickWidthTrackBar, tickLengthTrackBar });

            TabPage ellipsePage = new TabPage("Ellipse");
            ellipsePage.Controls.Add(ellipsePanel);
            TabPage numbersPage = new TabPage("Numbers");
            numbersPage.Controls.Add(numbersPanel);
            TabPage otherPage = new TabPage("Other");
            otherPage.Controls.Add(otherPanel);
            TabPage tickPage = new TabPage("Tick");
            tickPage.Controls.Add(tickPanel);
            TabPage helpingLinesPage = new TabPage("Helping Lines");
            helpingLinesPage.Controls.Add(helpingLinePanel);
            TabPage helpingLinesAccentPage = new TabPage("Helping Accent Lines");
            helpingLinesAccentPage.Controls.Add(helpingLineAccentPanel);


            Panel[] panels = new Panel[] { ellipsePanel, numbersPanel, otherPanel, tickPanel, helpingLinePanel, helpingLineAccentPanel };
            ModifyPanels(panels, true, true);

            TabPage[] tabPages = new TabPage[] { ellipsePage, numbersPage, otherPage, tickPage, helpingLinesPage, helpingLinesAccentPage };
            tabControl.Controls.AddRange(tabPages);

            circle.UpdateCoreValues(backgroundBox.Width, backgroundBox.Height);

            circle.drawAction += () => backgroundBox.Invalidate();

            timer.Start();
        }

        private void box_Paint(object sender, PaintEventArgs e)
        {
            circle.UpdateCoreValues(backgroundBox.Width, backgroundBox.Height);
            circle.DrawBackground(e.Graphics);
            circle.DrawFront(e.Graphics, totalTicks);
        }

        private void Form1_Resize(object sender, EventArgs e)
        {
            backgroundBox.Invalidate();
        }

        private void timer_Tick(object sender, EventArgs e)
        {
            totalTicks++;
            if (totalTicks >= circle.Ticks)
            {
                totalTicks = 0;
            }
            backgroundBox.Invalidate();
        }

        public PictureBox GetPictureBox(Control control, bool transparent, int indent = 0)
        {

            PictureBox pictureBox = new PictureBox();

            pictureBox.Anchor = (((AnchorStyles.Top | AnchorStyles.Bottom)
            | AnchorStyles.Left)
            | AnchorStyles.Right);
            pictureBox.Location = new Point(indent, indent);
            pictureBox.Size = new Size(control.Width - indent, control.Height - indent);
            pictureBox.TabIndex = 0;
            pictureBox.TabStop = false;

            if (transparent)
                pictureBox.BackColor = Color.Transparent;

            return pictureBox;
        }

        public void ModifyPanels(Panel[] panels, bool anchor=true, bool autoscroll=false)
        {
            foreach (Panel panel in panels)
            {
                if (anchor)
                    panel.Anchor = (((AnchorStyles.Top | AnchorStyles.Bottom)
                        | AnchorStyles.Left)
                        | AnchorStyles.Right);
                panel.AutoScroll = autoscroll;
            }
        }
    }
}
