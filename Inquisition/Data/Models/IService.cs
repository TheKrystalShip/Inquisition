using System;
using System.Threading;

namespace Inquisition.Data.Models
{
	public interface IService
    {
		Timer Timer { get; set; }
		event EventHandler LoopStarted;
		event EventHandler LoopStopped;
		event EventHandler LoopTick;

		void StartLoop();
		void Loop(object state);
		void StopLoop();
    }
}
