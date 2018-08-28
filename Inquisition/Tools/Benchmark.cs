using System;
using System.Diagnostics;

using TheKrystalShip.Logging;

namespace TheKrystalShip.Inquisition.Tools
{
    public class Benchmark : IDisposable
    {
        private Stopwatch Stopwatch;
        private readonly ILogger<Benchmark> _logger;

        public Benchmark()
        {
            Stopwatch = Stopwatch.StartNew();
            _logger = new Logger<Benchmark>();
        }

        public void Dispose()
        {
            Stopwatch.Stop();
            _logger.LogInformation($"{Stopwatch.Elapsed:hh\\:mm\\:ss\\:ffffff}");
        }
    }
}
