using Discord;
using Discord.Commands;
using Discord.WebSocket;

using Inquisition.Data.Models;
using Inquisition.Database;
using Inquisition.Handlers;
using Inquisition.Logging;

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;

namespace Inquisition.Modules
{
    [RequireUserPermission(GuildPermission.Administrator)]
    public class AdminModule : ModuleBase<SocketCommandContext>
    {
		private readonly DatabaseContext _dbContext;
        private readonly ReportHandler _reportHandler;
        private readonly ILogger<AdminModule> _logger;

        public AdminModule(
            DatabaseContext dbContext,
            ReportHandler reportHandler,
            ILogger<AdminModule> logger)
        {
            _dbContext = dbContext;
            _reportHandler = reportHandler;
            _logger = logger;
        }

        [Command("prune")]
        [Summary("[Admin] Prunes all inactive members from the server")]
        public async Task PruneMembersAsync(int days)
        {
            try
            {
                if (days < 7)
                {
                    await ReplyAsync("Minimum is 7 days of innactivity");
                    return;
                }

                int members = await Context.Guild.PruneUsersAsync(days);
                await ReplyAsync(ReplyHandler.Info.UsersPruned(members, days));
            }
            catch (Exception e)
            {
                await ReplyAsync(ReplyHandler.Context(Result.Failed));
                _reportHandler.ReportAsync(e);
                _logger.LogError(e);
            }
        }

        [Command("ban")]
        [Summary("[Admin] Bans a user from the server")]
        public async Task BanMemberAsync(SocketGuildUser user, [Remainder] string reason = "")
        {
            try
            {
                await user.SendMessageAsync($"You've been banned from {Context.Guild}, reason: {reason}.");
                await Context.Guild.AddBanAsync(user, 0, reason);
                await ReplyAsync(ReplyHandler.Info.UserBanned(user));
            }
            catch (Exception e)
            {
                await ReplyAsync(ReplyHandler.Context(Result.Failed));
                _reportHandler.ReportAsync(e);
                _logger.LogError(e);
            }
        }

        [Command("wipe")]
		[Alias("wipe last", "wipe the last")]
        [Summary("[Admin] Wipes X number of messages from a text channel")]
        public async Task WipeChannelAsync(uint amount = 1, [Remainder] string s = "")
        {
            try
            {
                IEnumerable<IMessage> messages = await Context.Channel
                    .GetMessagesAsync((int)amount + 1)
                    .Flatten();

                await Context.Channel
                    .DeleteMessagesAsync(messages);

                const int delay = 5000;
                IUserMessage reply = await ReplyAsync($"Deleted {amount} messages. _This message will be deleted in {delay / 1000} seconds._");

                await Task.Delay(delay);
                await reply.DeleteAsync();
            }
            catch (Exception e)
            {
                await ReplyAsync(ReplyHandler.Context(Result.Failed));
                _reportHandler.ReportAsync(e);
                _logger.LogError(e);
            }
        }

        [Command("error")]
        public async Task RaiseErrorAsync()
        {
			try
			{
				try
				{
					var num = int.Parse("abc");
				}
				catch (Exception inner)
				{
					try
					{
						var openLog = File.Open("DoesNotExist", FileMode.Open);
					}
					catch (Exception inner2)
					{
						throw new FileNotFoundException(inner2.Message, inner);
					}
				}
			}
			catch (Exception e)
			{
				await ReplyAsync("", false, EmbedHandler.Create(e).Build());
				_reportHandler.ReportAsync(Context, e);
                _logger.LogError(e);
			}
        }

		[Command("hello there")]
		public async Task TestCommandAsync()
		{
            //User me = _repository.Users.SelectFirst(x => x.Id == Context.User.Id.ToString());
            await ReplyAsync($"General ⚔️⚔️");
		}

		[Command("restart")]
		[Alias("reboot")]
		public async Task RebootAsync()
		{
			await ReplyAsync("Rebooting...");

			Process.Start(new ProcessStartInfo()
                {
                    FileName = "dotnet",
                    Arguments = "\"" + Assembly.GetExecutingAssembly().Location + "\"",
                    WindowStyle = ProcessWindowStyle.Normal,
                    CreateNoWindow = false
                }
            );

			Environment.Exit(0);
		}

		[Command("shutdown")]
		[Alias("quit")]
		public async Task Shutdown()
		{
			await ReplyAsync("Shutting down...");
			Environment.Exit(0);
		}
	}

	[Group("stop")]
	public class StopAdminModule : ModuleBase<SocketCommandContext>
	{
        private readonly ServiceHandler _serviceHandler;
        private readonly ILogger<AdminModule> _logger;

        public StopAdminModule(ServiceHandler serviceHandler, ILogger<AdminModule> logger)
        {
            _serviceHandler = serviceHandler;
            _logger = logger;
        }

		[Command("loops")]
		[Alias("all loops")]
		public async Task StopAllLoopsAsync()
		{
			_serviceHandler.StopAllLoops();
			await ReplyAsync("All loops stopped");
            _logger.LogInformation("Stopped all loops");
		}
	}

	[Group("start")]
	public class StartAdminModule : ModuleBase<SocketCommandContext>
	{
        private readonly ServiceHandler _serviceHandler;

        public StartAdminModule(ServiceHandler serviceHandler)
        {
            _serviceHandler = serviceHandler;
        }

		[Command("loops")]
		[Alias("all loops")]
		public async Task StartAllLoopsAsync()
		{
			_serviceHandler.StartAllLoops();
			await ReplyAsync("All loops started");
		}
	}
}
