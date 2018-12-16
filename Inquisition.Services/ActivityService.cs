
using System;
using System.Threading;

namespace TheKrystalShip.Inquisition.Services
{
    public class ActivityService : Service
    {
        public override Timer Timer { get; protected set; }
        public override event Action<Service> Start;
        public override event Action<Service> Stop;
        public override event Action<Service> Tick;

        public ActivityService()
        {

        }

        public override void Init(int startDelay = 0, int interval = 1000)
        {
            Timer = new Timer(Loop, null, startDelay, interval);
            Start?.Invoke(this);
        }

        protected override void Loop(object state)
        {

            Tick?.Invoke(this);
        }

        public override void Dispose()
        {

            Stop?.Invoke(this);
        }
    }
}
