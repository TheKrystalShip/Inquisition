using Discord.Commands;
using System;
using System.Collections.Generic;
using System.Text;
using Inquisition.Data;
using System.Threading.Tasks;
using System.Linq;
using System.Diagnostics;
using Discord.WebSocket;
using Discord;

namespace Inquisition.Modules
{
    [Group("show")]
    [Alias("gimme", "show me", "show me a", "tell", "tell me", "tell me a")]
    public class ShowModule : ModuleBase<SocketCommandContext>
    {
        [Group("game")]
        public class ShowGameModule : ModuleBase<SocketCommandContext>
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

        [Group("joke")]
        public class ShowJokeModule : ModuleBase<SocketCommandContext>
        {
            [Command("")]
            [Alias("by")]
            public async Task ShowJokeAsync(SocketUser user = null)
            {
                List<Joke> Jokes;
                Random rn = new Random();

                if (user is null)
                {
                    Jokes = DbHandler.ListAll(new Joke());
                    user = Context.User;
                } else
                {
                    Jokes = DbHandler.ListAll(new Joke(), user);
                }

                try
                {
                    Joke joke = Jokes[rn.Next(Jokes.Count)];
                    EmbedBuilder embed = EmbedTemplate.Create(Context.Client.CurrentUser, user);
                    embed.WithDescription($"Random joke submitted by {joke.AuthorName}:");
                    embed.AddField($"{joke.Text}", $"P:{joke.PositiveVotes} - N:{joke.NegativeVotes}");

                    await ReplyAsync($"Here you go:", false, embed.Build());
                }
                catch (Exception ex)
                {
                    await ReplyAsync($"Something happened, oops. Let the admin know pls thx <3");
                    Console.WriteLine(ex.Message);
                    throw;
                }
            }
        }

        [Group("meme")]
        [Alias("meme pls", "memes", "memes pls", "spice", "spice pls", "some spice", "some spice pls", "spiciness", " some spiciness", "spiciness pls", "some spiciness pls")]
        public class ShowMemeModule : ModuleBase<SocketCommandContext>
        {
            [Command("")]
            [Alias("by")]
            public async Task ShowMemeAsync(SocketUser user = null)
            {
                List<Meme> Memes;
                Random rn = new Random();

                if (user is null)
                {
                    Memes = DbHandler.ListAll(new Meme());
                    user = Context.User;
                } else
                {
                    Memes = DbHandler.ListAll(new Meme(), user);
                }

                try
                {
                    Meme meme = Memes[rn.Next(Memes.Count)];
                    EmbedBuilder embed = EmbedTemplate.Create(Context.Client.CurrentUser, user);
                    embed.WithDescription($"Random meme submitted by {meme.AuthorName}:");
                    embed.WithImageUrl(meme.Url);

                    await ReplyAsync($"Here you go:", false, embed.Build());
                }
                catch (Exception ex)
                {
                    await ReplyAsync($"Something happened, oops. Let the admin know pls thx <3");
                    Console.WriteLine(ex.Message);
                }

            }
        }
    }
}
