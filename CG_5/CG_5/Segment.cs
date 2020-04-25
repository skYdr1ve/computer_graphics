using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CG_5
{
    public class Segment
    {
        public int X1 { get; set; }
        public int Y1 { get; set; }
        public int X2 { get; set; }
        public int Y2 { get; set; }

        public Segment(int x1, int y1, int x2, int y2)
        {
            X1 = x1;
            Y1 = y1;
            X2 = x2;
            Y2 = y2;
        }

        public Segment(float x1, float y1, float x2, float y2)
        {
            X1 = (int)x1;
            Y1 = (int)y1;
            X2 = (int)x2;
            Y2 = (int)y2;
        }

    }
}
