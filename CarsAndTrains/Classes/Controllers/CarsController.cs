﻿using System.Diagnostics;
using System.Threading;

namespace CarsAndTrains.Classes.Controllers
{
    class CarsController : Controller
    {
        protected override void RunThread()
        {
            do
            {
                //Debug.WriteLine("[Vehicles TICK]");
                PublicAvaliableReferences.UpdateAllCars();
                Thread.Sleep(THREAD_TICK);
            } while (!PublicAvaliableReferences.IsCarPoolFinished);

            //once finished looping, abort self
            Debug.WriteLine("[Vehicles END]");
            this.Abort();
        }
    }
}
