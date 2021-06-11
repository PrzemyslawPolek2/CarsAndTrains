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

            positionVector = PublicAvaliableReferences.GetTrainNode(CounterNodes).Vector;
            DistanceToTravel = positionVector.Length;
        }

        public override void UpdateVehicle()
        {
            Node nextNode = PublicAvaliableReferences.GetTrainNode(CounterNodes);
            if (!CanMove || !nextNode.CanGoThrough)
                return;

            //apply movement
            Debug.WriteLine($"PRE\t{ActualPosition.X} {ActualPosition.Y}");
            MoveVehicleForward();
            Debug.WriteLine($"POST\t{ActualPosition.X} {ActualPosition.Y}");

            bool didAriveToNode = (DistanceToTravel - TraveledDistance) <= NODE_DISTANCE_OFFSET;
            if (didAriveToNode)
                UpdateNode();

            Debug.WriteLine($"VEC\t{positionVector.NormalizedX} {positionVector.NormalizedY}");
            if (CounterNodes == 0)
            {
                CanMove = false;
                PublicAvaliableReferences.ReverseTrainPath(this);
                Debug.WriteLine("Stopped");
            }
        }
        public override void GetNewGraphic() => this.CurrentGraphics = PublicAvaliableReferences.GetNextTrainGraphic(positionVector.NormalizedX, positionVector.NormalizedY);

        protected override void UpdateNode()
        {
            //reducing count of nodes left
            CounterNodes--;
            Node nextNode = PublicAvaliableReferences.GetTrainNode(CounterNodes);
            if (nextNode is null)
                return;

            if (nextNode is TrainTriggerNode triggerNode)
                triggerNode.TriggerTurnpike();

            GetNewGraphic();

            positionVector = nextNode.Vector;
            DistanceToTravel += positionVector.Length;
        }
    }
}
