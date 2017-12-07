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

            await ReplyAsync($"Here's the server list:", false, builder.Build());
        }

        [Command("jokes")]
        [Alias("jokes by")]
        public async Task ListJokesAsync(SocketUser user = null)
        {
            List<Joke> Jokes = DbHandler.ListAll(new Joke(), user);
            EmbedBuilder builder = EmbedTemplate.Create(Context.Client.CurrentUser, Context.User);

            foreach (Joke joke in Jokes)
            {
                builder.AddField($"{joke.Text}", $"Y: {joke.PositiveVotes} - N: {joke.NegativeVotes}");
            }

            await ReplyAsync($"Here's the jokes list {Context.User.Mention}:", false, builder.Build());
        }

        [Command("memes")]
        [Alias("memes by")]
        public async Task ListMemesAsync(SocketUser user = null)
        {
            List<Meme> Memes = DbHandler.ListAll(new Meme(), user);
            EmbedBuilder builder = EmbedTemplate.Create(Context.Client.CurrentUser, Context.User);

            int i = 1;

            foreach (Meme meme in Memes)
            {
                builder.AddField($"Shitpost nr {i}:", $"{meme.Url}");
                i++;
            }

            await ReplyAsync($"Here's the meme list {Context.User.Mention}:", false, builder.Build());
        }

        [Command("reminders")]
        [Summary("Returns a list of all set reminders")]
        public async Task ListRemindersAsync(SocketUser user = null)
        {
            List<Reminder> Reminders;
            switch (user)
            {
                case null:
                    Reminders = DbHandler.ListAll(new Reminder(), Context.User);
                    break;
                default:
                    Reminders = DbHandler.ListAll(new Reminder(), user);
                    break;
            }
            EmbedBuilder builder = EmbedTemplate.Create(Context.Client.CurrentUser, Context.User);

            foreach (Reminder reminder in Reminders)
            {
                builder.AddField($"{reminder.Message}", $"{reminder.DueDate}");
            }
            await ReplyAsync($"Here's a list of all your reminders", false, builder.Build());
        }

        [Command("notifications")]
        [Summary("Returns a list of all notifications")]
        public async Task ListNotificationsAsync(SocketUser user = null)
        {
            List<Notification> list;
            switch (user)
            {
                case null:
                    list = DbHandler.ListAll(new Notification(), Context.User);
                    break;
                default:
                    list = DbHandler.ListAll(new Notification(), user);
                    break;
            }
            EmbedBuilder embed = EmbedTemplate.Create(Context.Client.CurrentUser, Context.User);

            foreach (Notification n in list)
            {
                embed.AddField($"By {n.User.Username}", $"For when {n.TargetUser.Username} comes online");
            }

            await ReplyAsync($"Here's the list of all your notifications", false, embed.Build());
        }
    }
}
