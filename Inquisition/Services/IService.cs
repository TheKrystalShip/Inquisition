using System;
using System.Threading;

namespace Inquisition.Services
{
    public interface IService : IDisposable
    {
        Timer Timer { get; set; }

        event Action<Service> Start;
        event Action<Service> Stop;
        event Action<Service> Tick;

        void Init(int startDelay = 0, int interval = 1000);
        void Loop(object state);

    }
}