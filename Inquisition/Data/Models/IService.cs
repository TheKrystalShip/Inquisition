using System;
using System.Threading;

namespace Inquisition.Data.Models
{
	public interface IService
    {
		Timer Timer { get; set; }
		event EventHandler Start;
		event EventHandler Stop;
		event EventHandler Tick;

		void StartLoop();
		void Loop(object state);
		void StopLoop();
    }
}
