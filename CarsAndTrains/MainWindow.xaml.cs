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
        public static MainWindow GetMain;
        public static bool CreateNode = false;
        public MainWindow()
        {
            GetMain = this;
            InitializeComponent();
            PublicAvaliableReferences.Initialize(canvas);
            clickPositionL.Content = "[" + GetMain.GetCanvasHeight() + ";" + GetMain.GetCanvasWidth() + "]";
            StartThreads();
        }

        private async static void StartThreads()
        {
            CarsController carsController = new CarsController();
            carsController.Start();
        }

        private void CanvasMouseDownEventHandler(object sender, MouseButtonEventArgs e)
        {
            Point p = Mouse.GetPosition(canvas);
            string str = p.X + " " + p.Y;

            clickPositionL.Content = str;

            if (!CreateNode)
                return;
            CreateNodePosition(str);
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
            Canvas.SetLeft(ellipse, p.X);
            Canvas.SetTop(ellipse, p.Y);
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
