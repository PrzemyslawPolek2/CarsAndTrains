using System.Threading;

namespace CarsAndTrains.Classes.Controllers
{
    public class TurnpikesAndLightsController : Controller
    {
        protected override void RunThread()
        {
            do
            {
                bool _turnpikeStatus = PublicAvaliableReferences.GetTurnPikeStatus();

                PublicAvaliableReferences.UpdateAllTurnpikes(_turnpikeStatus);
                PublicAvaliableReferences.UpdateAllLights(_turnpikeStatus);

                Thread.Sleep(THREAD_TICK);
            } while (true);

            //once finished looping, abort self
            this.Abort();
        }
    }
}
