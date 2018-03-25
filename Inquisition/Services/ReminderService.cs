using Discord;
using Discord.WebSocket;

using Inquisition.Data.Models;
using Inquisition.Database.Core;
using Inquisition.Database.Models;

using Microsoft.EntityFrameworkCore;

using System;
using System.Collections.Generic;
using System.Linq;

namespace Inquisition.Services
{
	public class ReminderService : BaseService
	{
		private DiscordSocketClient Client;

		public ReminderService(DiscordSocketClient client)
		{
			Client = client;
		}

		public override void StartLoop()
		{
			base.StartLoop();
		}

		public override void Loop(object state)
		{
			base.Loop(state);
			List<Reminder> RemindersList = GetReminderList(10);

			foreach (Reminder r in RemindersList)
			{
				Client.GetUser(Convert.ToUInt64(r.User.Id)).SendMessageAsync($"Reminder: {r.Message}");
				RemoveReminder(r);
			}
		}

		public override void StopLoop()
		{
			base.StopLoop();
		}

		private List<Reminder> GetReminderList(int amount)
		{
			DatabaseContext db = new DatabaseContext();
			return db.Reminders
				.Where(x => x.DueDate <= DateTimeOffset.UtcNow)
				.Take(amount)
				.Include(x => x.User)
				.ToList() ?? new List<Reminder>();
		}

		private void RemoveReminder(Reminder r)
		{
			DatabaseContext db = new DatabaseContext();
			db.Reminders.Remove(r);
			db.SaveChanges();
		}

		public override string ToString()
		{
			return "Reminder service";
		}
	}
}
