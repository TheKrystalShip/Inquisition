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
        [Alias("a game", "new game")]
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
                await db.AddAsync(new Data.Game { Name = name, Port = port, Version = version });
                await db.SaveChangesAsync();
                await ReplyAsync($"{name}, on port {port}, with version {version} successfully added to the server list");
            }
        }

        [Command("joke")]
        [Alias("a joke", "new joke")]
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
                await ReplyAsync($"{Context.User.Mention}, your joke was added successfully, thanks!");
            }
            catch (Exception ex)
            {
                await ReplyAsync($"There was an error while trying to add your joke to the database, " +
                    $"please let the knobhead who porgrammed this know about it, thanks");
                Console.WriteLine(ex.Message);
                throw;
            }
        }

        [Command("meme")]
        [Alias("a meme", "new meme")]
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
                await ReplyAsync($"{Context.User.Mention}, your meme was added successfully, thanks!");
            }
            catch (Exception ex)
            {
                await ReplyAsync($"There was an error while trying to add your meme to the database, " +
                    $"please let the knobhead who porgrammed this know about it, thanks");
                Console.WriteLine(ex.Message);
                throw;
            }
        }

        [Command("reminder")]
        [Alias("a reminder", "new reminder")]
        [Summary("Add a reminder")]
        public async Task AddReminderAsync(DateTime dateTime, [Remainder] string remainder)
        {
            SocketUser user = Context.Message.Author;
            Data.Reminder reminder = new Data.Reminder
            {
                Username = user.Username,
                Time = dateTime,
                Message = remainder
            };

            await db.Reminders.AddAsync(reminder);
            await db.SaveChangesAsync();

            await ReplyAsync($"Reminder set {Context.Message.Author.Mention}");
        }
    }
}
