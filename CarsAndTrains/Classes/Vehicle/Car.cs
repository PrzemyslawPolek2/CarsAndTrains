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

        public Car(double VehicleSpeed, int CounterNodes, double DeathAfterArivalTime) : base(VehicleSpeed,
                                                                                          CounterNodes,
                                                                                          DeathAfterArivalTime)
        {

        }
        public override void UpdateVehicle()
        {
            throw new NotImplementedException();
        }

    }
}
