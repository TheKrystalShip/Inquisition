using System;
using System.Threading;

namespace Inquisition.Services.Core
{
	public class Service : IService
	{
		public Timer Timer { get; private set; }
		public event Action<Service> Start;
		public event Action<Service> Stop;
		public event Action<Service> Tick;

		public virtual void Init(int delay = 0, int interval = 1000)
		{
			Timer = new Timer(Loop, null, delay, interval);
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

		public override string ToString()
		{
			return base.ToString().Replace("Inquisition.Services.", "");
		}
	}
}
