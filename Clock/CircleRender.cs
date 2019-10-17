using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Clock
{
    class CircleRender
    {
        public Timer timerTick;
        public Timer timerFramerate;

        public int totalTicks = 0;

        public PictureBox panel;

        public CircleDraw Draw { get; }
        public CirclesManager Manager { get; }

        public CircleRender(PictureBox panel)
        {
            timerTick = new Timer();
            timerTick.Tick += timer_Tick;
            timerTick.Interval = 1000;

            timerFramerate = new Timer();
            timerFramerate.Tick += timer_Framerate;
            timerFramerate.Interval = 16;


            this.panel = panel;
            Manager = new CirclesManager(100, 60, panel.ClientRectangle.Width, panel.ClientRectangle.Height, 1, 1);
            Draw = new CircleDraw(Manager);
            panel.Paint += panel_Paint;
        }

        public void Start()
        {
            timerTick.Start();
        }

        public void StartRotating(bool byXAxis)
        {
            Manager.StartRotating(byXAxis);
            timerFramerate.Start();
        }

        public void StopRotating()
        {
            Manager.StopRotating();
            timerFramerate.Stop();
        }

        private void panel_Paint(object sender, PaintEventArgs e)
        {
            Draw.Draw(e.Graphics, totalTicks);
        }

        private void timer_Tick(object sender, EventArgs e)
        {
            totalTicks++;
            if (totalTicks >= Draw.CC.Ticks)
            {
                totalTicks = 0;
            }
            panel.Invalidate();
        }

        private void timer_Framerate(object sender, EventArgs e)
        {
            panel.Invalidate();
        }

        public void DrawOnPanel()
        {
            panel.Invalidate();
        }
    }
}
