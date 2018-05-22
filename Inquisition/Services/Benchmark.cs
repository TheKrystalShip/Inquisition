using Inquisition.Logging;

using System.Diagnostics;

namespace Inquisition.Services
{
	public class Benchmark : Service
	{
		private Stopwatch Stopwatch;

		public Benchmark()
		{
			Stopwatch = Stopwatch.StartNew();
		}

		public override void Dispose()
		{
			Stopwatch.Stop();
			LogHandler.WriteLine(LogTarget.Console, $"{Stopwatch.Elapsed:hh\\:mm\\:ss\\:ffffff}");
		}
	}
}
