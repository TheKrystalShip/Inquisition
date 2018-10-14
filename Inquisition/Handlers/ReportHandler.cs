using Discord;
using Discord.Commands;
using Discord.WebSocket;

using Inquisition.Reporting;

using System;

using TheKrystalShip.Inquisition.Data.Models;
using TheKrystalShip.Inquisition.Extensions;

namespace TheKrystalShip.Inquisition.Handlers
{
    public class ReportHandler
    {
        private Reporter _reporter = new ReporterBuilder().Build();

        // CommandService.ExecuteAsync errors
        public async void ReportAsync(string errorReason, SocketUserMessage message)
        {
            EmbedBuilder embed = new EmbedBuilder()
                .Create()
                .WithColor(Color.DarkRed);

            embed.WithTitle("Error ocurred");
            embed.WithDescription(errorReason);

            await message.Channel.SendMessageAsync("Oops...", false, embed.Build()).ConfigureAwait(false);
        }

        // Non-Guild related
        public async void ReportAsync(Exception e)
        {
            Report report = new Report
            {
                ErrorMessage = e.Message,
                StackTrace = e.StackTrace
                    .Replace("<", string.Empty)
                    .Replace(">", string.Empty)
                    .Replace("&", string.Empty)
                    .Trim()
            };

            CatchInnerReports(ref report, e);

            await _reporter.ReportAsync(report).ConfigureAwait(false);
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

            await _reporter.ReportAsync(report).ConfigureAwait(false);
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
