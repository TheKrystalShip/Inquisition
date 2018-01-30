using System;

namespace Inquisition.Data.Interfaces
{
	public interface IThreadLoop
    {
		event EventHandler LoopStarted;
		event EventHandler LoopStopped;

		void StartLoop();
		void StopLoop();
    }
}
