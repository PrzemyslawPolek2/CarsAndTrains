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
    public class Car : Vehicle
    {
        public Car()
        {


        }

        public Car(int VehicleSpeed, int CounterNodes, float DeathAfterArivalTime) : base(VehicleSpeed,
                                                                                          CounterNodes,
                                                                                          DeathAfterArivalTime)
        {

        }
        public override void UpdateVehicle()
        {

        }

    }
}
