using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Inquisition.Data.Handlers;
using Inquisition.Data.Models;
using Inquisition.Handlers;
using Inquisition.Services;

using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Inquisition.Modules
{
	[RequireUserPermission(GuildPermission.Administrator)]
    public class AdminModule : ModuleBase<SocketCommandContext>
    {
		private DbHandler db;

		public AdminModule(DbHandler dbHandler) => db = dbHandler;

        [Command("prune", RunMode = RunMode.Async)]
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
                ReportService.Report(e);
            }
        }

        [Command("ban", RunMode = RunMode.Async)]
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
                ReportService.Report(e);
            }
        }

        [Command("wipe", RunMode = RunMode.Async), Alias("wipe last", "wipe the last")]
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
                ReportService.Report(e);
            }
        }

        [Command("error", RunMode = RunMode.Async)]
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
					catch
					{
						throw new FileNotFoundException(inner.Message, inner);
					}
				}
			}
			catch (Exception e)
			{
				ReportService.Report(Context, e);
				await ReplyAsync("", false, EmbedHandler.Create(e));
			}
        }

		[Command("test", RunMode = RunMode.Async)]
		public async Task TestCommandAsync(params string[] s)
		{

		}
    }
}
