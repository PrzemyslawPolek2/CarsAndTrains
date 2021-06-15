using CarsAndTrains.Classes.Nodes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace CarsAndTrains.Classes.Objects
{
    /// <summary>
    /// Factory class that creates a <see cref="Light"></see> class instance
    /// </summary>
    public class LightFactory
    {
        /// <summary>
        /// Initializes Factory
        /// </summary>
        public static void Initialize()
        {
            
        }
        /// <summary>
        /// Creates a new <see cref="Light"/>
        /// </summary>
        /// <param name="position">position on canvas</param>
        /// <returns></returns>
        public static Light Create(Point position)
        {
            Light light = new Light(position);
            return light;
        }
    }
}
