using System;
using System.Threading;

namespace Inquisition.Services
{
    public class Service : IDisposable
    {
		public Timer Timer { get; set; }
		public event Action<Service> Start;
		public event Action<Service> Stop;
		public event Action<Service> Tick;

		public virtual void Init(int startDelay = 0, int interval = 1000)
		{
			Timer = new Timer(Loop, null, startDelay, interval);
			Start?.Invoke(this);
		}

		public virtual void Loop(object state)
		{
			Tick?.Invoke(this);
		}

		public virtual void Dispose()
		{
			Timer.Dispose();
			Stop?.Invoke(this);
		}
	}
}
