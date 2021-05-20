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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace CarsAndTrains
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public static MainWindow GetMain;
        
        public MainWindow()
        {
            GetMain = this;
            InitializeComponent();
            clickPositionL.Content = "[" + GetMain.getCanvasHeight() + ";" + GetMain.getCanvasWidth() + "]";
        }

        private void CanvasMouseDownEventHandler(object sender, MouseButtonEventArgs e)
        {
            Point p = Mouse.GetPosition(canvas);
            clickPositionL.Content = "[" + p.X + ";" + p.Y + "]";
        }

        public double getCanvasWidth()
        {
            return canvas.Width;
        }
        public double getCanvasHeight()
        {
            return canvas.Height;
        }
    }
}
