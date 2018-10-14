using System.Collections.Generic;
using System.Threading.Tasks;

using TheKrystalShip.Inquisition.Services;
using TheKrystalShip.Logging;

namespace TheKrystalShip.Inquisition.Handlers
{
    public class ServiceHandler
    {
        private List<Service> _serviceList;
        private const int InitDelay = 5000;
        private const int Interval = 1000;
        private readonly ILogger<ServiceHandler> _logger;

        /// <summary>
        /// Services can be directly injected from the IoC container,
        /// CommandHandler takes care of it automatically
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="reminderService"></param>
		public ServiceHandler(ILogger<ServiceHandler> logger, ReminderService reminderService)
        {
            _logger = logger;
            _serviceList = new List<Service>();

            RegisterService(reminderService);
            StartAllLoops();
        }

        private void RegisterService(Service service)
        {
            HandleEvents(service);
            _serviceList.Add(service);
        }

        private void HandleEvents(Service service)
        {
            service.Start += Service_Start;
            service.Tick += Service_Tick;
            service.Stop += Service_Stop;
        }

        private void Service_Start(Service service)
        {
            _logger.LogInformation(service.GetType().Name, "Started");
        }

        private void Service_Tick(Service service)
        {
            _logger.LogInformation(service.GetType().Name, "Ticked");
        }

        private void Service_Stop(Service service)
        {
            _logger.LogInformation(service.GetType().Name, "Stopped");
        }

        public async void StartAllLoops()
        {
            await Task.Delay(InitDelay);

            _logger.LogInformation("Starting loops...");

            foreach (Service service in _serviceList)
            {
                service.Init(InitDelay, Interval);
            }
        }

        public void StopAllLoops()
        {
            _logger.LogInformation("Stopping loops...");

            foreach (Service service in _serviceList)
            {
                service.Dispose();
            }
        }
    }
}
