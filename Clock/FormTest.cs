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
    public partial class FormTest : Form
    {
        CircleRender circle;

        public FormTest()
        {
            InitializeComponent();

            SplitterPanel circlePanel = splitContainer.Panel1;
            SplitterPanel settingsPanel = splitContainer.Panel2;
            PictureBox backgroundBox = GetPictureBox(circlePanel, false);

            PictureBox frontBox = GetPictureBox(circlePanel, true);
            frontBox.Parent = backgroundBox;

            circle = new CircleRender(backgroundBox);

            circlePanel.Controls.Add(backgroundBox);
            
            //
            // Ellipse Panel
            //
            TableLayoutPanel ellipsePanel = new TableLayoutPanel();

            TrackBarControl ellipseTrackBar = new TrackBarControl("Width", 1, 20, 5);
            ellipseTrackBar.ValueChanged += () => UpdateAfterChange(() => circle.Draw.ellipsePen.Width = ellipseTrackBar.Value);

            ButtonControl ellipseButton = new ButtonControl("Color", "Change Color");
            ellipseButton.ButtonClicked += () => UpdateAfterChange(() => circle.Draw.ellipsePen.Color = ellipseButton.SetLabelColor());

            ellipsePanel.Controls.AddRange(new Control[] { ellipseTrackBar, ellipseButton });
            //
            //  Numbers Panel
            //
            TableLayoutPanel numbersPanel = new TableLayoutPanel();

            ButtonControl numberFontButton = new ButtonControl("Font", "Change font");
            numberFontButton.ButtonClicked += () => UpdateAfterChange(() => circle.Draw.numberFont = numberFontButton.SetLabelFont());

            ButtonControl numberFontAccentButton = new ButtonControl("Font Accent", "Change font");
            numberFontAccentButton.ButtonClicked += () => UpdateAfterChange(() => circle.Draw.numberAccentFont = numberFontAccentButton.SetLabelFont());

            TrackBarControl numberDistanceTrackBar = new TrackBarControl("Font Distance", 1, 20, 1);
            numberDistanceTrackBar.ValueChanged += () => UpdateAfterChange(() => circle.Draw.numberDistance = numberDistanceTrackBar.Value * 0.1f);

            ButtonControl numberColorButton = new ButtonControl("Color", "Change color");
            numberColorButton.ButtonClicked += () => UpdateAfterChange(() => circle.Draw.numberBrush.Color = numberColorButton.SetLabelColor());

            ButtonControl numberColorAccentButton = new ButtonControl("Color Accent", "Change color");
            numberColorAccentButton.ButtonClicked += () => UpdateAfterChange(() => circle.Draw.numberAccentBrush.Color = numberColorAccentButton.SetLabelColor());

            TrackBarControl numberAccentDistanceTrackBar = new TrackBarControl("Font Accent Distance", 1, 20, 1);
            numberAccentDistanceTrackBar.ValueChanged += () => UpdateAfterChange(() => circle.Draw.numberAccentDistance = numberAccentDistanceTrackBar.Value * 0.1f);

            numbersPanel.Controls.AddRange(new Control[] { 
                numberFontButton, numberFontAccentButton, numberColorButton, numberColorAccentButton, numberDistanceTrackBar, numberAccentDistanceTrackBar });
            //
            // Other Panel
            //
            TableLayoutPanel otherPanel = new TableLayoutPanel();

            TrackBarControl indentTrackBar = new TrackBarControl("Indent", 1, 200, 40);
            indentTrackBar.ValueChanged += () => UpdateAfterChange(() => circle.Manager.UpdateForIndentChange(indentTrackBar.Value));

            TrackBarControl everyNumberTrackBar = new TrackBarControl("Every number", 2, 160, 5);
            everyNumberTrackBar.ValueChanged += () => UpdateAfterChange(() => circle.Draw.everyNumber = everyNumberTrackBar.Value);

            TrackBarControl ticksTrackBar = new TrackBarControl("Ticks", 2, 360, 60);
            ticksTrackBar.ValueChanged += () => UpdateAfterChange(() => { circle.Manager.UpdateForTicksChange(ticksTrackBar.Value); circle.totalTicks = 0; });

            TrackBarControl speedTrackBar = new TrackBarControl("Speed", 1, 20, 10);
            speedTrackBar.ValueChanged += () => UpdateAfterChange(() => circle.timerTick.Interval = speedTrackBar.Value * 100);

            TrackBarControl rotateXTrackBar = new TrackBarControl("Rotate X", -10, 10, 10);
            rotateXTrackBar.ValueChanged += () => UpdateAfterChange(() => circle.Manager.UpdateForRotationChange(rotateXTrackBar.Value * 0.1f, 1));

            TrackBarControl rotateYTrackBar = new TrackBarControl("Rotate Y", -10, 10, 10);
            rotateYTrackBar.ValueChanged += () => UpdateAfterChange(() => circle.Manager.UpdateForRotationChange(1, rotateYTrackBar.Value * 0.1f));

            CheckControl xRotationCheckBox = new CheckControl("Rotation X", "rotate x");
            xRotationCheckBox.OnChecked += () => UpdateAfterChange(() => { circle.StartRotating(true); });
            xRotationCheckBox.OffChecked += () => UpdateAfterChange(() => { circle.StopRotating(); } );
            
            CheckControl yRotationCheckBox = new CheckControl("Rotation Y", "rotate y");
            yRotationCheckBox.OnChecked += () => UpdateAfterChange(() => { circle.StartRotating(false); });
            yRotationCheckBox.OffChecked += () => UpdateAfterChange(() => { circle.StopRotating(); });

            otherPanel.Controls.AddRange(new Control[] {
            ticksTrackBar, speedTrackBar, indentTrackBar, everyNumberTrackBar, rotateXTrackBar, rotateYTrackBar, xRotationCheckBox, yRotationCheckBox });
            otherPanel.AutoScroll = true;
            //
            //  Helping Panel
            //
            TableLayoutPanel helpingLinePanel = new TableLayoutPanel();

            ButtonControl helpingLineButton = new ButtonControl("Color", "Change color");
            helpingLineButton.ButtonClicked += () => UpdateAfterChange(() => circle.Draw.helpingLinePen.Color = helpingLineButton.SetLabelColor());

            TrackBarControl helpingLineWidthTrackBar = new TrackBarControl("Width", 1, 20, 4);
            helpingLineWidthTrackBar.ValueChanged += () => UpdateAfterChange(() => circle.Draw.helpingLinePen.Width = helpingLineWidthTrackBar.Value);

            TrackBarControl helpingLineLengthTrackBar = new TrackBarControl("Length", 1, 10, 5);
            helpingLineLengthTrackBar.ValueChanged += () => UpdateAfterChange(() => circle.Draw.HelpingLineLength = helpingLineLengthTrackBar.Value * 0.05f);

            helpingLinePanel.Controls.AddRange(new Control[] { helpingLineButton, helpingLineWidthTrackBar, helpingLineLengthTrackBar });
            //
            //  Helping Accent Panel
            //
            TableLayoutPanel helpingLineAccentPanel = new TableLayoutPanel();

            ButtonControl helpingLineAccentButton = new ButtonControl("Color", "Change color");
            helpingLineAccentButton.ButtonClicked += () => UpdateAfterChange(() => circle.Draw.helpingLineAccentPen.Color = helpingLineAccentButton.SetLabelColor());

            TrackBarControl helpingLineAccentWidthTrackBar = new TrackBarControl("Width", 1, 20, 4);
            helpingLineAccentWidthTrackBar.ValueChanged += () => UpdateAfterChange(() => circle.Draw.helpingLineAccentPen.Width = helpingLineAccentWidthTrackBar.Value);

            TrackBarControl helpingLineAccentLengthTrackBar = new TrackBarControl("Length", 1, 10, 5);
            helpingLineAccentLengthTrackBar.ValueChanged += () => UpdateAfterChange(() => circle.Draw.HelpingLineAccentLength = helpingLineAccentLengthTrackBar.Value * 0.05f);

            helpingLineAccentPanel.Controls.AddRange(new Control[] { helpingLineAccentButton, helpingLineAccentWidthTrackBar, helpingLineAccentLengthTrackBar });
            //
            //  Tick Panel
            //
            TableLayoutPanel tickPanel = new TableLayoutPanel();

            ButtonControl tickColorButton = new ButtonControl("Color", "Change color");
            tickColorButton.ButtonClicked += () => UpdateAfterChange(() => circle.Draw.tickPen.Color = tickColorButton.SetLabelColor());

            TrackBarControl tickWidthTrackBar = new TrackBarControl("Width", 1, 20, 4);
            tickWidthTrackBar.ValueChanged += () => UpdateAfterChange(() => circle.Draw.tickPen.Width = tickWidthTrackBar.Value);

            TrackBarControl tickLengthTrackBar = new TrackBarControl("Length", 1, 10, 5);
            tickLengthTrackBar.ValueChanged += () => UpdateAfterChange(() => circle.Draw.tickLength = tickLengthTrackBar.Value * 0.1f);
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

            circle.Start();
        }

        private void Form1_Resize(object sender, EventArgs e)
        {
            circle.Manager.UpdateForScreenChange(circle.panel.ClientRectangle.Width, circle.panel.ClientRectangle.Height);
            circle.panel.Invalidate();
        }

        private Action UpdateAfterChange(Action action)
        {
            action.Invoke();
            circle.panel.Invalidate();
            return null;
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
