using Discord.Commands;
using System;
using System.Threading.Tasks;
using Inquisition.Data;
using Discord.WebSocket;

namespace Inquisition.Modules
{
    [Group("add")]
    [Alias("add a", "add a new", "make", "make a", "make a new", "create", "create a", "create a new")]
    public class AddModule : ModuleBase<SocketCommandContext>
    {
        [Command("game")]
        [Alias("game:")]
        [Summary("Add a game to the server list")]
        [RequireUserPermission(Discord.GuildPermission.Administrator)]
        public async Task AddGameAsync(string name, string port = "?", string version = "?")
        {
            if (name is null)
            {
                await ReplyAsync(Message.Error.IncorrectStructure(new Game()));
                return;
            } else
            {
                Game game = new Game { Name = name, Port = port, Version = version };
                if (!DbHandler.Exists(game))
                {
                    if (DbHandler.AddToDb(game))
                    {
                        await ReplyAsync(Message.Info.SuccessfullyAdded(game));
                    } else
                    {
                        await ReplyAsync(Message.Error.DatabaseAccess);
                    }
                } else
                {
                    await ReplyAsync(Message.Error.AlreadyExists(game));
                }
            }
        }

        [Command("joke")]
        [Alias("joke:")]
        [Summary("Adds a joke to the db")]
        public async Task AddJokeAsync([Remainder] string jokeText)
        {
            if (jokeText is null)
            {
                await ReplyAsync(Message.Error.IncorrectStructure(new Joke()));
                return;
            } else
            {
                Joke joke = new Joke
                {
                    Text = jokeText,
                    User = DbHandler.GetFromDb(Context.User)
                };

                if (DbHandler.AddToDb(joke))
                {
                    await ReplyAsync(Message.Info.SuccessfullyAdded(joke));
                } else
                {
                    await ReplyAsync(Message.Error.DatabaseAccess);
                }
            }
        }

        [Command("meme")]
        [Alias("meme:")]
        [Summary("Adds a meme to the db")]
        public async Task AddMemeAsync([Remainder] string url)
        {
            if (url is null)
            {
                await ReplyAsync(Message.Error.IncorrectStructure(new Meme()));
                return;
            } else
            {
                Meme meme = new Meme
                {
                    Url = url,
                    User = DbHandler.GetFromDb(Context.User)
                };

                if (DbHandler.AddToDb(meme))
                {
                    await ReplyAsync(Message.Info.SuccessfullyAdded(meme));
                }
                else
                {
                    await ReplyAsync(Message.Error.DatabaseAccess);
                }
            }
        }

        [Command("reminder")]
        [Alias("reminder:", "reminder at", "reminder at:")]
        [Summary("Add a reminder")]
        public async Task AddReminderAsync(DateTimeOffset dueDate, [Remainder] string remainder = null)
        {
            Reminder reminder = new Reminder
            {
                CreateDate = DateTimeOffset.Now,
                DueDate = dueDate,
                Duration = dueDate - DateTimeOffset.Now,
                Message = remainder,
                User = DbHandler.GetFromDb(Context.User)
            };

            if (DbHandler.AddToDb(reminder))
            {
                await ReplyAsync(Message.Info.SuccessfullyAdded(reminder));
            } else
            {
                await ReplyAsync(Message.Error.DatabaseAccess);
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
                } else
                {
                    await ReplyAsync(Message.Error.DatabaseAccess);
                }
            } else
            {
                await ReplyAsync(Message.Error.AlreadyExists(user));
            }
        }

        [Command("notification")]
        [Alias("notification when", "notification:", "notification when:")]
        public async Task AddNotificationAsync(SocketGuildUser user = null, [Remainder] string etc = "")
        {
            if (user is null)
            {
                await ReplyAsync(Message.Error.Generic);
                return;
            } else
            {
                User author = DbHandler.GetFromDb(Context.User);
                User target = DbHandler.GetFromDb(user);

                Notification n = new Notification
                {
                    User = author,
                    TargetUser = target
                };
                if (DbHandler.AddToDb(n))
                {
                    await ReplyAsync(Message.Info.SuccessfullyAdded(n));
                } else
                {
                    await ReplyAsync(Message.Error.DatabaseAccess);
                }
            }
        }
    }
}
