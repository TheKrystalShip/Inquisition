using Discord;
using Discord.Commands;
using Discord.WebSocket;

using Inquisition.Data.Models;
using Inquisition.Database.Core;
using Inquisition.Database.Models;
using Inquisition.Handlers;
using Inquisition.Services;

using Microsoft.EntityFrameworkCore;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Inquisition.Modules
{
	public class AlertModule : ModuleBase<SocketCommandContext>
    {
		private DatabaseContext db;

		public AlertModule(DatabaseContext dbHandler) => db = dbHandler;

		[Command("alerts", RunMode = RunMode.Async)]
		[Summary("Displays a list of all of your notifications")]
		public async Task ListAlertsAsync()
		{
			try
			{
				User localUser = db.Users.FirstOrDefault(x => x.Id == Context.User.Id.ToString());
				List<Alert> Alerts = db.Alerts
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
				ReportService.Report(Context, e);
			}
		}

		[Command("add alert", RunMode = RunMode.Async)]
		[Summary("Add a new alert, must specify a target user")]
		public async Task AddAlertAsync(SocketGuildUser targetAlert)
		{
			try
			{
				User author = db.Users.FirstOrDefault(x => x.Id == Context.User.Id.ToString());
				User target = db.Users.FirstOrDefault(x => x.Id == targetAlert.Id.ToString());

				Alert alert = new Alert
				{
					User = author,
					TargetUser = target
				};

				db.Alerts.Add(alert);
				db.SaveChanges();

				await ReplyAsync(ReplyHandler.Context(Result.Successful));
			}
			catch (Exception e)
			{
				await ReplyAsync(ReplyHandler.Context(Result.Failed));
				ReportService.Report(Context, e);
			}
		}

		[Command("delete alert", RunMode = RunMode.Async)]
		[Alias("remove alert")]
		[Summary("Removes an alert, must specify a target user")]
		public async Task RemoveAlertAsync(SocketGuildUser targetUser)
		{
			try
			{
				User author = db.Users.FirstOrDefault(x => x.Id == Context.User.Id.ToString());
				User target = db.Users.FirstOrDefault(x => x.Id == targetUser.Id.ToString());

				Alert alert = db.Alerts.Where(x => x.User == author && x.TargetUser == target).FirstOrDefault();

				if (alert is null)
				{
					await ReplyAsync(ReplyHandler.Error.NotFound.Alert);
					return;
				}

				db.Alerts.Remove(alert);
				db.SaveChanges();

				await ReplyAsync(ReplyHandler.Context(Result.Successful));
			}
			catch (Exception e)
			{
				await ReplyAsync(ReplyHandler.Context(Result.Failed));
				ReportService.Report(Context, e);
			}
		}
	}
}
