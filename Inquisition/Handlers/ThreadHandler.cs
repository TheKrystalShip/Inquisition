using Discord.WebSocket;

using Inquisition.Data.Handlers;
using Inquisition.Data.Interfaces;
using Inquisition.Services;

using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Inquisition.Handlers
{
	public enum LoopType
	{
		Reminder,
		Deal,
		Activity
	}

	public class ThreadHandler
    {
		private static Dictionary<LoopType, Thread> LoopDictionary { get; set; } = new Dictionary<LoopType, Thread>();

		public ThreadHandler(DiscordSocketClient socketClient)
		{
			ReminderService reminderService = new ReminderService(socketClient);
			HandleEvents(reminderService);
			LoopDictionary.Add(LoopType.Reminder, new Thread(reminderService.StartLoop));

			DealService dealService = new DealService();
			HandleEvents(dealService);
			LoopDictionary.Add(LoopType.Deal, new Thread(dealService.StartLoop));

			ActivityService activityService = new ActivityService();
			HandleEvents(activityService);
			LoopDictionary.Add(LoopType.Activity, new Thread(activityService.StartLoop));

			StartAllLoops();
		}

		private void HandleEvents<T>(T service) where T: IThreadLoop
		{
			service.LoopStarted += Service_LoopStarted;
			service.LoopTick += Service_LoopTick;
			service.LoopStopped += Service_LoopStopped;
		}

		private void Service_LoopStarted(object sender, EventArgs e)
			=> LogHandler.WriteLine(sender, "Started");

		private void Service_LoopTick(object sender, EventArgs e)
		{
			//LogHandler.WriteLine(sender, "Ticked");
		}

		private void Service_LoopStopped(object sender, EventArgs e) 
			=> LogHandler.WriteLine(sender, "Stopped");

		public async void StartAllLoops()
		{
			await Task.Delay(5000);
			foreach (KeyValuePair<LoopType, Thread> loop in LoopDictionary)
			{
				loop.Value?.Start();
			}
		}

		public void StopAllLoops()
		{
			foreach (KeyValuePair<LoopType, Thread> loop in LoopDictionary)
			{
				loop.Value?.Abort();
			}
		}

		public static void StartLoop(LoopType loop) 
			=> LoopDictionary.GetValueOrDefault(loop).Start();

		/// <summary>
		/// I know it's not ideal, just a temporary thing until i can get a flag in place to
		/// gracefully close down the threads. Also it doesn't work anyway, platform doesn't support it.
		/// </summary>
		/// <param name="loop">Which loop to close down</param>
		public static void StopLoop(LoopType loop) 
			=> LoopDictionary.GetValueOrDefault(loop).Abort();
	}
}
