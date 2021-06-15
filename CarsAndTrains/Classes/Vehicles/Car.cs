using CarsAndTrains.Classes.Nodes;
using System;
using System.Diagnostics;
using System.Windows;

namespace CarsAndTrains.Classes.Vehicles
{
    public class Car : Vehicle
    {
        #region Constants
        public const double CAR_HEIGHT = 30.0f;
        public const double CAR_WIDTH = 40.0f;
        #endregion

        #region Fields
        public static int CarsID { get; private set; } = 0;
        public int CarID { get; protected set; } = 0;
        public bool IsReinacarnated { get; set; } = false;
        public bool IgnoreNodeLimit { get; private set; }
        #endregion

        #region Variables
        public static double FullTravelDistance = 0.0f;
        #endregion

        #region Constructors
        public Car(double VehicleSpeed, int CounterNodes, double DeathAfterArivalTime, int NextVehicleIndex) : base(VehicleSpeed,
                                                                                          CounterNodes,
                                                                                          DeathAfterArivalTime, NextVehicleIndex)
        {
            CarID = CarsID;
            CarsID++;
            EnableVehicle();
            this.positionVector = GetNextNode(CounterNodes).Vector;
            DistanceToTravel = positionVector.Length;
        }
        #endregion

        #region Updates

        public override void UpdateVehicle()
        {
            if (!IsActive)
                return;

            //There are 3 more nodes outside of Canvas, skipping them for optimalization
            if (NodesLeftToTravel == 3)
            {
                //Vehicle Arrived
                DisableVehicle();
                IsActive = false;
                IsReinacarnated = true;
            }
            //get next node
            Node _nextNode = GetNextNode(NodesLeftToTravel - 1);
            if (_nextNode == null)
            {
                return;
            }

            //if cannot move and doesn't ignore CanGoTo
            if (!CanMove || (!_nextNode.CanGoTo && !IgnoreNodeLimit))
            {
                CurrentSpeed = 0.0f;
                return;
            }
            //Reset speed
            this.CurrentSpeed = this.VehicleSpeed;

            if (CanColide)
                LimitSpeedByVehicleDistance();
            //apply speed to position
            MoveVehicleForward();

            bool _didAriveToNode = (DistanceToTravel - TraveledDistance) <= NODE_DISTANCE_OFFSET;
            if (_didAriveToNode)
            {
                UpdateNode();
                //Reset Node's CanGoTo ignorance
                IgnoreNodeLimit = false;
            }
        }
        #endregion

        #region Gets
        protected override Node GetNextNode(int nodeCount) => PublicAvaliableReferences.GetCarNode(nodeCount);
        #endregion

        #region Sets
        /// <summary>
        /// Resets <see cref="Car"/> position to the first <see cref="Node"/>
        /// </summary>
        public void ResetPosition()
        {
            this.TraveledDistance = 0;
            this.CurrentSpeed = VehicleSpeed;
            this.positionVector = GetNextNode(NodesLeftToTravel).Vector;
            this.DistanceToTravel = positionVector.Length;
            this.ActualPosition = new Point(this.positionVector.X, this.positionVector.Y);
        }
        /// <summary>
        /// Allows Car to ignore next <see cref="Node.CanGoTo"/>
        /// </summary>
        public void IgnoreCanGoThrough()
        {
            IgnoreNodeLimit = true;
        }
        #endregion

        /// <summary>
        /// Header for values of toString for <see cref="Car"/>
        /// </summary>
        /// <returns></returns>
        public static string Header()
        {
            return "\t| Active \t| ID \t| Speed | CanMove | Colide | Visible | Vehicle | Ignore | Nodes | Distance ";
        }
        public override string ToString()
        {
            string _isActive = IsActive ? "Active" : "NotActive";
            if (IsActive)
                return $"{_isActive}\t {CarID:0}|{NextVehicleIndex:0}\t{CurrentSpeed:0.00}\t{CanMove}\t{CanColide}\t{IsVisible}\t{IsBehindVehicle}\t{IgnoreNodeLimit}\t{NodesLeftToTravel}\t{TraveledDistance / FullTravelDistance * 100:00}% {RelatvieTraveledDistance / RelativeDistanceToTravel * 100:00}% [{TraveledDistance:0000}->{DistanceToTravel:0000}] / {FullTravelDistance:0000}";
            else
                return $"{_isActive}\t {CarID:0}|{NextVehicleIndex:0}";
        }
    }
}
