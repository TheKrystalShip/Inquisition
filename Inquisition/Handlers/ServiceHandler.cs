using Discord.WebSocket;

using Inquisition.Logging;
using Inquisition.Services;

using System;
using System.Collections.Generic;

namespace Inquisition.Handlers
{
	public class ServiceHandler : BaseHandler
	{
		private static List<BaseService> ServiceList { get; set; } = new List<BaseService>();

		public ServiceHandler(DiscordSocketClient client)
		{
			RegisterService(new ReminderService(client));
			RegisterService(new DealService());
			RegisterService(new ActivityService());
		}

		private void RegisterService<T>(T service) where T : BaseService
		{
			HandleEvents(service);
			ServiceList.Add(service);
		}

		private void HandleEvents<T>(T service) where T : BaseService
		{
			service.Start += Service_LoopStarted;
			service.Tick += Service_LoopTick;
			service.Stop += Service_LoopStopped;
		}

		private void Service_LoopStarted(object sender, EventArgs e)
		{
			LogHandler.WriteLine(LogTarget.Console, sender, "Started");
		}

		private void Service_LoopTick(object sender, EventArgs e)
		{
			LogHandler.WriteLine(LogTarget.Console, sender, "Ticked");
		}

		private void Service_LoopStopped(object sender, EventArgs e)
		{
			LogHandler.WriteLine(LogTarget.Console, sender, "Stopped");
		}

		public static void StartAllLoops()
		{
			foreach (BaseService service in ServiceList)
			{
				service.StartLoop();
			}
		}

		public static void StopAllLoops()
		{
			foreach (BaseService service in ServiceList)
			{
				service.StopLoop();
			}
		}
	}
}
