using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clock
{
    class CirclesManager: ICircleUpdate
    {
        public bool rotating;
        public CircleInstance circleInstance;
        public CircleRotatingInstances circleRotatingInstances;

        public ICircleUpdate UpdatingCircle {
            get
            {
                if (rotating)
                    return circleRotatingInstances;
                return circleInstance;
            }
        }

        public CircleInstance NextInstance
        {
            get => rotating == true ? circleRotatingInstances.GetNext() : circleInstance;
        }

        public CirclesManager(int indent, int ticks, int screenWidth, int screenHeight, float rotationX, float rotationY)
        {
            circleInstance = new CircleInstance(indent, ticks, screenWidth, screenHeight, rotationX, rotationY);
            UpdateForScreenChange(screenWidth, screenHeight);
            rotating = false;
        }

        public void StartRotating(bool byXAxis)
        {
            rotating = true;
            circleRotatingInstances = new CircleRotatingInstances(circleInstance);
            circleRotatingInstances.GenerateRotations(byXAxis);
        }

        public void StopRotating()
        {
            circleInstance = circleRotatingInstances.GetNext();
            rotating = false;
        }

        public void UpdateForScreenChange(int screenWidth, int screenHeight) =>
            UpdatingCircle.UpdateForScreenChange(screenWidth, screenHeight);

        public void UpdateForTicksChange(int ticks) =>
            UpdatingCircle.UpdateForTicksChange(ticks);

        public void UpdateForIndentChange(int indent) =>
            UpdatingCircle.UpdateForIndentChange(indent);

        public void UpdateForRotationChange(float x, float y) =>
            UpdatingCircle.UpdateForRotationChange(x, y);
    }
}
