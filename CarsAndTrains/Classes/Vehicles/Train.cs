using CarsAndTrains.Classes.Nodes;
using System;
using System.Diagnostics;
using System.Windows;

namespace CarsAndTrains.Classes.Vehicles
{
    public class Train : Vehicle
    {
        #region Constants

        public const double TRAIN_HEIGHT = 60.0f;
        public const double TRAIN_WIDTH = 100.0f;

        #endregion

        #region Constructors
        public Train(double VehicleSpeed, int CounterNodes, int NextVehicleIndex, double DeathAfterArivalTime)
        {
            this.VehicleSpeed = VehicleSpeed;
            this.CurrentSpeed = VehicleSpeed;
            this.DeathTime = DeathAfterArivalTime;
            this.NodesLeftToTravel = CounterNodes;
            this.NextVehicleIndex = NextVehicleIndex;
            this.IsActive = true;
            UpdatePositionVector(CounterNodes);
            DistanceToTravel = positionVector.Length;
            ResetPosition();
        }
        #endregion

        #region Updates
        public void UpdatePositionVector(int CounterNodes)
        {
            positionVector = PublicAvaliableReferences.GetTrainNode(CounterNodes).Vector;
        }

        protected override void UpdateNode()
        {
            //reducing count of nodes left
            Node nextNode = GetNextNode(NodesLeftToTravel - 1);
            if (nextNode is null)
                return;
            if (!nextNode.CanGoTo)
                return;

            if (nextNode is TrainTriggerNode triggerNode)
                triggerNode.TriggerTurnpike();

            GetNewGraphic();
            NodesLeftToTravel--;

            positionVector = nextNode.Vector;
            DistanceToTravel += positionVector.Length;
        }
        #endregion

        #region Sets
        /// <summary>
        /// Sets Starting Position of a train to the first Node
        /// </summary>
        public void ResetPosition()
        {
            this.ActualPosition = PublicAvaliableReferences.GetTrainNode(counterNodes - 1).GetNodePosition();
        }
        protected override void SetCounterNodes(int value)
        {
            this.counterNodes = value;
        }
        #endregion

        #region Gets
        public override void GetNewGraphic() => this.CurrentGraphics = PublicAvaliableReferences.GetNextTrainGraphic(positionVector.NormalizedX, positionVector.NormalizedY);

        protected override Node GetNextNode(int index)
        {
            Node nextNode = PublicAvaliableReferences.GetTrainNode(index);
            if (nextNode == null)
                IsActive = false;
            return nextNode;
        }
        #endregion

        public override string ToString()
        {
            return $"{IsActive} {CurrentSpeed:#.##} {CanMove} {CanColide} {IsVisible} {NodesLeftToTravel}";
        }

    }
}
