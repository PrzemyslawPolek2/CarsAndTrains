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
        public const float TICK_VALUE = 1.0f;
        public const int ALPHA = 255;
        protected const float DEATH_TICK_VALUE = 1.0f;
        protected const int SPAWN_DELAY = 100;

        public static bool DrawNodes { get; set; } = false;
        public static bool IsCarPoolFinished { get; protected set; }

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

        protected static int RailsNodeIndex { get; set; } = 12;

        private static readonly float[,] DIRECTIONS =
        {
            // Y value increases the lower object is on canvas - Y value is inversed

            //X-min      X-max      Y-min       Y-max
            {-.25f,     0.25f,      -.75f,       -1.0f},       // UP
            {0.00f,     0.75f,      -.00f,       -.75f},       // UP_RIGHT
            {0.75f,     1.0f,       0.25f,       -.25f},       // RIGHT
            {0.00f,     0.75f,      0.00f,       0.75f},       // DOWN_RIGHT
            {-.25f,     0.25f,      0.75f,       1.00f},       // DOWN
            {-.75f,     0.00f,      0.00f,       0.75f},       // DOWN_LEFT
            {-1.0f,     -.75f,      0.25f,       -.25f},       // LEFT
            {-.75f,     0.00f,      -.00f,       -.75f},       // UP_LEFT
        };


        #region Initialization

        public static void Initialize(Canvas passedCanvas)
        {
            canvas = passedCanvas;
            IsCarPoolFinished = false;
            CreateBitmapImages();
            CreateNodes();
            CreatePools();
        }

        /// <summary>
        /// Creates all <see cref="List{T}"/> of <see cref="Node"/>
        /// </summary>
        private static void CreateNodes()
        {
            CreateCarsNodes();
            CreateTrainNodes();
        }
        /// <summary>
        /// Creates all <see cref="List{T}"/> of <see cref="Vehicle"/>, <see cref="Turnpike"/>, <see cref="Light"/>
        /// </summary>
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

            //reset
            Car.FullTravelDistance = 0;

            //path to the folder with the nodes.txt
            string _mainPath = System.Reflection.Assembly.GetEntryAssembly().Location;
            _mainPath = System.IO.Path.GetDirectoryName(_mainPath) + CAR_NODES_FILE_NAME;

            using (StreamReader _streamReader = File.OpenText(_mainPath))
            {
                string _line = "";
                while ((_line = _streamReader.ReadLine()) != null)
                {
                    string[] _values = _line.Split(' ');
                    float _xValue = float.Parse(_values[0]);
                    float _yValue = float.Parse(_values[1]);
                    carNodes.Add(new Node(new Point(_xValue, _yValue)));
                    if (!DrawNodes)
                        continue;
                    CreateNodeArt(_xValue, _yValue);
                }
            }

            //Calculate Vector for all nodes
            for (int i = 0; i < carNodes.Count; i++)
            {
                Node _node = carNodes[i];
                if (i + 1 >= carNodes.Count)
                {
                    // don't go to the last Node; casues a weird "move across screen" bug
                    _node.CanGoTo = false; 
                    _node.CalculateVector(carNodes[i]);
                }
                else
                    _node.CalculateVector(carNodes[i + 1]);
                //Sum of the vector lengths
                Car.FullTravelDistance += _node.Vector.Length;
            }
        }

        private static void CreateTrainNodes()
        {
            if (trainNodes is null)
                trainNodes = new List<Node>();

            string _mainPath = System.Reflection.Assembly.GetEntryAssembly().Location;
            _mainPath = System.IO.Path.GetDirectoryName(_mainPath) + TRAIN_NODES_FILE_NAME;

            using (StreamReader _streamReader = File.OpenText(_mainPath))
            {
                int i = 0;
                string _readLine = "";
                while ((_readLine = _streamReader.ReadLine()) != null)
                {
                    string[] _values = _readLine.Split(' ');
                    double _xValue = double.Parse(_values[0]);
                    double _yValue = double.Parse(_values[1]);
                    int _triggeringNode = int.Parse(_values[2]);

                    //if _triggeringNode == 1; this Node should be a Trigger Node
                    if (_triggeringNode == 0)
                    {
                        Node _trainNode = new Node(new Point(_xValue, _yValue));
                        trainNodes.Add(_trainNode);
                    }
                    else
                    {
                        TrainTriggerNode _trainNode = new TrainTriggerNode(new Point(_xValue, _yValue));
                        trainNodes.Add(_trainNode);
                    }

                    if (DrawNodes)
                        CreateNodeArt(canvas, _xValue, _yValue, 255, (byte)(_triggeringNode * ALPHA), 128, i);
                    i++;

                }
            }

            //Calculate Vector for all nodes
            for (int i = 0; i < trainNodes.Count; i++)
            {
                Node _node = trainNodes[i];
                if (i + 1 >= trainNodes.Count)
                {
                    _node.CalculateVector();
                }
                else
                    _node.CalculateVector(trainNodes[i + 1]);
            }
        }

        #endregion

        #region Bitmaps and NodeArts
        /// <summary>
        /// Import Bitmaps from Resources
        /// </summary>
        private static void CreateBitmapImages()
        {
            string _defaultResourcesFolder = System.IO.Path.GetDirectoryName(System.IO.Path.GetDirectoryName(Directory.GetCurrentDirectory()));
            string _fileExtension = ".png";
            string _path;

            //Cars 
            _path = $"{_defaultResourcesFolder}{CAR_RESOURCES_FOLDER}{CAR_IMAGE_PREFIX}";
            for (int i = 0; i < CAR_DIRECTIONS; i++)
                carsBitmaps[i] = new BitmapImage(new Uri($"{_path}{i}{_fileExtension}"));
            

            //Trains           
            _path = $"{_defaultResourcesFolder}{TRAIN_RESOURCES_FOLDER}{TRAIN_IMAGE_PREFIX}";
            for (int i = 0; i < TRAIN_DIRECTIONS; i++)
                trainsBitmaps[i] = new BitmapImage(new Uri($"{_path}{i}{_fileExtension}"));
            

            //Lights          
            _path = $"{_defaultResourcesFolder}{LIGHT_RESOURCES_FOLDER}{LIGHT_IMAGE_PREFIX}";
            Light.lightsOff = new BitmapImage(new Uri($"{_path}{0}{_fileExtension}"));
            Light.lightsOn[0] = new BitmapImage(new Uri($"{_path}{1}{_fileExtension}"));
            Light.lightsOn[1] = new BitmapImage(new Uri($"{_path}{2}{_fileExtension}"));

            //Turnpikes
            _path = $"{_defaultResourcesFolder}{TURNPIKE_RESOURCES_FOLDER}{TURNPIKE_IMAGE_PREFIX}";
            Turnpike.TurnpikeGraphic[0, 0] = new BitmapImage(new Uri($"{_path}{0}_{0}{_fileExtension}"));
            Turnpike.TurnpikeGraphic[0, 1] = new BitmapImage(new Uri($"{_path}{0}_{1}{_fileExtension}"));
            Turnpike.TurnpikeGraphic[1, 0] = new BitmapImage(new Uri($"{_path}{1}_{0}{_fileExtension}"));
            Turnpike.TurnpikeGraphic[1, 1] = new BitmapImage(new Uri($"{_path}{1}_{1}{_fileExtension}"));

        }

        public static void CreateNodeArt(double xValue, double yValue, byte red = 128, byte green = 255, byte blue = 0) => CreateNodeArt(canvas, xValue, yValue, red, green, blue);

        public static void CreateNodeArt(Canvas canvas, double xValue, double yValue, byte red = 128, byte green = 255, byte blue = 0, int nodeInddex = -1)
        {
            Ellipse _ellipse = new Ellipse
            {
                Width = Node.NODE_SIZE,
                Height = Node.NODE_SIZE,
                Fill = new SolidColorBrush
                {
                    Color = Color.FromArgb(ALPHA,
                                           red,
                                           green,
                                           blue)
                }
            };

            TextBlock _textBlock = new TextBlock
            {
                Text = nodeInddex == -1 ? "" : $"{nodeInddex}",
                TextAlignment = TextAlignment.Center,
                FontSize = 10,
            };
            Canvas.SetLeft(_ellipse, xValue);
            Canvas.SetTop(_ellipse, yValue);
            Canvas.SetLeft(_textBlock, xValue + Node.NODE_SIZE / 2);
            Canvas.SetTop(_textBlock, yValue + (Node.NODE_SIZE / 2));

            Panel.SetZIndex(_ellipse, 5);
            Panel.SetZIndex(_textBlock, 5);
            canvas.Children.Add(_ellipse);
            canvas.Children.Add(_textBlock);
        }
        #endregion

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
                    car.DeathTime = SPAWN_DELAY * i;
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

            Turnpike _turnpikeTop = TurnpikeFactory.Create(new Point(50, 330));
            Turnpike _turnpikeBottom = TurnpikeFactory.Create(new Point(60, 390), true);

            Image _turnpikeTopImage = new Image
            {
                Source = _turnpikeTop.CurrentGraphic,
                Width = Turnpike.TURNPIKE_WIDTH,
            };
            Image _turnpikeBottomImage = new Image
            {
                Source = _turnpikeBottom.CurrentGraphic,
                Width = Turnpike.TURNPIKE_WIDTH,
            };

            turnpikes.Add(_turnpikeTop);
            turnpikes.Add(_turnpikeBottom);

            turnpikesArt.Add(_turnpikeTopImage);
            turnpikesArt.Add(_turnpikeBottomImage);

            canvas.Children.Add(_turnpikeTopImage);
            canvas.Children.Add(_turnpikeBottomImage);

            Canvas.SetLeft(_turnpikeTopImage, _turnpikeTop.ActualPosition.X);
            Canvas.SetTop(_turnpikeTopImage, _turnpikeTop.ActualPosition.Y);
            Canvas.SetZIndex(_turnpikeTopImage, 5);

            Canvas.SetLeft(_turnpikeBottomImage, _turnpikeBottom.ActualPosition.X);
            Canvas.SetTop(_turnpikeBottomImage, _turnpikeBottom.ActualPosition.Y);
            Canvas.SetZIndex(_turnpikeBottomImage, 10); //has to be higher, because it's "closer"
        }

        private static void CreateLightsPool()
        {
            if (lights is null)
                lights = new List<Light>();
            if (lightsArt is null)
                lightsArt = new List<Image>();

            TurnpikeFactory.Initialize();

            //for 600x800 these are proper positions; I was way to lazy to put it in another file for more flexibility. Sorry.
            Light _lightsTop = LightFactory.Create(new Point(160, 350)); 
            Light _lightsBottom = LightFactory.Create(new Point(20, 410));

            Image _lightsTopImage = new Image
            {
                Source = _lightsTop.CurrentGraphic,
                Width = Light.LIGHT_WIDTH,
            };
            Image _lightsBottomImage = new Image
            {
                Source = _lightsBottom.CurrentGraphic,
                Width = Light.LIGHT_WIDTH,
            };

            lights.Add(_lightsTop);
            lights.Add(_lightsBottom);

            lightsArt.Add(_lightsTopImage);
            lightsArt.Add(_lightsBottomImage);

            canvas.Children.Add(_lightsTopImage);
            canvas.Children.Add(_lightsBottomImage);

            Canvas.SetLeft(_lightsTopImage, _lightsTop.ActualPosition.X);
            Canvas.SetTop(_lightsTopImage, _lightsTop.ActualPosition.Y);
            Panel.SetZIndex(_lightsTopImage, 5);

            Canvas.SetLeft(_lightsBottomImage, _lightsBottom.ActualPosition.X);
            Canvas.SetTop(_lightsBottomImage, _lightsBottom.ActualPosition.Y);
            Panel.SetZIndex(_lightsBottomImage, 10); //has to be higher because it's "closer"
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

                        //if train is not active, revive it after DeathAfterArivalTime
                        if (!train.IsActive)
                        {
                            // if DeathTime is reduced to zero revive train
                            train.DeathTime -= DEATH_TICK_VALUE;
                            if (train.DeathTime > 0.0f)
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
                        Car _car = cars[i];

                        //if car was reincarnated already, skip it
                        if (_car.IsReinacarnated)
                            continue;

                        //if car is not active, revive it after DeathAfterArivalTime
                        if (!_car.IsActive)
                        {
                            // if DeathTime is reduced to zero revive car
                            _car.DeathTime -= DEATH_TICK_VALUE;
                            if (_car.DeathTime <= 0.0f)
                            {
                                ReincarnateCar(_car);
                            }
                            continue;
                        }

                        _car.UpdateVehicle();

                        if (!_car.IsVisible || !DrawCars)
                            continue;

                        MainWindow.GetMain.Dispatcher.Invoke(UpdateOnCanvas(carsArt[i], _car));
                    }
                }
        }

        public static void UpdateAllTurnpikes(bool turnpikeStatus)
        {
            lock (turnpikes) lock (turnpikesArt)
                {
                    for (int i = 0; i < turnpikes.Count; i++)
                    {
                        Turnpike _turnpike = turnpikes[i];
                        //if there is a car on the railway, do not close the turnpike
                        if (IsCarOnRailWay())
                            continue;
                        _turnpike.Opened = turnpikeStatus;
                        _turnpike.Update();

                        MainWindow.GetMain.Dispatcher.Invoke(UpdateOnCanvas(turnpikesArt[i], _turnpike.CurrentGraphic));
                    }
                }
        }

        public static void UpdateAllLights(bool turnpikeStatus)
        {
            lock (lights) lock (lightsArt)
                {
                    for (int i = 0; i < lights.Count; i++)
                    {
                        Light _light = lights[i];
                        _light.SetStatus(turnpikeStatus);
                        _light.Update();
                        MainWindow.GetMain.Dispatcher.Invoke(UpdateOnCanvas(lightsArt[i], _light.CurrentGraphic));
                    }
                }
        }

        private static bool IsCarOnRailWay()
        {
            lock (cars)
            {
                bool _isCarOnRailway = false;
                for (int i = 0; i < cars.Count; i++)
                {
                    Car _car = cars[i];

                    //if car's current Node is not railway node, skip the car
                    if (_car.NodesLeftToTravel != (carNodes.Count - RailsNodeIndex + 1))
                    {
                        continue;
                    }

                    //if car is 10% on the path to the railway, don't let it stop and don't let the turnpikes to close
                    if (_car.GetRelativeDistanceTravelRatio() > 0.1f)
                    {
                        _car.IgnoreCanGoThrough();
                        _isCarOnRailway = true;
                    }
                }

                return _isCarOnRailway;
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
                train.DeathTime = TrainFactory.DEATH_TIME;
                train.NodesLeftToTravel = trainNodes.Count;
                train.ResetPosition();
                train.UpdatePositionVector(train.NodesLeftToTravel);
                train.CanMove = true;
                train.IsActive = true;
            }
        }

        private static void ReincarnateCar(Car car)
        {
            car.IsActive = true;
            car.NodesLeftToTravel = carNodes.Count;
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

        public static bool IsVehicleInTheWay(Vehicle thisVehicle)
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
        /// Checks if the next Vehicle is valid
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

        public static void TriggerTurnPike()
        {
            lock (carNodes)
            {
                carNodes[RailsNodeIndex].CanGoTo = !carNodes[RailsNodeIndex].CanGoTo;
            }
        }
        #endregion

        #region Gets

        public static Node GetCarNode(int nodesLeftToTravel) => GetNode(carNodes, nodesLeftToTravel);

        public static Node GetTrainNode(int nodesLeftToTravel) => GetNode(trainNodes, nodesLeftToTravel);

        public static Node GetNode(List<Node> nodesArray, int rawNodesLeftToTravel)
        {
            lock (nodesArray)
            {
                //rawNodesLeftToTravel are indexed from 0, while the array is indexed from 0
                rawNodesLeftToTravel--;

                //rawNodesLeftToTravel are inversed
                int _nodesCount = nodesArray.Count() - 1;
                int _currentNodeIndex = _nodesCount - rawNodesLeftToTravel;

                if (_currentNodeIndex < 0)
                    return null;
                if (_currentNodeIndex > nodesArray.Count() - 1)
                    return null;
                return nodesArray[_currentNodeIndex];
            }
        }

        public static BitmapImage GetNextCarGraphic(double normalizedX, double normalizedY)
        {
            lock (DIRECTIONS)
            {
                bool _determined = false; //did search loop found matching direction

                // set default as UP_RIGHT direction (easiest to spot for errors in the search alg)
                int _selectedDirection = (int)Enums.GraphicDirection.UP_RIGHT;

                for (int i = 0; i < CAR_DIRECTIONS; i++)
                {
                    //Search for X
                    if (!IsInBetween(normalizedX, DIRECTIONS[i, 0], DIRECTIONS[i, 1]))
                        continue;

                    //Search for Y
                    if (!IsInBetween(normalizedY, DIRECTIONS[i, 2], DIRECTIONS[i, 3]))
                        continue;

                    _selectedDirection = i;
                    _determined = true;
                    break;
                }

                if (_determined)
                    return carsBitmaps[_selectedDirection];
                else
                {
                    // algorithm did not find a proper direction; use RIGHT or LEFT depending on X
                    int _index = normalizedX >= 0.0f ? (int)Enums.GraphicDirection.RIGHT : (int)Enums.GraphicDirection.LEFT;
                    return carsBitmaps[_index];
                }
            }
        }

        public static BitmapImage GetNextTrainGraphic(double normalizedX, double normalizedY)
        {
            lock (trainsBitmaps)
            {
                //train is visible in only two states, left or right
                return normalizedX <= 0 ? trainsBitmaps[0] : trainsBitmaps[1];
            }
        }

        public static double GetNextVehicleSpeed(Vehicle vehicle)
        {
            lock (cars)
            {
                // if nextVehicle doesn't exist, get self-speed
                if (vehicle.NextVehicleIndex == -1)
                    return vehicle.VehicleSpeed;
                return cars[vehicle.NextVehicleIndex].CurrentSpeed;
            }
        }

        public static bool GetTurnPikeStatus()
        {
            return carNodes[RailsNodeIndex].CanGoTo;
        }
        #endregion
    }
}
