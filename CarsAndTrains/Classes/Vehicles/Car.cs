namespace CarsAndTrains.Classes.Vehicles
{
    public class Car : Vehicle
    {
        public const double CAR_HEIGHT = 20.0f;
        public const double CAR_WIDTH = 30.0f;
        public Car()
        {


        }

        public Car(double VehicleSpeed, int CounterNodes, double DeathAfterArivalTime, int NextVehicleIndex) : base(VehicleSpeed,
                                                                                          CounterNodes,
                                                                                          DeathAfterArivalTime, NextVehicleIndex)
        {

        }
        public override void UpdateVehicle()
        {
            base.UpdateVehicle();
        }

    }
}
