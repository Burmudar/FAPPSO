using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PSOFAP.FAPModel
{
    public class Coordinate
    {
        public Coordinate(float x, float y)
        {
            X = x;
            Y = y;
        }

        public float X { get; set; }
        public float Y { get; set; }
    }
}
