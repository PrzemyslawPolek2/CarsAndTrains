using System;
using System.Collections.Generic;
using System.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.IO;
using System.Diagnostics;

namespace CarsAndTrains.Classes
{
    class PublicAvaliableReferences
    {
        protected static List<Train> trains;
        protected static List<Car> cars;
        protected static List<Node> carNodes;
        protected static List<Node> trainNodes;

        protected string[] vehicleGraphicsURL = new string[8];
        protected string[] trainGraphicsURL = new string[8];
        protected static Canvas canvas;
        private const int LIMIT = 6;

        protected static bool drawNodes = true; 
        protected static bool drawCars = true; 
        protected static bool drawTrains = true; 


        public static void Initialize(Canvas passedCanvas)
        {
            canvas = passedCanvas;
            //CreateCarsPool();
            //CreateTrainsPool();
            CreateNodes();
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
            
            string path = System.Reflection.Assembly.GetEntryAssembly().Location;
            path = System.IO.Path.GetDirectoryName(path) + "/nodePositions.txt";
            using (StreamReader sr = File.OpenText(path))
            {
                string s = "";
                while ((s = sr.ReadLine()) != null)
                {
                    string[] values = s.Split(' ');
                    if (drawNodes)
                    {
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
                        Canvas.SetLeft(ellipse, float.Parse(values[0]));
                        Canvas.SetTop(ellipse, float.Parse(values[1]));
                        Panel.SetZIndex(ellipse, 5);
                        canvas.Children.Add(ellipse);
                    }
                    
                }
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
            cars = new List<Car>();
            throw new NotImplementedException();

        }

        public static bool IsCarTravelingFromNode(Node node)
        {
            throw new NotImplementedException();
            foreach (Car car in cars)
            {
                return false;
            }    
        }

        public static void UpdateAllVehicles()
        {
            throw new NotImplementedException();
            lock (cars)
            {
                foreach(Car car in cars)
                {
                    car.updateVehicle();
                }
            }
        }

        public static bool IsVehicleInTheWay(Car car)
        {
            throw new NotImplementedException();
            return false;
        }

    }
}
