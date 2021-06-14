using System.Windows;

namespace CarsAndTrains.Classes.Objects
{
    public class TurnpikeFactory
    {
        public static void Initialize()
        {

        }

        public static Turnpike Create(Point position, bool left = false)
        {
            Turnpike turnpike = new Turnpike(left, position);
            return turnpike;
        }
    }
}
