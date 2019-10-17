using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clock
{
    class CircleRotatingInstances: ICircleUpdate
    {
        public List<CircleInstance> Circles = new List<CircleInstance>();

        // 0.03 rotation change for ~66.66 fps
        public static float rotationRate = 0.03f;

        private int counter = 0;

        private bool goingUp = true;

        private CircleInstance initialCircle;

        //
        // public CircleRotatingInstances(int indent, int ticks, int screenWidth, int screenHeight, float rotationX, float rotationY)
        //     : base(indent, ticks, screenWidth, screenHeight, rotationX, rotationY)
        // {
        //     
        // }

        public CircleRotatingInstances(CircleInstance circleInstance)
        {
            initialCircle = circleInstance;
        }

        public CircleInstance GetNext()
        {
            if (counter + 1 >= Circles.Count)
                goingUp = false;
            else if (counter - 1 <= 0)
                goingUp = true;

            if (goingUp)
                counter++;
            else
                counter--;
            return Circles[counter];
        }

        public void GenerateRotations(bool byXAxis)
        {
            Circles.Clear();

            float start = 1;
            while (start >= -1)
            {
                CircleInstance newCircle;
                start -= rotationRate;
                if (byXAxis)
                    newCircle = new CircleInstance(
                        initialCircle.Indent, initialCircle.Ticks, 
                        initialCircle.ScreenWidth, initialCircle.ScreenHeight, 
                        start, initialCircle.RotationY);
                else
                    newCircle = new CircleInstance(
                        initialCircle.Indent, initialCircle.Ticks,
                        initialCircle.ScreenWidth, initialCircle.ScreenHeight,
                        initialCircle.RotationX, start);
                Circles.Add(newCircle);
            }
        }

        public void UpdateForIndentChange(int indent)
        {
            foreach (CircleInstance circle in Circles)
                circle.UpdateForIndentChange(indent);
        }

        public void UpdateForScreenChange(int screenWidth, int screenHeight)
        {
            foreach (CircleInstance circle in Circles)
                circle.UpdateForScreenChange(screenWidth, screenHeight);
        }

        public void UpdateForTicksChange(int ticks)
        {
            foreach (CircleInstance circle in Circles)
                circle.UpdateForTicksChange(ticks);
        }

        public void UpdateForRotationChange(float x, float y)
        {
            foreach (CircleInstance circle in Circles)
                circle.UpdateForRotationChange(x, y);
        }
    }
}
