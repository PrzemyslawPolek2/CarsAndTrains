using System.Diagnostics;
using System.Threading;

namespace CarsAndTrains.Classes.Controllers
{
    class TrainsController : Controller
    {
        protected override void RunThread()
        {
            do
            {
                PublicAvaliableReferences.UpdateAllTrains();
                Thread.Sleep(THREAD_TICK);
            } while (!PublicAvaliableReferences.IsCarPoolFinished);

            //once finished looping, abort self
            this.Abort();
        }
    }
}
