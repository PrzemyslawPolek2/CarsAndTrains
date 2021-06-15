using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CarsAndTrains.Classes.Controllers
{
    public class VehiclesInfoController : Controller
    {
        
        protected override void RunThread()
        {
            do
            {
                MainWindow.GetMain.UpdateCarsLB();
                MainWindow.GetMain.UpdateTrainsLB();
                MainWindow.GetMain.UpdateCarNodesLB();
                Thread.Sleep(THREAD_TICK);
            } while (true);
        }
    }
}
