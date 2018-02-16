using Inquisition.Data.Handlers;

using System;
using System.Diagnostics;

namespace Inquisition.Services
{
	public class Benchmark : IDisposable
	{
		private Stopwatch Stopwatch;

		public Benchmark() => Stopwatch = Stopwatch.StartNew();

		public void Dispose()
		{
			Stopwatch.Stop();
			LogHandler.WriteLine($"{Stopwatch.Elapsed:hh\\:mm\\:ss\\:ffffff}");
		}
	}
}
