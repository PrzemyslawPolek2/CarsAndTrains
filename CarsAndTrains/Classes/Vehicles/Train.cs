using CarsAndTrains.Classes.Nodes;
using System;
using System.Diagnostics;
using System.Windows;

namespace CarsAndTrains.Classes.Vehicles
{
    public class Train : Vehicle
    {
        public const double TRAIN_HEIGHT = 60.0f;
        public const double TRAIN_WIDTH = 100.0f;
        public Train(double VehicleSpeed, int CounterNodes, int NextVehicleIndex, double DeathAfterArivalTime)
        {
            this.VehicleSpeed = VehicleSpeed;
            this.CurrentSpeed = VehicleSpeed;
            this.DeathAfterArivalTime = DeathAfterArivalTime;
            this.CounterNodes = CounterNodes;
            this.NextVehicleIndex = NextVehicleIndex;
            this.IsActive = true;
            UpdatePositionVector(CounterNodes);
            DistanceToTravel = positionVector.Length;
            ResetPosition();
        }

        public void UpdatePositionVector(int CounterNodes)
        {
            positionVector = PublicAvaliableReferences.GetTrainNode(CounterNodes).Vector;
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
            
        }
        public override void GetNewGraphic() => this.CurrentGraphics = PublicAvaliableReferences.GetNextTrainGraphic(positionVector.NormalizedX, positionVector.NormalizedY);

        protected override void UpdateNode()
        {
            //reducing count of nodes left
            Node nextNode = GetNextNode(CounterNodes - 1);
            if (nextNode is null)
                return;
            if (!nextNode.CanGoThrough)
                return;

            if (nextNode is TrainTriggerNode triggerNode)
                triggerNode.TriggerTurnpike();

            GetNewGraphic();
            CounterNodes--;

            positionVector = nextNode.Vector;
            DistanceToTravel += positionVector.Length;
        }

        protected override void SetCounterNodes(int value)
        {
            this.counterNodes = value;
        }
        public override bool Arived()
        {
            return CounterNodes <= 0;
        }
        protected override Node GetNextNode(int index)
        {
            Node nextNode = PublicAvaliableReferences.GetTrainNode(index);
            if (nextNode == null)
                IsActive = false;
            return nextNode;
        }
        public override string ToString()
        {
            return $"{IsActive} {CurrentSpeed:#.##} {CanMove} {CanColide} {IsVisible} {CounterNodes}";
        }
        internal void ResetPosition()
        {
            this.ActualPosition = PublicAvaliableReferences.GetTrainNode(counterNodes - 1).GetNodePosition();
        }
    }
}
