using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using System.IO;
using CarsAndTrains.Classes.Vehicle;

namespace CarsAndTrains.Classes
{
    class PublicAvaliableReferences
    {
        protected static List<Train> trains;
        protected static List<Car> cars;
        protected static List<Node> carNodes;
        protected static List<Node> trainNodes;

        protected static string[] vehicleGraphicsURL = new string[8];
        protected static string[] trainGraphicsURL = new string[8];
        protected static Canvas canvas;
        private const int LIMIT = 6;
        private const float DEATH_TICK_VALUE = 1.0f;
        public const float TICK_VALUE = 1.0f;
        protected static bool drawNodes = true; 
        protected static bool drawCars = true; 
        protected static bool drawTrains = true; 


        public static void Initialize(Canvas passedCanvas)
        {
            canvas = passedCanvas;
            //CreateTrainsPool();
            CreateNodes();
            CreateCarsPool();
            //ForceNodeCalculation();
        }

        private static void ForceNodeCalculation()
        {
            for (int i = 0; i < carNodes.Count(); i++)
            {
                if (i >= carNodes.Count())
                    break;
            }
        }

        private static void CreateNodes()
        {
            string path = System.Reflection.Assembly.GetEntryAssembly().Location;
            path = System.IO.Path.GetDirectoryName(path) + "/nodePositions.txt";
            using (StreamReader sr = File.OpenText(path))
            {
                string s = "";
                while ((s = sr.ReadLine()) != null)
                {
                    string[] values = s.Split(' ');
                    float xValue = float.Parse(values[0]);
                    float yValue = float.Parse(values[1]);
                    carNodes.Add(new Node(new Point(xValue, yValue)));
                    if (!drawNodes)
                        continue;

                    Ellipse ellipse = new Ellipse
                    {
                        Width = 20,
                        Height = 20,
                        Fill = new SolidColorBrush
                        {
                            Color = Color.FromArgb(255,
                                                   128,
                                                   255,
                                                   0)
                        }
                    };
                    
                    Canvas.SetLeft(ellipse, xValue);
                    Canvas.SetTop(ellipse, yValue);
                    
                    Panel.SetZIndex(ellipse, 5);
                    canvas.Children.Add(ellipse);
                }
            }
        }

        private static void CreateTrainNodes()
        {
            throw new NotImplementedException();
        }

        private static void CreateCarNodes()
        {
            throw new NotImplementedException();
        }

        private static void CreateTrainsPool()
        {
            throw new NotImplementedException();
        }

        private static void CreateCarsPool()
        {
            int nodesCount = carNodes.Count();
            cars = new List<Car>();
            for (int i = 0; i < LIMIT; i++)
                cars.Add(CarFactory.CreateCar(nodesCount));
        }

        public static void UpdateAllVehicles()
        {
            lock (cars)
            {
                foreach(Car car in cars)
                {
                    car.UpdateVehicle();
                    if (car.Arrived())
                    {
                        
                        if (car.DeathAfterArivalTime <= 0.0f)
                            ReincarnateCar(car);
                        else
                            car.DeathAfterArivalTime -= DEATH_TICK_VALUE;
                    }
                }
            }
        }

        private static void ReincarnateCar(Car car)
        {
            //won't do for now.
            //Vehicle vehicle;
        }

        public static bool VehiclesExistOnPath(Vehicle.Vehicle passedVehicle)
        {
            lock (carNodes)
            {
                foreach(Vehicle.Vehicle vehicle in cars)
                {
                    if (vehicle == passedVehicle)
                        continue;

                    if (vehicle.CounterNodes == passedVehicle.CounterNodes)
                        return true;
                }
                return false;
            }
        }
        public static bool IsVehicleInTheWay(Vehicle.Vehicle passedVehicle)
        {
            lock (carNodes)
            {
                foreach (Vehicle.Vehicle vehicle in cars)
                {
                    if (vehicle == passedVehicle)
                        continue;

                    if (vehicle.CounterNodes != passedVehicle.CounterNodes)
                        continue;

                    double thisVehicleBack = vehicle.TraveledDistance - vehicle.WidthGraphics / 2;
                    double passedVehicleLimit = passedVehicle.TraveledDistance - passedVehicle.WidthGraphics / 2;
                    if (passedVehicleLimit > thisVehicleBack)
                        continue;
                    passedVehicleLimit += passedVehicle.WidthGraphics;
                    if (passedVehicleLimit >= thisVehicleBack)
                        return true;
                    else
                        continue;
                }
                return false;
            }
        }

        public static Node.Node GetNextNode(int rawCurrentlyUsedNode)
        {
            lock (carNodes)
            {
                int currentNodeIndex = carNodes.Count() - rawCurrentlyUsedNode;
                if (currentNodeIndex < 0)
                    currentNodeIndex = 0;
                if (currentNodeIndex > carNodes.Count() - 1)
                    currentNodeIndex = carNodes.Count() - 1;

                return carNodes[currentNodeIndex];
            }
        }

        public static string GetNextGraphic()
        {
            lock (vehicleGraphicsURL)
            {
                return vehicleGraphicsURL[0];
            }
        }

        public static double GetNextVehicleWidth(Vehicle.Vehicle vehicle)
        {
            throw new NotImplementedException();
        }

        public static double GetNextVehicleSpeed(Vehicle.Vehicle vehicle)
        {
            throw new NotImplementedException();
        }

    }
}
