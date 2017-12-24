using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Inquisition.Handlers;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Inquisition.Data;

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
                await Task.Delay(1000);
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
                    local = DbHandler.Select.User(Context.User);
                    break;
                default:
                    local = DbHandler.Select.User(user);
                    break;
            }

            if (local.TimezoneOffset is null)
            {
                await ReplyAsync(Reply.Error.TimezoneNotSet);
                return;
            }

            await ReplyAsync(Reply.Info.UserTimezone(local));
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
                    localUser = DbHandler.Select.User(Context.User);
                    Jokes = DbHandler.Select.Jokes();
                    break;
                default:
                    localUser = DbHandler.Select.User(user);
                    Jokes = DbHandler.Select.Jokes(localUser);
                    break;
            }

            if (Jokes.Count == 0)
            {
                await ReplyAsync(Reply.Error.NoContent(localUser));
                return;
            }

            Joke joke = Jokes[rn.Next(Jokes.Count)];
            EmbedBuilder embed = EmbedTemplate.Create(Context.Client.CurrentUser, Context.User);
            embed.WithTitle($"{joke.Id} - {joke.Text}");
            embed.WithFooter($"Submitted by: {joke.User.Username}#{joke.User.Discriminator}", joke.User.AvatarUrl);

            await ReplyAsync(Reply.Generic, false, embed.Build());
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
                    localUser = DbHandler.Select.User(Context.User);
                    Jokes = DbHandler.Select.Jokes(10);
                    break;
                default:
                    localUser = DbHandler.Select.User(user);
                    Jokes = DbHandler.Select.Jokes(10, localUser);
                    break;
            }

            if (Jokes.Count == 0)
            {
                await ReplyAsync(Reply.Error.NoContent(localUser));
                return;
            }

            EmbedBuilder embed = EmbedTemplate.Create(Context.Client.CurrentUser, Context.User);

            foreach (Joke joke in Jokes)
            {
                embed.AddField($"{joke.Id} - {joke.Text}", $"Submitted by {joke.User.Username} on {joke.CreatedAt}");
            }

            await ReplyAsync(Reply.Generic, false, embed.Build());
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
                    localUser = DbHandler.Select.User(Context.User);
                    Memes = DbHandler.Select.Memes();
                    break;
                default:
                    localUser = DbHandler.Select.User(user);
                    Memes = DbHandler.Select.Memes(localUser);
                    break;
            }

            if (Memes.Count == 0)
            {
                await ReplyAsync(Reply.Error.NoContent(localUser));
                return;
            }

            Meme meme = Memes[rn.Next(Memes.Count)];
            EmbedBuilder embed = EmbedTemplate.Create(Context.Client.CurrentUser, Context.User);
            embed.WithFooter($"Submitted by: {meme.User.Username}#{meme.User.Discriminator}", meme.User.AvatarUrl);
            embed.WithImageUrl(meme.Url);
            embed.WithTitle($"{meme.Id} - {meme.Url}");

            await ReplyAsync(Reply.Generic, false, embed.Build());
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

            await ReplyAsync(Reply.Generic, false, embed.Build());
        }

        [Command("memes", RunMode = RunMode.Async)]
        [Alias("memes by")]
        [Summary("Shows a list of all memes from all users unless user is specified")]
        public async Task ListMemesAsync(SocketUser user = null)
        {
            List<Meme> Memes;
            User localUser;

            switch (user)
            {
                case null:
                    localUser = DbHandler.Select.User(Context.User);
                    Memes = DbHandler.Select.Memes(10);
                    break;
                default:
                    localUser = DbHandler.Select.User(user);
                    Memes = DbHandler.Select.Memes(10, localUser);
                    break;
            }

            if (Memes.Count == 0)
            {
                await ReplyAsync(Reply.Error.NoContent(localUser));
                return;
            }

            EmbedBuilder embed = EmbedTemplate.Create(Context.Client.CurrentUser, Context.User);

            foreach (Meme meme in Memes)
            {
                embed.AddField($"{meme.Id} - {meme.Url}", $"Submitted by {meme.User.Username} on {meme.CreatedAt}");
            }

            await ReplyAsync(Reply.Generic, false, embed.Build());
        }

        [Command("reminders", RunMode = RunMode.Async)]
        [Summary("Displays a list with all of your reminders")]
        public async Task ListRemindersAsync()
        {
            User localUser = DbHandler.Select.User(Context.User);
            List<Reminder> Reminders = DbHandler.Select.Reminders(localUser);

            if (Reminders.Count == 0)
            {
                await ReplyAsync(Reply.Error.NoContentGeneric);
                return;
            }

            EmbedBuilder embed = EmbedTemplate.Create(Context.Client.CurrentUser, Context.User);

            foreach (Reminder reminder in Reminders)
            {
                embed.AddField($"{reminder.Id} - {reminder.Message ?? "No message"}", $"{reminder.DueDate}");
            }

            await ReplyAsync(Reply.Generic, false, embed.Build());
        }

        [Command("alerts", RunMode = RunMode.Async)]
        [Summary("Displays a list of all of your notifications")]
        public async Task ListAlertsAsync()
        {
            User localUser = DbHandler.Select.User(Context.User);
            List<Alert> Alerts = DbHandler.Select.Alerts(localUser);

            if (Alerts.Count == 0)
            {
                await ReplyAsync(Reply.Error.NoContentGeneric);
                return;
            }

            EmbedBuilder embed = EmbedTemplate.Create(Context.Client.CurrentUser, Context.User);

            foreach (Alert n in Alerts)
            {
                embed.AddField($"For when {n.TargetUser.Username} joins", $"Created: {n.CreatedAt}");
            }

            await ReplyAsync(Reply.Generic, false, embed.Build());
        }
    }

    [Group("add")]
    public class AddGeneralModule : ModuleBase<SocketCommandContext>
    {
        [Command("joke", RunMode = RunMode.Async)]
        [Summary("Adds a new joke")]
        public async Task AddJokeAsync([Remainder] string jokeText)
        {
            User localUser = DbHandler.Select.User(Context.User);

            if (jokeText is null)
            {
                await ReplyAsync(Reply.Error.Command.Joke);
                return;
            }

            Joke joke = new Joke
            {
                Text = jokeText,
                User = localUser
            };

            Result result = DbHandler.Insert.Joke(joke);
            await ReplyAsync(Reply.Context(result));
        }

        [Command("meme", RunMode = RunMode.Async)]
        [Summary("Adds a new meme")]
        public async Task AddMemeAsync([Remainder] string url)
        {
            User localUser = DbHandler.Select.User(Context.User);

            if (url is null)
            {
                await ReplyAsync(Reply.Error.Command.Meme);
                return;
            }

            Meme meme = new Meme
            {
                Url = url,
                User = localUser
            };

            Result result = DbHandler.Insert.Meme(meme);
            await ReplyAsync(Reply.Context(result));
        }

        [Command("reminder", RunMode = RunMode.Async)]
        [Summary("Add a new reminder")]
        public async Task AddReminderAsync(string dueDate, [Remainder] string remainder = "")
        {
            User localUser = DbHandler.Select.User(Context.User);

            if (localUser.TimezoneOffset is null)
            {
                await ReplyAsync(Reply.Error.TimezoneNotSet);
                return;
            }

            DateTimeOffset dueDateUtc;

            try
            {
                dueDateUtc = new DateTimeOffset(DateTime.Parse(dueDate), 
                    new TimeSpan((int)localUser.TimezoneOffset, 0, 0));
            }
            catch (Exception)
            {
                await ReplyAsync(Reply.Error.Command.Reminder);
                return;
            }

            Reminder reminder = new Reminder
            {
                CreateDate = DateTimeOffset.UtcNow,
                DueDate = dueDateUtc,
                Message = remainder,
                User = localUser
            };

            Result result = DbHandler.Insert.Reminder(reminder);
            await ReplyAsync(Reply.Context(result));
        }

        [Command("alert", RunMode = RunMode.Async)]
        [Summary("Add a new alert, must specify a target user")]
        public async Task AddAlertAsync(SocketUser user = null)
        {
            User localUserAuthor = DbHandler.Select.User(Context.User);

            if (localUserAuthor.TimezoneOffset is null)
            {
                await ReplyAsync(Reply.Error.TimezoneNotSet);
                return;
            }

            if (user is null)
            {
                await ReplyAsync(Reply.Error.Command.Alert);
                return;
            }

            User localUserTarget = DbHandler.Select.User(user);

            DateTimeOffset creationDate;

            try
            {
                creationDate = new DateTimeOffset(DateTime.Now, 
                    new TimeSpan((int)localUserAuthor.TimezoneOffset, 0, 0));
            }
            catch (Exception)
            {
                await ReplyAsync(Reply.Error.Command.Alert);
                return;
            }

            Alert n = new Alert
            {
                User = localUserAuthor,
                TargetUser = localUserTarget,
                CreatedAt = creationDate
            };

            Result result = DbHandler.Insert.Alert(n);
            await ReplyAsync(Reply.Context(result));
        }
    }

    [Group("remove")]
    public class RemoveGeneralModule : ModuleBase<SocketCommandContext>
    {
        [Command("joke")]
        [Summary("Delete a joke")]
        public async Task RemoveJokeAsync(int id)
        {
            User localUser = DbHandler.Select.User(Context.User);
            Joke joke = DbHandler.Select.Joke(id, localUser);

            if (joke is null)
            {
                await ReplyAsync(Reply.Error.NotTheOwner);
                return;
            }

            Result result = DbHandler.Delete.Joke(joke);
            await ReplyAsync(Reply.Context(result));
        }

        [Command("meme")]
        [Summary("Delete a meme")]
        public async Task RemoveMemeAsync(int id)
        {
            User localUser = DbHandler.Select.User(Context.User);
            Meme meme = DbHandler.Select.Meme(id, localUser);

            if (meme is null)
            {
                await ReplyAsync(Reply.Error.NotTheOwner);
                return;
            }

            Result result = DbHandler.Delete.Meme(meme);
            await ReplyAsync(Reply.Context(result));
        }

        [Command("reminder", RunMode = RunMode.Async)]
        [Summary("Remove a reminder")]
        public async Task RemoveReminderAsync(int id)
        {
            User localUser = DbHandler.Select.User(Context.User);
            Reminder reminder = DbHandler.Select.Reminder(id, localUser);

            Result result = DbHandler.Delete.Reminder(reminder);
            await ReplyAsync(Reply.Context(result));
        }

        [Command("alert", RunMode = RunMode.Async)]
        [Summary("Removes an alert, must specify a target user")]
        public async Task RemoveAlertAsync(SocketUser target = null, [Remainder] string etc = "")
        {
            if (target is null)
            {
                await base.ReplyAsync(Reply.Error.Command.Alert);
                return;
            }

            User authorUser = DbHandler.Select.User(Context.User);
            User targetUser = DbHandler.Select.User(target);

            Alert alert = DbHandler.Select.Alert(authorUser, targetUser);

            if (alert is null)
            {
                await ReplyAsync(Reply.Error.NotFound.Alert);
                return;
            }

            Result result = DbHandler.Delete.Alert(alert);
            await ReplyAsync(Reply.Context(result));
        }
    }

    [Group("set")]
    public class SetGeneralModule : ModuleBase<SocketCommandContext>
    {
        [Command("timezone", RunMode = RunMode.Async)]
        [Summary("Set your timezone")]
        public async Task SetTimezoneAsync(int offset)
        {
            User localUser = DbHandler.Select.User(Context.User);

            localUser.TimezoneOffset = offset;
            DbHandler.Update.User(localUser);

            await ReplyAsync(Reply.Info.UserTimezone(localUser));
        }
    }
}
