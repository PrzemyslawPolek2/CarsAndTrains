using System.Diagnostics;
using System.Threading;

namespace CarsAndTrains.Classes.Controllers
{
    class CarsController : Controller
    {
        public override void RunThread()
        {
            do
            {
                Debug.WriteLine("[Vehicles TICK]");
                PublicAvaliableReferences.UpdateAllCars();
                Thread.Sleep(THREAD_TICK);
            } while (!PublicAvaliableReferences.IsFinished);

        }
    }
}
