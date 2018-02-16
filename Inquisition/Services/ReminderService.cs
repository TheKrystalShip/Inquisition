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

		public string Name { get; set; } = "Reminider service";
		public Timer Timer { get; set; }
		public event EventHandler LoopStarted;
		public event EventHandler LoopStopped;
		public event EventHandler LoopTick;

		public ReminderService(DiscordSocketClient client) 
			=> Client = client;

		public void StartLoop()
		{
			Timer = new Timer(Loop, null, 0, 1000);
			LoopStarted?.Invoke(Name, EventArgs.Empty);
		}

		public void Loop(object state)
		{
			List<Reminder> RemindersList = GetReminderList(10);

			foreach (Reminder r in RemindersList)
			{
				Client.GetUser(Convert.ToUInt64(r.User.Id)).SendMessageAsync($"Reminder: {r.Message}");
				RemoveReminder(r);
			}
			LoopTick?.Invoke(Name, EventArgs.Empty);
		}

		public void StopLoop()
		{
			Timer.Dispose();
			LoopStopped?.Invoke(Name, EventArgs.Empty);
		}

		private List<Reminder> GetReminderList(int amount)
		{
			DbHandler db = new DbHandler();
			return db.Reminders
				.Where(x => x.DueDate <= DateTimeOffset.UtcNow)
				.Take(amount)
				.Include(x => x.User)
				.ToList() ?? new List<Reminder>();
		}

		private void RemoveReminder(Reminder r)
		{
			DbHandler db = new DbHandler();
			db.Reminders.Remove(r);
			db.SaveChanges();
		}
	}
}
