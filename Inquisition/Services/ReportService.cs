using Discord;
using Discord.Commands;
using Discord.WebSocket;

using Inquisition.Data.Models;
using Inquisition.Handlers;
using Inquisition.Properties;
using Inquisition.Reporting.Core;

using System;
using System.IO;
using System.Threading.Tasks;

namespace Inquisition.Services
{
	public class ReportService
	{
		private static Reporter Reporter;

		static ReportService()
		{
			Reporter = new Reporter
			{
				OutputPath = Path.Combine("Data", "Logs", $"{DateTime.Now:yyyy}", $"{DateTime.Now:MMMM}", $"{DateTime.Now:dd-dddd}"),
				FileName = $"{DateTime.Now:hh-mm-ss}",
				SendEmail = true,
				Host = EmailInfo.Host,
				Port = 587,
				Username = EmailInfo.Username,
				Password = EmailInfo.Password,
				FromAddress = EmailInfo.SenderAddress,
				ToAddress = EmailInfo.TargetAddress,
				XSLFile = Path.Combine("Data", "XSL.xslt")
			};
		}

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
				LogHandler.WriteLine(e);
			}
		}

		// Non-Guild related
		public static void Report(Exception e)
		{
			try
			{
				Report report = new Report
				{
					Guid = Guid.NewGuid(),
					ErrorMessage = e.Message,
					StackTrace = e.StackTrace.Trim().Replace("<", "").Replace(">", "").Replace("&", "")
				};

				Reporter.Report(report);
			}
			catch (InvalidOperationException ex)
			{
				LogHandler.WriteLine(ex);
			}
		}

		// Guild related
		public static void Report(SocketCommandContext ctx, Exception e)
        {
			try
			{
				Report report = new Report
				{
					Guid = Guid.NewGuid(),
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
			catch (InvalidOperationException ex)
			{
				LogHandler.WriteLine(ex);
			}
		}
	}
}
