using CarsAndTrains.Classes.Nodes;
using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Media.Imaging;

namespace CarsAndTrains.Classes.Vehicles
{
    public abstract class Vehicle
    {
        public static int CarID { get; private set; } = 0;
        public int carID = 0;
        public const double VEHICLE_DISTANCE_OFFSET = 2.5f;
        public const double NODE_DISTANCE_OFFSET = .5f;
        #region public fields
        public bool CanMove { get; set; }
        public bool CanColiding { get; set; }
        public bool IsVisible { get; set; }
        public bool IsActive { get; set; }
        public int CounterNodes { get; set; }
        public BitmapImage CurrentGraphics { get; protected set; }
        public double DeathAfterArivalTime { get; set; }
        public double VehicleSpeed { get; set; }
        public Point ActualPosition { get; set; } = new Point();
        public double WidthGraphics { get; protected set; }
        public double TraveledDistance { get; protected set; }
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
            CanMove = true;
            CanColiding = true;
            IsVisible = true;
            IsActive = true;
            carID = CarID;
            CarID++;
        }

        public Vehicle(double VehicleSpeed, int CounterNodes, double DeathAfterArivalTime, int NextVehicleIndex) : this()
        {
            this.VehicleSpeed = VehicleSpeed;
            this.CounterNodes = CounterNodes;
            this.DeathAfterArivalTime = DeathAfterArivalTime;
            this.NextVehicleIndex = NextVehicleIndex;
            this.positionVector = PublicAvaliableReferences.GetNode(CounterNodes).Vector;
        }

        #endregion


        #region public methods

        public virtual void UpdateVehicle()
        {
            //GetNextNode będzie wysyłać parametr CounterNodes
            Node nextNode = PublicAvaliableReferences.GetNode(CounterNodes);

            if (!CanMove || !nextNode.CanGoThrough)
                return;

            if (CanColiding)
                LimitSpeedByVehicleDistance();

            //apply movement
            MoveVehicleForward();

            bool didAriveToNode = (this.positionVector.Length - TraveledDistance) <= NODE_DISTANCE_OFFSET;
            if (didAriveToNode)
            {
                UpdateNode();
            }

            if (CounterNodes == 0)
                DisableVehicle();

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

            this.positionVector = nextNode.Vector;
            TraveledDistance = 0;
        }

        public bool Arived() => CounterNodes == 0;
        public void GetNewGraphic() => this.CurrentGraphics = PublicAvaliableReferences.GetNextGraphic(positionVector.NormalizedX, positionVector.NormalizedY);
        #endregion
        #region private methods
        protected void MoveVehicleForward()
        {
            // don't let vehicle pass node
            double _currentSpeed = this.CurrentSpeed;
            if (positionVector.Length - TraveledDistance < _currentSpeed)
                _currentSpeed -= this.positionVector.Length - TraveledDistance;

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
            if (!PublicAvaliableReferences.VehiclesExistOnPath(this))
                return;

            if (PublicAvaliableReferences.IsVehicleInTheWay(this))
                this.CurrentSpeed = PublicAvaliableReferences.GetNextVehicleSpeed(this.NextVehicleIndex);
            else
                this.CurrentSpeed = this.VehicleSpeed;
        }

        protected void DisableVehicle()
        {
            IsActive = false;
            IsVisible = false;
            CanMove = false;
            CanColiding = false;
        }
        #endregion
    }
}
