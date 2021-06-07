namespace CarsAndTrains.Classes.Vehicles
{
    public class Car : Vehicle
    {
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
