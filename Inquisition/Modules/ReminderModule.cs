using Discord;
using Discord.Commands;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using TheKrystalShip.Inquisition.Data.Models;
using TheKrystalShip.Inquisition.Database;
using TheKrystalShip.Inquisition.Database.Models;
using TheKrystalShip.Inquisition.Handlers;
using TheKrystalShip.Logging;

namespace TheKrystalShip.Inquisition.Modules
{
    public class ReminderModule : ModuleBase<SocketCommandContext>
    {
		private readonly DatabaseContext _dbContext;
        private readonly ReportHandler _reportHandler;
        private readonly ILogger<ReminderModule> _logger;

		public ReminderModule(
            DatabaseContext dbContext,
            ReportHandler reportHandler,
            ILogger<ReminderModule> logger)
        {
            _dbContext = dbContext;
            _reportHandler = reportHandler;
            _logger = logger;
        }

		[Command("reminders")]
		[Summary("Displays a list with all of your reminders")]
		public async Task ListRemindersAsync()
		{
			try
			{
				User localUser = _dbContext.Users.FirstOrDefault(x => x.Id == Context.User.Id.ToString());
				List<Reminder> Reminders = _dbContext.Reminders.Where(x => x.User == localUser).ToList();

				if (Reminders.Count == 0)
				{
					await ReplyAsync(ReplyHandler.Error.NoContentGeneric);
					return;
				}

				EmbedBuilder embed = EmbedHandler.Create(Context.User);

				foreach (Reminder reminder in Reminders)
				{
					embed.AddField($"{reminder.Id} - {reminder.Message ?? "No message"}", $"{reminder.DueDate}");
				}

				await ReplyAsync(ReplyHandler.Generic, false, embed.Build());
			}
			catch (Exception e)
			{
                await ReplyAsync(ReplyHandler.Context(Result.Failed));
                _reportHandler.ReportAsync(Context, e);
                _logger.LogError(e);
			}
		}

		[Command("add reminder")]
		[Summary("Add a new reminder")]
		public async Task AddReminderAsync(string dueDate, [Remainder] string remainder = "")
		{
			try
			{
				User localUser = _dbContext.Users.FirstOrDefault(x => x.Id == Context.User.Id.ToString());

				if (localUser.TimezoneOffset is null)
				{
					await ReplyAsync(ReplyHandler.Error.TimezoneNotSet);
					return;
				}

				DateTimeOffset dueDateUtc;

				try
				{
					dueDateUtc = new DateTimeOffset(DateTime.Parse(dueDate),
						new TimeSpan((int) localUser.TimezoneOffset, 0, 0));
				}
				catch (Exception)
				{
					await ReplyAsync(ReplyHandler.Error.Command.Reminder);
					return;
				}

				Reminder reminder = new Reminder
				{
					DueDate = dueDateUtc,
					Message = remainder,
					User = localUser
				};

				_dbContext.Reminders.Add(reminder);
				_dbContext.SaveChanges();

				await ReplyAsync(ReplyHandler.Context(Result.Successful));
			}
			catch (Exception e)
			{
				await ReplyAsync(ReplyHandler.Context(Result.Failed));
				_reportHandler.ReportAsync(Context, e);
                _logger.LogError(e);
			}
		}

		[Command("delete reminder")]
		[Alias("remove reminder")]
		[Summary("Remove a reminder")]
		public async Task RemoveReminderAsync(int id)
		{
			try
			{
				User localUser = _dbContext.Users.FirstOrDefault(x => x.Id == Context.User.Id.ToString());
				Reminder reminder = _dbContext.Reminders.FirstOrDefault(x => x.Id == id && x.User == localUser);

				_dbContext.Reminders.Remove(reminder);
				_dbContext.SaveChanges();

				await ReplyAsync(ReplyHandler.Context(Result.Successful));
			}
			catch (Exception e)
			{
				await ReplyAsync(ReplyHandler.Context(Result.Failed));
				_reportHandler.ReportAsync(Context, e);
                _logger.LogError(e);
			}
		}
	}
}
