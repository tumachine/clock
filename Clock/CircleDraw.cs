using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Clock
{
    class CircleDraw
    {
        // Ellipse - Done
        public Pen ellipsePen = new Pen(Color.Black, 5);

        // Number Font - Done
        public Font numberFont = new Font("Arial", 8);
        public Font numberAccentFont = new Font("Arial", 16);

        public SolidBrush numberBrush = new SolidBrush(Color.Black);
        public SolidBrush numberAccentBrush = new SolidBrush(Color.Black);

        public StringFormat numberStringFormat = new StringFormat();

        public float numberDistance = 1.15f;
        public float numberAccentDistance = 1.15f;

        // Helping Line - Done
        public Pen helpingLinePen = new Pen(Color.Black, 5);
        private float helpingLineStartLen = 0.95f;
        private float helpingLineEndLen = 1.05f;
        public float HelpingLineLength
        {
            get => helpingLineStartLen - helpingLineEndLen;
            set
            {
                helpingLineStartLen = 1 - value;
                helpingLineEndLen = 1 + value;
            }
        }

        // Helping Line Accent - Done
        public Pen helpingLineAccentPen = new Pen(Color.Black, 5);
        private float helpingLineAccentStartLen = 0.9f;
        private float helpingLineAccentEndLen = 1.1f;
        public float HelpingLineAccentLength
        {
            get => helpingLineAccentEndLen - helpingLineAccentStartLen;
            set
            {
                helpingLineAccentStartLen = 1 - value;
                helpingLineAccentEndLen = 1 + value;
            }
        }

        // Tick - Done
        public Pen tickPen = new Pen(Color.Black, 5);
        public float tickLength = 0.8f;

        // Other - Done
        public int everyNumber = 5;

        private CirclesManager circlesManager;
        public CircleInstance CC;

        // PerformanceCounter performanceCounter = new PerformanceCounter("Processor", "% Processor Time", "_Total");

        public CircleDraw(CirclesManager circlesManager)
        {
            this.circlesManager = circlesManager;
        }
        
        public void Draw(Graphics g, int tick)
        {
            CC = circlesManager.NextInstance;
            // CC.tickPositions
            DrawEllipse(g);
            DrawHelpingLines(g);
            DrawNumbers(g);
            DrawTick(g, tick);
        }

        private void DrawEllipse(Graphics g)
        {
            // g.DrawEllipse(ellipsePen, CC.ellipse.X, CC.ellipse.Y, Math.Abs(CC.centerPoint.X - CC.ellipse.X) * 2, Math.Abs(CC.centerPoint.Y - CC.ellipse.Y) * 2);
            g.DrawEllipse(ellipsePen, CC.ellipse.X, CC.ellipse.Y, CC.ellipse.Width, CC.ellipse.Height);
        }

        private void DrawNumbers(Graphics g)
        {
            for (int i = 0; i < CC.Ticks; i++)
            {
                if (i % everyNumber == 0)
                    DrawCenteredNumberInRectangle(g, CC.GetPoint(i, numberAccentDistance), numberAccentFont, numberAccentBrush, i);
                else
                    DrawCenteredNumberInRectangle(g, CC.GetPoint(i, numberDistance), numberFont, numberBrush, i);
            }
        }

        private void DrawCenteredNumberInRectangle(Graphics g, PointF point, Font font, Brush brush, int number)
        {
            int rectWidth;
            int rectHeight;

            int fontSize = (int)font.Size;

            if (number < 10)
                rectWidth = fontSize;
            else
                rectWidth = fontSize * 3;

            rectHeight = fontSize + fontSize / 2;

            RectangleF rect = new RectangleF(point.X - rectWidth / 2, point.Y - rectHeight / 2, rectWidth, rectHeight);

            //g.DrawRectangle(pen, rectangle);
            g.DrawString(number.ToString(), font, brush, rect, numberStringFormat);
        }

        private void DrawTick(Graphics g, int tick)
        {
            PointF tickEnd = CC.GetPoint(tick, tickLength);
            double distance = GetDistance(CC.centerPoint, tickEnd);
            g.DrawLine(tickPen, CC.centerPoint, tickEnd);

            g.TranslateTransform(CC.centerPoint.X, CC.centerPoint.Y);
            g.RotateTransform((float)CC.realTickDegrees[tick]);
            g.TranslateTransform(-CC.centerPoint.X, -CC.centerPoint.Y);
            // g.DrawRectangle(new Pen(Color.Red), CC.centerPoint.X, CC.centerPoint.Y, tickLength, (float)distance);

            // float performanceLength = (float)distance / 100 * performanceCounter.NextValue();
            // LinearGradientBrush brush = new LinearGradientBrush(CC.centerPoint, new PointF(CC.centerPoint.X, CC.centerPoint.Y + (float)distance), Color.Green, Color.Red);
            // RectangleF rectangle = new RectangleF(CC.centerPoint.X, CC.centerPoint.Y, 20, performanceLength);
            // g.FillRectangle(brush, rectangle);
            
            g.ResetTransform();
        }

        private double GetDistance(PointF a, PointF b)
        {
            return Math.Sqrt(Math.Pow(b.X - a.X, 2) + Math.Pow(b.Y - a.Y, 2));
        }

        private void DrawHelpingLines(Graphics g)
        {
            for (int i = 0; i < CC.Ticks; i++)
            {
                PointF startPoint, endPoint;
                if (i % everyNumber == 0)
                {
                    startPoint = CC.GetPoint(i, helpingLineAccentStartLen);
                    endPoint = CC.GetPoint(i, helpingLineAccentEndLen);
                    g.DrawLine(helpingLineAccentPen, startPoint, endPoint);
                }
                else
                {
                    startPoint = CC.GetPoint(i, helpingLineStartLen);
                    endPoint = CC.GetPoint(i, helpingLineEndLen);
                    g.DrawLine(helpingLinePen, startPoint, endPoint);
                }
            }
        }
    }
}
