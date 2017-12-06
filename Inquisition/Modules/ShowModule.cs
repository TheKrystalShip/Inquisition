using Discord.Commands;
using System;
using System.Collections.Generic;
using System.Text;
using Inquisition.Data;
using System.Threading.Tasks;
using System.Linq;
using System.Diagnostics;
using Discord.WebSocket;

namespace Inquisition.Modules
{
    [Group("show")]
    [Alias("gimme", "show me", "show me a", "tell", "tell me", "tell me a")]
    public class ShowModule : ModuleBase<SocketCommandContext>
    {
        [Group("game")]
        public class ShowGameModule : ModuleBase<SocketCommandContext>
        {
            InquisitionContext db = new InquisitionContext();

            [Command("status")]
            [Alias("info")]
            [Summary("Returns if a game server is online")]
            public async Task StatusAsync(string name)
            {
                Data.Game game = db.Games.Where(x => x.Name == name).FirstOrDefault();
                if (game is null)
                {
                    await ReplyAsync(Message.Error.GameNotFound(game.Name));
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
                Data.Game game = db.Games.Where(x => x.Name == name).FirstOrDefault();
                if (game is null)
                {
                    await ReplyAsync(Message.Error.GameNotFound(game.Name));
                    return;
                }

                await ReplyAsync($"{game.Name}'s version is {game.Version}");
            }

            [Command("port")]
            [Summary("Returns a game's port")]
            public async Task GamePortAsync(string name)
            {
                Data.Game game = db.Games.Where(x => x.Name == name).FirstOrDefault();
                if (game is null)
                {
                    await ReplyAsync(Message.Error.GameNotFound(game.Name));
                    return;
                }

                await ReplyAsync($"{game.Name}'s port is {game.Port}");
            }
        }

        [Group("joke")]
        public class ShowJokeModule : ModuleBase<SocketCommandContext>
        {
            InquisitionContext db = new InquisitionContext();

            [Command("")]
            [Alias("by")]
            public async Task ShowJokeAsync(SocketUser user = null)
            {
                List<Joke> Jokes;
                Random rn = new Random();

                if (user is null)
                {
                    Jokes = db.Jokes.ToList();
                } else
                {
                    Jokes = db.Jokes.Where(x => x.AuthorName == user.Username).ToList();
                }

                try
                {
                    Joke joke = Jokes[rn.Next(Jokes.Count)];

                    await ReplyAsync($"Random joke, submitted by {joke.AuthorName}:\n ```{joke.Text}``` \n" +
                        $"With {joke.PositiveVotes} positive votes and {joke.NegativeVotes} negative votes");
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
            InquisitionContext db = new InquisitionContext();

            [Command("")]
            [Alias("by")]
            public async Task ShowMemeAsync(SocketUser user = null)
            {
                List<Meme> Memes;
                Random rn = new Random();

                if (user is null)
                {
                    Memes = db.Memes.ToList();
                } else
                {
                    Memes = db.Memes.Where(x => x.AuthorName == user.Username).ToList();
                }

                try
                {
                    Meme meme = Memes[rn.Next(Memes.Count)];
                    await ReplyAsync($"Random meme, submitted by {meme.AuthorName}:\n" +
                        $"{meme.Url}");
                }
                catch (Exception ex)
                {
                    await ReplyAsync($"Something happened, oops. Let the admin know pls thx <3");
                    Console.WriteLine(ex.Message);
                    throw;
                }

            }
        }

        public static bool FindUserInDb(SocketUser username)
        {
            return false;
        }
    }
}
