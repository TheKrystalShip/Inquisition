using Discord.Commands;
using System;
using System.Threading.Tasks;
using Inquisition.Data;
using Discord.WebSocket;

namespace Inquisition.Modules
{
    [Group("add")]
    public class AddModule : ModuleBase<SocketCommandContext>
    {
        InquisitionContext db = new InquisitionContext();

        [Command("game")]
        [Alias("a game", "new game", "a new game")]
        [Summary("Add a game to the server list")]
        [RequireUserPermission(Discord.GuildPermission.Administrator)]
        public async Task AddGameAsync(string name, string port = "?", string version = "?")
        {
            if (name is null)
            {
                await ReplyAsync($"Incorrect struncture, use: \"[game]\" \"[port]\" \"[version]\"");
                return;
            } else
            {
                Game game = new Game { Name = name, Port = port, Version = version };
                await db.AddAsync(game);
                await db.SaveChangesAsync();
                await ReplyAsync(InfoMessage.SuccessfullyAdded(game));
            }
        }

        [Command("joke")]
        [Alias("a joke", "new joke", "a new joke")]
        [Summary("Adds a joke to the db")]
        public async Task AddJokeAsync([Remainder] string jokeText)
        {
            try
            {
                Joke joke = new Joke
                {
                    Author = Context.User.Username,
                    Text = jokeText
                };

                db.Jokes.Add(joke);
                await db.SaveChangesAsync();
                await ReplyAsync(InfoMessage.SuccessfullyAdded(joke));
            }
            catch (Exception ex)
            {
                await ReplyAsync(ErrorMessage.DatabaseAccess());
                Console.WriteLine(ex.Message);
                throw;
            }
        }

        [Command("meme")]
        [Alias("a meme", "new meme", "a new meme")]
        [Summary("Adds a meme to the db")]
        public async Task AddMemeAsync([Remainder] string url)
        {
            try
            {
                Meme meme = new Meme
                {
                    Author = Context.User.Username,
                    Url = url
                };

                db.Memes.Add(meme);
                await db.SaveChangesAsync();
                await ReplyAsync(InfoMessage.SuccessfullyAdded(meme));
            }
            catch (Exception ex)
            {
                await ReplyAsync(ErrorMessage.DatabaseAccess());
                Console.WriteLine(ex.Message);
                throw;
            }
        }

        [Command("reminder")]
        [Alias("a reminder", "new reminder", "a new reminder")]
        [Summary("Add a reminder")]
        public async Task AddReminderAsync(DateTime dateTime, [Remainder] string remainder)
        {
            SocketUser user = Context.Message.Author;
            Reminder reminder = new Reminder
            {
                Username = user.Username,
                Time = dateTime,
                Message = remainder
            };

            try
            {
                await db.Reminders.AddAsync(reminder);
                await db.SaveChangesAsync();

                await ReplyAsync(InfoMessage.SuccessfullyAdded(reminder));
            }
            catch (Exception ex)
            {
                await ReplyAsync(ErrorMessage.DatabaseAccess());
                Console.WriteLine(ex.Message);
                throw;
            }
        }
    }
}
