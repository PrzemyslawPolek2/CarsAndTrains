using CarsAndTrains.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace CarsAndTrains.Classes.Node
{
    class Node
    {

        public bool CanGoThrough { get; set; }
        protected Point Position { get; set; }
        private bool isActive;

        public bool IsActive
        {
            get { return isActive; }
            set { isActive = value; }
        }

        public PositionVector Vector { get; private set; }

        public Node()
        {

        }

        public Node(Point position)
        {
            this.Position = position;
        }

        public Point GetNodePosition()
        {
            return Position;
        }

        private void CalculateVector(Node nextNode) //funkja obliczająca długość między dwoma node'ami na mapie - obecnym oraz następnym
        {
            double xVector, yVector, finalVector;

            //Długość wektora jest obliczana ze wzoru |AB| = PIERWIASTEK[(Xb - Xa)^2 + (Yb - Ya)^2]
            //gdzie B oznacza Node wysłany jako parametr funkcji, zaś A - this.Node
            //finalVector jest zmienną przechowującą długość wektora pomiędzy punktami A i B
            //Wykorzystujemy również znormalizowane długości w celu obliczenia stosunku przesunięcia między node'ami w płaszczyźnie XY

            xVector = Math.Pow((nextNode.Position.X - this.Position.X), 2);
            yVector = Math.Pow((nextNode.Position.Y - this.Position.Y), 2);
            finalVector = Math.Sqrt(xVector + yVector);

            Vector = new PositionVector(Position.X, Position.Y, finalVector);

            Point normalizedPosition = new Point
            {
                X = xVector / finalVector,
                Y = yVector / finalVector
            };
            Vector.SetNormalized(normalizedPosition.X, normalizedPosition.Y);
        }


    }
}
