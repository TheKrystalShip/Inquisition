using Discord;
using Discord.WebSocket;

using Inquisition.Database;
using Inquisition.Database.Models;
using Inquisition.Database.Repositories;
using Inquisition.Logging;

using Microsoft.EntityFrameworkCore;

using System;
using System.Collections.Generic;
using System.Linq;

namespace Inquisition.Services
{
    public class ReminderService : Service
	{
		private readonly DiscordSocketClient _client;
		private readonly DatabaseContext _dbContext;
        private readonly IRepositoryWrapper _repository;
        private readonly ILogger<ReminderService> _logger;

		public ReminderService(
            DiscordSocketClient client,
            DatabaseContext dbContext,
            IRepositoryWrapper repository,
            ILogger<ReminderService> logger)
		{
			_client = client;
            _dbContext = dbContext;
            _repository = repository;
            _logger = logger;
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
				_client.GetUser(Convert.ToUInt64(r.User.Id)).SendMessageAsync($"Reminder: {r.Message}");
			}

			RemoveReminderList(RemindersList);
		}

		public override void Dispose()
		{
			base.Dispose();
		}

		private List<Reminder> GetReminderList(int amount)
		{
			return _dbContext.Reminders
				.Where(x => x.DueDate <= DateTimeOffset.UtcNow)
				.Include(x => x.User)
				.Take(amount)
				.ToList() ?? new List<Reminder>();
		}

		private void RemoveReminderList(List<Reminder> r)
		{
			_dbContext.Reminders.RemoveRange(r);
			_dbContext.SaveChanges();
		}
	}
}
