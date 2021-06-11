using System;
using System.Windows;

namespace CarsAndTrains.Classes.Nodes
{
    public class Node
    {
        #region Fields
        public bool CanGoThrough { get; set; }
        protected Point Position { get; set; }
        private bool isActive;
        #endregion

        #region Getter_Setter
        public bool IsActive
        {
            get { return isActive; }
            set { isActive = value; }
        }

        public PositionVector Vector { get; private set; }
        #endregion

        #region Constructors
        public Node()
        {
            CanGoThrough = true;
        }

        public Node(Point position) : this()
        {
            this.Position = position;
        }

        public Point GetNodePosition()
        {
            return this.Position;
        }
        #endregion

        #region Methods
        public void CalculateVector(Node nextNode) //funkja obliczająca długość między dwoma node'ami na mapie - obecnym oraz następnym
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

            xVector = nextNode.Position.X - this.Position.X;
            yVector = nextNode.Position.Y - this.Position.Y;

            Point normalizedPosition = new Point
            {
                X = xVector / finalVector,
                Y = yVector / finalVector
            };
            Vector.SetNormalized(normalizedPosition.X, normalizedPosition.Y);
        }
        #endregion

    }
}
