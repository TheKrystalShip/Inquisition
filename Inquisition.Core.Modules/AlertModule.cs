using Discord;
using Discord.Commands;
using Discord.WebSocket;

using Microsoft.EntityFrameworkCore;

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using TheKrystalShip.Inquisition.Domain;
using TheKrystalShip.Inquisition.Tools;

namespace TheKrystalShip.Inquisition.Core.Modules
{
    public class AlertModule : Module
    {
        [Command("alerts")]
        [Summary("Displays a list of all of your notifications")]
        public async Task<RuntimeResult> ListAlertsAsync()
        {
            List<Alert> alertList = await Database.Alerts
                .Where(x => x.User.Id == User.Id)
                .Include(x => x.User)
                .Include(x => x.TargetUser)
                .ToListAsync();

            if (alertList.Count is 0)
            {
                return new ErrorResult("No alerts were found");
            }

            Embed embed = EmbedFactory.Create(ResultType.Success, builder => {
                string description = "";
                foreach (Alert alert in alertList)
                {
                    description += $"Alert for **{alert.TargetUser.Username}**\n";
                }

                builder.WithDescription(description);
            });

            return new SuccessResult(embed);
        }

        [Command("add alert")]
        [Summary("Add a new alert, must specify a target user")]
        public async Task<RuntimeResult> AddAlertAsync(SocketGuildUser targetAlert)
        {
            User target = Database.Users
                .FirstOrDefault(x => x.Id == targetAlert.Id);

            if (target is null)
            {
                return new ErrorResult(CommandError.ObjectNotFound, "Target user was not found");
            }

            Alert alert = new Alert
            {
                User = User,
                TargetUser = target
            };

            Database.Alerts.Add(alert);

            return new SuccessResult();
        }

        [Command("delete alert")]
        [Alias("remove alert")]
        [Summary("Removes an alert, must specify a target user")]
        public async Task<RuntimeResult> RemoveAlertAsync(SocketGuildUser targetUser)
        {
            User target = await Database.Users
                .FirstOrDefaultAsync(x => x.Id == targetUser.Id);

            if (target is null)
            {
                return new ErrorResult(CommandError.ObjectNotFound, "Target user was not found");
            }

            Alert alert = await Database.Alerts
                .FirstOrDefaultAsync(x => x.User.Id == User.Id && x.TargetUser == target);

            if (alert is null)
            {
                return new ErrorResult(CommandError.ObjectNotFound, "Alert not found");
            }

            Database.Alerts.Remove(alert);

            return new SuccessResult();
        }
    }
}
