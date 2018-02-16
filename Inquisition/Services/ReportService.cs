using Discord;
using Discord.Commands;
using Discord.WebSocket;

using Inquisition.Data.Handlers;
using Inquisition.Data.Models;
using Inquisition.Handlers;

using System;
using System.Threading.Tasks;

namespace Inquisition.Services
{
	public class ReportService
	{
		// CommandService.ExecuteAsync errors
		public static async Task Report(string e, SocketMessage msg)
		{
			try
			{
				EmbedBuilder embed = EmbedHandler
					.Create()
					.WithColor(Color.DarkRed);

				embed.WithTitle("Error ocurred");
				embed.WithDescription(e);

				await msg.Channel.SendMessageAsync("Oops...", false, embed);
			}
			catch (Exception ex)
			{
				LogHandler.WriteLine(ex);
			}
		}

		// Non-Guild related
		public static void Report(Exception e, Severity s = Severity.Warning, Data.Models.Type t = Data.Models.Type.General)
		{
			DbHandler db = new DbHandler();
			Report report = new Report
			{
				Guid = Guid.NewGuid(),
				Severity = s,
				Type = t,
				ErrorMessage = e.Message,
				StackTrace = e.StackTrace.Trim().Replace("<", "").Replace(">", "").Replace("&", "")
			};

			FillInnerExceptions(ref report, e);

			LogHandler.GenerateLog(ref report);
			db.Reports.Add(report);
			db.SaveChanges();
			LogHandler.WriteLine("Unexpected error ocurred, a log file has been created.");
		}

		// Guild related
		public static void Report(SocketCommandContext ctx, Exception e)
        {
			DbHandler db = new DbHandler();
			Report report = new Report
			{
				Guid = Guid.NewGuid(),
				Severity = Severity.Critical,
				Type = Data.Models.Type.Guild,
				Channel = ctx.Channel.Name,
				ErrorMessage = e.Message,
				Message = ctx.Message.Content.Replace("<@304353122019704842> ", ""),
				GuildID = ctx.Guild.Id.ToString(),
				GuildName = ctx.Guild.Name,
				StackTrace = e.StackTrace.Trim().Replace("<", "").Replace(">", "").Replace("&", ""),
				UserID = ctx.User.Id.ToString(),
				UserName = ctx.User.Username
			};

			FillInnerExceptions(ref report, e);

			LogHandler.GenerateLog(ref report);
			EmailHandler.SendReport(report);
			db.Reports.Add(report);
			db.SaveChanges();
        }

		private static void FillInnerExceptions(ref Report report, Exception e)
		{
			while (e.InnerException != null)
			{
				e = e.InnerException;
				report.InnerExceptions.Add(new Report()
				{
					Guid = Guid.NewGuid(),
					Severity = Severity.Critical,
					Type = Data.Models.Type.Inner,
					ErrorMessage = e.Message,
					StackTrace = e.StackTrace.Trim().Replace("<", "").Replace(">", "").Replace("&", "")
				});
			}
		}
	}
}
