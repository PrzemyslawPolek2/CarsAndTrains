using CarsAndTrains.Classes.Nodes;
using System;
using System.Diagnostics;
using System.Windows;

namespace CarsAndTrains.Classes.Vehicles
{
    public class Train : Vehicle
    {
        public Train(double VehicleSpeed, int CounterNodes, int NextVehicleIndex)
        {
            this.VehicleSpeed = VehicleSpeed;
            this.CurrentSpeed = VehicleSpeed;
            this.CounterNodes = CounterNodes;
            this.NextVehicleIndex = NextVehicleIndex;
            this.IsActive = true;

            positionVector = PublicAvaliableReferences.GetTrainNode(CounterNodes).Vector;
            DistanceToTravel = positionVector.Length;
        }

        public override void UpdateVehicle()
        {
            base.UpdateVehicle(); 
        }
        public override void GetNewGraphic() => this.CurrentGraphics = PublicAvaliableReferences.GetNextTrainGraphic(positionVector.NormalizedX, positionVector.NormalizedY);

        protected override void UpdateNode()
        {
            //reducing count of nodes left
            if (CounterNodes == 3)
            {
                Debug.WriteLine("Reseting a train");
                CanMove = false;
                PublicAvaliableReferences.ReverseTrainPath(this);
            }

            counterNodes--;
            Node nextNode = GetNextNode(CounterNodes);
            if (nextNode is null)
                return;
            Debug.WriteLine(CounterNodes);

            if (nextNode is TrainTriggerNode triggerNode)
                triggerNode.TriggerTurnpike();

            GetNewGraphic();

            positionVector = nextNode.Vector;
            DistanceToTravel += positionVector.Length;
        }
        protected override void SetCounterNodes(int value)
        {
            this.counterNodes = value - 2;
        }

        protected override Node GetNextNode(int index)
        {
            return PublicAvaliableReferences.GetTrainNode(index);
        }
    }
}
