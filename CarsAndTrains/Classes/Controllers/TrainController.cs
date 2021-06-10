using System.Diagnostics;
using System.Threading;

namespace CarsAndTrains.Classes.Controllers
{
    class TrainController : Controller
    {
        protected override void RunThread()
        {
            do
            {
                //Debug.WriteLine("[Trains TICK]");
                PublicAvaliableReferences.UpdateAllTrains();
                Thread.Sleep(THREAD_TICK);
            } while (!PublicAvaliableReferences.IsCarPoolFinished);

            //once finished looping, abort self
            Debug.WriteLine("[Trains END]");
            this.Abort();
        }
    }
}
