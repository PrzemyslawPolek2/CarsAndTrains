using System.Windows;
using System.Windows.Media.Imaging;

namespace CarsAndTrains.Classes.Objects
{
    public class Turnpike
    {
        public BitmapImage CurrentGraphic { get; protected set; }
        public static BitmapImage[,] TurnpikeGraphic = new BitmapImage[2,2];
        public Point ActualPosition;
        public Turnpike(bool left, Point position)
        {
            this.ActualPosition = position;
            Left = left;
            UpdateGraphic();
        }

        public bool Opened { get; set; } = true;
        public bool Left { get; set; } = false;

        public void Update()
        {
            UpdateGraphic();
        }

        private void UpdateGraphic()
        {
            CurrentGraphic = TurnpikeGraphic[Left ? 0 : 1, Opened ? 0 : 1];
        }
    }
}
