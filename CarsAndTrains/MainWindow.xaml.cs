using CarsAndTrains.Classes;
using CarsAndTrains.Classes.Controllers;
using CarsAndTrains.Classes.Nodes;
using CarsAndTrains.Classes.Vehicles;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

namespace CarsAndTrains
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public static MainWindow GetMain;
        public static List<Controller> threads;
        public static int TickCounter = 0;

        public static bool MouseNodeCreation = false;
        public static bool MouseTrainNodeCreation = false;
        public static bool MouseCarNodeCreation = false;

        public MainWindow()
        {
            GetMain = this;
            InitializeComponent();

            //If Creating Nodes, don't initialize PAR and Threads
            if (MouseNodeCreation)
                return;

            //Initalize PAR (shared resources)
            PublicAvaliableReferences.Initialize(canvas);
            InitializeInfoDisplay();
            CreateThreads();
            StartThreads();

            this.Closing += MainWindow_Closing;
        }

        #region Threads
        private static void CreateThreads()
        {
            threads = new List<Controller>
            {
                new CarsController(),
                new TrainsController(),
                new TurnpikesAndLightsController(),
                new VehiclesInfoController()
            };
        }

        private static void AbortThreads()
        {
            foreach (Controller controller in threads)
                controller.Abort();
        }
        private static void StartThreads()
        {
            foreach (Controller controller in threads)
                controller.Start();
        }
        #endregion

        #region Canvas Mouse Down Events
        private void CanvasRightMouseDownEventHandler(object sender, MouseButtonEventArgs e)
        {
            if (!MouseNodeCreation)
                return;
            Point _canvasPoint = Mouse.GetPosition(canvas);
            SaveNewNode(_canvasPoint, Node.TRAIN_TRIGGER_NODE);
        }

        private void CanvasLeftMouseDownEventHandler(object sender, MouseButtonEventArgs e)
        {
            if (!MouseNodeCreation)
                return;
            Point _canvasPoint = Mouse.GetPosition(canvas);
            SaveNewNode(_canvasPoint, Node.NORMAL_NODE);
        }

        #endregion

        #region Create Nodes 

        /// <summary>
        /// Create new Node
        /// </summary>
        /// <param name="canvasPoint"> point at which new node is created</param>
        /// <param name="trainTriggeringNode">1 if node is a triggernode, otherwise 0</param>
        private void SaveNewNode(Point canvasPoint, string trainTriggeringNode)
        {
            double _nodePositionX = (canvasPoint.X - (Node.NODE_SIZE / 2));
            double _nodePositionY = (canvasPoint.Y - (Node.NODE_SIZE / 2));

            string _positionsString = _nodePositionX + " " + _nodePositionY;
            if (MouseCarNodeCreation)
                CreateCarNodePosition(_positionsString);
            if (MouseTrainNodeCreation)
                CreateTrainNodePosition($"{_positionsString} {trainTriggeringNode}");
            PublicAvaliableReferences.CreateNodeArt(this.canvas,
                                                    _nodePositionX,
                                                    _nodePositionY,
                                                    255,
                                                    (byte)(int.Parse(trainTriggeringNode) * PublicAvaliableReferences.ALPHA),
                                                    0);
        }

        /// <summary>
        /// Creates a node record for trains in the trains.txt file
        /// </summary>
        /// <param name="positionsString">string containing position and IsTrainTriggerValue</param>
        private void CreateTrainNodePosition(string positionsString)
        {
            string _defaultFolderPath = System.Reflection.Assembly.GetEntryAssembly().Location;
            _defaultFolderPath = System.IO.Path.GetDirectoryName(_defaultFolderPath);
            _defaultFolderPath += PublicAvaliableReferences.TRAIN_NODES_FILE_NAME;

            if (!File.Exists(_defaultFolderPath))
            {
                using (StreamWriter _streamWriter = File.CreateText(_defaultFolderPath))
                {
                    _streamWriter.WriteLine(positionsString);
                }
            }
            else
            {
                using (StreamWriter _streamWriter = File.AppendText(_defaultFolderPath))
                {
                    _streamWriter.WriteLine(positionsString);
                }
            }
        }
        /// <summary>
        /// Creates a node record for cars in the cars.txt file
        /// </summary>
        /// <param name="positionsString">string containing position</param>
        private static void CreateCarNodePosition(string positionString)
        {
            string _defaultFolderPath = System.Reflection.Assembly.GetEntryAssembly().Location;
            _defaultFolderPath = System.IO.Path.GetDirectoryName(_defaultFolderPath);
            _defaultFolderPath += PublicAvaliableReferences.CAR_NODES_FILE_NAME;

            if (!File.Exists(_defaultFolderPath))
            {
                using (StreamWriter _streamWriter = File.CreateText(_defaultFolderPath))
                {
                    _streamWriter.WriteLine(positionString);
                }
            }
            else
            {
                using (StreamWriter _streamWriter = File.AppendText(_defaultFolderPath))
                {
                    _streamWriter.WriteLine(positionString);
                }
            }
        }

        #endregion

        /// <summary>
        /// Initialiazes ListBoxes with the proper values
        /// </summary>
        private void InitializeInfoDisplay()
        {
            ///Cars
            List<Car> _cars = PublicAvaliableReferences.cars;
            CarsLB.Items.Add(Car.Header());
            for (int i = 0; i < _cars.Count; i++)
                CarsLB.Items.Add(_cars[i].ToString());
            ///Trains
            List<Train> _trains = PublicAvaliableReferences.trains;
            TrainsLB.Items.Add(Car.Header());
            for (int i = 0; i < _trains.Count; i++)
                TrainsLB.Items.Add(_trains[i].ToString());
            ///Nodes
            //List<Node> _carNodes = PublicAvaliableReferences.carNodes;
            //CarNodesLB.Items.Add(Node.Header());
            //for (int i = 0; i < _carNodes.Count; i++)
            //    CarNodesLB.Items.Add(_carNodes[i].ToString());
        }

        #region LB Updates

        public void UpdateCarsLB()
        {
            if (CarsLB.Items.Count == 0)
                return;

            List<Car> _cars = PublicAvaliableReferences.cars;
            for (int i = 0; i < _cars.Count; i++)
            {
                Dispatcher.Invoke(() =>
                {
                    CarsLB.Items[i + 1] = ($"Car {_cars[i]}");
                });
            }

        }

        public void UpdateTrainsLB()
        {
            if (TrainsLB.Items.Count == 0)
                return;
            List<Train> _trains = PublicAvaliableReferences.trains;
            for (int i = 0; i < _trains.Count; i++)
            {
                Dispatcher.Invoke(() =>
                {
                    TrainsLB.Items[i + 1] = $"Train {_trains[i]}";
                });
            }
        }

        public void UpdateCarNodesLB()
        {
            if (CarNodesLB.Items.Count == 0)
                return;

            List<Node> _carNodes = PublicAvaliableReferences.carNodes;
            for (int i = 0; i < _carNodes.Count; i++)
            {
                Dispatcher.Invoke(() =>
                {
                    CarNodesLB.Items[i + 1] = $"Node {_carNodes.Count - i} {_carNodes[i]}";
                });
            }
        }

        #endregion

        private void MainWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            AbortThreads();
        }

    }
}
