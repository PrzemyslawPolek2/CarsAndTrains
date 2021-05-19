using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarsAndTrains
{
    class Node
    {

        public Boolean canGoThrough { get; set; }
        protected Node positionX { get; set; }
        protected Node positionY { get; set; }

        public Node()
        {

        }

        public Node(Node positionX, Node positionY)
        {
            positionX = this.positionX;
            positionY = this.positionY;
        }
        public void setDirection(Node nextNode)
        {

        }

        private void calculateVector()
        {

        }

        public void getNodePosition()
        {

        }
    }
}
