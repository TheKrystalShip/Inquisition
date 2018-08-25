using System;
using System.Threading;

namespace TheKrystalShip.Inquisition.Services
{
    public class Service : IService
    {
        private Timer _timer;
		public event Action<IService> Start;
		public event Action<IService> Stop;
		public event Action<IService> Tick;

		public virtual void Init(int startDelay = 0, int interval = 1000)
		{
			_timer = new Timer(Loop, null, startDelay, interval);
			Start?.Invoke(this);
		}

		public virtual void Loop(object state)
		{
			Tick?.Invoke(this);
		}

		public virtual void Dispose()
		{
			_timer.Dispose();
			Stop?.Invoke(this);
		}
	}
}
