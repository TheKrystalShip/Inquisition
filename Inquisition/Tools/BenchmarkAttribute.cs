using System;
using System.Diagnostics;

using TheKrystalShip.Logging;

namespace TheKrystalShip.Inquisition
{
    public class BenchmarkAttribute : Attribute, IDisposable
    {
        private readonly Stopwatch _stopwatch;
        private readonly ILogger<Benchmark> _logger;

        public BenchmarkAttribute()
        {
            _stopwatch = Stopwatch.StartNew();
            _logger = new Logger<Benchmark>();
        }

        public void Dispose()
        {
            _stopwatch.Stop();
            _logger.LogInformation($"{_stopwatch.Elapsed:hh\\:mm\\:ss\\:ffffff}");
        }
    }
}
