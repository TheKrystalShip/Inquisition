using Discord.WebSocket;

using Inquisition.Logging;
using Inquisition.Services;

using System.Collections.Generic;

namespace Inquisition.Handlers
{
	public class ServiceHandler : Handler
	{
		private static List<Service> ServiceList { get; set; } = new List<Service>();
		private const int InitDelay = 5000;
		private const int Interval = 1000;

		public ServiceHandler(DiscordSocketClient client)
			=> Init(client);

		private async void Init(DiscordSocketClient client)
		{
			RegisterService(new ReminderService(client));
			RegisterService(new DealService());
			RegisterService(new ActivityService());

			//StartAllLoops();
		}

		private void RegisterService<T>(T service) where T: Service
		{
			HandleEvents(service);
			ServiceList.Add(service);
		}

		private void HandleEvents<T>(T service) where T : Service
		{
			service.Start += Service_Start;
			service.Tick += Service_Tick;
			service.Stop += Service_Stop;
		}

		private void Service_Start(Service service)
		{
			LogHandler.WriteLine(LogTarget.Console, service.ToString(), "Started");
		}

		private void Service_Tick(Service service)
		{
			LogHandler.WriteLine(LogTarget.Console, service.ToString(), "Ticked");
		}

		private void Service_Stop(Service service)
		{
			LogHandler.WriteLine(LogTarget.Console, service.ToString(), "Stopped");
		}

		public static void StartAllLoops()
		{
			LogHandler.WriteLine(LogTarget.Console, "Starting loops...");

			foreach (Service service in ServiceList)
			{
				service.Init(InitDelay, Interval);
			}
		}

		public static void StopAllLoops()
		{
			LogHandler.WriteLine(LogTarget.Console, "Stopping loops...");

			foreach (Service service in ServiceList)
			{
				service.Dispose();
			}
		}
	}
}
