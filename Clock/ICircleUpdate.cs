using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clock
{
    interface ICircleUpdate
    {
        void UpdateForIndentChange(int indent);

        void UpdateForScreenChange(int screenWidth, int screenHeight);

        void UpdateForTicksChange(int ticks);

        void UpdateForRotationChange(float x, float y);
    }
}
