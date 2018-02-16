using Discord;
using Discord.Commands;

using Inquisition.Data.Handlers;
using Inquisition.Data.Models;
using Inquisition.Handlers;

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace Inquisition.Modules
{
	public class ActivityModule : ModuleBase<SocketCommandContext>
	{
		private DbHandler db;
		public ActivityModule(DbHandler dbHandler) => db = dbHandler;

		[Command("activities")]
		[Alias("tasks")]
		[Summary("Show a user's activities")]
		public async Task ShowActivitiesAsync()
		{
			string contextUserId = Context.User.Id.ToString();
			User user = db.Users.FirstOrDefault(x => x.Id == contextUserId);

			List<Data.Models.Activity> ActivityList = db.Activities.Where(x => x.User == user).ToList();

			if (ActivityList.Count == 0)
			{
				await ReplyAsync(ReplyHandler.Error.NoContentGeneric);
				return;
			}

			EmbedBuilder embed = EmbedHandler.Create(ActivityList);

			await ReplyAsync(ReplyHandler.Generic, false, embed.Build());
		}
	}

	[Group("schedule")]
	[RequireUserPermission(Discord.GuildPermission.Administrator)]
    public class ScheduleActivityModule : ModuleBase<SocketCommandContext>
    {
		private DbHandler db;
		public ScheduleActivityModule(DbHandler dbHandler) => db = dbHandler;

		[Command("shutdown")]
		[Alias("shutdown in")]
		public async Task ScheduleShutdownAsync(int time = 0)
		{
			ScheduleTask($"/s /t {time}", time);
		}

		[Command("reboot")]
		[Alias("reboot in", "restart", "restart in")]
		public async Task ScheduleRebootAsync(int time = 0)
		{
			ScheduleTask($"/r /t {time}", time);
		}

		[Command("abort")]
		[Alias("cancel")]
		public async Task AbortActivityAsync()
		{
			Process.Start("shutdown", "/a");
		}

		private void ScheduleTask(string Args, int time)
		{
			string contextUserId = Context.User.Id.ToString();
			string contextGuildId = Context.Guild.Id.ToString();
			User user = db.Users.FirstOrDefault(x => x.Id == contextUserId);
			Guild guild = db.Guilds.FirstOrDefault(x => x.Id == contextGuildId);

			Data.Models.Activity task = new Data.Models.Activity
			{
				Name = "shutdown",
				Arguments = Args,
				ScheduledTime = DateTime.Now,
				DueTime = DateTime.Now.AddSeconds(time),
				User = user,
				Guild = guild
			};

			db.Activities.Add(task);
			db.SaveChanges();
		}
    }
}
