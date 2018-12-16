
using System;
using System.Threading;

namespace TheKrystalShip.Inquisition.Services
{
    public class ReminderService : Service
    {
        public override Timer Timer { get; protected set; }
        public override event Action<Service> Start;
        public override event Action<Service> Stop;
        public override event Action<Service> Tick;

        public ReminderService()
        {

        }

        public override void Init(int startDelay = 0, int interval = 5000)
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

            Timer.Dispose();
            Stop?.Invoke(this);
        }
    }
}
