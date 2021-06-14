using CarsAndTrains.Classes.Nodes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace CarsAndTrains.Classes.Objects
{
    public class LightFactory
    {
        public static void Initialize()
        {
            
        }

        public static Light Create(Point position)
        {
            Light light = new Light(position);
            return light;
        }
    }
}
