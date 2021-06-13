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
        protected static List<Image> carsArt;

        //Trains
        protected static List<Train> trains;
        protected static List<Image> trainsArt;

        //Nodes
        protected static List<Node> carNodes;
        protected static List<Node> trainNodes;

        protected static BitmapImage[] carsBitmaps = new BitmapImage[8];
        protected static BitmapImage[] trainsBitmaps = new BitmapImage[8];


        public const string CAR_NODES_FILE_NAME = "/nodePositions.txt";
        public const string TRAIN_NODES_FILE_NAME = "/trainNodesPositions.txt";
        public const string CAR_RESOURCES_FOLDER = @"\Resources\Images\Cars\Blue\";

        public const float TICK_VALUE = 1.0f;
        public const int ALPHA_FULL = 255;

        private const int SPAWN_CAR_LIMIT = 6;
        private const int SPAWN_TRAIN_LIMIT = 1;

        private const float DEATH_TICK_VALUE = 1.0f;
        private const int SPAWN_DELAY = 200;
        private const int VEHICLE_DIRECTIONS = 8;
        private static int railsNodeIndex = 12;

        private static bool drawNodes = true;
        private static bool drawCars = true;
        private static bool drawTrains = true;

        private static readonly float[,] directions =
        {
            // Y value goes higher the lower you are, so the Y value has to be inversed

            //X-min      X-max      Y-min       Y-max
            {-.50f,     .5f,       -.5f,       -1.0f},      // UP
            {-.25f,    .00f,     -.25f,      -1.0f},        // UP_RIGHT
            {-.00f,     -.5f,      .5f,      -0.5f},        // RIGHT
            {-.75f,     .0f,     .25f,     1.0f},           // DOWN_RIGHT
            {-.50f,     .5f,       .5f,      1.0f},         // DOWN
            {.00f,     .75f,      .25f,     1.0f},          // DOWN_LEFT
            {.50f,      .0f,       .5f,      -0.5f},        // LEFT
            {.00f,      .75f,      -.25f,      -1.0f},      // UP_LEFT
        };

        public static bool IsCarPoolFinished { get; protected set; }

        #region Initialization

        public static void Initialize(Canvas passedCanvas)
        {
            canvas = passedCanvas;
            IsCarPoolFinished = false;
            CreateBitmapImages();
            CreateNodes();
            CreatePools();
            //ForceNodeCalculation();
        }

        private static void CreateNodes()
        {
            CreateCarsNodes();
            CreateTrainNodes();
        }

        private static void CreatePools()
        {
            CreateCarsPool();
            CreateTrainsPool();
        }

        private static void CreateBitmapImages()
        {
            //TODO: asign graphics
            string path = System.IO.Path.GetDirectoryName(System.IO.Path.GetDirectoryName(Directory.GetCurrentDirectory()));
            path += $"{CAR_RESOURCES_FOLDER}car_";
            string fileExtension = ".png";

            for (int i = 0; i < VEHICLE_DIRECTIONS; i++)
            {
                carsBitmaps[i] = new BitmapImage(new Uri($"{path}{((int)Enums.GraphicDirection.RIGHT)}{fileExtension}"));
                trainsBitmaps[i] = new BitmapImage(new Uri($"{path}{i}{fileExtension}"));
            }
        }

        private static void CreateCarsNodes()
        {
            if (carNodes is null)
                carNodes = new List<Node>();

            string path = System.Reflection.Assembly.GetEntryAssembly().Location;
            path = System.IO.Path.GetDirectoryName(path) + CAR_NODES_FILE_NAME;

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
                    CreateNodeArt(xValue, yValue);
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

            }
        }

        private static void CreateTrainNodes()
        {
            if (trainNodes is null)
                trainNodes = new List<Node>();

            string path = System.Reflection.Assembly.GetEntryAssembly().Location;
            path = System.IO.Path.GetDirectoryName(path) + TRAIN_NODES_FILE_NAME;

            using (StreamReader streamReader = File.OpenText(path))
            {
                int i = 0;
                string readLine = "";
                while ((readLine = streamReader.ReadLine()) != null)
                {
                    string[] values = readLine.Split(' ');
                    double xValue = double.Parse(values[0]);
                    double yValue = double.Parse(values[1]);
                    int triggeringNode = int.Parse(values[2]);

                    if (triggeringNode == 0)
                    {
                        Node trainNode = new Node(new Point(xValue, yValue));
                        trainNodes.Add(trainNode);
                    }
                    else
                    {
                        TrainTriggerNode trainNode = new TrainTriggerNode(new Point(xValue, yValue));
                        trainNodes.Add(trainNode);
                    }

                    if (drawNodes)
                        CreateNodeArt(canvas, xValue, yValue, 255, (byte)(triggeringNode * ALPHA_FULL), 128, i);
                    i++;

                }
            }
            for (int i = 0; i < trainNodes.Count; i++)
            {
                Node node = trainNodes[i];
                if (i + 1 >= trainNodes.Count)
                {
                    node.CanGoThrough = false;
                    node.CalculateVector(trainNodes[i]);
                }
                else
                    node.CalculateVector(trainNodes[i + 1]);

            }
            //ForceTrainNodeCalculation();
        }

        public static void CreateNodeArt(Canvas canvas, double xValue, double yValue, byte red = 128, byte green = 255, byte blue = 0, int nodeInddex = -1)
        {
            Ellipse ellipse = new Ellipse
            {
                Width = Node.NODE_SIZE,
                Height = Node.NODE_SIZE,
                Fill = new SolidColorBrush
                {
                    Color = Color.FromArgb(ALPHA_FULL,
                                           red,
                                           green,
                                           blue)
                }
            };

            //TextBlock values = new TextBlock
            //{
            //    Text = text,
            //    TextAlignment = TextAlignment.Center,
            //    FontSize = 10,
            //};
            Canvas.SetLeft(ellipse, xValue);
            Canvas.SetTop(ellipse, yValue);
            //Canvas.SetLeft(values, xValue + Node.NODE_SIZE / 2);
            //Canvas.SetTop(values, yValue + (Node.NODE_SIZE / 2));

            Panel.SetZIndex(ellipse, 5);
            //Panel.SetZIndex(values, 5);
            canvas.Children.Add(ellipse);
            //canvas.Children.Add(values);
        }

        public static void CreateNodeArt(double xValue, double yValue, byte red = 128, byte green = 255, byte blue = 0) => CreateNodeArt(canvas, xValue, yValue, red, green, blue);


        private static void CreateTrainsPool()
        {
            int nodesCount = trainNodes.Count();
            if (nodesCount <= 0)
                throw new Exception();

            trains = new List<Train>();
            trainsArt = new List<Image>();

            //Initialize car factory
            TrainFactory.Initialize();

            for (int i = 0; i < SPAWN_TRAIN_LIMIT; i++)
            {
                int nextIndex = i - 1 < 0 ? -1 : i - 1;

                Train train = TrainFactory.Create(nodesCount, nextIndex);
                //set first node as car's stariting position
                train.ActualPosition = trainNodes[0].GetNodePosition();
                //assign first grahpic to the car
                train.CurrentGraphics = trainsBitmaps[(int)Enums.GraphicDirection.RIGHT];


                trains.Add(train);

                Image trainImage = new Image
                {
                    Width = Car.CAR_WIDTH * 10,
                    Height = Car.CAR_HEIGHT + 10,
                    Source = train.CurrentGraphics
                };


                Panel.SetZIndex(trainImage, 10);
                canvas.Children.Add(trainImage);
                trainsArt.Add(trainImage);
                MainWindow.GetMain.Dispatcher.Invoke(UpdateOnCanvas(trainsArt[i], train));
            }
        }

        private static void CreateCarsPool()
        {
            int nodesCount = carNodes.Count();
            if (nodesCount <= 0)
                throw new Exception();

            cars = new List<Car>();
            carsArt = new List<Image>();

            //Initialize car factory
            CarFactory.Initialize();

            for (int i = 0; i < SPAWN_CAR_LIMIT; i++)
            {
                int nextIndex = i - 1 < 0 ? -1 : i - 1;

                Car car = CarFactory.Create(nodesCount, nextIndex);

                //set first node as car's stariting position
                car.ActualPosition = carNodes[0].GetNodePosition();
                //assign first grahpic to the car
                car.CurrentGraphics = carsBitmaps[(int)Enums.GraphicDirection.RIGHT];

                if (i != 0)
                {
                    car.EnableVehicle();
                    car.DeathAfterArivalTime = SPAWN_DELAY * i;
                }

                cars.Add(car);

                Image carImage = new Image
                {
                    Width = Car.CAR_WIDTH,
                    Height = Car.CAR_HEIGHT,
                    Source = car.CurrentGraphics
                };

                carsArt.Add(carImage);
                Panel.SetZIndex(carImage, 5);
                canvas.Children.Add(carImage);

                MainWindow.GetMain.Dispatcher.Invoke(UpdateOnCanvas(carsArt[i], car));
            }
        }

        #endregion

        #region Updates

        public static void UpdateAllTrains()
        {
            lock (trains) lock (trainsArt)
                {
                    for (int i = 0; i < trains.Count; i++)
                    {
                        Train train = trains[i];
                        if (!train.IsActive)
                        {
                            //ReincarnateVehicle(train);
                            continue;
                        }

                        train.UpdateVehicle();

                        //if (!train.IsVisible)
                        //    continue;
                        MainWindow.GetMain.Dispatcher.Invoke(UpdateOnCanvas(trainsArt[i], train));
                    }
                }
        }

        public static void UpdateAllCars()
        {
            lock (cars) lock (carsArt)
                {
                    for (int i = 0; i < cars.Count; i++)
                    {
                        Car car = cars[i];

                        if (!car.IsActive)
                        {
                            car.DeathAfterArivalTime -= DEATH_TICK_VALUE;
                            if (car.DeathAfterArivalTime <= 0.0f)
                            {
                                ReincarnateVehicle(car);
                            }
                            continue;
                        }

                        car.UpdateVehicle();


                        if (!car.IsVisible || !drawCars)
                            continue;

                        MainWindow.GetMain.Dispatcher.Invoke(UpdateOnCanvas(carsArt[i], car));
                    }

                    IsCarPoolFinished = DidAllCarsArrive();
                }
        }

        private static Action UpdateOnCanvas(Image image, Vehicle vehicle)
        {
            if (vehicle is Train train)
            {
                return () =>
                {
                    image.Source = train.CurrentGraphics;
                    Canvas.SetLeft(image, train.ActualPosition.X);
                    Canvas.SetTop(image, train.ActualPosition.Y);
                };
            }

            return () =>
            {
                image.Source = vehicle.CurrentGraphics;
                Canvas.SetLeft(image, vehicle.ActualPosition.X);
                Canvas.SetTop(image, vehicle.ActualPosition.Y);
            };
        }

        #endregion

        public static void ReverseTrainPath(Train train)
        {
            lock (train)
            {
                if (train.CounterNodes != 0)
                    return;

                Debug.WriteLine("Reactivating Train");
                //if (invertedTrainRoute)
                //    ForceTrainNodeLeftHandCalculation();
                //else
                //    ForceTrainNodeRightHandCalculation();

                train.CounterNodes = trainNodes.Count;
                train.CanMove = true;
            }
        }

        private static void ReincarnateVehicle(Vehicle vehicle)
        {
            vehicle.IsActive = true;
            vehicle.EnableVehicle();
            vehicle.GetNewGraphic();
        }


        private static bool DidAllCarsArrive()
        {
            foreach (Vehicle vehicle in cars)
            {
                if (!vehicle.Arived())
                    return false;
            }
            return true;
        }

        public static bool IsAnyVehicleInFront(Vehicle thisVehicle)
        {
            lock (cars)
            {
                if (!BasicChecksForVehicle(thisVehicle))
                    return false;
                Vehicle nextCar = cars[thisVehicle.NextVehicleIndex];

                bool vehicleExists = nextCar.IsActive && nextCar.CanColide && !nextCar.Arived();
                return vehicleExists;
            }
        }

        public static bool IsCarInTheWay(Vehicle thisVehicle)
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
                return (differenceInDistance < Vehicle.VEHICLE_DISTANCE_OFFSET);
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
            if (!nextCar.IsActive || !nextCar.CanColide || nextCar.Arived())
                return false;
            return true;
        }

        public static Node GetCarNode(int nodesLeftToTravel) => GetNode(carNodes, nodesLeftToTravel);

        public static Node GetTrainNode(int nodesLeftToTravel) => GetNode(trainNodes, nodesLeftToTravel);

        public static Node GetNode(List<Node> nodesArray, int index)
        {
            lock (nodesArray)
            {
                int currentNodeIndex = nodesArray.Count() - index;

                if (currentNodeIndex < 0)
                    currentNodeIndex = 0;
                if (currentNodeIndex > nodesArray.Count() - 1)
                    currentNodeIndex = nodesArray.Count() - 1;
                return nodesArray[currentNodeIndex];
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
                //Debug.WriteLine($"Graphic {(Enums.GraphicDirection)selectedDirection}");
                //Debug.WriteLine($"NormalizedX  {directions[selectedDirection, 0]} {normalizedX} {directions[selectedDirection, 1]}");
                //Debug.WriteLine($"NormalizedY  {directions[selectedDirection, 2]} {normalizedY} {directions[selectedDirection, 3]}");
                return carsBitmaps[selectedDirection];
            }
        }

        public static BitmapImage GetNextTrainGraphic(double normalizedX, double normalizedY)
        {
            lock (directions)
            {
                int selectedDirection = (int)Enums.GraphicDirection.RIGHT;
                for (int i = 0; i < VEHICLE_DIRECTIONS / 2; i++)
                {
                    if (!IsInBetween(normalizedX, directions[i % 2, 0], directions[i % 2, 1]))
                        continue;

                    if (!IsInBetween(normalizedY, directions[i % 2, 2], directions[i % 2, 3]))
                        continue;

                    selectedDirection = i % 2;
                    break;
                }
                //Debug.WriteLine($"Graphic {(Enums.GraphicDirection)selectedDirection}");
                //Debug.WriteLine($"NormalizedX  {directions[selectedDirection, 0]} {normalizedX} {directions[selectedDirection, 1]}");
                //Debug.WriteLine($"NormalizedY  {directions[selectedDirection, 2]} {normalizedY} {directions[selectedDirection, 3]}");
                return trainsBitmaps[selectedDirection];
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

            bool isInBetweenValues = (value >= smallerLimit & value <= biggerLimit);
            //Debug.WriteLine($"\t {smallerLimit} <= {value} <= {biggerLimit} == {isInBetweenValues}");
            return isInBetweenValues;
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

        public static void TriggerTurnPike()
        {
            lock (carNodes)
            {
                carNodes[railsNodeIndex].CanGoThrough = !carNodes[railsNodeIndex].CanGoThrough;
            }
        }
    }
}
