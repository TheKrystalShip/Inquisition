using System;
using System.Threading;

namespace Inquisition.Services.Core
{
	internal interface IService : IDisposable
	{
		Timer Timer { get; }
		event Action<Service> Start;
		event Action<Service> Tick;
		event Action<Service> Stop;

		void Init(int delay, int interval);
		void Loop(object state);
	}
}