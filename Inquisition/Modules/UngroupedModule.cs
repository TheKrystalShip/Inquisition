using Discord.Commands;
using Discord.WebSocket;
using Inquisition.Data;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Diagnostics;
using System;
using Discord;

namespace Inquisition.Modules
{
    [RequireUserPermission(GuildPermission.Administrator)]
    public class UngroupedAdminModule : ModuleBase<SocketCommandContext>
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
        [RequireBotPermission(GuildPermission.BanMembers)]
        public async Task KickMemberAsync(SocketUser user)
        {
            await Context.Guild.AddBanAsync(user);
            await ReplyAsync(Message.Info.UserBanned(user.Username));
        }

        [Command("unban")]
        [Summary("Unbans a user")]
        [RequireBotPermission(GuildPermission.BanMembers)]
        public async Task UnbanMemberAsync(SocketUser user)
        {
            await Context.Guild.RemoveBanAsync(user);
            await ReplyAsync(Message.Info.UserUnbanned(user.Username));
        }

        [Command("wipe")]
        [Alias("wipe last", "wipe the last")]
        [Summary("Wipes a text channel")]
        [RequireBotPermission(ChannelPermission.ManageMessages)]
        public async Task WipeChannelAsync(uint amount = 1, [Remainder] string s = "")
        {
            var messages = await Context.Channel.GetMessagesAsync((int)amount + 1).Flatten();
            await Context.Channel.DeleteMessagesAsync(messages);
            const int delay = 5000;
            var m = await ReplyAsync($"Deleted {amount} messages. _This message will be deleted in {delay / 1000} seconds._");
            await Task.Delay(delay);
            await m.DeleteAsync();
        }
    }

    public class UngroupedModule : ModuleBase<SocketCommandContext>
    {
        [Command("poll")]
        [Alias("poll:")]
        public async Task AddReactionAsync([Remainder] string r = "")
        {
            SocketUserMessage msg = Context.Message;
            List<Emoji> reactions = new List<Emoji>
            { new Emoji("👍🏻"),
              new Emoji("👎🏻"),
              new Emoji("🤷🏻") };

            foreach (var item in reactions)
            {
                await msg.AddReactionAsync(item);
            }
        }

        [Command("start")]
        [Summary("Starts up a game server")]
        public async Task StartGameAsync(string name)
        {
            Data.Game game = DbHandler.GetFromDb(new Data.Game { Name = name });
            string Path = ProcessDictionary.Path;

            if (game is null)
            {
                await ReplyAsync(Message.Error.GameNotFound(game));
                return;
            }

            try
            {
                if (ProcessDictionary.Instance.TryGetValue(game.Name, out Process temp))
                {
                    await ReplyAsync(Message.Error.GameAlreadyRunning(game));
                    return;
                }

                Process p = new Process();
                p.StartInfo.FileName = Path + game.Exe;
                p.StartInfo.Arguments = game.LaunchArgs;
                p.Start();

                ProcessDictionary.Instance.Add(game.Name, p);

                game.IsOnline = true;
                DbHandler.UpdateInDb(game);

                await ReplyAsync(Message.Info.GameStartingUp(game));
            }
            catch (Exception ex)
            {
                await ReplyAsync(Message.Error.UnableToStartGameServer(game));
                Console.WriteLine(ex.Message);
            }
        }

        [Command("stop")]
        [Summary("Stops a game server")]
        public async Task StopGameAsync(string name)
        {
            Data.Game game = DbHandler.GetFromDb(new Data.Game { Name = name });
            if (game is null)
            {
                await ReplyAsync(Message.Error.GameNotFound(game));
                return;
            }

            try
            {
                if (ProcessDictionary.Instance.TryGetValue(game.Name, out Process p))
                {
                    p.CloseMainWindow();
                    p.Close();
                    ProcessDictionary.Instance.Remove(game.Name);

                    game.IsOnline = false;
                    DbHandler.UpdateInDb(game);

                    await ReplyAsync(Message.Info.GameShuttingDown(game.Name));
                    return;
                }

                await ReplyAsync(Message.Error.GameNotRunning(game));
            }
            catch (Exception ex)
            {
                await ReplyAsync(Message.Error.UnableToStopGameServer(game));
                Console.WriteLine(ex.Message);
            }
        }
    }
}
