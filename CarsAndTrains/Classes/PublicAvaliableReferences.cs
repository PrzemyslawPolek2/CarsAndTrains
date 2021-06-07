using CarsAndTrains.Classes.Nodes;
using CarsAndTrains.Classes.Vehicles;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace CarsAndTrains.Classes
{
    class PublicAvaliableReferences
    {
        protected static List<Train> trains;
        protected static List<Car> cars;
        protected static List<Rectangle> carsDrawings;
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
                carNodes = new List<Node>();

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

            for (int i = 0; i < carNodes.Count; i++)
            {
                Node node = carNodes[i];
                if (i + 1 >= carNodes.Count)
                {
                    node.CanGoThrough = false;
                    node.CalculateVector(carNodes[i]);
                }
                else
                    node.CalculateVector(carNodes[i + 1]);

                if (!drawNodes)
                    continue;

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
            CarFactory.Initialize();
            for (int i = 0; i < LIMIT; i++)
            {
                int nextIndex = i + 1 >= LIMIT ? -1 : i + 1;

                Car car = CarFactory.CreateCar(nodesCount, nextIndex);
                car.ActualPosition = carNodes[0].GetNodePosition();
                if (i != 0)
                {
                    car.IsActive = false;
                    car.CanColiding = false;
                    car.IsVisible = false;
                    car.CanMove = false;
                    car.DeathAfterArivalTime = 100 * i;
                }

                cars.Add(car);

                Rectangle carRectangle = new Rectangle
                {
                    Width = 30,
                    Height = 20,
                    Fill = new SolidColorBrush
                    {
                        Color = Color.FromArgb(0,
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

        public static void UpdateAllCars()
        {
            lock (cars)
            {
                bool allCarsArrived = true;
                for (int i = 0; i < cars.Count; i++)
                {
                    Car car = cars[i];

                    if (!car.IsVisible || !car.IsActive)
                    {
                        MainWindow.GetMain.Dispatcher.Invoke(() =>
                        {
                            carsDrawings[i].Fill = new SolidColorBrush(Color.FromArgb(0, 255, 0, 0));
                        });
                    }
                    else
                    {
                        MainWindow.GetMain.Dispatcher.Invoke(() =>
                        {
                            carsDrawings[i].Fill = new SolidColorBrush(Color.FromArgb(255, 255, 255, 0));
                        });
                    }

                    if (!car.IsActive)
                    {
                        car.DeathAfterArivalTime -= DEATH_TICK_VALUE;
                        if (car.DeathAfterArivalTime <= 0.0f)
                        {
                            car.IsActive = true;
                            car.CanColiding = true;
                            car.IsVisible = true;
                            car.CanMove = true;
                        }

                        continue;
                    }

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

        public static bool VehiclesExistOnPath(int nextVehicleIndex)
        {
            lock (cars)
            {
                if (nextVehicleIndex == -1)
                    nextVehicleIndex = 0;
                return cars[nextVehicleIndex].IsActive && cars[nextVehicleIndex].CanColiding && !cars[nextVehicleIndex].Arived();
            }
        }
        public static bool IsVehicleInTheWay(int nextVehicleIndex, Vehicle car)
        {
            lock (carNodes)
            {
                if (nextVehicleIndex == -1)
                    nextVehicleIndex = 0;
                Vehicle nextCar = cars[nextVehicleIndex];
                if (!(!cars[nextVehicleIndex].IsActive || !cars[nextVehicleIndex].CanColiding || cars[nextVehicleIndex].Arived()))
                    return false;

                double passedVehicleLimit = nextCar.TraveledDistance - nextCar.WidthGraphics / 2;
                double thisVehicleBack = car.TraveledDistance - car.WidthGraphics / 2;

                if (passedVehicleLimit > thisVehicleBack)
                    return false;
                return true;
            }
        }

        public static Node GetNode(int rawCurrentlyUsedNode)
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
            lock (carsDrawings)
            {
                return carsDrawings[index].Width;
            }
            //cars[index].CurrentGraphics;
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
