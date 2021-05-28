using CarsAndTrains;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace CarsAndTrains.Classes.Vehicle
{
    public class Train : Vehicle
    {
        public Train()
        {

        }

        public Train(int VehicleSpeed, int CounterNodes) //Train
        {
            this.VehicleSpeed = VehicleSpeed;
            this.CounterNodes = CounterNodes;

            CanMove = true;
            CanColiding = false;
            IsVisible = false;
            IsActive = true;
        }

        public override void UpdateVehicle()
        {
            
        }
    }
}
