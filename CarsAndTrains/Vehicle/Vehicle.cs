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

        public Vehicle(Boolean isColding, Boolean isVisible, int deathAfterArivalTime, int vehicleSpeed, int currentSpeed, int[] rememberedNodes, 
            List<int> nodeList, Image currentGraphics, Image widthGraphics) 
        {
            this.isColiding = isColding;
            this.isVisible = isVisible;
            this.deathAfterArivalTime = deathAfterArivalTime;
            this.vehicleSpeed = vehicleSpeed;
            this.currentSpeed = currentSpeed;
            this.rememberedNodes = rememberedNodes;
            this.nodeList = nodeList;
            this.currentGraphics = currentGraphics;
            this.widthGraphics = widthGraphics;
        
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
