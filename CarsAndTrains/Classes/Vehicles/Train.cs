using System;

namespace CarsAndTrains.Classes.Vehicles
{
    public class Train : Vehicle
    {
        public Train()
        {
            EnableVehicle();
        }

        public Train(double VehicleSpeed, int CounterNodes, int NextVehicleIndex) : this()
        {
            this.VehicleSpeed = VehicleSpeed;
            this.CurrentSpeed = VehicleSpeed;
            this.CounterNodes = CounterNodes;
            this.NextVehicleIndex = NextVehicleIndex;
        }

        public override void UpdateVehicle()
        {
            base.UpdateVehicle();


            if (CounterNodes == 0)
                DisableVehicle();
        }
    }
}
