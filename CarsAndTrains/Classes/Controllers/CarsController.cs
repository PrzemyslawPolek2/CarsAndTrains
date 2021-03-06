using System.Diagnostics;
using System.Threading;

namespace CarsAndTrains.Classes.Controllers
{
    class CarsController : Controller
    {
        protected override void RunThread()
        {
            do
            {
                PublicAvaliableReferences.UpdateAllCars();
                Thread.Sleep(THREAD_TICK);
            } while (true);

            //once finished looping, abort self
            this.Abort();
        }
    }
}
