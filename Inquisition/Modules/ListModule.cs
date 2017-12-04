using Discord;
using Discord.Commands;
using System.Collections.Generic;
using System.Threading.Tasks;
using Inquisition.Data;
using System.Linq;
using Discord.WebSocket;

namespace Inquisition.Modules
{
    [Group("list")]
    [Alias("tell me all", "tell all", "list all")]
    public class ListModule : ModuleBase<SocketCommandContext>
    {
        [Group("games")]
        public class ListGameModule : ModuleBase<SocketCommandContext>
        {
            InquisitionContext db = new InquisitionContext();

            [Command("")]
            [Alias("all")]
            public async Task ListAllGamesAsync()
            {
                List<Data.Game> Games = db.Games.ToList();
                EmbedBuilder builder = new EmbedBuilder
                {
                    Color = Color.Orange,
                    Title = "Domain: ffs.game-host.org",
                    Description = $"List of all game servers with ports"
                };

                foreach (Data.Game game in Games)
                {
                    string st = game.IsOnline ? "Online " : "Offline ";
                    builder.AddInlineField(game.Name, st + $"on: {game.Port}, v: {game.Version}");
                }

                await ReplyAsync($"Here's the server list {Context.User.Mention}:", false, builder.Build());
            }

            [Command("online")]
            public async Task ListOnlineGamesAsync()
            {
                List<Data.Game> Games = db.Games.ToList();
                EmbedBuilder builder = new EmbedBuilder
                {
                    Color = Color.Orange,
                    Title = "Domain: ffs.game-host.org",
                    Description = $"List of online game servers with ports"
                };

                foreach (Data.Game game in Games)
                {
                    if (game.IsOnline)
                        builder.AddInlineField(game.Name, $"Port: {game.Port}, v: {game.Version}");
                }

                await ReplyAsync($"Here's the server list {Context.User.Mention}:", false, builder.Build());
            }

            [Command("offline")]
            public async Task ListOfflineGamesAsync()
            {
                List<Data.Game> Games = db.Games.ToList();
                EmbedBuilder builder = new EmbedBuilder
                {
                    Color = Color.Orange,
                    Title = "Domain: ffs.game-host.org",
                    Description = $"List of offline game servers with ports"
                };

                foreach (Data.Game game in Games)
                {
                    if (!game.IsOnline)
                        builder.AddInlineField(game.Name, $"Port: {game.Port}, v: {game.Version}");
                }

                await ReplyAsync($"Here's the server list {Context.User.Mention}:", false, builder.Build());
            }
        }

        [Group("reminders")]
        public class ListRemindersModule : ModuleBase<SocketCommandContext>
        {
            InquisitionContext db = new InquisitionContext();

            [Summary("Returns a list of all set reminders")]
            public async Task ListRemindersAsync()
            {
                SocketUser user = Context.Message.Author;

                List<Data.Reminder> Reminders = db.Reminders.ToList();
                EmbedBuilder builder = new EmbedBuilder
                {
                    Color = Color.Blue,
                    Title = $"{user.Username}'s reminders:",
                };

                builder.WithAuthor(user.Username);

                foreach (Data.Reminder reminder in Reminders)
                {
                    builder.AddField($"{reminder.Time}", $"{reminder.Message}, {reminder.Time}", true);
                }
                await ReplyAsync($"Here's a list of all your reminders {Context.Message.Author.Mention}", false, builder.Build());
            }
        }

        [Group("jokes")]
        public class ListJokesModule : ModuleBase<SocketCommandContext>
        {
            InquisitionContext db = new InquisitionContext();

            [Command("")]
            [Alias("by")]
            public async Task ListJokesAsync(SocketUser user, [Remainder] string garbage = "")
            {
                if (user is null)
                {
                    user = Context.User;
                }

                List<Joke> Jokes = db.Jokes.Where(x => x.Author == user.Username).ToList();
                EmbedBuilder builder = new EmbedBuilder();
                builder.WithAuthor(user.Username);

                foreach (Joke joke in Jokes)
                {
                    builder.AddField($"{joke.Text}", $"Y: {joke.PositiveVotes} - N: {joke.NegativeVotes}");
                }

                await ReplyAsync($"Here's the jokes list {Context.User.Mention}:", false, builder.Build());
            }

        }

        [Group("memes")]
        [RequireUserPermission(GuildPermission.Administrator)]
        public class ListMemesModule : ModuleBase<SocketCommandContext>
        {
            InquisitionContext db = new InquisitionContext();

            [Command("")]
            [Alias("by")]
            public async Task ListMemesAsync(SocketUser user = null)
            {
                if (user is null)
                {
                    user = Context.User;
                }

                List<Meme> Memes = db.Memes.Where(x => x.Author == user.Username).ToList();
                EmbedBuilder builder = new EmbedBuilder();
                builder.WithAuthor(user.Username);

                foreach (Meme meme in Memes)
                {
                    builder.WithImageUrl(meme.Url);
                }

                await ReplyAsync($"Here's the jokes list {Context.User.Mention}:", false, builder.Build());
            }
        }
    }
}
