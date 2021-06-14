using CarsAndTrains.Classes.Nodes;
using System.Diagnostics;

namespace CarsAndTrains.Classes.Vehicles
{
    public class Car : Vehicle
    {
        public static int CarID { get; private set; } = 0;
        public int carID = 0;

        public const double CAR_HEIGHT = 30.0f;
        public const double CAR_WIDTH = 40.0f;

        public Car(double VehicleSpeed, int CounterNodes, double DeathAfterArivalTime, int NextVehicleIndex) : base(VehicleSpeed,
                                                                                          CounterNodes,
                                                                                          DeathAfterArivalTime, NextVehicleIndex)
        {
            carID = CarID;
            CarID++;
            EnableVehicle();
            this.positionVector = GetNextNode(CounterNodes).Vector;
            DistanceToTravel = positionVector.Length;
        }
        public override void UpdateVehicle()
        {
            if (!IsActive)
                return;

            //get next node
            Node nextNode = GetNextNode(CounterNodes - 1);

            if (nextNode == null)
                return;

            if (!CanMove || !nextNode.CanGoThrough)
            {
                CurrentSpeed = 0.0f;
                return;
            }

            if (CanColide)
                LimitSpeedByVehicleDistance();
            //apply movement
            MoveVehicleForward();

            bool didAriveToNode = (DistanceToTravel - TraveledDistance) <= NODE_DISTANCE_OFFSET;
            if (didAriveToNode)
                UpdateNode();

            if (CounterNodes == 3)
            {
                DisableVehicle();
                IsActive = false;
            }
        }

        public static string Header()
        {
            return "\tActive\tID\tSpeed\tCanMove Colide Visible\tNodes Left";
        }
        public override string ToString()
        {
            string isActive = IsActive ? "Active" : "NotActive";
            return $"{isActive}\t {carID}\t{CurrentSpeed:#.##}\t{CanMove}\t{CanColide}\t{IsVisible}\t{CounterNodes}";
        }

        protected override Node GetNextNode(int nodeCount) => PublicAvaliableReferences.GetCarNode(nodeCount);
    }
}
