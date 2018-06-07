using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Emuses.Tasks
{
    public class SessionClearTask : IHostedService, IDisposable
    {
        private readonly ILogger _logger;
        private Timer _timer;

        public SessionClearTask(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger("SessionClearTaskLogger");
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("***** Timed Background Service is starting.");
            _timer = new Timer(DoWork, null, TimeSpan.Zero, TimeSpan.FromSeconds(5));

            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("***** Timed Background Service is stopping.");
            _timer?.Change(Timeout.Infinite, 0);

            return Task.CompletedTask;
        }

        public void Dispose()
        {
            _timer?.Dispose();
        }

        private void DoWork(object state)
        {
            Console.WriteLine("***** test *****");
            _logger.LogInformation("***** Timed Background Service is working.");
        }
    }
}
