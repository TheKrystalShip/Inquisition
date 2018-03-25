using Discord;
using Discord.WebSocket;

using Inquisition.Database;
using Inquisition.Database.Models;

using Microsoft.EntityFrameworkCore;

using System;
using System.Collections.Generic;
using System.Linq;

namespace Inquisition.Services
{
	public class ReminderService : Service
	{
		private DiscordSocketClient Client;

		public ReminderService(DiscordSocketClient client)
		{
			Client = client;
		}

		public override void Init(int startDelay = 0, int interval = 1000)
		{
			base.Init(startDelay, interval);
		}

		public override void Loop(object state)
		{
			base.Loop(state);
			List<Reminder> RemindersList = GetReminderList(10);

			foreach (Reminder r in RemindersList)
			{
				Client.GetUser(Convert.ToUInt64(r.User.Id)).SendMessageAsync($"Reminder: {r.Message}");
			}

			RemoveReminderList(RemindersList);
		}

		public override void Dispose()
		{
			base.Dispose();
		}

		private List<Reminder> GetReminderList(int amount)
		{
			DatabaseContext db = new DatabaseContext();
			return db.Reminders
				.Where(x => x.DueDate <= DateTimeOffset.UtcNow)
				.Include(x => x.User)
				.Take(amount)
				.ToList() ?? new List<Reminder>();
		}

		private void RemoveReminderList(List<Reminder> r)
		{
			DatabaseContext db = new DatabaseContext();
			db.Reminders.RemoveRange(r);
			db.SaveChanges();
		}
	}
}
