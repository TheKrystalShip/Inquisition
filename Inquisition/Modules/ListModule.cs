using Discord;
using Discord.Commands;
using System.Collections.Generic;
using System.Threading.Tasks;
using Inquisition.Data;
using System.Linq;
using Discord.WebSocket;
using System;

namespace Inquisition.Modules
{
    [Group("list")]
    [Alias("list all", "list me", "list me all")]
    public class ListModule : ModuleBase<SocketCommandContext>
    {
        [Command("games")]
        [Summary("Returns list of all games in the database")]
        public async Task ListAllGamesAsync()
        {
            List<Data.Game> Games = DbHandler.ListAll(new Data.Game());
            EmbedBuilder builder = EmbedTemplate.Create(Context.Client.CurrentUser, Context.User);

            foreach (Data.Game game in Games)
            {
                string st = game.IsOnline ? "Online " : "Offline ";
                builder.AddInlineField(game.Name, st + $"on: {game.Port}, v: {game.Version}");
            }

            await ReplyAsync(Message.Info.Generic, false, builder.Build());
        }

        [Command("jokes")]
        [Alias("jokes by")]
        public async Task ListJokesAsync(SocketUser user = null)
        {
            List<Joke> Jokes;

            switch (user)
            {
                case null:
                    Jokes = DbHandler.ListAll(new Joke(), DbHandler.GetFromDb(Context.User));
                    break;
                default:
                    Jokes = DbHandler.ListAll(new Joke(), DbHandler.GetFromDb(user));
                    break;
            }

            EmbedBuilder embed = EmbedTemplate.Create(Context.Client.CurrentUser, Context.User);

            foreach (Joke joke in Jokes)
            {
                embed.AddField($"{joke.Text}", $"Submitted {joke.CreatedAt}");
            }

            await ReplyAsync(Message.Info.Generic, false, embed.Build());
        }

        [Command("memes")]
        [Alias("memes by")]
        public async Task ListMemesAsync(SocketUser user = null)
        {
            List<Meme> Memes;

            switch (user)
            {
                case null:
                    Memes = DbHandler.ListAll(new Meme(), DbHandler.GetFromDb(Context.User));
                    break;
                default:
                    Memes = DbHandler.ListAll(new Meme(), DbHandler.GetFromDb(user));
                    break;
            }

            EmbedBuilder embed = EmbedTemplate.Create(Context.Client.CurrentUser, Context.User);

            int i = 1;

            foreach (Meme meme in Memes)
            {
                embed.AddField($"Shitpost nr {i}:", $"{meme.Url}");
                i++;
            }

            await ReplyAsync(Message.Info.Generic, false, embed.Build());
        }

        [Command("reminders")]
        [Summary("Returns a list of all set reminders")]
        public async Task ListRemindersAsync(SocketUser user = null)
        {
            List<Reminder> Reminders;
            switch (user)
            {
                case null:
                    Reminders = DbHandler.ListAll(new Reminder(), DbHandler.GetFromDb(Context.User));
                    break;
                default:
                    Reminders = DbHandler.ListAll(new Reminder(), DbHandler.GetFromDb(user));
                    break;
            }
            EmbedBuilder embed = EmbedTemplate.Create(Context.Client.CurrentUser, Context.User);

            foreach (Reminder reminder in Reminders)
            {
                embed.AddField($"{reminder.Message}", $"{reminder.DueDate}");
            }
            await ReplyAsync(Message.Info.Generic, false, embed.Build());
        }

        [Command("notifications")]
        [Summary("Returns a list of all notifications")]
        public async Task ListNotificationsAsync(SocketUser user = null)
        {
            List<Notification> Notifications;
            switch (user)
            {
                case null:
                    Notifications = DbHandler.ListAll(new Notification(), DbHandler.GetFromDb(Context.User));
                    break;
                default:
                    Notifications = DbHandler.ListAll(new Notification(), DbHandler.GetFromDb(user));
                    break;
            }
            EmbedBuilder embed = EmbedTemplate.Create(Context.Client.CurrentUser, Context.User);

            foreach (Notification n in Notifications)
            {
                embed.AddField($"By {n.User.Username}", $"For when {n.TargetUser.Username} comes online");
            }

            await ReplyAsync(Message.Info.Generic, false, embed.Build());
        }
    }
}
