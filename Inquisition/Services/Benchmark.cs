
using System.Diagnostics;

using TheKrystalShip.Logging;

namespace Inquisition.Services
{
    public class Benchmark : Service
	{
		private Stopwatch Stopwatch;
        private readonly ILogger<Benchmark> _logger;

		public Benchmark()
		{
			Stopwatch = Stopwatch.StartNew();
            _logger = new Logger<Benchmark>();
		}

		public override void Dispose()
		{
			Stopwatch.Stop();
            _logger.LogInformation($"{Stopwatch.Elapsed:hh\\:mm\\:ss\\:ffffff}");
		}
	}
}
