using Discord.WebSocket;

using Inquisition.Data.Models;
using Inquisition.Services;

using System;
using System.Collections.Generic;

namespace Inquisition.Handlers
{
	public class ServiceHandler
    {
		private static List<IService> ServiceList { get; set; } = new List<IService>();

		public ServiceHandler(DiscordSocketClient socketClient)
		{
			RegisterService(new ReminderService(socketClient));
			RegisterService(new DealService());
			RegisterService(new ActivityService());
		}

		private void RegisterService<T>(T service) where T: IService
		{
			HandleEvents(service);
			ServiceList.Add(service);
		}

		private void HandleEvents<T>(T service) where T: IService
		{
			service.Start += Service_LoopStarted;
			service.Tick += Service_LoopTick;
			service.Stop += Service_LoopStopped;
		}

		private void Service_LoopStarted(object sender, EventArgs e)
		{
			LogHandler.WriteLine(sender, "Started");
		}

		private void Service_LoopTick(object sender, EventArgs e)
		{ 
			LogHandler.WriteLine(sender, "Ticked");
		}

		private void Service_LoopStopped(object sender, EventArgs e)
		{ 
			LogHandler.WriteLine(sender, "Stopped");
		}

		public static void StartAllLoops()
		{
			foreach (IService thread in ServiceList)
			{
				thread.StartLoop();
			}
		}

		public static void StopAllLoops()
		{
			foreach (IService thread in ServiceList)
			{
				thread.StopLoop();
			}
		}
	}
}
