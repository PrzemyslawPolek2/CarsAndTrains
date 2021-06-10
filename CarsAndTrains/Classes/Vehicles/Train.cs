using CarsAndTrains.Classes.Nodes;
using System;
using System.Diagnostics;

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

            this.positionVector = PublicAvaliableReferences.GetTrainNode(CounterNodes).Vector;
            DistanceToTravel = positionVector.Length;
        }

        public override void EnableVehicle()
        {
            CanColide = false;
            CanMove = true;
            IsActive = true;
            IsVisible = true;
        }

        public override void UpdateVehicle()
        {
            Node nextNode = PublicAvaliableReferences.GetTrainNode(CounterNodes);
            if (!CanMove || !nextNode.CanGoThrough)
                return;

            if (CanColide)
                LimitSpeedByVehicleDistance();

            //apply movement
            MoveVehicleForward();

            bool didAriveToNode = (DistanceToTravel - TraveledDistance) <= NODE_DISTANCE_OFFSET;
            if (didAriveToNode)
                UpdateNode();

            if (nextNode is TrainTriggerNode triggerNode)
                triggerNode.TriggerTurnpike();

            if (CounterNodes == 0)
                PublicAvaliableReferences.ReverseTrainPath(this);
        }
        protected override void UpdateNode()
        {
            //reducing count of nodes left
            CounterNodes--;
            Node nextNode = PublicAvaliableReferences.GetTrainNode(CounterNodes);
            if (nextNode is null)
                return;

            GetNewGraphic();

            positionVector = nextNode.Vector;
            DistanceToTravel += positionVector.Length;
        }
    }
}
