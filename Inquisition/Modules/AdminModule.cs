using Discord;
using Discord.Commands;
using Discord.WebSocket;

using Inquisition.Database;
using Inquisition.Database.Models;
using Inquisition.Database.Repositories;
using Inquisition.Handlers;
using Inquisition.Logging;

using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;

namespace Inquisition.Modules
{
    [RequireUserPermission(GuildPermission.Administrator)]
    public class AdminModule : ModuleBase<SocketCommandContext>
    {
		private readonly DatabaseContext db;
        private readonly IRepositoryWrapper Repository;
        private readonly ILogger<AdminModule> _logger;

        public AdminModule(DatabaseContext dbHandler, IRepositoryWrapper repository, ILogger<AdminModule> logger)
        {
            db = dbHandler;
            Repository = repository;
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

                var members = await Context.Guild.PruneUsersAsync(days);
                await ReplyAsync(ReplyHandler.Info.UsersPruned(members, days));
            }
            catch (Exception e)
            {
                ReportHandler.Report(e);
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
                ReportHandler.Report(e);
            }
        }

        [Command("wipe")]
		[Alias("wipe last", "wipe the last")]
        [Summary("[Admin] Wipes X number of messages from a text channel")]
        public async Task WipeChannelAsync(uint amount = 1, [Remainder] string s = "")
        {
            try
            {
                var messages = await Context.Channel.GetMessagesAsync((int)amount + 1).Flatten();
                await Context.Channel.DeleteMessagesAsync(messages);

                const int delay = 5000;
                var m = await ReplyAsync($"Deleted {amount} messages. _This message will be deleted in {delay / 1000} seconds._");

                await Task.Delay(delay);
                await m.DeleteAsync();
            }
            catch (Exception e)
            {
                ReportHandler.Report(e);
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
				ReportHandler.Report(Context, e);
				await ReplyAsync("", false, EmbedHandler.Create(e));
			}
        }

		[Command("hello there")]
		public async Task TestCommandAsync()
		{
            User me = Repository.Users.SelectFirst(x => x.Id == Context.User.Id.ToString());
            _logger.LogInformation(me.Username);
            await ReplyAsync($"General {me.Username} ⚔️⚔️");
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

        public StopAdminModule(ServiceHandler serviceHandler)
        {
            _serviceHandler = serviceHandler;
        }

		[Command("loops")]
		[Alias("all loops")]
		public async Task StopAllLoopsAsync()
		{
			_serviceHandler.StopAllLoops();
			await ReplyAsync("All loops stopped");
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
