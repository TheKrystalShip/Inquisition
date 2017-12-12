using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Inquisition.Data;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Inquisition.Modules
{
    public class GaneralModule : ModuleBase<SocketCommandContext>
    {
        [Command("poll", RunMode = RunMode.Async)]
        [Alias("poll:")]
        [Summary("Create a poll")]
        public async Task AddReactionAsync([Remainder] string r = "")
        {
            List<Emoji> reactions = new List<Emoji> { new Emoji("👍🏻"), new Emoji("👎🏻"), new Emoji("🤷🏻") };

            var messages = await Context.Channel.GetMessagesAsync(1).Flatten();
            await Context.Channel.DeleteMessagesAsync(messages);

            EmbedBuilder embed = EmbedTemplate.Create(Context.Client.CurrentUser, Context.User);
            embed.WithTitle(r);
            embed.WithFooter($"Asked by: {Context.User}", Context.User.GetAvatarUrl());

            var msg = await ReplyAsync("", false, embed.Build());

            foreach (Emoji e in reactions)
            {
                await msg.AddReactionAsync(e);
            }
        }

        [Command("joke", RunMode = RunMode.Async)]
        [Alias("joke by")]
        [Summary("Displays a random joke by random user unless user is specified")]
        public async Task ShowJokeAsync(SocketUser user = null)
        {
            List<Joke> Jokes;
            Random rn = new Random();
            User local;

            switch (user)
            {
                case null:
                    Jokes = DbHandler.ListAll(new Joke());
                    local = DbHandler.GetFromDb(Context.User);
                    break;
                default:
                    local = DbHandler.GetFromDb(user);
                    Jokes = DbHandler.ListAll(new Joke(), local);
                    break;
            }

            if (Jokes.Count > 0)
            {
                Joke joke = Jokes[rn.Next(Jokes.Count)];
                EmbedBuilder embed = EmbedTemplate.Create(Context.Client.CurrentUser, Context.User);
                embed.WithTitle(joke.Text);
                embed.WithFooter($"Submitted by: {joke.User.Username}#{joke.User.Discriminator}", joke.User.AvatarUrl);

                await ReplyAsync($"Here you go:", false, embed.Build());
            }
            else
            {
                await ReplyAsync(Message.Error.NoContent(local));
            }
        }

        [Command("jokes", RunMode = RunMode.Async)]
        [Alias("jokes by")]
        [Summary("Shows a list of all jokes from all users unless user is specified")]
        public async Task ListJokesAsync(SocketUser user = null)
        {
            List<Joke> Jokes;
            User local;

            switch (user)
            {
                case null:
                    Jokes = DbHandler.ListAll(new Joke());
                    local = DbHandler.GetFromDb(Context.User);
                    break;
                default:
                    local = DbHandler.GetFromDb(user);
                    Jokes = DbHandler.ListAll(new Joke(), local);
                    break;
            }

            if (Jokes.Count > 0)
            {
                EmbedBuilder embed = EmbedTemplate.Create(Context.Client.CurrentUser, Context.User);

                foreach (Joke joke in Jokes)
                {
                    embed.AddField($"{joke.Text}", $"Submitted by {joke.User.Username} on {joke.CreatedAt}");
                }

                await ReplyAsync(Message.Info.Generic, false, embed.Build());
            }
            else
            {
                await ReplyAsync(Message.Error.NoContent(local));
            }
        }

        [Command("meme", RunMode = RunMode.Async)]
        [Alias("meme by")]
        [Summary("Displays a random meme by random user unless user is specified")]
        public async Task ShowMemeAsync(SocketUser user = null)
        {
            List<Meme> Memes;
            Random rn = new Random();
            User local;

            switch (user)
            {
                case null:
                    Memes = DbHandler.ListAll(new Meme());
                    local = DbHandler.GetFromDb(Context.User);
                    break;
                default:
                    local = DbHandler.GetFromDb(user);
                    Memes = DbHandler.ListAll(new Meme(), local);
                    break;
            }

            if (Memes.Count > 0)
            {
                Meme meme = Memes[rn.Next(Memes.Count)];
                EmbedBuilder embed = EmbedTemplate.Create(Context.Client.CurrentUser, Context.User);
                embed.WithFooter($"Submitted by: {meme.User.Username}#{meme.User.Discriminator}", meme.User.AvatarUrl);
                embed.WithImageUrl(meme.Url);
                embed.WithTitle(meme.Url);

                await ReplyAsync($"Here you go:", false, embed.Build());
            }
            else
            {
                await ReplyAsync(Message.Error.NoContent(local));
            }
        }

        [Command("memes", RunMode = RunMode.Async)]
        [Alias("memes by")]
        [Summary("Shows a list of all memes from all users unless user is specified")]
        public async Task ListMemesAsync(SocketUser user = null)
        {
            List<Meme> Memes;
            User local;

            switch (user)
            {
                case null:
                    Memes = DbHandler.ListAll(new Meme());
                    local = DbHandler.GetFromDb(Context.User);
                    break;
                default:
                    local = DbHandler.GetFromDb(user);
                    Memes = DbHandler.ListAll(new Meme(), local);
                    break;
            }

            if (Memes.Count > 0)
            {
                EmbedBuilder embed = EmbedTemplate.Create(Context.Client.CurrentUser, Context.User);

                foreach (Meme meme in Memes)
                {
                    embed.AddField($"{meme.Url}", $"Shitposted by {meme.User.Username} on {meme.CreatedAt}");
                }

                await ReplyAsync(Message.Info.Generic, false, embed.Build());
            }
            else
            {
                await ReplyAsync(Message.Error.NoContent(local));
            }
        }

        [Command("reminders", RunMode = RunMode.Async)]
        [Summary("Displays a list with all of your reminders")]
        public async Task ListRemindersAsync()
        {
            List<Reminder> Reminders = DbHandler.ListAll(new Reminder(), DbHandler.GetFromDb(Context.User));

            if (Reminders.Count > 0)
            {
                EmbedBuilder embed = EmbedTemplate.Create(Context.Client.CurrentUser, Context.User);

                foreach (Reminder reminder in Reminders)
                {
                    embed.AddField($"{reminder.Message}", $"{reminder.DueDate}");
                }
                await ReplyAsync(Message.Info.Generic, false, embed.Build());
            }
            else
            {
                await ReplyAsync(Message.Error.NoContentGeneric);
            }
        }

        [Command("notifications", RunMode = RunMode.Async)]
        [Summary("Displays a list of all of your notifications")]
        public async Task ListNotificationsAsync()
        {
            List<Notification> Notifications = DbHandler.ListAll(new Notification(), DbHandler.GetFromDb(Context.User));

            if (Notifications.Count > 0)
            {
                EmbedBuilder embed = EmbedTemplate.Create(Context.Client.CurrentUser, Context.User);

                foreach (Notification n in Notifications)
                {
                    string p = n.IsPermanent ? "**Permanent** - " : "";
                    embed.AddField($"By {n.User.Username}", $"{p}For when {n.TargetUser.Username} joins");
                }

                await ReplyAsync(Message.Info.Generic, false, embed.Build());
            }
            else
            {
                await ReplyAsync(Message.Error.NoContentGeneric);
            }
        }
    }

    [Group("add")]
    public class AddModule : ModuleBase<SocketCommandContext>
    {
        [Command("joke", RunMode = RunMode.Async)]
        [Summary("Adds a new joke")]
        public async Task AddJokeAsync([Remainder] string jokeText)
        {
            if (jokeText is null)
            {
                await ReplyAsync(Message.Error.IncorrectStructure(new Joke()));
                return;
            }
            else
            {
                Joke joke = new Joke
                {
                    Text = jokeText,
                    User = DbHandler.GetFromDb(Context.User)
                };

                if (DbHandler.AddToDb(joke))
                {
                    await ReplyAsync(Message.Info.SuccessfullyAdded(joke));
                }
                else
                {
                    await ReplyAsync(Message.Error.DatabaseAccess);
                }
            }
        }

        [Command("meme", RunMode = RunMode.Async)]
        [Summary("Adds a new meme")]
        public async Task AddMemeAsync([Remainder] string url)
        {
            if (url is null)
            {
                await ReplyAsync(Message.Error.IncorrectStructure(new Meme()));
                return;
            }
            else
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

        [Command("reminder", RunMode = RunMode.Async)]
        [Summary("Add a new reminder")]
        public async Task AddReminderAsync(string dueDate, [Remainder] string remainder = null)
        {
            Reminder reminder = new Reminder
            {
                CreateDate = DateTimeOffset.Now,
                DueDate = DateTimeOffset.Parse(dueDate),
                Message = remainder,
                User = DbHandler.GetFromDb(Context.User)
            };

            if (DbHandler.AddToDb(reminder))
            {
                await ReplyAsync(Message.Info.SuccessfullyAdded(reminder));
            }
            else
            {
                await ReplyAsync(Message.Error.DatabaseAccess);
            }
        }

        [Command("notification", RunMode = RunMode.Async)]
        [Summary("Add a new notifications, must specify a target user")]
        public async Task AddNotificationAsync(SocketUser user = null, [Remainder] string etc = "")
        {
            if (user is null)
            {
                await ReplyAsync(Message.Error.IncorrectStructure(new Notification()));
                return;
            }

            User author = DbHandler.GetFromDb(Context.User);
            User target = DbHandler.GetFromDb(user);

            bool permanent = etc.Equals("permanent");

            Notification n = new Notification
            {
                User = author,
                TargetUser = target,
                IsPermanent = permanent
            };
            if (DbHandler.AddToDb(n))
            {
                await ReplyAsync(Message.Info.SuccessfullyAdded(n));
            }
            else
            {
                await ReplyAsync(Message.Error.DatabaseAccess);
            }
        }
    }

    [Group("remove")]
    public class RemoveModule : ModuleBase<SocketCommandContext>
    {
        [Command("reminder", RunMode = RunMode.Async)]
        [Summary("Remove a reminder")]
        public async Task RemoveReminderAsync(string dueDate, [Remainder] string remainder = null)
        {
            Reminder reminder = new Reminder
            {
                CreateDate = DateTimeOffset.Now,
                DueDate = DateTimeOffset.Parse(dueDate),
                Message = remainder,
                User = DbHandler.GetFromDb(Context.User)
            };

            if (DbHandler.RemoveFromDb(reminder))
            {
                await ReplyAsync(Message.Info.SuccessfullyRemoved(reminder));
            }
            else
            {
                await ReplyAsync(Message.Error.DatabaseAccess);
            }
        }

        [Command("notification", RunMode = RunMode.Async)]
        [Summary("Removes a notification, must specify a target user")]
        public async Task RemoveNotificationAsync(SocketUser user = null, [Remainder] string etc = "")
        {
            if (user is null)
            {
                await ReplyAsync(Message.Error.IncorrectStructure(new Notification()));
                return;
            }

            User author = DbHandler.GetFromDb(Context.User);
            User target = DbHandler.GetFromDb(user);

            bool permanent = etc.Equals("permanent");

            Notification n = new Notification
            {
                User = author,
                TargetUser = target,
                IsPermanent = permanent
            };
            if (DbHandler.RemoveFromDb(n))
            {
                await ReplyAsync(Message.Info.SuccessfullyRemoved(n));
            }
            else
            {
                await ReplyAsync(Message.Error.DatabaseAccess);
            }
        }
    }
}
