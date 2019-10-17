using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clock
{
    abstract class BaseCircle
    {
        protected const double DEGREE = Math.PI / 180;

        private int ticks = 60;
        public int Ticks
        {
            get => ticks;
            set
            {
                degreesEveryTick = 360f / value;
                ticks = value;
            }
        }

        protected float degreesEveryTick;

        private int indent;
        public int Indent
        {
            get => indent;
            set
            {
                indent = value;
                doubleIndent = value * 2;
                Width = screenWidth - doubleIndent;
                Height = screenHeight - doubleIndent;
            }
        }

        private int doubleIndent;

        private int width;
        public int Width { get => width; set { width = value; halfWidth = width / 2; } }
        private int height;
        public int Height { get => height; set { height = value; halfHeight = height / 2; } }

        protected int halfWidth;
        protected int halfHeight;

        public float RotationX { get; set; }
        public float RotationY { get; set; }

        public int ScreenWidth { get => screenWidth; set { screenWidth = value; Width = screenWidth - doubleIndent; } }
        public int ScreenHeight { get => screenHeight; set { screenHeight = value; Height = screenHeight - doubleIndent; } }
        private int screenWidth;
        private int screenHeight;

        public PointF centerPoint;

        public BaseCircle(int indent, int ticks, int screenWidth, int screenHeight, float rotationX, float rotationY)
        {
            Indent = indent;
            Ticks = ticks;

            ScreenWidth = screenWidth;
            ScreenHeight = screenHeight;

            RotationX = rotationX;
            RotationY = rotationY;

            centerPoint = new Point(ScreenWidth / 2, ScreenHeight / 2);
        }
    }
}
