using CarsAndTrains.Classes.Nodes;
using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Media.Imaging;

namespace CarsAndTrains.Classes.Vehicles
{
    public abstract class Vehicle
    {

        public const double VEHICLE_DISTANCE_OFFSET = 40f;
        public const double NODE_DISTANCE_OFFSET = .5f;
        #region public fields
        public bool CanMove { get; set; }
        public bool CanColide { get; set; }
        public bool IsVisible { get; set; }
        public bool IsActive { get; set; }
        public int CounterNodes { get; set; }
        public BitmapImage CurrentGraphics { get; set; }
        public double DeathAfterArivalTime { get; set; }
        public double VehicleSpeed { get; set; }
        public Point ActualPosition { get; set; } = new Point();
        public double WidthGraphics { get; protected set; }
        public double TraveledDistance { get; protected set; }
        public double DistanceToTravel { get; protected set; }
        public int NextVehicleIndex { get; protected set; }
        public double CurrentSpeed
        {
            get => currentSpeed;
            set => LimitSpeed(value);
        }

        protected void LimitSpeed(double value)
        {
            currentSpeed = value;
            if (currentSpeed > VehicleSpeed)
                currentSpeed = VehicleSpeed;
            if (currentSpeed < 0)
                currentSpeed = 0;
        }

        #endregion
        #region private fields
        protected double currentSpeed;
        private PositionVector positionVector;
        #endregion
        #region public constructors
        public Vehicle()
        {
            EnableVehicle();

        }

        public Vehicle(double VehicleSpeed, int CounterNodes, double DeathAfterArivalTime, int NextVehicleIndex) : this()
        {
            this.VehicleSpeed = VehicleSpeed;
            this.CurrentSpeed = VehicleSpeed;
            this.CounterNodes = CounterNodes;
            this.DeathAfterArivalTime = DeathAfterArivalTime;
            this.NextVehicleIndex = NextVehicleIndex;

            this.positionVector = PublicAvaliableReferences.GetNode(CounterNodes).Vector;
            DistanceToTravel = positionVector.Length;
        }

        #endregion


        #region public methods

        public virtual void UpdateVehicle()
        {
            //GetNextNode będzie wysyłać parametr CounterNodes
            Node nextNode = PublicAvaliableReferences.GetNode(CounterNodes);
            if (!CanMove || !nextNode.CanGoThrough)
                return;

            if (CanColide)
                LimitSpeedByVehicleDistance();

            //apply movement
            MoveVehicleForward();

            bool didAriveToNode = (DistanceToTravel - TraveledDistance) <= NODE_DISTANCE_OFFSET;
            if (didAriveToNode)
                UpdateNode();

        }

        private void UpdateNode()
        {
            //reducing count of nodes left
            CounterNodes--;
            Node nextNode = PublicAvaliableReferences.GetNode(CounterNodes);
            if (nextNode is null)
                return;
            /*if (nextNode is TrainTriggerNode node)
                node.TriggerTurnpike();*/

            GetNewGraphic();

            positionVector = nextNode.Vector;
            DistanceToTravel += positionVector.Length;
        }

        public bool Arived() => CounterNodes == 0;
        public void GetNewGraphic() => this.CurrentGraphics = PublicAvaliableReferences.GetNextGraphic(positionVector.NormalizedX, positionVector.NormalizedY);
        #endregion
        #region private methods
        protected void MoveVehicleForward()
        {
            // don't let vehicle pass node
            double _currentSpeed = this.CurrentSpeed;
            if (DistanceToTravel - TraveledDistance < _currentSpeed)
                _currentSpeed -= DistanceToTravel - TraveledDistance;

            //apply to position
            ActualPosition = new Point(
                ActualPosition.X + (_currentSpeed * this.positionVector.NormalizedX),
                ActualPosition.Y + (_currentSpeed * this.positionVector.NormalizedY)
                );

            TraveledDistance += _currentSpeed;
        }

        protected void LimitSpeedByVehicleDistance()
        {
            //if no vehicle exists on path, no need to limit speed
            if (!PublicAvaliableReferences.IsAnyVehicleInFront(this))
                return;

            if (PublicAvaliableReferences.IsCarInTheWay(this))
            {
                this.CurrentSpeed = PublicAvaliableReferences.GetNextVehicleSpeed(this.NextVehicleIndex);
            }
            else
                this.CurrentSpeed = this.VehicleSpeed;
        }

        public void DisableVehicle()
        {
            IsActive = false;
            IsVisible = false;
            CanMove = false;
            CanColide = false;
        }
        public void EnableVehicle()
        {
            IsActive = false;
            IsVisible = false;
            CanMove = false;
            CanColide = false;
        }
        #endregion
    }
}
