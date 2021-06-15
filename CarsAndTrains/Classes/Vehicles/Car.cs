using CarsAndTrains.Classes.Nodes;
using System;
using System.Diagnostics;
using System.Windows;

namespace CarsAndTrains.Classes.Vehicles
{
    public class Car : Vehicle
    {
        public static int CarID { get; private set; } = 0;
        public bool IsReinacarnated { get; internal set; } = false;
        public bool IgnoreNodeLimit { get; private set; }

        public int carID = 0;
        public static double FullTravelDistance = 0.0f;

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
            if (CounterNodes == 3)
            {
                DisableVehicle();
                IsActive = false;
                IsReinacarnated = true;
            }
            //get next node
            Node nextNode = GetNextNode(CounterNodes - 1);
            if (nextNode == null)
            {
                return;
            }

            if (!CanMove || (!nextNode.CanGoThrough && !IgnoreNodeLimit))
            {
                CurrentSpeed = 0.0f;
                return;
            }

            this.CurrentSpeed = this.VehicleSpeed;

            if (CanColide)
                LimitSpeedByVehicleDistance();
            //apply movement
            MoveVehicleForward();

            bool didAriveToNode = (DistanceToTravel - TraveledDistance) <= NODE_DISTANCE_OFFSET;
            if (didAriveToNode)
            {
                UpdateNode();
                IgnoreNodeLimit = false;
            }
        }

        public static string Header()
        {
            return "\t| Active \t| ID \t| Speed | CanMove | Colide | Visible | Vehicle | Ignore | Nodes | Distance ";
        }
        public override string ToString()
        {
            string isActive = IsActive ? "Active" : "NotActive";
            if (IsActive)
                return $"{isActive}\t {carID:0}|{NextVehicleIndex:0}\t{CurrentSpeed:0.00}\t{CanMove}\t{CanColide}\t{IsVisible}\t{IsVehicleInFront}\t{IgnoreNodeLimit}\t{CounterNodes}\t{TraveledDistance / FullTravelDistance * 100:00}% {RelatvieTraveledDistance / RelativeDistanceToTravel * 100:00}% [{TraveledDistance:0000}->{DistanceToTravel:0000}] / {FullTravelDistance:0000}";
            else
                return $"{isActive}\t {carID:0}|{NextVehicleIndex:0}";
        }

        protected override Node GetNextNode(int nodeCount) => PublicAvaliableReferences.GetCarNode(nodeCount);

        internal void ResetPosition()
        {
            this.TraveledDistance = 0;
            this.CurrentSpeed = VehicleSpeed;
            this.positionVector = GetNextNode(CounterNodes).Vector;
            this.DistanceToTravel = positionVector.Length;
            this.ActualPosition = new Point(this.positionVector.X, this.positionVector.Y);
        }

        internal void IgnoreCanGoThrough()
        {
            IgnoreNodeLimit = true;
        }
    }
}
