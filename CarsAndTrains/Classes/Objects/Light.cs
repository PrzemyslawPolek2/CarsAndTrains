using System.Windows;
using System.Windows.Media.Imaging;

namespace CarsAndTrains.Classes.Objects
{
    public class Light
    {
        public BitmapImage CurrentGraphic { get; protected set; }
        public static BitmapImage LightsOff;
        public static BitmapImage[] LightsOn = new BitmapImage[2];
        public Point ActualPosition;

        public Light(Point actualPosition)
        {
            ActualPosition = actualPosition;
            UpdateGraphic();
        }

        public bool Lights { get; set; } = false;
        public bool LeftBlink { get; set; } = false;

        private int _lightSwitchDelay = 10;
        private int _lightSwitchCurrent = 0;


        public void SetStatus(bool status)
        {
            Lights = !status;
        }
        public void Update()
        {
            UpdateGraphic();
        }

        private void UpdateGraphic()
        {
            if (Lights)
            {
                _lightSwitchCurrent++;
                if (_lightSwitchCurrent != _lightSwitchDelay)
                    return;
                CurrentGraphic = LeftBlink ? LightsOn[0] : LightsOn[1];
                LeftBlink = !LeftBlink;
                _lightSwitchCurrent = 0;
            }
            else
                CurrentGraphic = LightsOff;
        }
    }
}
