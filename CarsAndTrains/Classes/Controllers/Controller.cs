using System.Threading;

namespace CarsAndTrains.Classes.Controllers
{
    public abstract class Controller
    {
        private readonly Thread _thread;
        public const int THREAD_TICK = 10;

        protected Controller()
        {
            _thread = new Thread(new ThreadStart(this.RunThread));
            
        }

        // Thread methods / properties
        public void Start() => _thread.Start();
        public void Join() => _thread.Join();
        public bool IsAlive => _thread.IsAlive;
        public void Abort() => _thread.Abort();

        // Override in base class
        protected abstract void RunThread();
    }
}
