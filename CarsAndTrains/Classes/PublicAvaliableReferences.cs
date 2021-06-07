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
        protected static List<Rectangle> carsDrawings;
        protected static List<Node.Node> carNodes;
        protected static List<Node.Node> trainNodes;

        protected static string[] vehicleGraphicsURL = new string[8];
        protected static string[] trainGraphicsURL = new string[8];
        protected static Canvas canvas;
        private const int LIMIT = 6;
        private const float DEATH_TICK_VALUE = 1.0f;
        public const float TICK_VALUE = 1.0f;
        protected static bool drawNodes = true;
        protected static bool drawCars = true;
        protected static bool drawTrains = true;

        public static bool IsFinished { get; protected set; }

        public static void Initialize(Canvas passedCanvas)
        {
            canvas = passedCanvas;
            IsFinished = false;
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
            if (carNodes is null)
                carNodes = new List<Node.Node>();
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
                    carNodes.Add(new Node.Node(new Point(xValue, yValue)));
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

            for (int i = 0; i < carNodes.Count; i++)
            {
                Node.Node node = carNodes[i];
                if (i + 1 >= carNodes.Count)
                    node.CalculateVector(carNodes[0]);
                else
                    node.CalculateVector(carNodes[i + 1]);
                TextBlock nodeNumber = new TextBlock()
                {
                    Text = $"{node.Vector.NormalizedX.ToString("0.00")} {node.Vector.NormalizedY.ToString("0.00")}",
                    HorizontalAlignment = HorizontalAlignment.Center,
                    FontSize = 12,
                };
                Canvas.SetLeft(nodeNumber, node.Vector.X);
                Canvas.SetTop(nodeNumber, node.Vector.Y);
                canvas.Children.Add(nodeNumber);
                Panel.SetZIndex(nodeNumber, 5);

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
            carsDrawings = new List<Rectangle>();
            Random random = new Random();
            for (int i = 0; i < LIMIT; i++)
            {
                int nextIndex = i + 1 >= LIMIT ? -1 : i + 1;

                Car car = CarFactory.CreateCar(nodesCount, nextIndex);
                car.ActualPosition = carNodes[0].GetNodePosition();
                cars.Add(car);

                Rectangle carRectangle = new Rectangle
                {
                    Width = 30,
                    Height = 20,
                    Fill = new SolidColorBrush
                    {
                        Color = Color.FromArgb(255,
                                                   (byte)random.Next(255),
                                                   (byte)random.Next(255),
                                                   (byte)random.Next(255))
                    }
                };

                MainWindow.GetMain.Dispatcher.Invoke(() =>
                {
                    Canvas.SetLeft(carRectangle, car.ActualPosition.X);
                    Canvas.SetTop(carRectangle, car.ActualPosition.Y);
                });

                Panel.SetZIndex(carRectangle, 5);
                canvas.Children.Add(carRectangle);
                carsDrawings.Add(carRectangle);
            }
        }

        public static void UpdateAllVehicles()
        {
            lock (cars)
            {
                bool allCarsArrived = true;
                for (int i = 0; i < cars.Count; i++)
                {
                    Car car = cars[i];
                    car.UpdateVehicle();
                    MainWindow.GetMain.Dispatcher.Invoke(() =>
                    {
                        Canvas.SetLeft(carsDrawings[i], car.ActualPosition.X);
                        Canvas.SetTop(carsDrawings[i], car.ActualPosition.Y);
                    });

                    if (car.Arived())
                    {
                        if (!allCarsArrived)
                            allCarsArrived = true;

                        if (car.DeathAfterArivalTime <= 0.0f)
                            ReincarnateCar(car);
                        else
                            car.DeathAfterArivalTime -= DEATH_TICK_VALUE;
                    }
                    else
                    {
                        allCarsArrived = false;
                    }


                }

                IsFinished = allCarsArrived;
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
                foreach (Vehicle.Vehicle vehicle in cars)
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

        public static double GetNextVehicleWidth(int index)
        {
            if (index == -1)
                return 1f;
            return 30f;//cars[index].CurrentGraphics;
        }

        public static double GetNextVehicleSpeed(int index)
        {
            lock (cars)
            {
                if (index == -1)
                    return 99999f;
                return cars[index].CurrentSpeed;
            }
        }
    }
}
