using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarsAndTrains.Classes
{
    class PositionVector
    {
        public double x { get; private set; }
        public double y {get; private set;}
        public double length {get; private set;}
        public double normalizedX {get; private set;}
        public double normalizedY {get; private set;}

        public PositionVector(double x, double y, double length)
        {
            this.x = x;
            this.y = y;
            this.length = length;
        }

        private void setNormalized(double normalizedX, double normalizedY)
        {
            this.normalizedX = normalizedX;
            this.normalizedY = normalizedY;
        }
    }
}
