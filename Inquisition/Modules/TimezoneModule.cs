using Discord.Commands;

using Inquisition.Data.Models;
using Inquisition.Database;
using Inquisition.Database.Models;
using Inquisition.Handlers;

using System;
using System.Linq;
using System.Threading.Tasks;

using TheKrystalShip.Logging;

namespace Inquisition.Modules
{
    public class TimezoneModule : ModuleBase<SocketCommandContext>
    {
		private readonly DatabaseContext _dbContext;
        private readonly ReportHandler _reportHandler;
        private readonly ILogger<TimezoneModule> _logger;

		public TimezoneModule(
            DatabaseContext dbContext,
            ReportHandler reportHandler,
            ILogger<TimezoneModule> logger)
        {
            _dbContext = dbContext;
            _reportHandler = reportHandler;
            _logger = logger;
        }

		[Command("timezone")]
		[Summary("Tells you your timezone from the database")]
		public async Task ShowTimezoneAsync()
		{
			try
			{
				User localUser = _dbContext.Users.FirstOrDefault(x => x.Id == Context.User.Id.ToString());

				if (localUser.TimezoneOffset is null)
				{
					await ReplyAsync(ReplyHandler.Error.TimezoneNotSet);
					return;
				}

				await ReplyAsync(ReplyHandler.Info.UserTimezone(localUser));
			}
			catch (Exception e)
			{
                await ReplyAsync(ReplyHandler.Context(Result.Failed));
				_reportHandler.ReportAsync(Context, e);
                _logger.LogError(e);
			}
		}

		[Command("set timezone")]
		[Summary("Set your timezone")]
		public async Task SetTimezoneAsync(int offset)
		{
			try
			{
				User localUser = _dbContext.Users.FirstOrDefault(x => x.Id == Context.User.Id.ToString());
				localUser.TimezoneOffset = offset;

				_dbContext.Users.Update(localUser);
				_dbContext.SaveChanges();

				await ReplyAsync(ReplyHandler.Info.UserTimezone(localUser));
			}
			catch (Exception e)
			{
				await ReplyAsync(ReplyHandler.Context(Result.Failed));
				_reportHandler.ReportAsync(Context, e);
                _logger.LogError(e);
			}
		}
	}
}
