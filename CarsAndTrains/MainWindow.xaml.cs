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
            Point canvasPoint = Mouse.GetPosition(canvas);
            SaveNewNode(canvasPoint, Node.TRAIN_TRIGGER_NODE);
        }

        private void CanvasLeftMouseDownEventHandler(object sender, MouseButtonEventArgs e)
        {
            if (!MouseNodeCreation)
                return;
            Point canvasPoint = Mouse.GetPosition(canvas);
            SaveNewNode(canvasPoint, Node.NORMAL_NODE);
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
            double nodePositionX = (canvasPoint.X - (Node.NODE_SIZE / 2));
            double nodePositionY = (canvasPoint.Y - (Node.NODE_SIZE / 2));

            string positionsString = nodePositionX + " " + nodePositionY;
            if (MouseCarNodeCreation)
                CreateCarNodePosition(positionsString);
            if (MouseTrainNodeCreation)
                CreateTrainNodePosition($"{positionsString} {trainTriggeringNode}");
            PublicAvaliableReferences.CreateNodeArt(this.canvas,
                                                    nodePositionX,
                                                    nodePositionY,
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
            string path = System.Reflection.Assembly.GetEntryAssembly().Location;
            path = System.IO.Path.GetDirectoryName(path);
            path += PublicAvaliableReferences.TRAIN_NODES_FILE_NAME;

            if (!File.Exists(path))
            {
                using (StreamWriter sw = File.CreateText(path))
                {
                    sw.WriteLine(positionsString);
                }
            }
            else
            {
                using (StreamWriter streamWriter = File.AppendText(path))
                {
                    streamWriter.WriteLine(positionsString);
                }
            }
        }
        /// <summary>
        /// Creates a node record for cars in the cars.txt file
        /// </summary>
        /// <param name="positionsString">string containing position</param>
        private static void CreateCarNodePosition(string positionString)
        {
            string path = System.Reflection.Assembly.GetEntryAssembly().Location;
            path = System.IO.Path.GetDirectoryName(path);
            path += PublicAvaliableReferences.CAR_NODES_FILE_NAME;

            if (!File.Exists(path))
            {
                using (StreamWriter streamWriter = File.CreateText(path))
                {
                    streamWriter.WriteLine(positionString);
                }
            }
            else
            {
                using (StreamWriter sw = File.AppendText(path))
                {
                    sw.WriteLine(positionString);
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
            List<Car> cars = PublicAvaliableReferences.cars;
            CarsLB.Items.Add(Car.Header());
            for (int i = 0; i < cars.Count; i++)
                CarsLB.Items.Add(cars[i].ToString());
            ///Trains
            List<Train> trains = PublicAvaliableReferences.trains;
            TrainsLB.Items.Add(Car.Header());
            for (int i = 0; i < trains.Count; i++)
                TrainsLB.Items.Add(trains[i].ToString());
            ///Nodes
            //List<Node> carNodes = PublicAvaliableReferences.carNodes;
            //CarNodesLB.Items.Add(Node.Header());
            //for (int i = 0; i < carNodes.Count; i++)
            //    CarNodesLB.Items.Add(carNodes[i].ToString());
        }

        #region LB Updates

        public void UpdateCarsLB()
        {
            if (CarsLB.Items.Count == 0)
                return;

            List<Car> cars = PublicAvaliableReferences.cars;
            for (int i = 0; i < cars.Count; i++)
            {
                Dispatcher.Invoke(() =>
                {
                    CarsLB.Items[i + 1] = ($"Car {cars[i]}");
                });
            }

        }

        public void UpdateTrainsLB()
        {
            if (TrainsLB.Items.Count == 0)
                return;
            List<Train> trains = PublicAvaliableReferences.trains;
            for (int i = 0; i < trains.Count; i++)
            {
                Dispatcher.Invoke(() =>
                {
                    TrainsLB.Items[i + 1] = $"Train {trains[i]}";
                });
            }
        }

        public void UpdateCarNodesLB()
        {
            if (CarNodesLB.Items.Count == 0)
                return;

            List<Node> carNodes = PublicAvaliableReferences.carNodes;
            for (int i = 0; i < carNodes.Count; i++)
            {
                Dispatcher.Invoke(() =>
                {
                    CarNodesLB.Items[i + 1] = $"Node {carNodes.Count - i} {carNodes[i]}";
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
