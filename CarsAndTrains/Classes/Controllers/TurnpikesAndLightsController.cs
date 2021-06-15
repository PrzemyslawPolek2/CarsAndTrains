using System.Threading;

namespace CarsAndTrains.Classes.Controllers
{
    public class TurnpikesAndLightsController : Controller
    {
        protected override void RunThread()
        {
            do
            {
                bool turnpikeStatus = PublicAvaliableReferences.GetTurnPikeStatus();

                PublicAvaliableReferences.UpdateAllTurnpikes(turnpikeStatus);
                PublicAvaliableReferences.UpdateAllLights(turnpikeStatus);

                Thread.Sleep(THREAD_TICK);
            } while (true);

            //once finished looping, abort self
            this.Abort();
        }
    }
}
