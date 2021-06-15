using System.Windows;

namespace CarsAndTrains.Classes.Objects
{
    /// <summary>
    /// Factory class that creates a <see cref="Turnpike"></see> class instance
    /// </summary>
    public class TurnpikeFactory
    {
        /// <summary>
        /// Initializes the Factory
        /// </summary>
        public static void Initialize()
        {

        }
        /// <summary>
        /// Creates a new <see cref="Turnpike"/>
        /// </summary>
        /// <param name="position"> position on Canvas</param>
        /// <param name="left">is turnpike leftsided</param>
        /// <returns></returns>
        public static Turnpike Create(Point position, bool left = false)
        {
            Turnpike _turnpike = new Turnpike(left, position);
            return _turnpike;
        }
    }
}
