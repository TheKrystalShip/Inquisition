using System;

namespace TheKrystalShip.Inquisition.Services
{
    public interface IService : IDisposable
    {
        event Action<IService> Start;
        event Action<IService> Stop;
        event Action<IService> Tick;

        void Init(int startDelay = 0, int interval = 1000);
        void Loop(object state);
    }
}