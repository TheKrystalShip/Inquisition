using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Inquisition.Data;
using System.Threading.Tasks;

namespace Inquisition.Modules
{
    [RequireUserPermission(GuildPermission.Administrator)]
    public class AdminModule : ModuleBase<SocketCommandContext>
    {
        [Command("prune")]
        [Summary("Prunes all inactive members from the server")]
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

        [Command("ban")]
        [Summary("Bans a user from the server")]
        public async Task KickMemberAsync(SocketUser user)
        {
            await Context.Guild.AddBanAsync(user);
            await ReplyAsync(Message.Info.UserBanned(user.Username));
        }

        [Command("unban")]
        [Summary("Unbans a user")]
        public async Task UnbanMemberAsync(SocketUser user)
        {
            await Context.Guild.RemoveBanAsync(user);
            await ReplyAsync(Message.Info.UserUnbanned(user.Username));
        }

        [Command("wipe")]
        [Alias("wipe last", "wipe the last")]
        [Summary("Wipes a text channel")]
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
        [Alias("add a", "add a new", "make", "make a", "make a new", "create", "create a", "create a new")]
        public class AddModule : ModuleBase<SocketCommandContext>
        {
            [Command("game")]
            [Alias("game:")]
            [Summary("Add a game to the server list")]
            public async Task AddGameAsync(string name, string port = "?", string version = "?")
            {
                if (name is null)
                {
                    await ReplyAsync(Message.Error.IncorrectStructure(new Data.Game()));
                    return;
                }
                else
                {
                    Data.Game game = new Data.Game { Name = name, Port = port, Version = version };
                    if (!DbHandler.Exists(game))
                    {
                        if (DbHandler.AddToDb(game))
                        {
                            await ReplyAsync(Message.Info.SuccessfullyAdded(game));
                        }
                        else
                        {
                            await ReplyAsync(Message.Error.DatabaseAccess);
                        }
                    }
                    else
                    {
                        await ReplyAsync(Message.Error.AlreadyExists(game));
                    }
                }
            }

            [Command("user")]
            [Alias("user:")]
            public async Task AddUserAsync(SocketGuildUser user)
            {
                if (!DbHandler.Exists(user))
                {
                    if (DbHandler.AddToDb(user))
                    {
                        await ReplyAsync(Message.Info.SuccessfullyAdded(user));
                    }
                    else
                    {
                        await ReplyAsync(Message.Error.DatabaseAccess);
                    }
                }
                else
                {
                    await ReplyAsync(Message.Error.AlreadyExists(user));
                }
            }
        }

        [Group("remove")]
        [Alias("remove a", "delete", "delete a")]
        public class RemoveModule : ModuleBase<SocketCommandContext>
        {
            [Command("game")]
            [Summary("Remove a game from db")]
            public async Task DeleteGameAsync(string name)
            {
                Data.Game game = DbHandler.GetFromDb(new Data.Game { Name = name });

                if (game is null)
                {
                    await ReplyAsync(Message.Error.GameNotFound(game));
                }
                else
                {
                    DbHandler.RemoveFromDb(game);
                    await ReplyAsync(Message.Info.SuccessfullyRemoved(game));
                }
            }
        }
    }
}
