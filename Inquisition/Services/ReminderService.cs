using Discord;
using Discord.WebSocket;

using Inquisition.Data.Handlers;
using Inquisition.Data.Interfaces;
using Inquisition.Data.Models;

using Microsoft.EntityFrameworkCore;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace Inquisition.Services
{
	public class ReminderService : IThreadLoop
	{
		private DiscordSocketClient Client;
		private DbHandler db;

		public Timer Timer { get; set; }
		public event EventHandler LoopStarted;
		public event EventHandler LoopStopped;
		public event EventHandler LoopTick;

		public ReminderService(DiscordSocketClient client, DbHandler dbHandler)
		{
			Client = client;
			db = dbHandler;
		}

		public void StartLoop()
		{
			Timer = new Timer(Loop, null, 0, 1000);
			LoopStarted?.Invoke(this, EventArgs.Empty);
		}

		public void Loop(object state)
		{
			List<Reminder> RemindersList = GetReminderList(10);
			if (RemindersList.Count > 0)
			{
				foreach (Reminder r in RemindersList)
				{
					Client.GetUser(Convert.ToUInt64(r.User.Id)).SendMessageAsync($"Reminder: {r.Message}");
					db.Reminders.Remove(r);
				}
				db.SaveChanges();
			}
			LoopTick?.Invoke(this, EventArgs.Empty);
		}

		public void StopLoop()
		{
			Timer.Dispose();
			LoopStopped?.Invoke(this, EventArgs.Empty);
		}

		private List<Reminder> GetReminderList(int amount)
		{
			if (db.Reminders.Any())
			{
				return db.Reminders
						 .Where(x => x.DueDate <= DateTimeOffset.UtcNow)
						 .Take(amount)
						 .Include(x => x.User)
						 .ToList();
			}
			return new List<Reminder>();
		}
	}
}
