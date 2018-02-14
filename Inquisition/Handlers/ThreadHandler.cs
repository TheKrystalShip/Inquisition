using Discord.WebSocket;

using Inquisition.Data.Handlers;
using Inquisition.Data.Models;
using Inquisition.Services;

using System;
using System.Collections.Generic;
using System.Threading;

namespace Inquisition.Handlers
{
	public class ThreadHandler
    {
		private Dictionary<LoopType, Thread> LoopDictionary { get; set; } = new Dictionary<LoopType, Thread>();

		private ReminderService ReminderService;
		private DealService DealService;

		private Thread ReminderThread;
		private Thread DealThread;

		private DiscordSocketClient Client;
		private DbHandler db;

		public ThreadHandler(DiscordSocketClient socketClient, DbHandler dbHandler)
		{
			Client = socketClient;
			db = dbHandler;

			ReminderService = new ReminderService(Client, db);
			ReminderService.LoopStarted += ReminderService_LoopStarted;
			ReminderThread = new Thread(ReminderService.StartLoop);
			LoopDictionary.Add(LoopType.Reminder, ReminderThread);

			DealService = new DealService(db);
			DealService.LoopStarted += DealService_LoopStarted;
			DealThread = new Thread(DealService.StartLoop);
			LoopDictionary.Add(LoopType.Deal, DealThread);
		}

		private void ReminderService_LoopStarted(object sender, EventArgs e)
		{
			Console.WriteLine("Reminder loop started...");
		}

		private void DealService_LoopStarted(object sender, EventArgs e)
		{
			Console.WriteLine("Offer loop started...");
		}

		public void StartLoop(LoopType loop) 
			=> LoopDictionary.GetValueOrDefault(loop).Start();

		public void StopLoop(LoopType loop) 
			=> LoopDictionary.GetValueOrDefault(loop).Abort();

		public ThreadHandler StartAllLoops()
		{
			ReminderThread?.Start();
			DealThread?.Start();
			return this;
		}

		public ThreadHandler StopAllLoops()
		{
			ReminderThread?.Abort();
			DealThread?.Abort();
			return this;
		}
    }
}
