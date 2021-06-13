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
        public Point ActualPosition { get; set; }
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
        protected PositionVector positionVector;
        #endregion
        #region Constructors
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

        }

        #endregion


        #region public methods

        public virtual void UpdateVehicle()
        {
            //GetNextNode będzie wysyłać parametr CounterNodes
            Node nextNode = PublicAvaliableReferences.GetCarNode(CounterNodes - 1);
            if (!CanMove || !nextNode.CanGoThrough)
                return;

            if (CanColide)
                LimitSpeedByVehicleDistance();
            Debug.WriteLine($"{CurrentSpeed}");
            //apply movement
            MoveVehicleForward();

            bool didAriveToNode = (DistanceToTravel - TraveledDistance) <= NODE_DISTANCE_OFFSET;
            if (didAriveToNode)
                UpdateNode();

        }

        protected virtual void UpdateNode()
        {
            //reducing count of nodes left
            CounterNodes--;
            Node nextNode = PublicAvaliableReferences.GetCarNode(CounterNodes);
            if (nextNode is null)
                return;

            GetNewGraphic();

            positionVector = nextNode.Vector;
            DistanceToTravel += positionVector.Length;
        }

        public virtual bool Arived() => CounterNodes == 0;
        public virtual void GetNewGraphic() => this.CurrentGraphics = PublicAvaliableReferences.GetNextGraphic(positionVector.NormalizedX, positionVector.NormalizedY);
        #endregion
        #region private methods
        protected virtual void MoveVehicleForward()
        {
            // don't let vehicle pass node
            double _currentSpeed = this.CurrentSpeed;
            if (DistanceToTravel - TraveledDistance < _currentSpeed)
                _currentSpeed -= DistanceToTravel - TraveledDistance;
            //apply to position
            Debug.WriteLine($"{positionVector.NormalizedX} {positionVector.NormalizedY}");
            double xValue = _currentSpeed * positionVector.NormalizedX;
            double yValue = _currentSpeed * positionVector.NormalizedY;

            this.ActualPosition = new Point(
                this.ActualPosition.X + xValue,
                this.ActualPosition.Y + yValue
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

        public virtual void DisableVehicle()
        {
            IsVisible = false;
            CanMove = false;
            CanColide = false;
        }
        public virtual void EnableVehicle()
        {
            IsVisible = true;
            CanMove = true;
            CanColide = true;
        }
        #endregion
    }
}
