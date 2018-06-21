using Discord;
using Discord.Commands;

using Inquisition.Database;
using Inquisition.Database.Models;
using Inquisition.Database.Repositories;
using Inquisition.Handlers;
using Inquisition.Logging;

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace Inquisition.Modules
{
    public class ActivityModule : ModuleBase<SocketCommandContext>
	{
		private readonly DatabaseContext _dbContext;
        private readonly IRepositoryWrapper _repository;
        private readonly ILogger<ActivityModule> _logger;

		public ActivityModule(
            DatabaseContext dbContext,
            IRepositoryWrapper repository,
            ILogger<ActivityModule> logger)
        {
            _dbContext = dbContext;
            _repository = repository;
            _logger = logger;
        }

		[Command("activities")]
		[Alias("tasks")]
		public async Task ShowActivitiesAsync()
		{
			string contextUserId = Context.User.Id.ToString();
			User user = _dbContext.Users.FirstOrDefault(x => x.Id == contextUserId);

			List<Database.Models.Activity> ActivityList = _dbContext.Activities.Where(x => x.User == user).ToList();

			if (ActivityList.Count == 0)
			{
				await ReplyAsync(ReplyHandler.Error.NoContentGeneric);
				return;
			}

			EmbedBuilder embed = EmbedHandler.Create(ActivityList);

			await ReplyAsync(ReplyHandler.Generic, false, embed.Build());
		}
	}

	[Group("add activity")]
	[Alias("schedule")]
	[RequireUserPermission(GuildPermission.Administrator)]
    public class ScheduleActivityModule : ModuleBase<SocketCommandContext>
    {
		private DatabaseContext db;
		public ScheduleActivityModule(DatabaseContext dbHandler) => db = dbHandler;

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
			string contextUserId = Context.User.Id.ToString();
			string contextGuildId = Context.Guild.Id.ToString();
			User user = db.Users.FirstOrDefault(x => x.Id == contextUserId);
			Guild guild = db.Guilds.FirstOrDefault(x => x.Id == contextGuildId);

			Database.Models.Activity activity = new Database.Models.Activity
			{
				Name = "shutdown",
				Arguments = Args,
				ScheduledTime = DateTime.Now,
				DueTime = DateTime.Now.AddSeconds(time),
				User = user,
				Guild = guild
			};

			db.Activities.Add(activity);
			db.SaveChanges();
		}
    }
}
