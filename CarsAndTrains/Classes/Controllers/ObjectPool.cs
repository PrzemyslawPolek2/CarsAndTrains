using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarsAndTrains.Classes.Controllers
{
    public class ObjectPool<T>
    {
        private List<T> objectList;
        private List<int> activeObjects;
        public ObjectPool()
        {
            objectList = new List<T>();
            activeObjects = new List<int>();
        }

        public 

        public T Access(int index)
        {
            index = ControlIndex(index);
            


        }

        private int ControlIndex(int index)
        {
            if (index >= activeObjects.Count())
                index = activeObjects.Count();
            if (index < 0)
                index = 0;
            return index;
        }
    }
}
