using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Inquisition.Handlers;
using System.Threading.Tasks;
using Inquisition.Data;

namespace Inquisition.Modules
{
    [RequireUserPermission(GuildPermission.Administrator)]
    public class AdminModule : ModuleBase<SocketCommandContext>
    {
        [Command("prune", RunMode = RunMode.Async)]
        [Summary("[Admin] Prunes all inactive members from the server")]
        public async Task PruneMembersAsync(int d)
        {
            if (d < 7)
            {
                await ReplyAsync("Minimum is 7 days of innactivity");
                return;
            }

            var n = await Context.Guild.PruneUsersAsync(d);
            await ReplyAsync(Message.Info.UsersPruned(n, d));
        }

        [Command("ban", RunMode = RunMode.Async)]
        [Summary("[Admin] Bans a user from the server")]
        public async Task BanMemberAsync(SocketGuildUser user, [Remainder] string reason = "")
        {
            await user.SendMessageAsync($"You've been banned from {Context.Guild}, reason: {reason}.");
            await Context.Guild.AddBanAsync(user, 0, reason);
            await ReplyAsync(Message.Info.UserBanned(user.Username));
        }

        [Command("wipe", RunMode = RunMode.Async)]
        [Alias("wipe last", "wipe the last")]
        [Summary("[Admin] Wipes X number of messages from a text channel")]
        public async Task WipeChannelAsync(uint amount = 1, [Remainder] string s = "")
        {
            var messages = await Context.Channel.GetMessagesAsync((int)amount + 1).Flatten();
            await Context.Channel.DeleteMessagesAsync(messages);

            const int delay = 5000;
            var m = await ReplyAsync($"Deleted {amount} messages. _This message will be deleted in {delay / 1000} seconds._");

            await Task.Delay(delay);
            await m.DeleteAsync();
        }

        [Group("add")]
        public class AddAdminModule : ModuleBase<SocketCommandContext>
        {
            [Command("game")]
            public async Task AddGameAsync(string name, string port = "", string version = "")
            {
                Data.Game game = new Data.Game
                {
                    Name = name,
                    Port = port,
                    Version = version
                };

                if (DbHandler.Exists(game))
                {
                    await ReplyAsync(Message.Error.AlreadyExists(game));
                    return;
                }

                DbHandler.Result result = DbHandler.Insert.Game(game);
                await ReplyAsync(Message.Context(result));
            }
        }

        [Group("remove")]
        [Alias("delete")]
        public class RemoveAdminModule : ModuleBase<SocketCommandContext>
        {
            [Command("game", RunMode = RunMode.Async)]
            [Summary("[Admin] Remove a game from db")]
            public async Task DeleteGameAsync(string name)
            {
                Data.Game game = DbHandler.Select.Game(name);

                if (game is null)
                {
                    await ReplyAsync(Message.Error.GameNotFound(game));
                    return;
                }

                DbHandler.Result result = DbHandler.Delete.Game(game);
                await ReplyAsync(Message.Context(result));
            }
        }
    }
}
