using System;
using System.Diagnostics;

namespace Inquisition.Data.Services
{
	public class Benchmark : IDisposable
    {
		private Stopwatch Stopwatch;

		public Benchmark()
		{
			Stopwatch = Stopwatch.StartNew();
		}

		public void Dispose()
		{
			Stopwatch.Stop();
			Console.WriteLine("Elapsed Time (hh.mm.ss.ffffff) - " + Stopwatch.Elapsed);
		}
	}
}
