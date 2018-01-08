using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Inquisition.Handlers;
using System.Threading.Tasks;
using System;
using Inquisition.Data;
using Inquisition.Services;

namespace Inquisition.Modules
{
    [RequireUserPermission(GuildPermission.Administrator)]
    public class AdminModule : ModuleBase<SocketCommandContext>
    {
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
                await ReplyAsync(Reply.Info.UsersPruned(members, days));
            }
            catch (Exception e)
            {
                await ExceptionService.SendErrorAsync(Context, e);
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
                await ReplyAsync(Reply.Info.UserBanned(user));
            }
            catch (Exception e)
            {
                await ExceptionService.SendErrorAsync(Context, e);
            }
        }

        [Command("wipe", RunMode = RunMode.Async)]
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
                await ExceptionService.SendErrorAsync(Context, e);
            }
        }

        [Command("error")]
        public async Task RaiseErrorAsync()
        {
            try
            {
                SocketUser user = new DiscordSocketClient().GetUser(1212412412412123124);
                await user.SendMessageAsync("Test");
            }
            catch (Exception e)
            {
                await ExceptionService.SendErrorAsync(Context, e);
            }
        }

        //[Command("prefix")]
        //[Summary("Change the bot prefix")]
        //public async Task ChangePrefixAsync(string prefix)
        //{
            
        //}
        
        [Group("add")]
        public class AddAdminModule : ModuleBase<SocketCommandContext>
        {
            [Command("game", RunMode = RunMode.Async)]
            public async Task AddGameAsync(string name, string port = "", string version = "")
            {
                try
                {
                    Data.Game game = new Data.Game
                    {
                        Name = name,
                        Port = port,
                        Version = version
                    };

                    Result result = DbHandler.Insert.Game(game);
                    await ReplyAsync(Reply.Context(result));
                }
                catch (Exception e)
                {
                    await ExceptionService.SendErrorAsync(Context, e);
                }
            }
        }

        [Group("remove")]
        [Alias("delete")]
        public class RemoveAdminModule : ModuleBase<SocketCommandContext>
        {
            [Command("game")]
            [Summary("[Admin] Remove a game from db")]
            public async Task DeleteGameAsync(string name)
            {
                try
                {
                    Data.Game game = DbHandler.Select.Game(name);

                    if (game is null)
                    {
                        await ReplyAsync(Reply.Error.NotFound.Game);
                        return;
                    }

                    Result result = DbHandler.Delete.Game(game);
                    await ReplyAsync(Reply.Context(result));
                }
                catch (Exception e)
                {
                    await ExceptionService.SendErrorAsync(Context, e);
                }
            }
        }
    }
}
