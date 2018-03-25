using System;
using System.Threading;

namespace Inquisition.Services
{
	public class BaseService
    {
		public Timer Timer { get; set; }
		public event EventHandler Start;
		public event EventHandler Stop;
		public event EventHandler Tick;

		public virtual void StartLoop()
		{
			Timer = new Timer(Loop, null, 0, 1000);
			Start?.Invoke(this, EventArgs.Empty);
		}

		public virtual void Loop(object state)
		{
			Tick?.Invoke(this, EventArgs.Empty);
		}

		public virtual void StopLoop()
		{
			Timer.Dispose();
			Stop?.Invoke(this, EventArgs.Empty);
		}
    }
}
