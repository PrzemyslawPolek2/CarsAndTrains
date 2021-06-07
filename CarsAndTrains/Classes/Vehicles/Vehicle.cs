using CarsAndTrains.Classes.Nodes;
using System.Diagnostics;
using System.Windows;

namespace CarsAndTrains.Classes.Vehicles
{
    public abstract class Vehicle
    {
        private static int CarID = 0;
        public int carID = 0;
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

        public string CurrentGraphics { get; protected set; }
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
            Node nextNode = PublicAvaliableReferences.GetNode(CounterNodes);//GetNextNode będzie wysyłać parametr CounterNodes

            if (!CanMove || !nextNode.CanGoThrough)
                return;

            if (CanColiding)
            {
                SpeedControlBasedOnNextVehicle();
            }
            //przesuwanie vehicle miedzy nodami
            MoveVehicleBeetweenNodes();
            //sprawdzanie czy dojechal do node
            _ = DidArriveToNode(nextNode);

            if (CounterNodes == 0)
                EmptiedNodesAction();

        }
        #endregion
        #region protected methods
        protected void GetNewGraphic() => this.CurrentGraphics = PublicAvaliableReferences.GetNextGraphic(this.carID);

        #endregion
        #region private methods
        private Node DidArriveToNode(Node nextNode)
        {
            if ((this.positionVector.Length - TraveledDistance) <= OFFSET)
            {
                CounterNodes--;
                nextNode = PublicAvaliableReferences.GetNode(CounterNodes);
                Debug.WriteLine($"{carID}|{positionVector.NormalizedX} {positionVector.NormalizedY}");
                if (nextNode is TrainTriggerNode node)
                    node.TriggerTurnpike();
                GetNewGraphic();
                this.positionVector = nextNode.Vector;
                /*if (nextNode.CanGoThrough)
                    CurrentGraphics = PublicAvaliableReferences.GetNextGraphic(this.carID);*/
                TraveledDistance = 0;
            }
            return nextNode;
        }
        private void MoveVehicleBeetweenNodes()
        {
            if (this.positionVector.Length < CurrentSpeed)
                CurrentSpeed = CurrentSpeed - this.positionVector.Length;
            ActualPosition = new Point(
                ActualPosition.X + (CurrentSpeed * this.positionVector.NormalizedX),
                ActualPosition.Y + (CurrentSpeed * this.positionVector.NormalizedY)
                );

            TraveledDistance = TraveledDistance + CurrentSpeed;
        }

        private void SpeedControlBasedOnNextVehicle()
        {
            if (!PublicAvaliableReferences.VehiclesExistOnPath(this.NextVehicleIndex))
                return;

            if (PublicAvaliableReferences.IsVehicleInTheWay(this.NextVehicleIndex, this))
                this.CurrentSpeed = PublicAvaliableReferences.GetNextVehicleSpeed(this.NextVehicleIndex);
            else
                this.CurrentSpeed = this.VehicleSpeed;
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
