using Discord.Commands;

using Inquisition.Data.Handlers;
using Inquisition.Data.Models;
using Inquisition.Handlers;
using Inquisition.Services;

using System;
using System.Linq;
using System.Threading.Tasks;

namespace Inquisition.Modules
{
	public class TimezoneModule : ModuleBase<SocketCommandContext>
    {
		private DbHandler db;

		public TimezoneModule(DbHandler dbHandler) => db = dbHandler;

		[Command("timezone", RunMode = RunMode.Async)]
		[Summary("Tells you your timezone from the database")]
		public async Task ShowTimezoneAsync()
		{
			try
			{
				User localUser = db.Users.FirstOrDefault(x => x.Id == Context.User.Id.ToString());

				if (localUser.TimezoneOffset is null)
				{
					await ReplyAsync(ReplyHandler.Error.TimezoneNotSet);
					return;
				}

				await ReplyAsync(ReplyHandler.Info.UserTimezone(localUser));
			}
			catch (Exception e)
			{
				ReportService.Report(Context, e);
			}
		}

		[Command("set timezone", RunMode = RunMode.Async)]
		[Summary("Set your timezone")]
		public async Task SetTimezoneAsync(int offset)
		{
			try
			{
				User localUser = db.Users.FirstOrDefault(x => x.Id == Context.User.Id.ToString());
				localUser.TimezoneOffset = offset;

				db.Users.Update(localUser);
				db.SaveChanges();

				await ReplyAsync(ReplyHandler.Info.UserTimezone(localUser));
			}
			catch (Exception e)
			{
				await ReplyAsync(ReplyHandler.Context(Result.Failed));
				ReportService.Report(Context, e);
			}
		}
	}
}
