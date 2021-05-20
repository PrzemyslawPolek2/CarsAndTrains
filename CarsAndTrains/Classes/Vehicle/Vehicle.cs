using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace CarsAndTrains
{
    abstract public class Vehicle
    {
        public bool isColiding { get; set; }
        public bool isVisible { get; set; }
        public bool arrived { get; set; }
        public int deathAfterArivalTime { get; set; } //add Rand()
        private int vehicleSpeed { get; set; }
        private int currentSpeed { get; set; }
        private int []rememberedNodes = new int[2]; //dodaj get set
        private List<int> nodeList= new List<int> { };//dodaj get set
        private Image currentGraphics { get; set; }
        private Image widthGraphics { get; set; }


        public Vehicle() { 
        
        }

        virtual public void updateVehicle() 
        {
        
        }

        private void emptiedNodesAction()
        {

        }

        protected void getNewGraphic()
        { 
        
        }
    }
}
