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
        public bool IsColiding { get; set; }
        public bool IsVisible { get; set; }
        public bool Arrived { get; set; }
        public int DeathAfterArivalTime { get; set; } //add Rand()
        private int VehicleSpeed { get; set; }
        private int CurrentSpeed { get; set; }
        private Node PreviousNode { get; set; }
        private Image CurrentGraphics { get; set; }
        private Image WidthGraphics { get; set; }


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
