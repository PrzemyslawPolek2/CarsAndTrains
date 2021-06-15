﻿using CarsAndTrains.Classes.Nodes;
using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Media.Imaging;

namespace CarsAndTrains.Classes.Vehicles
{
    public abstract class Vehicle
    {
        #region Constants
        public const double VEHICLE_DISTANCE_OFFSET = 60f;
        public const double NODE_DISTANCE_OFFSET = .5f;
        #endregion

        #region Fields
        public bool CanMove { get; set; }
        public bool CanColide { get; set; }
        public bool IsVisible { get; set; }
        public bool IsActive { get; set; }
        public bool IsBehindVehicle { get; set; }
        public int NodesLeftToTravel
        {
            get => counterNodes;
            set => SetCounterNodes(value);
        }
        public BitmapImage CurrentGraphics { get; set; }
        public double DeathTime { get; set; }
        public double VehicleSpeed { get; set; }
        public Point ActualPosition { get; set; }
        public double WidthGraphics { get; protected set; }
        public double TraveledDistance { get; protected set; }
        public double RelatvieTraveledDistance { get; protected set; }
        public double DistanceToTravel { get; protected set; }
        public double RelativeDistanceToTravel { get; protected set; }
        public int NextVehicleIndex { get; set; }
        public double CurrentSpeed
        {
            get => currentSpeed;
            set => LimitSpeed(value);
        }

        #endregion

        #region Variables
        protected double currentSpeed;
        protected PositionVector positionVector;
        protected int counterNodes;
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
            this.NodesLeftToTravel = CounterNodes;
            this.DeathTime = DeathAfterArivalTime;
            this.NextVehicleIndex = NextVehicleIndex;
        }

        #endregion


        #region Updates

        public virtual void UpdateVehicle()
        {
            if (!IsActive)
                return;

            //Get Next Node
            Node nextNode = GetNextNode(NodesLeftToTravel - 1);

            if (nextNode == null)
                return;

            if (!CanMove || !nextNode.CanGoTo)
            {
                CurrentSpeed = 0.0f;
                return;
            }
            //Can go forward, reset Current Speed
            this.CurrentSpeed = this.VehicleSpeed;

            //If this vehicle doesn't ignore other vehicles
            if (CanColide)
                LimitSpeedByVehicleDistance();

            //Apply Speed to position
            MoveVehicleForward();

            bool didAriveToNode = (DistanceToTravel - TraveledDistance) <= NODE_DISTANCE_OFFSET;
            if (didAriveToNode)
                UpdateNode();

        }

        /// <summary>
        /// Get Next Node if possible and update values associated with the node
        /// </summary>
        protected virtual void UpdateNode()
        {
            //reducing count of nodes left
            Node nextNode = GetNextNode(NodesLeftToTravel - 1);
            if (nextNode is null)
                return;

            GetNewGraphic();
            NodesLeftToTravel--;

            positionVector = nextNode.Vector;
            DistanceToTravel += positionVector.Length;
            RelativeDistanceToTravel = positionVector.Length;
            RelatvieTraveledDistance = 0;
        }

        #endregion

        public virtual bool Arived() => NodesLeftToTravel <= 0;

        /// <summary>
        /// Updates vehicle position based on Current Speed and distance to the node
        /// </summary>
        protected virtual void MoveVehicleForward()
        {
            // Don't let vehicle pass node
            double _currentSpeed = this.CurrentSpeed;
            if (DistanceToTravel - TraveledDistance < _currentSpeed)
                _currentSpeed -= DistanceToTravel - TraveledDistance;
            if (_currentSpeed < 0)
                _currentSpeed = 0;
            if (_currentSpeed > VehicleSpeed)
                _currentSpeed = VehicleSpeed;
            //Apply to position
            double xValue = _currentSpeed * positionVector.NormalizedX;
            double yValue = _currentSpeed * positionVector.NormalizedY;

            this.ActualPosition = new Point(
                this.ActualPosition.X + xValue,
                this.ActualPosition.Y + yValue
                );

            TraveledDistance += _currentSpeed;
            RelatvieTraveledDistance += _currentSpeed;
        }

        /// <summary>
        /// Control Speed of this Vehicle based on the vehicle in front
        /// </summary>
        protected void LimitSpeedByVehicleDistance()
        {
            //If no vehicle exists in front the vehicle, no need to limit speed
            if (!PublicAvaliableReferences.IsAnyVehicleInFront(this))
            {
                IsBehindVehicle = false;
                return;
            }

            //If the next vehicle is too close, limit self speed to it's speed
            if (PublicAvaliableReferences.IsVehicleInTheWay(this))
            {
                IsBehindVehicle = true;
                this.CurrentSpeed = PublicAvaliableReferences.GetNextVehicleSpeed(this);
            }
            else
            {
                IsBehindVehicle = false;
                this.CurrentSpeed = this.VehicleSpeed;
            }
        }


        #region Gets
        public virtual void GetNewGraphic() => this.CurrentGraphics = PublicAvaliableReferences.GetNextCarGraphic(positionVector.NormalizedX, positionVector.NormalizedY);

        /// <summary>
        /// Relative ratio of Distance Travel is <see cref="double"/> in 0.0f - 1.0f range. The closer the vehicle is to the next node, the closer the value aproaches 1.0f. 
        /// </summary>
        /// <returns>Ratio between 0.0f-1.0f</returns>
        public double GetRelativeDistanceTravelRatio()
        {
            return RelatvieTraveledDistance / RelativeDistanceToTravel;
        }
        protected virtual Node GetNextNode(int index)
        {
            return PublicAvaliableReferences.GetCarNode(index);
        }

        #endregion

        #region Sets

        /// <summary>
        /// Enables Vehicle for movement and recognition during riding (Vehicle might still be InActive!)
        /// </summary>
        public virtual void EnableVehicle()
        {
            IsVisible = true;
            CanMove = true;
            CanColide = true;
        }
        /// <summary>
        /// Disables Vehicle from movement and recognition during riding (Vehicle is still Active!)
        /// </summary>
        public virtual void DisableVehicle()
        {
            IsVisible = false;
            CanMove = false;
            CanColide = false;
        }

        protected virtual void SetCounterNodes(int value)
        {
            this.counterNodes = value;
        }
        /// <summary>
        /// Control CurrentSpeed based on Min (0) and Max (VehicleSpeed)
        /// </summary>
        /// <param name="value"></param>
        private void LimitSpeed(double value)
        {
            currentSpeed = value;
            if (currentSpeed > VehicleSpeed)
                currentSpeed = VehicleSpeed;
            if (currentSpeed < 0)
                currentSpeed = 0;
        }

        #endregion
    }
}
