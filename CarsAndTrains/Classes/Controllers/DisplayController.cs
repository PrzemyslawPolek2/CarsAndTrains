using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CarsAndTrains.Classes.Controllers
{
    public class DisplayController : Controller
    {
        protected override void RunThread()
        {
            do
            {
                MainWindow.GetMain.UpdateCarsLB();
                MainWindow.GetMain.UpdateTrainsLB();
                MainWindow.GetMain.UpdateCarNodesLB();
                MainWindow.TickCounter += THREAD_TICK * 5;
                Thread.Sleep(THREAD_TICK * 5);
            } while (true);
        }
    }
}
