using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clock
{
    class Ticks
    {
        private List<ITick> TickInstances;
        private int totalCount;

        public Ticks()
        {
            TickInstances = new List<ITick>();
            totalCount = 0;
        }

        public void Update(PointF center, List<PointF> tickPositions)
        {
            foreach (PointF position in tickPositions)
            {

            }
        }
    }
}
