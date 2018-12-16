using Discord;
using Discord.Commands;
using Discord.WebSocket;

using Microsoft.EntityFrameworkCore;

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using TheKrystalShip.Inquisition.Core.Modules;
using TheKrystalShip.Inquisition.Domain;
using TheKrystalShip.Inquisition.Extensions;

namespace TheKrystalShip.Inquisition.Core.Modules
{
    public class AlertModule : Module
    {
        [Command("alerts")]
        [Summary("Displays a list of all of your notifications")]
        public async Task ListAlertsAsync()
        {
            List<Alert> Alerts = Database.Alerts
                .Where(x => x.User.Id == User.Id)
                .Include(x => x.User)
                .Include(x => x.TargetUser)
                .ToList();

            if (Alerts.Count is 0)
            {
                await ReplyAsync("No content");
                return;
            }

            EmbedBuilder embed = new EmbedBuilder().Create(Context.User);
            string description = "";

            foreach (Alert n in Alerts)
            {
                description += $"Alert for **{n.TargetUser.Username}**\n";
            }

            embed.WithDescription(description);

            await ReplyAsync(embed);
        }

        [Command("add alert")]
        [Summary("Add a new alert, must specify a target user")]
        public async Task AddAlertAsync(SocketGuildUser targetAlert)
        {
            User target = Database.Users
                .FirstOrDefault(x => x.Id == targetAlert.Id);

            Alert alert = new Alert
            {
                User = User,
                TargetUser = target
            };

            Database.Alerts.Add(alert);

            await ReplyAsync("Success");
        }

        [Command("delete alert")]
        [Alias("remove alert")]
        [Summary("Removes an alert, must specify a target user")]
        public async Task RemoveAlertAsync(SocketGuildUser targetUser)
        {
            User target = Database
                .Users
                .FirstOrDefault(x => x.Id == targetUser.Id);

            Alert alert = Database
                .Alerts
                .FirstOrDefault(x => x.User.Id == User.Id && x.TargetUser == target);

            if (alert is null)
            {
                await ReplyAsync("Not found");
                return;
            }

            Database.Alerts.Remove(alert);

            await ReplyAsync("Success");
        }
    }
}
