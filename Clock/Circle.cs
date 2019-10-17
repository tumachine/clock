using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Clock
{
    class Circle
    {
        private object key = new object();
        public event Action drawAction;

        public const double DEGREE = Math.PI / 180;

        private List<PointF> tickPositions = new List<PointF>();
        private List<float> tickDegreePositions = new List<float>();

        // Ellipse - Done
        public Pen ellipsePen = new Pen(Color.Black, 5);

        // Number Font - Done
        public Font numberFont = new Font("Arial", 8);
        public Font numberAccentFont = new Font("Arial", 16);

        public SolidBrush numberBrush = new SolidBrush(Color.Black);
        public SolidBrush numberAccentBrush = new SolidBrush(Color.Black);

        public StringFormat numberStringFormat = new StringFormat();

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

        private int ticks = 60;
        public int Ticks { get => ticks;
            set
            {
                degreesEveryTick = 360f / value;
                UpdateCoreValues(Width, Height);
                ticks = value;
            }
        }


        // make values changeble
        //  ellipse - color, width
        //  numbers - size, color, accent_size, accent_color
        //  helping lines - color, width, accent_color, accent_width
        //  tick line - length, width, color
        //  tick amount = from 2 to 360
        //  every number = any evenly divisible number

        private int indent;
        public int Indent { get => indent; set { indent = value; doubleIndent = value * 2; } }

        private int doubleIndent;

        private int width;
        public int Width { get => width; set { width = value; halfWidth = width / 2; } }
        private int height;
        public int Height { get => height; set { height = value; halfHeight = height / 2; } }

        private int halfWidth;
        private int halfHeight;

        public float rotationX = 1f;
        public float rotationY = 1f;

        private PointF centerPoint;

        public float degreesEveryTick;

        CancellationTokenSource xCancellationTokenSource;
        CancellationTokenSource yCancellationTokenSource;

        public Circle(int indent, int ticks)
        {
            // this.indent = indent;
            // doubleIndent = indent + indent;
            Indent = indent;

            degreesEveryTick = 360f / ticks;
            Ticks = ticks;

            // instead of cancellation token, I could use a small class - reference bool
            xCancellationTokenSource = new CancellationTokenSource();
            yCancellationTokenSource = new CancellationTokenSource();
        }

        public void UpdateCoreValues(int screenWidth, int screenHeight)
        {
            Width = screenWidth - doubleIndent;
            Height = screenHeight - doubleIndent;
            centerPoint = new Point(screenWidth / 2, screenHeight / 2);
            // await Task.Run(() => CalculateAllTickIntersections());
            CalculateTickDegrees();
            CalculateAllTickIntersections();
        }

        public void XCancelRotation() { xCancellationTokenSource.Cancel(); xCancellationTokenSource = new CancellationTokenSource(); }
        public void YCancelRotation() { yCancellationTokenSource.Cancel(); yCancellationTokenSource = new CancellationTokenSource(); }

        async public void RotateX()
        {
            await Task.Run(() => Rotate(() => rotationX, x => rotationX = x, xCancellationTokenSource.Token));
        }

        async public void RotateY()
        {
            await Task.Run(() => Rotate(() => rotationY, x => rotationY = x, yCancellationTokenSource.Token));
        }

        private async void Rotate(Func<float> getter, Action<float> setter, CancellationToken cancellationToken)
        {
            bool goPos = false;
            while (true)
            {
                if (cancellationToken.IsCancellationRequested)
                    break;

                float rotation = getter();
                lock (key)
                {
                    if (goPos)
                        setter(rotation + 0.01f);
                    else
                        setter(rotation - 0.01f);

                    if (rotation <= -1f)
                        goPos = true;
                    else if (rotation >= 1f)
                        goPos = false;
                }

                drawAction?.Invoke();
                await Task.Delay(10);
            }
        }

        public void UpdateRotation()
        {
            CalculateTickDegrees();
            CalculateAllTickIntersections();
        }

        public void DrawBackground(Graphics g)
        {
            DrawEllipse(g);
            DrawHelpingLines(g);
            DrawNumbers(g);
        }

        public void DrawFront(Graphics g, int tick)
        {
            DrawTick(g, tick);
        }

        public void DrawEllipse(Graphics g)
        {
            // int xPosition = (int)(width / 2 * (Math.Cos(Math.PI) * Math.Abs(rotationX)) + centerPoint.X);
            int xPosition = (int)(halfWidth * -1 * Math.Abs(rotationX) + centerPoint.X);
            // int yPosition = (int)(height / 2 * (Math.Sin(-90 * Math.PI / 180) * Math.Abs(rotationY)) + centerPoint.Y);
            int yPosition = (int)(halfHeight * (Math.Sin(-90 * DEGREE) * Math.Abs(rotationY)) + centerPoint.Y);
            g.DrawEllipse(ellipsePen, xPosition, yPosition, Math.Abs(centerPoint.X - xPosition) * 2, Math.Abs(centerPoint.Y - yPosition) * 2);
        }

        public void CalculateAllTickIntersections()
        {
            tickPositions.Clear();

            for (int i = 0; i < Ticks; i++)
                tickPositions.Add(CalculateInitialIntersectionPoint(i));
        }

        public void DrawNumbers(Graphics g)
        {
            for (int i = 0; i < Ticks; i++)
            {
                PointF textPoint = GetPoint(i, 1.15f);
                
                if (i % everyNumber == 0)
                    DrawCenteredNumberInRectangle(g, textPoint, numberAccentFont, numberAccentBrush, i);
                else
                    DrawCenteredNumberInRectangle(g, textPoint, numberFont, numberBrush, i);
            }
        }

        public void DrawCenteredNumberInRectangle(Graphics g, PointF point, Font font, Brush brush, int number)
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
        
        public void DrawTick(Graphics g, int tick) =>
            g.DrawLine(tickPen, centerPoint, GetPoint(tick, tickLength));

        public void DrawHelpingLines(Graphics g)
        {
            for (int i = 0; i < Ticks; i++)
            {
                PointF startPoint, endPoint;
                if (i % everyNumber == 0)
                {
                    startPoint = GetPoint(i, helpingLineAccentStartLen);
                    endPoint = GetPoint(i, helpingLineAccentEndLen);
                    g.DrawLine(helpingLineAccentPen, startPoint, endPoint);
                }
                else
                {
                    startPoint = GetPoint(i, helpingLineStartLen);
                    endPoint = GetPoint(i, helpingLineEndLen);
                    g.DrawLine(helpingLinePen, startPoint, endPoint);
                }
            }
        }
        

        public PointF CalculateInitialIntersectionPoint(int tick)
        {
            // float degrees = ConvertTickToDegrees(tick);
            // return new PointF((float)(width / 2 * Math.Cos(degree * Math.PI / 180) * rotationX),
            //                   (float)(height / 2 * Math.Sin(degree * Math.PI / 180) * rotationY));
            return new PointF((float)(halfWidth * Math.Cos(tickDegreePositions[tick] * DEGREE) * rotationX),
                              (float)(halfHeight * Math.Sin(tickDegreePositions[tick] * DEGREE) * rotationY));
        }

        public PointF GetPoint(int tick, float percentageLength=1f)
        {
            return new PointF(tickPositions[tick].X * percentageLength + centerPoint.X,
                              tickPositions[tick].Y * percentageLength + centerPoint.Y);
        }

        public void CalculateTickDegrees()
        {
            tickDegreePositions.Clear();

            for (int i = 0; i < Ticks; i++)
            {
                tickDegreePositions.Add(ConvertTickToDegrees(i));
            }
        }

        public float ConvertTickToDegrees(int tick) =>
            tick * degreesEveryTick - 90;


    }
}
