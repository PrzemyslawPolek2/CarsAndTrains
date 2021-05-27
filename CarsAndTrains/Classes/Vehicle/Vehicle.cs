﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows;
using System.Runtime;

namespace CarsAndTrains
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
        public int VehicleSpeed { get; set; }  //prędkość startowa samochodu brana od Polka
        private int CurrentSpeed { get; set; }  //aktualna prędkość brana od samochodu przede mną na liście samochodów, brana w momencie jak pojazd a dojedzie 
                                                //do WidthGraphics pojazdu a-1

        public int CounterNodes { get; set; }//licznik Nodów ile zostało do przejścia, jak dojdzie do zera to IsVisible=false dojechał do końca trasy
                                
        private string CurrentGraphics { get; set; }//dostaje od Polka
        private string WidthGraphics { get; set; }//dostaje od Polka 

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
            

            if (ActualPosition.X == NodeList[CounterNodes].X & ActualPosition.Y == NodeList[CounterNodes].Y & IsVisible)
            {
                CounterNodes -= 1;
                //aktualizacja grafik, w momencie jak samochód dojedzie do Node'a (niezawsze) i jesli jest is Visible
            }


            if ()
            {

                //zwalnianie predkosci z nodea biorę vector x i y i pointsy

            }
            if (CounterNodes == 0) {
                EmptiedNodesAction();
            }

            // ActualPosition.x = ActualPosition.x + CurrentSpeed.x; zadziałą jak VehicleSpeed będzie mi już oddawać Vectora
            //ActualPosition.y = ActualPosition.y + CurrentSpeed.y; 

            // throw new NotImplementedException();
        }

        private void EmptiedNodesAction()//TODO funkcja która się zrobi jak CounterNodes=0
        {

            throw new NotImplementedException();
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
    }
}
