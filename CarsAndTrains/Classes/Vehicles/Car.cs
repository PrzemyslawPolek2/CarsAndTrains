﻿namespace CarsAndTrains.Classes.Vehicles
{
    public class Car : Vehicle
    {
        public static int CarID { get; private set; } = 0;
        public int carID = 0;

        public const double CAR_HEIGHT = 20.0f;
        public const double CAR_WIDTH = 30.0f;
        public Car()
        {
            carID = CarID;
            CarID++;
            EnableVehicle();
        }

        public Car(double VehicleSpeed, int CounterNodes, double DeathAfterArivalTime, int NextVehicleIndex) : base(VehicleSpeed,
                                                                                          CounterNodes,
                                                                                          DeathAfterArivalTime, NextVehicleIndex)
        {

        }
        public override void UpdateVehicle()
        {
            base.UpdateVehicle();

            if (CounterNodes == 0)
                DisableVehicle();
        }

    }
}
