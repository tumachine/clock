using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clock
{
    interface ITick
    {
        void Draw(Graphics g, PointF center, PointF end);
    }
}
