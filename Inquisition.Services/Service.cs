using System;
using System.Threading;

namespace TheKrystalShip.Inquisition.Services
{
    public abstract class Service
    {
        public abstract Timer Timer { get; protected set; }
        public abstract event Action<Service> Start;
        public abstract event Action<Service> Stop;
        public abstract event Action<Service> Tick;

        public abstract void Init(int startDelay = 0, int interval = 1000);
        protected abstract void Loop(object state);
        public abstract void Dispose();
    }
}
