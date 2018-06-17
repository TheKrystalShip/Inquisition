using Discord;
using Discord.Commands;
using Discord.WebSocket;

using Inquisition.Data.Models;
using Inquisition.Properties;
using Inquisition.Reporting;

using System;
using System.IO;
using System.Threading.Tasks;

namespace Inquisition.Handlers
{
    public class ReportHandler
	{
		private static Reporter Reporter = new Reporter(new ReporterConfig()
			{
				OutputPath = Path.Combine("Data", "Logs", $"{DateTime.Now:yyyy}", $"{DateTime.Now:MMMM}", $"{DateTime.Now:dd-dddd}"),
				FileName = $"{DateTime.Now:HH-mm-ss}",
				SendEmail = true,
				Host = EmailInfo.Host,
				Port = 587,
				Username = EmailInfo.Username,
				Password = EmailInfo.Password,
				FromAddress = EmailInfo.SenderAddress,
				ToAddress = EmailInfo.TargetAddress,
				XSLFile = Path.Combine("Data", "XSL.xslt")
			}
		);

		// CommandService.ExecuteAsync errors
		public static async Task ReportAsync(string errorReason, SocketUserMessage message)
		{
			EmbedBuilder embed = EmbedHandler
				.Create()
				.WithColor(Color.DarkRed);

			embed.WithTitle("Error ocurred");
			embed.WithDescription(errorReason);

			await message.Channel.SendMessageAsync("Oops...", false, embed.Build());
		}

		// Non-Guild related
		public static void Report(Exception e)
		{
			Report report = new Report
			{
				Guid = Guid.NewGuid(),
				ErrorMessage = e.Message,
				StackTrace = e.StackTrace.Replace("<", "").Replace(">", "").Replace("&", "").Trim()
			};

			CatchInnerReports(ref report, e);

			Reporter.Report(report);
		}

		// Guild related
		public static void Report(SocketCommandContext context, Exception e)
        {
			Report report = new Report
			{
				Guid = Guid.NewGuid(),
				Channel = context.Channel.Name,
				ErrorMessage = e.Message,
				Message = context.Message.Content.Replace("<@304353122019704842> ", ""),
				GuildID = context.Guild.Id.ToString(),
				GuildName = context.Guild.Name,
				StackTrace = e.StackTrace.Replace("<", "").Replace(">", "").Replace("&", "").Trim(),
				UserID = context.User.Id.ToString(),
				UserName = context.User.Username
			};

			CatchInnerReports(ref report, e);

			Reporter.Report(report);
		}

		private static void CatchInnerReports(ref Report report, Exception e)
		{
			while (e.InnerException != null)
			{
				e = e.InnerException;
				report.InnerExceptions.Add(new Report.InnerReport {
					Guid = Guid.NewGuid(),
					ErrorMessage = e.Message,
					StackTrace = e.StackTrace.Replace("<", "").Replace(">", "").Replace("&", "").Trim()
				});
			}
		}
	}
}
