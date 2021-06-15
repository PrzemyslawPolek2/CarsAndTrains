using CarsAndTrains.Classes.Nodes;
using CarsAndTrains.Classes.Objects;
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
        public static bool DrawNodes {get;set;}= false;

        #region Cars
        public static bool DrawCars { get; set; } = true;
        public static List<Car> cars;
        public static List<Node> carNodes;
        protected static List<Image> carsArt;

        public const string CAR_NODES_FILE_NAME = "/nodePositions.txt";
        public const string CAR_RESOURCES_FOLDER = @"\Resources\Images\Cars\";
        public const string CAR_IMAGE_PREFIX = "car_";

        protected const int SPAWN_CAR_LIMIT = 14;
        protected const int CAR_DIRECTIONS = 8;

        protected static BitmapImage[] carsBitmaps = new BitmapImage[8];
        #endregion

        #region Trains
        public static bool DrawTrains { get; set; } = true;
        public static List<Train> trains;
        protected static List<Image> trainsArt;
        protected static List<Node> trainNodes;

        public const string TRAIN_NODES_FILE_NAME = "/trainNodesPositions.txt";
        public const string TRAIN_RESOURCES_FOLDER = @"\Resources\Images\Trains\";
        public const string TRAIN_IMAGE_PREFIX = "train_";

        protected const int TRAIN_DIRECTIONS = 2;
        protected const int SPAWN_TRAIN_LIMIT = 1;

        protected static BitmapImage[] trainsBitmaps = new BitmapImage[2];
        #endregion

        #region Lights and Turnpikes

        public static List<Light> lights;
        protected static List<Image> lightsArt;
        public const string LIGHT_RESOURCES_FOLDER = @"\Resources\Images\Objects\Lights\";
        public const string LIGHT_IMAGE_PREFIX = "lights_";

        public static List<Turnpike> turnpikes;
        protected static List<Image> turnpikesArt;
        public const string TURNPIKE_RESOURCES_FOLDER = @"\Resources\Images\Objects\Turnpikes\";
        public const string TURNPIKE_IMAGE_PREFIX = "turnpike_";

        #endregion

        protected static Canvas canvas;

        public const float TICK_VALUE = 1.0f;
        public const int ALPHA_FULL = 255;


        protected const float DEATH_TICK_VALUE = 1.0f;
        protected const int SPAWN_DELAY = 100;
        protected static int RailsNodeIndex = 12;


        private static readonly float[,] directions =
        {
            // Y value goes higher the lower you are, so the Y value has to be inversed

            //X-min      X-max      Y-min       Y-max
            {-.25f,     0.25f,      -.75f,       -1.00f},      // UP
            {0.00f,     0.75f,      -.00f,       -.75f},      // UP_RIGHT
            {0.75f,     1.0f,       0.25f,       -0.25f},      // RIGHT
            {0.00f,     0.75f,      0.00f,       0.75f},       // DOWN_RIGHT
            {-.25f,     0.25f,      0.75f,       1.00f},       // DOWN
            {-.75f,     0.00f,      0.00f,       0.75f},       // DOWN_LEFT
            {-1.0f,     -.75f,      0.25f,       -0.25f},      // LEFT
            {-.75f,     0.00f,      -.00f,       -.75f},       // UP_LEFT
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
            CreateLightsPool();
            CreateTurnpikesPool();
        }

        #region Nodes Creation

        private static void CreateCarsNodes()
        {
            if (carNodes is null)
                carNodes = new List<Node>();
            Car.FullTravelDistance = 0;
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
                    if (!DrawNodes)
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
                Car.FullTravelDistance += node.Vector.Length;
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

                    if (DrawNodes)
                        CreateNodeArt(canvas, xValue, yValue, 255, (byte)(triggeringNode * ALPHA_FULL), 128, i);
                    i++;

                }
            }
            for (int i = 0; i < trainNodes.Count; i++)
            {
                Node node = trainNodes[i];
                if (i + 1 >= trainNodes.Count)
                {
                    //node.CanGoThrough = false;
                    node.CalculateVector();
                }
                else
                    node.CalculateVector(trainNodes[i + 1]);
            }
            //ForceTrainNodeCalculation();
        }

        #endregion

        private static void CreateBitmapImages()
        {
            string defaultFolderPath = System.IO.Path.GetDirectoryName(System.IO.Path.GetDirectoryName(Directory.GetCurrentDirectory()));
            string fileExtension = ".png";
            string path;

            //Cars
            path = $"{defaultFolderPath}{CAR_RESOURCES_FOLDER}{CAR_IMAGE_PREFIX}";
            for (int i = 0; i < CAR_DIRECTIONS; i++)
            {
                carsBitmaps[i] = new BitmapImage(new Uri($"{path}{i}{fileExtension}"));
            }

            //Trains           
            path = $"{defaultFolderPath}{TRAIN_RESOURCES_FOLDER}{TRAIN_IMAGE_PREFIX}";
            for (int i = 0; i < TRAIN_DIRECTIONS; i++)
            {
                trainsBitmaps[i] = new BitmapImage(new Uri($"{path}{i}{fileExtension}"));
            }

            //Lights          
            path = $"{defaultFolderPath}{LIGHT_RESOURCES_FOLDER}{LIGHT_IMAGE_PREFIX}";
            Light.LightsOff = new BitmapImage(new Uri($"{path}{0}{fileExtension}"));
            Light.LightsOn[0] = new BitmapImage(new Uri($"{path}{1}{fileExtension}"));
            Light.LightsOn[1] = new BitmapImage(new Uri($"{path}{2}{fileExtension}"));

            //Turnpikes
            path = $"{defaultFolderPath}{TURNPIKE_RESOURCES_FOLDER}{TURNPIKE_IMAGE_PREFIX}";
            Turnpike.TurnpikeGraphic[0,0] = new BitmapImage(new Uri($"{path}{0}_{0}{fileExtension}"));
            Turnpike.TurnpikeGraphic[0,1] = new BitmapImage(new Uri($"{path}{0}_{1}{fileExtension}"));
            Turnpike.TurnpikeGraphic[1,0] = new BitmapImage(new Uri($"{path}{1}_{0}{fileExtension}"));
            Turnpike.TurnpikeGraphic[1,1] = new BitmapImage(new Uri($"{path}{1}_{1}{fileExtension}"));

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

            TextBlock values = new TextBlock
            {
                Text = nodeInddex == -1 ? "" : $"{nodeInddex}",
                TextAlignment = TextAlignment.Center,
                FontSize = 10,
            };
            Canvas.SetLeft(ellipse, xValue);
            Canvas.SetTop(ellipse, yValue);
            Canvas.SetLeft(values, xValue + Node.NODE_SIZE / 2);
            Canvas.SetTop(values, yValue + (Node.NODE_SIZE / 2));

            Panel.SetZIndex(ellipse, 5);
            Panel.SetZIndex(values, 5);
            canvas.Children.Add(ellipse);
            canvas.Children.Add(values);
        }

        public static void CreateNodeArt(double xValue, double yValue, byte red = 128, byte green = 255, byte blue = 0) => CreateNodeArt(canvas, xValue, yValue, red, green, blue);

        #region Pools Creation

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
                //assign first grahpic to the train
                train.CurrentGraphics = trainsBitmaps[1];

                trains.Add(train);

                Image trainImage = new Image
                {
                    Width = Train.TRAIN_WIDTH,
                    Height = Train.TRAIN_HEIGHT,
                    Source = train.CurrentGraphics
                };


                trainsArt.Add(trainImage);
                Panel.SetZIndex(trainImage, 5);
                canvas.Children.Add(trainImage);

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
                int nextIndex = i - 1 < 0 ? SPAWN_CAR_LIMIT - 1 : i - 1;

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

        private static void CreateTurnpikesPool()
        {
            if (turnpikes is null)
                turnpikes = new List<Turnpike>();
            if (turnpikesArt is null)
                turnpikesArt = new List<Image>();

            TurnpikeFactory.Initialize();
            //80 360


            Turnpike lightsTop = TurnpikeFactory.Create(new Point(50, 330));
            Turnpike lightsBottom = TurnpikeFactory.Create(new Point(60, 390), true);

            Image leftTurnpikeImage = new Image
            {
                Source = lightsTop.CurrentGraphic,
                Width = 140,
            }; 
            Image rightTurnpikeImage = new Image
            {
                Source = lightsBottom.CurrentGraphic,
                Width = 140,
            };

            turnpikes.Add(lightsTop);
            turnpikes.Add(lightsBottom);
            turnpikesArt.Add(leftTurnpikeImage);
            turnpikesArt.Add(rightTurnpikeImage);
            canvas.Children.Add(leftTurnpikeImage);
            canvas.Children.Add(rightTurnpikeImage);
            Canvas.SetLeft(leftTurnpikeImage, lightsTop.ActualPosition.X);
            Canvas.SetTop(leftTurnpikeImage, lightsTop.ActualPosition.Y);
            Canvas.SetZIndex(leftTurnpikeImage, 5);
            Canvas.SetLeft(rightTurnpikeImage, lightsBottom.ActualPosition.X);
            Canvas.SetTop(rightTurnpikeImage, lightsBottom.ActualPosition.Y);
            Canvas.SetZIndex(rightTurnpikeImage, 10);
        }

        private static void CreateLightsPool()
        {
            if (lights is null)
                lights = new List<Light>();
            if (lightsArt is null)
                lightsArt = new List<Image>();

            TurnpikeFactory.Initialize();
            //80 360

            Light lightsTop = LightFactory.Create(new Point(160, 350));
            Light rightTurnpike = LightFactory.Create(new Point(20, 410));

            Image leftTurnpikeImage = new Image
            {
                Source = lightsTop.CurrentGraphic,
                Width = 80,
            };
            Image rightTurnpikeImage = new Image
            {
                Source = rightTurnpike.CurrentGraphic,
                Width = 80,
            };

            lights.Add(lightsTop);
            lights.Add(rightTurnpike);
            lightsArt.Add(leftTurnpikeImage);
            lightsArt.Add(rightTurnpikeImage);
            canvas.Children.Add(leftTurnpikeImage);
            canvas.Children.Add(rightTurnpikeImage);
            Canvas.SetLeft(leftTurnpikeImage, lightsTop.ActualPosition.X);
            Canvas.SetTop(leftTurnpikeImage, lightsTop.ActualPosition.Y);
            Canvas.SetZIndex(leftTurnpikeImage, 5);
            Canvas.SetLeft(rightTurnpikeImage, rightTurnpike.ActualPosition.X);
            Canvas.SetTop(rightTurnpikeImage, rightTurnpike.ActualPosition.Y);
            Canvas.SetZIndex(rightTurnpikeImage, 10);
        }
        #endregion

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
                            train.DeathAfterArivalTime -= DEATH_TICK_VALUE;
                            if (train.DeathAfterArivalTime > 0.0f)
                                continue;

                            ReincarnateTrain(train);
                        }

                        train.UpdateVehicle();

                        if (!train.IsVisible || !DrawTrains)
                            continue;
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
                        if (car.IsReinacarnated)
                            continue;
                        if (!car.IsActive)
                        {
                            car.DeathAfterArivalTime -= DEATH_TICK_VALUE;
                            if (car.DeathAfterArivalTime <= 0.0f)
                            {
                                ReincarnateCar(car);
                            }
                            continue;
                        }

                        car.UpdateVehicle();

                        if (!car.IsVisible || !DrawCars)
                            continue;

                        MainWindow.GetMain.Dispatcher.Invoke(UpdateOnCanvas(carsArt[i], car));
                    }
                    
                }
        }

        public static void UpdateAllTurnpikes(bool turnpikeStatus)
        {
            lock (turnpikes)
            {
                for(int i = 0; i < turnpikes.Count; i++)
                {
                    Turnpike turnpike = turnpikes[i];
                    if (IsCarOnRailWay())
                        continue;
                    turnpike.Opened = turnpikeStatus;
                    turnpike.Update();

                    MainWindow.GetMain.Dispatcher.Invoke(UpdateOnCanvas(turnpikesArt[i], turnpike.CurrentGraphic));
                }
            }
        }

        private static bool IsCarOnRailWay()
        {
            lock (cars)
            {
                bool isCarOnRailway = false;
                for (int i = 0; i < cars.Count; i++)
                {
                    Car car = cars[i];
                    if (car.CounterNodes != (carNodes.Count - RailsNodeIndex + 1))
                    {
                        continue;
                    }

                    if (car.GetRelativeDistanceTravelRatio() > 0.2f)
                    {
                        car.IgnoreCanGoThrough();
                        isCarOnRailway = true;
                    }
                }

                return isCarOnRailway;
            }
        }

        public static void UpdateAllLights(bool turnpikeStatus)
        {
            for (int i = 0; i < lights.Count; i++)
            {
                Light light = lights[i];
                light.SetStatus(turnpikeStatus);
                light.Update();
                MainWindow.GetMain.Dispatcher.Invoke(UpdateOnCanvas(lightsArt[i], light.CurrentGraphic));
            }
        }

        private static Action UpdateOnCanvas(Image image, BitmapImage newImage)
        {
            return () =>
            {
                image.Source = newImage;
            };
        }

        private static Action UpdateOnCanvas(Image image, Vehicle vehicle)
        {
            return () =>
            {
                image.Source = vehicle.CurrentGraphics;
                Canvas.SetLeft(image, vehicle.ActualPosition.X);
                Canvas.SetTop(image, vehicle.ActualPosition.Y);
            };
        }

        #endregion

        #region Reincarnation

        public static void ReincarnateTrain(Train train)
        {
            lock (train)
            {
                //train = TrainFactory.Create(trainNodes.Count, train.NextVehicleIndex);
                train.DeathAfterArivalTime = TrainFactory.DEATH_TIME;
                train.CounterNodes = trainNodes.Count;
                train.ResetPosition();
                train.UpdatePositionVector(train.CounterNodes);
                train.CanMove = true;
                train.IsActive = true;
            }
        }

        private static void ReincarnateCar(Car car)
        {
            car.IsActive = true;
            car.CounterNodes = carNodes.Count;
            car.VehicleSpeed = CarFactory.RandomSpeedGenerator();
            car.ResetPosition();
            car.EnableVehicle();
        }

        #endregion

        #region Checks

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
                {
                    return false;
                }

                Vehicle nextCar = cars[thisVehicle.NextVehicleIndex];

                //calculate distance between two vehicles
                double nextVehicleBack = nextCar.TraveledDistance;
                double thisVehicleFront = thisVehicle.TraveledDistance;
                double differenceInDistance = Math.Abs(nextVehicleBack - thisVehicleFront);
                bool areTooClose = differenceInDistance < Vehicle.VEHICLE_DISTANCE_OFFSET;
                return areTooClose;
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
            if (vehicle.NextVehicleIndex <= -1)
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

        private static bool IsInBetween(double value, float smallerLimit, float biggerLimit)
        {
            if (smallerLimit > biggerLimit)
            {
                float temp = smallerLimit;
                smallerLimit = biggerLimit;
                biggerLimit = temp;
            }

            bool isInBetweenValues = (value >= smallerLimit & value <= biggerLimit);
            return isInBetweenValues;
        }

        #endregion

        #region Gets

        public static Node GetCarNode(int nodesLeftToTravel) => GetNode(carNodes, nodesLeftToTravel);

        public static Node GetTrainNode(int nodesLeftToTravel) => GetNode(trainNodes, nodesLeftToTravel);

        public static Node GetNode(List<Node> nodesArray, int index)
        {
            lock (nodesArray)
            {
                index--;
                int currentNodeIndex = nodesArray.Count() - 1 - index;

                if (currentNodeIndex < 0)
                    return null;
                if (currentNodeIndex > nodesArray.Count() - 1)
                    return null;
                return nodesArray[currentNodeIndex];
            }
        }

        public static BitmapImage GetNextCarGraphic(double normalizedX, double normalizedY)
        {
            lock (directions)
            {
                bool determined = false;
                int selectedDirection = (int)Enums.GraphicDirection.UP_RIGHT;
                for (int i = 0; i < CAR_DIRECTIONS; i++)
                {
                    if (!IsInBetween(normalizedX, directions[i, 0], directions[i, 1]))
                        continue;

                    if (!IsInBetween(normalizedY, directions[i, 2], directions[i, 3]))
                        continue;

                    selectedDirection = i;
                    determined = true;
                    break;
                }

                if (determined)
                    return carsBitmaps[selectedDirection];
                else
                {
                    int index = normalizedX >= 0.0f ? (int)Enums.GraphicDirection.RIGHT : (int)Enums.GraphicDirection.LEFT;
                    return carsBitmaps[index];
                }
            }
        }

        public static BitmapImage GetNextTrainGraphic(double normalizedX, double normalizedY)
        {
            lock (directions)
            {
                return normalizedX <= 0 ? trainsBitmaps[0] : trainsBitmaps[1];
            }
        }

        public static double GetNextVehicleSpeed(Vehicle vehicle)
        {
            lock (cars)
            {
                if (vehicle.NextVehicleIndex == -1)
                    return vehicle.VehicleSpeed;
                return cars[vehicle.NextVehicleIndex].CurrentSpeed;
            }
        }

        #endregion

        public static void TriggerTurnPike()
        {
            lock (carNodes)
            {
                carNodes[RailsNodeIndex].CanGoThrough = !carNodes[RailsNodeIndex].CanGoThrough;
            }
        }

        public static bool TurnPikeStatus()
        {
            return carNodes[RailsNodeIndex].CanGoThrough;
        }

    }
}
