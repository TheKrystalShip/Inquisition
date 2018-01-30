using Discord.WebSocket;

using Inquisition.Data.Handlers;
using Inquisition.Services;

using System;
using System.Threading;

namespace Inquisition.Handlers
{
	public class ThreadHandler
    {
		private ReminderService ReminderService;
		private OfferService OfferService;

		private Thread ReminderThread;
		private Thread OfferThread;

		private DiscordSocketClient Client;
		private DbHandler db;

		public ThreadHandler(DiscordSocketClient socketClient, DbHandler dbHandler)
		{
			Client = socketClient;
			db = dbHandler;

			ReminderService = new ReminderService(Client, db);
			ReminderService.LoopStarted += ReminderService_LoopStarted;
			ReminderThread = new Thread(ReminderService.StartLoop);

			OfferService = new OfferService(db);
			OfferService.LoopStarted += OfferService_LoopStarted;
			OfferThread = new Thread(OfferService.StartLoop);
		}

		private void OfferService_LoopStarted(object sender, EventArgs e)
		{
			Console.WriteLine("Offer loop started");
		}

		private void ReminderService_LoopStarted(object sender, EventArgs e)
		{
			Console.WriteLine("Reminder loop started");
		}

		public ThreadHandler StartAllLoops()
		{
			ReminderThread.Start();
			OfferThread.Start();
			return this;
		}

		public ThreadHandler StopAllLoops()
		{
			ReminderThread.Abort();
			OfferThread.Abort();
			return this;
		}
    }
}
