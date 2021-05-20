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

        protected Point normalizedPosition;

        public Point getDirection()
        {
            return normalizedPosition;
        }

        private void calculateVector(Node nextNode)
        {
            double xVector, yVector, finalVector;
 

            xVector = Math.Pow((nextNode.position.X - this.position.X), 2);
            yVector = Math.Pow((nextNode.position.Y - this.position.Y), 2);
            finalVector = Math.Sqrt(xVector + yVector);

            normalizedPosition.X = xVector / finalVector;
            normalizedPosition.Y = yVector / finalVector;

        }

        public Point getNodePosition()
        {
            return position;
        }
    }
}
