using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Clock
{
    class CircleInstance: BaseCircle, ICircleUpdate
    {
        public List<PointF> tickPositions;
        public List<float> tickDegrees;
        public List<double> realTickDegrees;
        public RectangleF ellipse;

        public CircleInstance(int indent, int ticks, int screenWidth, int screenHeight, float rotationX, float rotationY)
            :base(indent, ticks, screenWidth, screenHeight, rotationX, rotationY)
        {
            tickPositions = new List<PointF>();
            tickDegrees = new List<float>();
            realTickDegrees = new List<double>();

            CalculateTickDegrees();
            CalculateAllTickIntersections();
            CalculateEllipse();
        }

        public void UpdateForIndentChange(int indent)
        {
            Indent = indent;
            CalculateAllTickIntersections();
            CalculateEllipse();
        }

        public void UpdateForScreenChange(int screenWidth, int screenHeight)
        {
            ScreenWidth = screenWidth;
            ScreenHeight = screenHeight;
            centerPoint = new Point(screenWidth / 2, screenHeight / 2);
            CalculateTickDegrees();
            CalculateAllTickIntersections();
            CalculateEllipse();
        }

        public void UpdateForTicksChange(int ticks)
        {
            Ticks = ticks;
            CalculateTickDegrees();
            CalculateAllTickIntersections();
        }

        public void UpdateForRotationChange(float x, float y)
        {
            RotationX = x;
            RotationY = y;
            CalculateTickDegrees();
            CalculateAllTickIntersections();
            CalculateEllipse();
        }

        private void CalculateEllipse()
        {
            // int xPosition = (int)(width / 2 * (Math.Cos(Math.PI) * Math.Abs(rotationX)) + centerPoint.X);
            // int yPosition = (int)(height / 2 * (Math.Sin(-90 * Math.PI / 180) * Math.Abs(rotationY)) + centerPoint.Y);
            float xPosition = (float)(halfWidth * -1 * Math.Abs(RotationX) + centerPoint.X);
            float yPosition = (float)(halfHeight * (Math.Sin(-90 * DEGREE) * Math.Abs(RotationY)) + centerPoint.Y);
            ellipse = new RectangleF(xPosition, 
                                     yPosition,
                                     Math.Abs(centerPoint.X - xPosition) * 2, 
                                     Math.Abs(centerPoint.Y - yPosition) * 2);
        }

        private void CalculateAllTickIntersections()
        {
            tickPositions.Clear();
            realTickDegrees.Clear();

            for (int i = 0; i < Ticks; i++)
            {
                tickPositions.Add(CalculateInitialIntersectionPoint(i));
            }

            for (int i = 0; i < Ticks; i++)
            {
                PointF intersection = GetPoint(i);
                double rad = Math.Atan2(intersection.Y - centerPoint.Y, intersection.X - centerPoint.X);
                realTickDegrees.Add(rad * 180 / Math.PI - 90);
            }
        }

        private double GetDistance(PointF a, PointF b)
        {
            return Math.Sqrt(Math.Pow(b.X - a.X, 2) + Math.Pow(b.Y - a.Y, 2));
        }

        private PointF CalculateInitialIntersectionPoint(int tick)
        {
            // float degrees = ConvertTickToDegrees(tick);
            // return new PointF((float)(width / 2 * Math.Cos(degree * Math.PI / 180) * rotationX),
            //                   (float)(height / 2 * Math.Sin(degree * Math.PI / 180) * rotationY));
            return new PointF((float)(halfWidth * Math.Cos(tickDegrees[tick] * DEGREE) * RotationX),
                              (float)(halfHeight * Math.Sin(tickDegrees[tick] * DEGREE) * RotationY));
        }

        public PointF GetPoint(int tick, float percentageLength = 1f)
        {
            return new PointF(tickPositions[tick].X * percentageLength + centerPoint.X,
                              tickPositions[tick].Y * percentageLength + centerPoint.Y);
        }

        private void CalculateTickDegrees()
        {
            tickDegrees.Clear();

            for (int i = 0; i < Ticks; i++)
            {
                tickDegrees.Add(ConvertTickToDegrees(i));
            }
        }

        private float ConvertTickToDegrees(int tick) =>
            tick * degreesEveryTick - 90;
    }
}
