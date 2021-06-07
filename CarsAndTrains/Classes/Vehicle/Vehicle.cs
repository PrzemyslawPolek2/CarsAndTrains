using System;
using System.Diagnostics;
using System.Windows;
using CarsAndTrains.Classes;
using CarsAndTrains.Classes.Node;
using CarsAndTrains.Classes.Vehicle;

namespace CarsAndTrains.Classes.Vehicle
{
    public abstract class Vehicle
    {
        private const double OFFSET = .5f;
        #region public fields
        public bool CanMove { get; set; }
        public bool CanColiding { get; set; }
        public bool IsVisible { get; set; }
        public bool IsActive { get; set; }
        public int CounterNodes { get; set; }
        public double DeathAfterArivalTime { get; set; }
        public double VehicleSpeed { get; set; }
        public Point ActualPosition { get; set; } = new Point();
        public double WidthGraphics { get; protected set; }
        public double TraveledDistance { get; protected set; }
        public int NextVehicleIndex { get; protected set; }
        public double CurrentSpeed { 
            get => currentSpeed; 
            set => LimitSpeed(value); }

        protected void LimitSpeed(double value)
        {
            currentSpeed = value;
            if (currentSpeed > VehicleSpeed)
                currentSpeed = VehicleSpeed;
            if (currentSpeed < 0)
                currentSpeed = 0;
        }

        public string CurrentGraphics { get; protected set; }
        #endregion
        #region private fields
        private PositionVector positionVector;
        private double currentSpeed;
        #endregion
        #region public constructors
        public Vehicle()
        {
            CanMove = true;
            CanColiding = true;
            IsVisible = true;
            IsActive = true;
        }

        public Vehicle(double VehicleSpeed, int CounterNodes, double DeathAfterArivalTime, int NextVehicleIndex) : this() //Car
        {
            this.VehicleSpeed = VehicleSpeed;
            this.CounterNodes = CounterNodes;
            this.DeathAfterArivalTime = DeathAfterArivalTime;
            this.NextVehicleIndex = NextVehicleIndex;
            this.positionVector = PublicAvaliableReferences.GetNextNode(CounterNodes).Vector;
        }

        #endregion


        #region public methods
        public virtual void UpdateVehicle()
        {
            Node.Node nextNode = PublicAvaliableReferences.GetNextNode(CounterNodes);//GetNextNode będzie wysyłać parametr CounterNodes

            if (!CanMove | nextNode.CanGoThrough)
                return;

            if (CanColiding)
            {
                SpeedControlBasedOnNextVehicle();
            }
            //przesuwanie vehicle miedzy nodami
            MoveVehicleBeetweenNodes();
            //sprawdzanie czy dojechal do node
            nextNode = DidArriveToNode(nextNode);

            if (CounterNodes == 0)
            {
                EmptiedNodesAction();
            }
        }
        #endregion
        #region protected methods
        protected void GetNewGraphic() => this.CurrentGraphics = PublicAvaliableReferences.GetNextGraphic();

        #endregion
        #region private methods
        private Node.Node DidArriveToNode(Node.Node nextNode)
        {
            if (positionVector.Length - TraveledDistance <= OFFSET)
            {
                CounterNodes = CounterNodes - 1;
                nextNode = PublicAvaliableReferences.GetNextNode(CounterNodes);
                if (nextNode is TrainTriggerNode node)
                    node.TriggerTurnpike();

                positionVector = nextNode.Vector;
                if (nextNode.CanGoThrough)
                    CurrentGraphics = PublicAvaliableReferences.GetNextGraphic();
            }

            return nextNode;
        }
        private void MoveVehicleBeetweenNodes()
        {
            if (positionVector.Length < CurrentSpeed)
                CurrentSpeed = CurrentSpeed - positionVector.Length;
            ActualPosition = new Point(
                ActualPosition.X + (CurrentSpeed * positionVector.NormalizedX), 
                ActualPosition.Y + (CurrentSpeed * positionVector.NormalizedY)
                );

            TraveledDistance = TraveledDistance + CurrentSpeed;
        }

        private void SpeedControlBasedOnNextVehicle()
        {
            if (PublicAvaliableReferences.VehiclesExistOnPath(this))
            {
                if (PublicAvaliableReferences.IsVehicleInTheWay(this))
                    this.CurrentSpeed = PublicAvaliableReferences.GetNextVehicleSpeed(this.NextVehicleIndex);
                else
                    this.CurrentSpeed = this.VehicleSpeed;
            }
        }

        public bool Arived() => CounterNodes == 0;

        private void EmptiedNodesAction()
        {
            IsVisible = false;
            CanMove = false;
            CanColiding = false;
        }
        #endregion
    }
}
