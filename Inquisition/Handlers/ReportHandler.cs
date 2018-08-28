using Discord;
using Discord.Commands;
using Discord.WebSocket;

using Inquisition.Reporting;

using System;

using TheKrystalShip.Inquisition.Data.Models;

namespace TheKrystalShip.Inquisition.Handlers
{
    public class ReportHandler
    {
        private Reporter _reporter = new ReporterBuilder().Build();

        // CommandService.ExecuteAsync errors
        public async void ReportAsync(string errorReason, SocketUserMessage message)
        {
            EmbedBuilder embed = EmbedHandler
                .Create()
                .WithColor(Color.DarkRed);

            embed.WithTitle("Error ocurred");
            embed.WithDescription(errorReason);

            await message.Channel.SendMessageAsync("Oops...", false, embed.Build());
        }

        // Non-Guild related
        public async void ReportAsync(Exception e)
        {
            Report report = new Report
            {
                ErrorMessage = e.Message,
                StackTrace = e.StackTrace.Replace("<", "").Replace(">", "").Replace("&", "").Trim()
            };

            CatchInnerReports(ref report, e);

            await _reporter.ReportAsync(report);
        }

        // Guild related
        public async void ReportAsync(SocketCommandContext context, Exception e)
        {
            Report report = new Report
            {
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

            await _reporter.ReportAsync(report);
        }

        private void CatchInnerReports(ref Report report, Exception e)
        {
            while (e.InnerException != null)
            {
                e = e.InnerException;
                report.InnerExceptions.Add(new Report.InnerReport
                {
                    ErrorMessage = e.Message,
                    StackTrace = e.StackTrace.Replace("<", "").Replace(">", "").Replace("&", "").Trim()
                });
            }
        }
    }
}
