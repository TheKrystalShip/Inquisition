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
        public async Task CreatePollAsync([Remainder] string r = "")
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

        [Command("timezone", RunMode = RunMode.Async)]
        [Summary("Tells you your timezone from the database")]
        public async Task ShowTimezoneAsync(SocketUser user = null)
        {
            User local;
            switch (user)
            {
                case null:
                    local = DbHandler.GetFromDb(Context.User);
                    break;
                default:
                    local = DbHandler.GetFromDb(user);
                    break;
            }

            if (local.TimezoneOffset is null)
            {
                await ReplyAsync(Message.Error.TimezoneNotSet);
                return;
            }

            await ReplyAsync(Message.Info.Timezone(local));
        }

        [Command("joke", RunMode = RunMode.Async)]
        [Alias("joke by")]
        [Summary("Displays a random joke by random user unless user is specified")]
        public async Task ShowJokeAsync(SocketUser user = null)
        {
            List<Joke> Jokes;
            Random rn = new Random();
            User localUser;

            switch (user)
            {
                case null:
                    localUser = DbHandler.GetFromDb(Context.User);
                    Jokes = DbHandler.ListAll(new Joke());
                    break;
                default:
                    localUser = DbHandler.GetFromDb(user);
                    Jokes = DbHandler.ListAll(new Joke(), localUser);
                    break;
            }

            if (Jokes.Count > 0)
            {
                Joke joke = Jokes[rn.Next(Jokes.Count)];
                EmbedBuilder embed = EmbedTemplate.Create(Context.Client.CurrentUser, Context.User);
                embed.WithTitle($"{joke.Id} - {joke.Text}");
                embed.WithFooter($"Submitted by: {joke.User.Username}#{joke.User.Discriminator}", joke.User.AvatarUrl);

                await ReplyAsync($"Here you go:", false, embed.Build());
            }
            else
            {
                await ReplyAsync(Message.Error.NoContent(localUser));
            }
        }

        [Command("jokes", RunMode = RunMode.Async)]
        [Alias("jokes by")]
        [Summary("Shows a list of all jokes from all users unless user is specified")]
        public async Task ListJokesAsync(SocketUser user = null)
        {
            List<Joke> Jokes;
            User localUser;

            switch (user)
            {
                case null:
                    Jokes = DbHandler.ListAll(new Joke());
                    localUser = DbHandler.GetFromDb(Context.User);
                    break;
                default:
                    localUser = DbHandler.GetFromDb(user);
                    Jokes = DbHandler.ListAll(new Joke(), localUser);
                    break;
            }

            if (Jokes.Count > 0)
            {
                EmbedBuilder embed = EmbedTemplate.Create(Context.Client.CurrentUser, Context.User);

                foreach (Joke joke in Jokes)
                {
                    embed.AddField($"{joke.Id} - {joke.Text}", $"Submitted by {joke.User.Username} on {joke.CreatedAt}");
                }

                await ReplyAsync(Message.Info.Generic, false, embed.Build());
            }
            else
            {
                await ReplyAsync(Message.Error.NoContent(localUser));
            }
        }

        [Command("meme", RunMode = RunMode.Async)]
        [Alias("meme by")]
        [Summary("Displays a random meme by random user unless user is specified")]
        public async Task ShowMemeAsync(SocketUser user = null)
        {
            List<Meme> Memes;
            Random rn = new Random();
            User localUser;

            switch (user)
            {
                case null:
                    localUser = DbHandler.GetFromDb(Context.User);
                    Memes = DbHandler.ListAll(new Meme());
                    break;
                default:
                    localUser = DbHandler.GetFromDb(user);
                    Memes = DbHandler.ListAll(new Meme(), localUser);
                    break;
            }

            if (Memes.Count > 0)
            {
                Meme meme = Memes[rn.Next(Memes.Count)];
                EmbedBuilder embed = EmbedTemplate.Create(Context.Client.CurrentUser, Context.User);
                embed.WithFooter($"Submitted by: {meme.User.Username}#{meme.User.Discriminator}", meme.User.AvatarUrl);
                embed.WithImageUrl(meme.Url);
                embed.WithTitle($"{meme.Id} - {meme.Url}");

                await ReplyAsync(Message.Info.Generic, false, embed.Build());
            }
            else
            {
                await ReplyAsync(Message.Error.NoContent(localUser));
            }
        }

        [Command("meme random", RunMode = RunMode.Async)]
        [Alias("random meme")]
        [Summary("Shows a random meme")]
        public async Task ShowRandomMemeAsync()
        {
            Random rn = new Random();
            int limit = 33000;

            string Path(int n) => $"http://images.memes.com/meme/{n}.jpg";
            
            string meme = Path(rn.Next(limit));

            EmbedBuilder embed = EmbedTemplate.Create(Context.Client.CurrentUser, Context.User);
            embed.WithImageUrl(meme);
            embed.WithTitle(meme);

            await ReplyAsync(Message.Info.Generic, false, embed.Build());
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
                    embed.AddField($"{meme.Id} - {meme.Url}", $"Submitted by {meme.User.Username} on {meme.CreatedAt}");
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
            User localUser = DbHandler.GetFromDb(Context.User);
            List<Reminder> Reminders = DbHandler.ListAll(new Reminder(), localUser);

            if (Reminders.Count > 0)
            {
                EmbedBuilder embed = EmbedTemplate.Create(Context.Client.CurrentUser, Context.User);

                foreach (Reminder reminder in Reminders)
                {
                    embed.AddField($"{reminder.Id} - {reminder.Message ?? "No message"}", $"{reminder.DueDate}");
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
            User localUser = DbHandler.GetFromDb(Context.User);
            List<Notification> Notifications = DbHandler.ListAll(new Notification(), localUser);

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
    public class AddGeneralModule : ModuleBase<SocketCommandContext>
    {
        [Command("joke", RunMode = RunMode.Async)]
        [Summary("Adds a new joke")]
        public async Task AddJokeAsync([Remainder] string jokeText)
        {
            User localUser = DbHandler.GetFromDb(Context.User);

            if (jokeText is null)
            {
                await ReplyAsync(Message.Error.IncorrectStructure(new Joke()));
                return;
            }

            Joke joke = new Joke
            {
                Text = jokeText,
                User = localUser
            };

            switch (DbHandler.AddToDb(joke))
            {
                case DbHandler.Result.Successful:
                    await ReplyAsync(Message.Info.SuccessfullyAdded(joke));
                    break;
                default:
                    await ReplyAsync(Message.Error.Generic);
                    break;
            }
        }

        [Command("meme", RunMode = RunMode.Async)]
        [Summary("Adds a new meme")]
        public async Task AddMemeAsync([Remainder] string url)
        {
            User localUser = DbHandler.GetFromDb(Context.User);

            if (url is null)
            {
                await ReplyAsync(Message.Error.IncorrectStructure(new Meme()));
                return;
            }

            Meme meme = new Meme
            {
                Url = url,
                User = localUser
            };

            switch (DbHandler.AddToDb(meme))
            {
                case DbHandler.Result.Successful:
                    await ReplyAsync(Message.Info.SuccessfullyAdded(meme));
                    break;
                default:
                    await ReplyAsync(Message.Error.Generic);
                    break;
            }
        }

        [Command("reminder", RunMode = RunMode.Async)]
        [Summary("Add a new reminder")]
        public async Task AddReminderAsync(string dueDate, [Remainder] string remainder = "")
        {
            User localUser = DbHandler.GetFromDb(Context.User);

            if (localUser.TimezoneOffset is null)
            {
                await ReplyAsync(Message.Error.TimezoneNotSet);
                return;
            }

            DateTimeOffset dueDateUtc = new DateTimeOffset(DateTime.Parse(dueDate), 
                                                           new TimeSpan((int)localUser.TimezoneOffset, 0, 0));

            Reminder reminder = new Reminder
            {
                CreateDate = DateTimeOffset.UtcNow,
                DueDate = dueDateOffset,
                Message = remainder,
                User = localUser
            };

            switch (DbHandler.AddToDb(reminder))
            {
                case DbHandler.Result.Successful:
                    await ReplyAsync(Message.Info.SuccessfullyAdded(reminder));
                    break;
                default:
                    await ReplyAsync(Message.Error.Generic);
                    break;
            }
        }

        [Command("notification", RunMode = RunMode.Async)]
        [Summary("Add a new notifications, must specify a target user")]
        public async Task AddNotificationAsync(SocketUser user = null, [Remainder] string etc = "")
        {
            User localUserAuthor = DbHandler.GetFromDb(Context.User);

            if (user is null)
            {
                await ReplyAsync(Message.Error.IncorrectStructure(new Notification()));
                return;
            }

            User localUserTarget = DbHandler.GetFromDb(user);

            bool permanent = etc.Equals("permanent");

            Notification n = new Notification
            {
                User = localUserAuthor,
                TargetUser = localUserTarget,
                IsPermanent = permanent
            };

            switch (DbHandler.AddToDb(n))
            {
                case DbHandler.Result.Successful:
                    await ReplyAsync(Message.Info.SuccessfullyAdded(n));
                    break;
                default:
                    await ReplyAsync(Message.Error.Generic);
                    break;
            }
        }
    }

    [Group("remove")]
    public class RemoveGeneralModule : ModuleBase<SocketCommandContext>
    {
        [Command("joke")]
        [Summary("Delete a joke")]
        public async Task RemoveJokeAsync(int id)
        {
            User localUser = DbHandler.GetFromDb(Context.User);
            Joke joke = DbHandler.GetFromDb(new Joke { Id = id }, localUser);

            if (joke is null)
            {
                await ReplyAsync(Message.Error.NotTheOwner);
                return;
            }

            switch (DbHandler.RemoveFromDb(joke))
            {
                case DbHandler.Result.Successful:
                    await ReplyAsync(Message.Info.SuccessfullyRemoved(new Meme()));
                    break;
                default:
                    await ReplyAsync(Message.Error.Generic);
                    break;
            }
        }

        [Command("meme")]
        [Summary("Delete a meme")]
        public async Task RemoveMemeAsync(int id)
        {
            User localUser = DbHandler.GetFromDb(Context.User);
            Meme meme = DbHandler.GetFromDb(new Meme() { Id = id }, localUser);

            if (meme is null)
            {
                await ReplyAsync(Message.Error.NotTheOwner);
                return;
            }

            switch (DbHandler.RemoveFromDb(meme))
            {
                case DbHandler.Result.Successful:
                    await ReplyAsync(Message.Info.SuccessfullyRemoved(new Meme()));
                    break;
                default:
                    await ReplyAsync(Message.Error.Generic);
                    break;
            }
        }

        [Command("reminder", RunMode = RunMode.Async)]
        [Summary("Remove a reminder")]
        public async Task RemoveReminderAsync(int id)
        {
            User localUser = DbHandler.GetFromDb(Context.User);
            Reminder localReminder = DbHandler.GetFromDb(new Reminder() { Id = id }, localUser);

            switch (DbHandler.RemoveFromDb(localReminder))
            {
                case DbHandler.Result.Successful:
                    await ReplyAsync(Message.Info.SuccessfullyRemoved(new Reminder()));
                    break;
                default:
                    await ReplyAsync(Message.Error.Generic);
                    break;
            }
        }

        [Command("notification", RunMode = RunMode.Async)]
        [Summary("Removes a notification, must specify a target user")]
        public async Task RemoveNotificationAsync(SocketUser user = null, [Remainder] string etc = "")
        {
            User localUserAuthor = DbHandler.GetFromDb(Context.User);

            if (user is null)
            {
                await ReplyAsync(Message.Error.IncorrectStructure(new Notification()));
                return;
            }

            User localUserTarget = DbHandler.GetFromDb(user);

            Notification n = new Notification
            {
                User = localUserAuthor,
                TargetUser = localUserTarget
            };

            switch (DbHandler.RemoveFromDb(n))
            {
                case DbHandler.Result.Successful:
                    await ReplyAsync(Message.Info.SuccessfullyRemoved(n));
                    break;
                default:
                    await ReplyAsync(Message.Error.Generic);
                    break;
            }
        }
    }

    [Group("set")]
    public class SetGeneralModule : ModuleBase<SocketCommandContext>
    {
        [Command("timezone", RunMode = RunMode.Async)]
        [Summary("Set your timezone")]
        public async Task SetTimezoneAsync(int offset)
        {
            User localUser = DbHandler.GetFromDb(Context.User);

            localUser.TimezoneOffset = offset;
            DbHandler.UpdateInDb(localUser);

            await ReplyAsync(Message.Info.Timezone(localUser));
        }
    }
}
