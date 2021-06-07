using System;

namespace CarsAndTrains.Classes.Vehicles
{
    public class Train : Vehicle
    {
        public Train()
        {

        }

        public Train(double VehicleSpeed, int CounterNodes) //Train
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
            throw new NotImplementedException();
        }
    }
}
