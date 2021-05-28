using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows;
using System.Runtime;
using CarsAndTrains.Classes;
using CarsAndTrains.Classes.Node;

namespace CarsAndTrains.Classes.Vehicle
{
    public abstract class Vehicle
    {
        private const double OFFSET = 15;
        #region public fields
        public bool CanMove { get; set; }
        public bool CanColiding { get; set; }
        public bool IsVisible { get; set; }
        public bool IsActive { get; set; }
        public int CounterNodes { get; set; }
        public float DeathAfterArivalTime { get; set; }
        public double VehicleSpeed { get; set; }
        public Point ActualPosition { get; set; } = new Point();
        public double WidthGraphics { get; protected set; }
        public double TraveledDistance { get; protected set; }
        #endregion
        #region private fields
        private double currentSpeed;
        private PositionVector positionVector;
        private string currentGraphics;
        #endregion
        #region public constructors
        public Vehicle() 
        { 
        
        }

        public Vehicle(int VehicleSpeed, int CounterNodes, float DeathAfterArivalTime) //Car
        {
            this.VehicleSpeed = VehicleSpeed;
            this.CounterNodes = CounterNodes;
            this.DeathAfterArivalTime = DeathAfterArivalTime;
           
            CanMove = true;
            CanColiding = true;
            IsVisible = true;
            IsActive = true;
        }
        #endregion


        #region public methods
        public virtual void UpdateVehicle()
        {
            Node nextNode = PublicAvaliableReferences.GetNextNode(CounterNodes);//GetNextNode będzie wysyłać parametr CounterNodes

            if (!CanMove | nextNode.canGoThrough)
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
        protected void GetNewGraphic() //póxniej, czyli jak będą grafiki
        {
            throw new NotImplementedException();
        }
        #endregion
        #region private methods
        private Node DidArriveToNode(Node nextNode)
        {
            if (positionVector.length - TraveledDistance <= OFFSET)
            {
                CounterNodes = CounterNodes - 1;
                nextNode = PublicAvaliableReferences.GetNextNode();
                positionVector = nextNode.vector;
                if (nextNode.canGoThrough)
                    currentGraphics = PublicAvaliableReferences.GetNextGraphic();
            }

            return nextNode;
        }
        private void MoveVehicleBeetweenNodes()
        {
            if (positionVector.length < currentSpeed)
                currentSpeed = currentSpeed - positionVector.length;
            ActualPosition = new Point(ActualPosition.X * currentSpeed, ActualPosition.Y * currentSpeed);
            TraveledDistance = TraveledDistance + currentSpeed;
        }

        private void SpeedControlBasedOnNextVehicle()
        {
            if (PublicAvaliableReferences.VehiclesExistOnPath(this))
            {
                if (PublicAvaliableReferences.IsVehicleInTheWay(this))
                    this.currentSpeed = PublicAvaliableReferences.GetNextVehicleSpeed(this);
                else
                    this.currentSpeed = this.VehicleSpeed;
            }
        }

        private void EmptiedNodesAction()
        {
            IsVisible = false;
            CanMove = false;
            CanColiding = false;
        }
        #endregion
    }
}
