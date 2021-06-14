using CarsAndTrains.Classes;
using CarsAndTrains.Classes.Vehicles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace CarsAndTrains
{
    /// <summary>
    /// Interaction logic for ObjectsDisplayWindow.xaml
    /// </summary>
    public partial class ObjectsDisplayWindow : Window
    {
        public static ObjectsDisplayWindow GetDisplayWindow;
        public static int TickCounter = 0;
        public ObjectsDisplayWindow()
        {
            GetDisplayWindow = this;
            InitializeComponent();
            Initialize();
        }

        private void Initialize()
        {

            List<Car> cars = PublicAvaliableReferences.cars;
            CarsLB.Items.Add(Car.Header());
            for (int i = 0; i < cars.Count; i++)
                CarsLB.Items.Add(cars[i].ToString());
            List<Train> trains = PublicAvaliableReferences.trains;
            TrainsLB.Items.Add(Car.Header());
            for (int i = 0; i < trains.Count; i++)
                TrainsLB.Items.Add(trains[i].ToString());
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

    }
}
