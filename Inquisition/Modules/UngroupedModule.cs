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
            List<Emoji> reactions = new List<Emoji> { new Emoji("👍🏻"), new Emoji("👎🏻"), new Emoji("🤷🏻") };

            var messages = await Context.Channel.GetMessagesAsync(1).Flatten();
            await Context.Channel.DeleteMessagesAsync(messages);

            EmbedBuilder embed = EmbedTemplate.Create(Context.Client.CurrentUser, Context.User);
            embed.WithColor(Color.Gold);
            embed.WithTitle(r);
            embed.WithFooter($"Asked by: {Context.User}", Context.User.GetAvatarUrl());

            var id = await ReplyAsync("", false, embed.Build());
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

        [Command("joke")]
        [Alias("joke by")]
        public async Task ShowJokeAsync(SocketUser user = null)
        {
            List<Joke> Jokes;
            Random rn = new Random();

            switch (user)
            {
                case null:
                    Jokes = DbHandler.ListAll(new Joke(), DbHandler.GetFromDb(Context.User));
                    user = Context.User;
                    break;
                default:
                    Jokes = DbHandler.ListAll(new Joke(), DbHandler.GetFromDb(user));
                    break;
            }

            try
            {
                if (Jokes.Count > 0)
                {
                    Joke joke = Jokes[rn.Next(Jokes.Count - 1)];
                    EmbedBuilder embed = EmbedTemplate.Create(Context.Client.CurrentUser, Context.User);
                    embed.WithTitle(joke.Text);
                    embed.WithFooter($"Submitted by: {joke.User.Username}#{joke.User.Discriminator}", joke.User.AvatarUrl);
                    embed.WithColor(Color.Green);

                    await ReplyAsync($"Here you go:", false, embed.Build()); 
                } else
                {
                    await ReplyAsync(Message.Error.NoContent(user));
                }
            }
            catch (Exception ex)
            {
                await ReplyAsync($"Something happened, oops. Let the admin know pls thx <3");
                Console.WriteLine(ex.Message);
            }
        }

        [Command("meme")]
        [Alias("meme by")]
        public async Task ShowMemeAsync(SocketUser user = null)
        {
            List<Meme> Memes;
            Random rn = new Random();

            if (user is null)
            {
                Memes = DbHandler.ListAll(new Meme());
                user = Context.User;
            }
            else
            {
                Memes = DbHandler.ListAll(new Meme(), DbHandler.GetFromDb(user));
            }

            try
            {
                if (Memes.Count > 0)
                {
                    Meme meme = Memes[rn.Next(Memes.Count)];
                    EmbedBuilder embed = EmbedTemplate.Create(Context.Client.CurrentUser, Context.User);
                    embed.WithFooter($"Submitted by: {meme.User.Username}#{meme.User.Discriminator}", meme.User.AvatarUrl);
                    embed.WithImageUrl(meme.Url);

                    await ReplyAsync($"Here you go:", false, embed.Build()); 
                } else
                {
                    await ReplyAsync(Message.Error.NoContent(user));
                }
            }
            catch (Exception ex)
            {
                await ReplyAsync($"Something happened, oops. Let the admin know pls thx <3");
                Console.WriteLine(ex.Message);
            }

        }
    }

    [Group("game")]
    public class GameModule : ModuleBase<SocketCommandContext>
    {
        [Command("status")]
        [Alias("info")]
        [Summary("Returns if a game server is online")]
        public async Task StatusAsync(string name)
        {
            Data.Game game = DbHandler.GetFromDb(new Data.Game { Name = name });
            if (game is null)
            {
                await ReplyAsync(Message.Error.GameNotFound(game));
                return;
            }

            bool ProcessRunning = ProcessDictionary.Instance.TryGetValue(game.Name, out Process p);
            bool GameMarkedOnline = game.IsOnline;

            if (!ProcessRunning && !GameMarkedOnline)
            {
                await ReplyAsync($"{game.Name} server is offline. If you wish to start it up use: game start \"{game.Name}\"");
                return;
            }
            else if (ProcessRunning && !GameMarkedOnline)
            {
                await ReplyAsync($"{game.Name} has a process running, but is marked as offline in the database, " +
                    $"please let the knobhead who programmed this know abut this error, thanks");
                return;
            }
            else if (!ProcessRunning && GameMarkedOnline)
            {
                await ReplyAsync($"{game.Name} server is not running, but is marked as online in the database, " +
                    $"please let the knobhead who programmed this know abut this error, thanks");
                return;
            }

            await ReplyAsync($"{game.Name} server is online, version {game.Version} on port {game.Port}");
        }

        [Command("version")]
        [Summary("Returns a game's version")]
        public async Task GameVersionAsync(string name)
        {
            Data.Game game = DbHandler.GetFromDb(new Data.Game { Name = name });
            if (game is null)
            {
                await ReplyAsync(Message.Error.GameNotFound(game));
                return;
            }

            await ReplyAsync($"{game.Name}'s version is {game.Version}");
        }

        [Command("port")]
        [Summary("Returns a game's port")]
        public async Task GamePortAsync(string name)
        {
            Data.Game game = DbHandler.GetFromDb(new Data.Game { Name = name });
            if (game is null)
            {
                await ReplyAsync(Message.Error.GameNotFound(game));
                return;
            }

            await ReplyAsync($"{game.Name}'s port is {game.Port}");
        }
    }
}
