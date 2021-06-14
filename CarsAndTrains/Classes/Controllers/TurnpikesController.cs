using System.Threading;

namespace CarsAndTrains.Classes.Controllers
{
    public class TurnpikesController : Controller
    {
        protected override void RunThread()
        {
            do
            {
                bool turnpikeStatus = PublicAvaliableReferences.TurnPikeStatus();

                PublicAvaliableReferences.UpdateAllTurnpikes(turnpikeStatus);
                PublicAvaliableReferences.UpdateAllLights(turnpikeStatus);

                Thread.Sleep(THREAD_TICK);
            } while (!PublicAvaliableReferences.IsCarPoolFinished);

            //once finished looping, abort self
            this.Abort();
        }
    }
}
