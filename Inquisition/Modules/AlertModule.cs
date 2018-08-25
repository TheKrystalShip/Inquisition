using Discord;
using Discord.Commands;
using Discord.WebSocket;

using Microsoft.EntityFrameworkCore;

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
    public class AlertModule : ModuleBase<SocketCommandContext>
    {
		private DatabaseContext _dbContext;
        private readonly ReportHandler _reportHandler;
        private readonly ILogger<AlertModule> _logger;

		public AlertModule(
            DatabaseContext dbContext,
            ReportHandler reportHandler,
            ILogger<AlertModule> logger)
        {
            _dbContext = dbContext;
            _reportHandler = reportHandler;
            _logger = logger;
        }

		[Command("alerts")]
		[Summary("Displays a list of all of your notifications")]
		public async Task ListAlertsAsync()
		{
			try
			{
				User localUser = _dbContext.Users.FirstOrDefault(x => x.Id == Context.User.Id.ToString());

				List<Alert> Alerts = _dbContext.Alerts
					.Where(x => x.User == localUser)
					.Include(x => x.User)
					.Include(x => x.TargetUser)
					.ToList();

				if (Alerts.Count == 0)
				{
					await ReplyAsync(ReplyHandler.Error.NoContentGeneric);
					return;
				}

				EmbedBuilder embed = EmbedHandler.Create(Context.User);
				string description = "";

				foreach (Alert n in Alerts)
				{
					description += $"Alert for **{n.TargetUser.Username}**\n";
				}

				embed.WithDescription(description);

				await ReplyAsync(ReplyHandler.Generic, false, embed.Build());
			}
			catch (Exception e)
			{
				_reportHandler.ReportAsync(Context, e);
                _logger.LogError(e);
			}
		}

		[Command("add alert")]
		[Summary("Add a new alert, must specify a target user")]
		public async Task AddAlertAsync(SocketGuildUser targetAlert)
		{
			try
			{
				User author = _dbContext.Users.FirstOrDefault(x => x.Id == Context.User.Id.ToString());
				User target = _dbContext.Users.FirstOrDefault(x => x.Id == targetAlert.Id.ToString());

				Alert alert = new Alert
				{
					User = author,
					TargetUser = target
				};

				_dbContext.Alerts.Add(alert);
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

		[Command("delete alert")]
		[Alias("remove alert")]
		[Summary("Removes an alert, must specify a target user")]
		public async Task RemoveAlertAsync(SocketGuildUser targetUser)
		{
			try
			{
				User author = _dbContext
                    .Users
                    .FirstOrDefault(x => x.Id == Context.User.Id.ToString());

				User target = _dbContext
                    .Users
                    .FirstOrDefault(x => x.Id == targetUser.Id.ToString());

				Alert alert = _dbContext
                    .Alerts
                    .FirstOrDefault(x => x.User == author && x.TargetUser == target);

				if (alert is null)
				{
					await ReplyAsync(ReplyHandler.Error.NotFound.Alert);
					return;
				}

				_dbContext.Alerts.Remove(alert);
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
