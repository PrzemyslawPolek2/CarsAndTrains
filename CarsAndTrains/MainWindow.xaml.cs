using CarsAndTrains.Classes;
using CarsAndTrains.Classes.Controllers;
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
        private const string NODE_POSITION_FILE_NAME = "/nodePositions.txt";
        private const int NODE_SIZE = 20;
        public static MainWindow GetMain;
        public static bool CreateNode = false;
        public MainWindow()
        {
            GetMain = this;
            InitializeComponent();
            if (CreateNode)
                return;
            PublicAvaliableReferences.Initialize(canvas);
            StartThreads();
        }

        private async static void StartThreads()
        {
            CarsController carsController = new CarsController();
            carsController.Start();
        }

        private void CanvasMouseDownEventHandler(object sender, MouseButtonEventArgs e)
        {
            if (!CreateNode)
                return;

            Point canvasPoint = Mouse.GetPosition(canvas);

            double nodePositionX = (canvasPoint.X - (NODE_SIZE / 2));
            double nodePositionY = (canvasPoint.Y - (NODE_SIZE / 2));

            string positionsString = nodePositionX + " " + nodePositionY;
            
            CreateNodePosition(positionsString);
            Ellipse ellipse = new Ellipse
            {
                Width = NODE_SIZE,
                Height = NODE_SIZE,
                Fill = new SolidColorBrush
                {
                    Color = Color.FromArgb(255,
                                                   128,
                                                   255,
                                                   0)
                }
            };
            Canvas.SetLeft(ellipse, nodePositionX);
            Canvas.SetTop(ellipse, nodePositionY);
            Panel.SetZIndex(ellipse, 5);
            canvas.Children.Add(ellipse);
        }

        private static void CreateNodePosition(string str)
        {
            string path = System.Reflection.Assembly.GetEntryAssembly().Location;
            path = System.IO.Path.GetDirectoryName(path) + NODE_POSITION_FILE_NAME;

            if (!File.Exists(path))
            {
                using (StreamWriter sw = File.CreateText(path))
                {
                    sw.WriteLine(str);
                }
            }
            else
            {
                using (StreamWriter sw = File.AppendText(path))
                {
                    sw.WriteLine(str);
                }
            }
        }

        public double GetCanvasWidth()
        {
            return canvas.Width;
        }
        public double GetCanvasHeight()
        {
            return canvas.Height;
        }
    }
}
