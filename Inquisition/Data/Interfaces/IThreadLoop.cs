using System;
using System.Threading;

namespace Inquisition.Data.Interfaces
{
	public interface IThreadLoop
    {
		Timer Timer { get; set; }
		event EventHandler LoopStarted;
		event EventHandler LoopStopped;
		event EventHandler LoopTick;

		void StartLoop();
		void StopLoop();
		void Loop(object state);
    }
}
