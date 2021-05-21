using System;
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
            IsVisible = true;

            //ustaw wartosci dla booli domyslne

            DeathAfterArivalTime = random();
        }


        virtual public void updateVehicle() 
        {
            //jeśli jeden samochód zbliży się za blisko do drugiego to weź prędkość od samochodu -1 
            // if ((this.ActualPosition.y - Car[a + 1].ActualPosition.y) && (Car[a].ActualPosition.y < Car[a - 1].ActualPosition)


            //aktualizacja grafik, w momencie jak samochód skońćzy dojedzie do Node'a (niezawsze) i jesli jest is Visible

            //zwalnianie predkosci z nodea biorę vector x i y i pointsy

            //






            // ActualPosition.x = ActualPosition.x + CurrentSpeed.x; zadziałą jak VehicleSpeed będzie mi już oddawać Vectora
            //ActualPosition.y = ActualPosition.y + CurrentSpeed.y; 

            // throw new NotImplementedException();
        }

        private void emptiedNodesAction()//TODO funkcja która się zrobi jak CounterNodes=0
        {
            throw new NotImplementedException();
        }

        protected void getNewGraphic() //póxniej
        {
            throw new NotImplementedException();
        }
        protected int random(int minRandom = 1, int maxRandom = 10)
        {
            var rand = new Random();

            return (rand.Next(minRandom,maxRandom));
        }
    }
}
