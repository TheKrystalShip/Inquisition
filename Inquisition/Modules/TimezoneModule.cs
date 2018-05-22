﻿using Discord.Commands;

using Inquisition.Data.Models;
using Inquisition.Database;
using Inquisition.Database.Models;
using Inquisition.Handlers;

using System;
using System.Linq;
using System.Threading.Tasks;

namespace Inquisition.Modules
{
	public class TimezoneModule : ModuleBase<SocketCommandContext>
    {
		private DatabaseContext db;

		public TimezoneModule(DatabaseContext dbHandler) => db = dbHandler;

		[Command("timezone")]
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
				ReportHandler.Report(Context, e);
			}
		}

		[Command("set timezone")]
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
				ReportHandler.Report(Context, e);
			}
		}
	}
}
