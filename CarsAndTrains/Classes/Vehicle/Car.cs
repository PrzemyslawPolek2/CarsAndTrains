using CarsAndTrains;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

public class Car : Vehicle
{
    public Car()
    {


    }

    public Car(int VehicleSpeed, int CounterNodes, float DeathAfterArivalTime, Point ActualPosition) : base(VehicleSpeed,
                                                                                                            CounterNodes,
                                                                                                            DeathAfterArivalTime,
                                                                                                            ActualPosition)
    {

    }
    public override void UpdateVehicle()
    {

    }
    
}
