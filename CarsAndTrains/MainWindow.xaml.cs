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

        private const int NODE_SIZE = 20;

        public static bool CreateNode = false;
        public static bool CreateTrainNode = false;
        public static bool CreateCarNode = false;
        public MainWindow()
        {
            GetMain = this;
            InitializeComponent();
            if (CreateNode)
                return;
            PublicAvaliableReferences.Initialize(canvas);
            InitializeDebugWindow();
            CreateThreads();
            StartThreads();
            this.Closing += MainWindow_Closing;
        }

        private static void CreateThreads()
        {
            threads = new List<Controller>
            {
                new CarsController(),
                new TrainsController(),
                new TurnpikesController(),
                new DisplayController()
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
        private void CanvasRightMouseDownEventHandler(object sender, MouseButtonEventArgs e)
        {
            if (!CreateNode)
                return;
            Point canvasPoint = Mouse.GetPosition(canvas);
            SaveNewNode(canvasPoint, "1");
        }

        private void CanvasLeftMouseDownEventHandler(object sender, MouseButtonEventArgs e)
        {
            if (!CreateNode)
                return;
            Point canvasPoint = Mouse.GetPosition(canvas);
            SaveNewNode(canvasPoint, "0");
        }
        private void SaveNewNode(Point canvasPoint, string trainTriggeringNode)
        {
            double nodePositionX = (canvasPoint.X - (NODE_SIZE / 2));
            double nodePositionY = (canvasPoint.Y - (NODE_SIZE / 2));

            string positionsString = nodePositionX + " " + nodePositionY;
            if (CreateCarNode)
                CreateCarNodePosition(positionsString);
            if (CreateTrainNode)
                CreateTrainNodePosition($"{positionsString} {trainTriggeringNode}");
            PublicAvaliableReferences.CreateNodeArt(this.canvas,
                                                    nodePositionX,
                                                    nodePositionY,
                                                    255,
                                                    (byte)(int.Parse(trainTriggeringNode) * PublicAvaliableReferences.ALPHA_FULL),
                                                    0);
        }

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
        private void InitializeDebugWindow()
        {

            List<Car> cars = PublicAvaliableReferences.cars;
            CarsLB.Items.Add(Car.Header());
            for (int i = 0; i < cars.Count; i++)
                CarsLB.Items.Add(cars[i].ToString());
            List<Train> trains = PublicAvaliableReferences.trains;
            TrainsLB.Items.Add(Car.Header());
            for (int i = 0; i < trains.Count; i++)
                TrainsLB.Items.Add(trains[i].ToString());
            List<Node> carNodes = PublicAvaliableReferences.carNodes;
            CarNodesLB.Items.Add(Node.Header());
            for (int i = 0; i < carNodes.Count; i++)
                CarNodesLB.Items.Add(carNodes[i].ToString());
        }

        public void UpdateCarsLB()
        {
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
            List<Train> trains = PublicAvaliableReferences.trains;
            for (int i = 0; i < trains.Count; i++)
            {
                Dispatcher.Invoke(() =>
                {
                    TrainsLB.Items[i + 1] = $"Train {trains[i]}";
                });
            }
        }

        private void MainWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            AbortThreads();
        }

        internal void UpdateCarNodesLB()
        {
            List<Node> carNodes= PublicAvaliableReferences.carNodes;
            for (int i = 0; i < carNodes.Count; i++)
            {
                Dispatcher.Invoke(() =>
                {
                    CarNodesLB.Items[i + 1] = $"Node {carNodes.Count - i} {carNodes[i]}";
                });
            }
        }
    }
}
