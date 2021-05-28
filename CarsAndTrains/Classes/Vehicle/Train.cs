using CarsAndTrains;
using CarsAndTrains.Classes.Vehicles;
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
        public Train(int VehicleSpeed, int CounterNodes, float DeathAfterArivalTime, Point ActualPosition) : base(VehicleSpeed,
                                                                                                                CounterNodes,
                                                                                                                DeathAfterArivalTime,
                                                                                                                ActualPosition)
        {

        }

        public override void UpdateVehicle()
        {

        }
    }
}
