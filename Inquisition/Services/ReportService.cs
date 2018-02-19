using Discord;
using Discord.Commands;
using Discord.WebSocket;

using Inquisition.Data.Models;
using Inquisition.Handlers;
using Inquisition.Reporting.Core;

using System;
using System.Threading.Tasks;

namespace Inquisition.Services
{
	public class ReportService
	{
		private static Reporter Reporter;

		public ReportService(Reporter reporter) => Reporter = reporter;

		// CommandService.ExecuteAsync errors
		public static async Task Report(string error, SocketMessage msg)
		{
			try
			{
				EmbedBuilder embed = EmbedHandler
					.Create()
					.WithColor(Color.DarkRed);

				embed.WithTitle("Error ocurred");
				embed.WithDescription(error);

				await msg.Channel.SendMessageAsync("Oops...", false, embed.Build());
			}
			catch (Exception e)
			{
				Console.WriteLine(e);
			}
		}

		// Non-Guild related
		public static void Report(Exception e)
		{
			Report report = new Report
			{
				Guid = Guid.NewGuid(),
				ErrorMessage = e.Message,
				StackTrace = e.StackTrace.Trim().Replace("<", "").Replace(">", "").Replace("&", "")
			};

			Reporter.Report(report);
		}

		// Guild related
		public static void Report(SocketCommandContext ctx, Exception e)
        {
			Report report = new Report
			{
				Guid = Guid.NewGuid(),
				Type = Reporting.Models.Type.General,
				Channel = ctx.Channel.Name,
				ErrorMessage = e.Message,
				Message = ctx.Message.Content.Replace("<@304353122019704842> ", ""),
				GuildID = ctx.Guild.Id.ToString(),
				GuildName = ctx.Guild.Name,
				StackTrace = e.StackTrace.Trim().Replace("<", "").Replace(">", "").Replace("&", ""),
				UserID = ctx.User.Id.ToString(),
				UserName = ctx.User.Username
			};

			Reporter.Report(report);
		}
	}
}
