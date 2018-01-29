using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace Inquisition.Services
{
	public class Benchmark : IDisposable
	{
		private Stopwatch Stopwatch;

		public Benchmark() => Stopwatch = Stopwatch.StartNew();

		public void Dispose()
		{
			Stopwatch.Stop();
			Console.WriteLine("Elapsed Time (hh.mm.ss.ffffff) - " + Stopwatch.Elapsed);
		}
	}
}
