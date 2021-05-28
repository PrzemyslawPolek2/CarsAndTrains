using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows;
using System.Runtime;
using CarsAndTrains.Classes;

namespace CarsAndTrains.Classes.Vehicle
{
    abstract public class Vehicle
    {
        public bool CanMove { get; set; }
        public bool CanColiding { get; set; }
        public bool IsVisible { get; set; } 
        public bool Arrived { get; set; }   //czy dotarło, jeśli tak, to odejmuje -1 z licznika samochodów widocznych
        public Point ActualPosition { get; set; } = new Point();//Aktualna pozycja, która się aktualizuje co tick 
                                                                //ActualPosiion.x=ActualPosiion.x+CurrentSpeed.x ActualPosiion.y=ActualPosiion.y+CurrentSpeed.y;
        public float DeathAfterArivalTime { get; set; }//wysyłam Polkowi wartość wylosowaną, po x sekundach samochodzik się odrodzi na pozycji Node0
        public double VehicleSpeed { get; set; }  //prędkość startowa samochodu brana od Polka
        private double CurrentSpeed { get; set; }  //aktualna prędkość brana od samochodu przede mną na liście samochodów, brana w momencie jak pojazd a dojedzie 
                                                //do WidthGraphics pojazdu a-1
        private PositionVector positionVector;
        public double TraveledDistance { get; protected set; }
        private const double OFFSET=15;
        public int CounterNodes { get; set; }//licznik Nodów ile zostało do przejścia, jak dojdzie do zera to IsVisible=false dojechał do końca trasy
                                
        private string CurrentGraphics { get; set; }//dostaje od Polka
        public double WidthGraphics { get; set; }//dostaje od Polka 

        public Vehicle() 
        { 
        
        }
        public Vehicle(int VehicleSpeed, int CounterNodes, float DeathAfterArivalTime, Point ActualPosition) 
        {
            this.VehicleSpeed = VehicleSpeed;
            this.CounterNodes = CounterNodes;
            this.DeathAfterArivalTime = DeathAfterArivalTime;
            this.ActualPosition = ActualPosition;
           
            CanMove = true;
            CanColiding = true;
            IsVisible = true;
            Arrived = false;
            
            DeathAfterArivalTime = Random();
        }


        public virtual void UpdateVehicle() 
        {
            Node nextNode = PublicAvaliableReferences.GetNextNode();//GetNextNode wysyłą parametr CounterNodes

            if (!CanMove | nextNode.canGoThrough) 
                return;

            if (CanColiding)
            {
                SpeedControlBasedOnNextVehicle();
            }
            //przesuwanie auta miedzy nodami
            if (positionVector.length < CurrentSpeed) 
            {
                CurrentSpeed = CurrentSpeed - positionVector.length;
                
            }
            ActualPosition = new Point(ActualPosition.X * CurrentSpeed, ActualPosition.Y * CurrentSpeed);
            TraveledDistance = TraveledDistance + CurrentSpeed;
            //sprawdzanie czy dojechal do node
            if (positionVector.length - TraveledDistance <= OFFSET) 
            {
                CounterNodes = CounterNodes - 1;
                nextNode= PublicAvaliableReferences.GetNextNode();
                positionVector = nextNode.vector;    
                if (nextNode.canGoThrough)
                    CurrentGraphics =PublicAvaliableReferences.GetNextGraphic();
            }

            if (CounterNodes == 0) {
                EmptiedNodesAction();
            }

            // throw new NotImplementedException();
        }

        private void SpeedControlBasedOnNextVehicle()
        {
            if (PublicAvaliableReferences.VehiclesExistOnPath(this))
            {
                if (PublicAvaliableReferences.IsVehicleInTheWay(this))
                    this.CurrentSpeed = PublicAvaliableReferences.GetNextVehicleSpeed(this);
                else
                    this.CurrentSpeed = this.VehicleSpeed;
            }
        }

        private void EmptiedNodesAction()//TODO funkcja która się zrobi jak CounterNodes=0
        {
            IsVisible = false;
            CanMove = false;
            CanColiding = false;
        }

        protected void GetNewGraphic() //póxniej, czyli jak będą grafiki
        {
            throw new NotImplementedException();
        }
        protected int Random(int minRandom = 1, int maxRandom = 10)
        {
            var rand = new Random();

            return (rand.Next(minRandom,maxRandom));
        }

        public bool Arriveed()
        {
            return CounterNodes == 0;
        }
    }
}
