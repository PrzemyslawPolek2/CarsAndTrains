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
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace CarsAndTrains.Classes
{
    class PublicAvaliableReferences
    {
        protected static Canvas canvas;

        //Cars
        protected static List<Car> cars;
        protected static List<Image> carsDrawings;

        //Trains
        protected static List<Train> trains;

        //Nodes
        protected static List<Node> carNodes;
        protected static List<Node> trainNodes;

        protected static BitmapImage[] vehicleGraphics = new BitmapImage[8];
        protected static string[] trainGraphicsURL = new string[8];


        public const float TICK_VALUE = 1.0f;
        private const int SPAWN_CAR_LIMIT = 6;
        private const int VEHICLE_DIRECTIONS = 8;
        private const float DEATH_TICK_VALUE = 1.0f;
        private const int SPAWN_DELAY = 200;
        protected static bool drawNodes = true;
        protected static bool drawCars = true;
        protected static bool drawTrains = true;


        private static readonly float[,] directions =
        {
            {-0.25f, 0.25f, 0.75f, 1.0f}, // UP
            {0.25f, 0.75f, 0.25f, 0.75f}, // UP_RIGHT
            {0.75f, 1.0f, -0.25f, 0.25f}, // RIGHT
            {0.25f, 0.75f, -0.25f, -0.75f}, // DOWN_RIGHT
            {-0.25f, 0.25f, -0.75f, -1.0f}, // DOWN
            {-0.25f, -0.75f, -0.25f, -0.75f}, // DOWN_LEFT
            {-0.75f, -1.0f, -0.25f, 0.25f}, // LEFT
            {-0.75f, -0.25f, 0.25f, 0.75f}, // UP_LEFT
        };

        public static bool IsFinished { get; protected set; }

        public static void Initialize(Canvas passedCanvas)
        {
            canvas = passedCanvas;
            IsFinished = false;
            CreateBitmapImages();
            //CreateTrainsPool();
            CreateNodes();
            CreateCarsPool();
            //ForceNodeCalculation();
        }

        private static void CreateBitmapImages()
        {
            string path = System.IO.Path.GetDirectoryName(System.IO.Path.GetDirectoryName(System.IO.Directory.GetCurrentDirectory()));
            path += @"\Resources\Images\Cars\Blue\blue_car_left.png";
            
            vehicleGraphics[0] = new BitmapImage(new Uri(path));
            vehicleGraphics[1] = vehicleGraphics[0];
            vehicleGraphics[2] = vehicleGraphics[0];
            vehicleGraphics[3] = vehicleGraphics[0];
            vehicleGraphics[4] = vehicleGraphics[0];
            vehicleGraphics[5] = vehicleGraphics[0];
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
            carsDrawings = new List<Image>();
            Random random = new Random();
            CarFactory.Initialize();
            for (int i = 0; i < SPAWN_CAR_LIMIT; i++)
            {
                int nextIndex = i - 1 < 0 ? -1 : i - 1;

                Car car = CarFactory.CreateCar(nodesCount, nextIndex);
                car.ActualPosition = carNodes[0].GetNodePosition();
                if (i != 0)
                {
                    car.IsActive = false;
                    car.CanColiding = false;
                    car.IsVisible = false;
                    car.CanMove = false;
                    car.DeathAfterArivalTime = SPAWN_DELAY * i;
                }

                cars.Add(car);

                Image carRectangle = new Image
                {
                    Width = Car.CAR_WIDTH,
                    Height = Car.CAR_HEIGHT,
                    Source = vehicleGraphics[0]
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

                    if (!car.IsActive)
                    {
                        car.DeathAfterArivalTime -= DEATH_TICK_VALUE;
                        if (car.DeathAfterArivalTime <= 0.0f)
                        {
                            car.IsActive = true;
                            car.CanColiding = true;
                            car.IsVisible = true;
                            car.CanMove = true;
                            car.GetNewGraphic();
                        }
                        continue;
                    }

                    car.UpdateVehicle();
                    try
                    {
                        MainWindow.GetMain.Dispatcher.Invoke(() =>
                        {
                            Canvas.SetLeft(carsDrawings[i], car.ActualPosition.X);
                            Canvas.SetTop(carsDrawings[i], car.ActualPosition.Y);
                        });
                    } catch (Exception)
                    {
                        return;
                    }
                    
                    allCarsArrived = CheckIfAllCarsArrived(allCarsArrived, car);
                }

                IsFinished = allCarsArrived;
            }
        }

        private static bool CheckIfAllCarsArrived(bool allCarsArrived, Car car)
        {
            if (!car.Arived())
                allCarsArrived = false;
            
            return allCarsArrived;
        }

        private static void ReincarnateCar(Car car)
        {
            //won't do for now.
            //Vehicle vehicle;
        }

        public static bool VehiclesExistOnPath(Vehicle thisVehicle)
        {
            lock (cars)
            {
                if (!BasicChecksForVehicle(thisVehicle))
                    return false;
                Vehicle nextCar = cars[thisVehicle.NextVehicleIndex];

                bool vehicleExists = (nextCar.IsActive && nextCar.CanColiding && !nextCar.Arived());
                return vehicleExists;
            }
        }

        public static bool IsVehicleInTheWay(Vehicle thisVehicle)
        {
            lock (cars)
            {
                if (!BasicChecksForVehicle(thisVehicle))
                    return false;
                Vehicle nextCar = cars[thisVehicle.NextVehicleIndex];

                //calculate distance between two vehicles
                double nextVehicleBack = nextCar.TraveledDistance - (nextCar.WidthGraphics / 2);
                double thisVehicleFront = thisVehicle.TraveledDistance + (thisVehicle.WidthGraphics / 2);
                double differenceInDistance = Math.Abs(nextVehicleBack - thisVehicleFront);
                Debug.WriteLine($"{thisVehicle.carID} has [{thisVehicleFront} - {nextVehicleBack} ({differenceInDistance}) > {Vehicle.VEHICLE_DISTANCE_OFFSET}]");


                return (differenceInDistance >= Vehicle.VEHICLE_DISTANCE_OFFSET);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="vehicle">Vehicle to check</param>
        /// <returns>true if basics checks are fine</returns>
        private static bool BasicChecksForVehicle(Vehicle vehicle)
        {
            // first vehicle
            if (vehicle.NextVehicleIndex == -1)
                return false;
            Vehicle nextCar = cars[vehicle.NextVehicleIndex];
            //if it's the same vehicle
            if (vehicle.NextVehicleIndex == nextCar.NextVehicleIndex)
                return false;
            //if it's working vehicle
            if (!nextCar.IsActive || !nextCar.CanColiding || nextCar.Arived())
                return false;
            return true;
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

        public static BitmapImage GetNextGraphic(double normalizedX, double normalizedY)
        {
            lock (directions)
            {
                int selectedDirection = 0;
                for (int i = 0; i < VEHICLE_DIRECTIONS; i++)
                {
                    if (!IsInBetween(normalizedX, directions[i, 0], directions[i, 1]))
                        continue;
                    
                    if (!IsInBetween(normalizedY, directions[i, 2], directions[i, 3]))
                        continue;
                    
                    selectedDirection = i;
                    break;
                }

                return vehicleGraphics[selectedDirection];
            }
        }

        private static bool IsInBetween(double value, float smallerLimit, float biggerLimit)
        {
            if (smallerLimit > biggerLimit)
            {
                float temp = smallerLimit;
                smallerLimit = biggerLimit;
                biggerLimit = temp;
            }

            return (value >= smallerLimit && value < biggerLimit);
        }

        public static double GetNextVehicleWidth(int index)
        {
            if (index == -1)
                return 1f;
            lock (carsDrawings)
            {
                return carsDrawings[index].Width;
            }
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
