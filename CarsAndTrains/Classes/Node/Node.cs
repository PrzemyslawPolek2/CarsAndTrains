using CarsAndTrains.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace CarsAndTrains
{
    class Node
    {

        public bool canGoThrough { get; set; }
        protected Point position { get; set; }


        public PositionVector vector { get; private set; }


        private void calculateVector(Node nextNode) //funkja obliczająca długość między dwoma node'ami na mapie - obecnym oraz następnym
        {
            double xVector, yVector, finalVector;

            //Długość wektora jest obliczana ze wzoru |AB| = PIERWIASTEK[(Xb - Xa)^2 + (Yb - Ya)^2]
            //gdzie B oznacza Node wysłany jako parametr funkcji, zaś A - this.Node
            //finalVector jest zmienną przechowującą długość wektora pomiędzy punktami A i B
            //Wykorzystujemy również znormalizowane długości w celu obliczenia stosunku przesunięcia między node'ami w płaszczyźnie XY

            xVector = Math.Pow((nextNode.position.X - this.position.X), 2);
            yVector = Math.Pow((nextNode.position.Y - this.position.Y), 2);
            finalVector = Math.Sqrt(xVector + yVector);

            vector = new PositionVector(position.X, position.Y, finalVector);

            Point normalizedPosition = new Point();
            normalizedPosition.X = xVector / finalVector;
            normalizedPosition.Y = yVector / finalVector;
            vector.setNormalized(normalizedPosition.X, normalizedPosition.Y);
        }

        public Point getNodePosition()
        {
            return position;
        }
    }
}
