using System;
using System.Diagnostics;
using System.Windows;

namespace CarsAndTrains.Classes.Nodes
{
    public class Node
    {
        #region Fields
        public const double NODE_SIZE = 20;
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
        public void CalculateVector() //funkja obliczająca długość między dwoma node'ami na mapie - obecnym oraz następnym
        {
            CalculateVector(this);
        }

        public void CalculateVector(Node nextNode) //funkja obliczająca długość między dwoma node'ami na mapie - obecnym oraz następnym
        {
            double xDifferenceSquared;
            double yDifferenceSquared; 
            double vectorLength;

            //Długość wektora jest obliczana ze wzoru |AB| = PIERWIASTEK[(Xb - Xa)^2 + (Yb - Ya)^2]
            //gdzie B oznacza Node wysłany jako parametr funkcji, zaś A - this.Node
            //finalVector jest zmienną przechowującą długość wektora pomiędzy punktami A i B
            //Wykorzystujemy również znormalizowane długości w celu obliczenia stosunku przesunięcia między node'ami w płaszczyźnie XY

            xDifferenceSquared = Math.Pow((nextNode.Position.X - this.Position.X), 2);
            yDifferenceSquared = Math.Pow((nextNode.Position.Y - this.Position.Y), 2);

            vectorLength = Math.Sqrt(xDifferenceSquared + yDifferenceSquared);

            Vector = new PositionVector(Position.X, Position.Y, vectorLength);

            double xValue = nextNode.Position.X - this.Position.X;
            double yValue = nextNode.Position.Y - this.Position.Y;

            //it's the same node
            if (vectorLength == 0)
            {
                vectorLength = 1;
            }

            Point magnitudeVector = new Point
            {
                X = xValue / vectorLength,
                Y = yValue / vectorLength
            };
            Vector.SetNormalized(magnitudeVector.X, magnitudeVector.Y);
        }

        public override string ToString()
        {
            return $"\t{Position.X} \t {Position.Y} \t {CanGoThrough} \t {Vector.NormalizedX:#.###} \t {Vector.NormalizedY:#.###}";
        }

        public static string Header()
        {
            return $"\t X \t Y \t CanGoThrough normX\t normY";
        }
        #endregion

    }
}
