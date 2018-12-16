using System;
using System.Diagnostics;

using TheKrystalShip.Logging;

namespace TheKrystalShip.Inquisition.Tools
{
    public class Benchmark : IDisposable
    {
        private readonly Stopwatch _stopwatch;
        private readonly ILogger<Benchmark> _logger;

        public Benchmark()
        {
            _stopwatch = Stopwatch.StartNew();
            _logger = new Logger<Benchmark>();
        }

        public void Dispose()
        {
            _stopwatch.Stop();
            _logger.LogInformation($"Elapsed time: {_stopwatch.Elapsed:hh\\:mm\\:ss\\:ffffff}");
        }
    }
}
