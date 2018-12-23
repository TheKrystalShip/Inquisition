using Discord;
using Discord.Commands;

using Microsoft.EntityFrameworkCore;

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

using TheKrystalShip.Inquisition.Domain;
using TheKrystalShip.Inquisition.Extensions;

namespace TheKrystalShip.Inquisition.Core.Modules
{
    public class ActivityModule : Module
    {
        [Command("activities")]
        [Alias("tasks")]
        public async Task<RuntimeResult> ShowActivitiesAsync()
        {
            List<Domain.Activity> ActivityList = await Database.Activities
                .Where(x => x.User.Id == User.Id)
                .ToListAsync();

            if (ActivityList.Count is 0)
            {
                return new ErrorResult(CommandError.ObjectNotFound, "No activities were found");
            }

            EmbedBuilder embed = new EmbedBuilder()
                .Create(ActivityList);

            return new SuccessResult("Success", embed);
        }
    }

    [Group("add activity")]
    [Alias("schedule")]
    [RequireUserPermission(GuildPermission.Administrator)]
    public class ScheduleActivityModule : Module
    {
        [Command("shutdown")]
        [Alias("shutdown in")]
        public async Task ScheduleShutdownAsync(int time = 0)
        {
            ScheduleActivity($"/s /t {time}", time);
        }

        [Command("reboot")]
        [Alias("reboot in", "restart", "restart in")]
        public async Task ScheduleRebootAsync(int time = 0)
        {
            ScheduleActivity($"/r /t {time}", time);
        }

        [Command("abort")]
        [Alias("cancel")]
        public async Task AbortActivityAsync()
        {
            Process.Start("shutdown", "/a");
        }

        private void ScheduleActivity(string Args, int time)
        {
            Domain.Activity activity = new Domain.Activity
            {
                Name = "shutdown",
                Arguments = Args,
                ScheduledTime = DateTime.Now,
                DueTime = DateTime.Now.AddSeconds(time),
                User = User,
                Guild = Guild
            };

            Database.Activities.Add(activity);
        }
    }
}
