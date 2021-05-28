﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarsAndTrains.Classes
{
    class PositionVector
    {
        #region Fields
        public double X { get; private set; }
        public double Y {get; private set;}
        public double Length {get; private set;}
        public double NormalizedX {get; private set;}
        public double NormalizedY {get; private set;}

        #endregion

        #region Getter_Setter
        public PositionVector(double x, double y, double length)
        {
            this.X = x;
            this.Y = y;
            this.Length = length;
        }



        public void SetNormalized(double normalizedX, double normalizedY)
        {
            this.NormalizedX = normalizedX;
            this.NormalizedY = normalizedY;
        }
        #endregion
    }
}
