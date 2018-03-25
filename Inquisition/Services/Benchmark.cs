using Inquisition.Logging;

using System;
using System.Diagnostics;

namespace Inquisition.Services
{
	public class Benchmark : Service, IDisposable
	{
		private Stopwatch Stopwatch;

		public Benchmark() => Stopwatch = Stopwatch.StartNew();

		public void Dispose()
		{
			Stopwatch.Stop();
			LogHandler.WriteLine(LogTarget.Console, $"{Stopwatch.Elapsed:hh\\:mm\\:ss\\:ffffff}");
		}
	}
}
