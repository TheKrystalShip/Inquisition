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

            if (Games.Count > 0)
            {
                EmbedBuilder builder = EmbedTemplate.Create(Context.Client.CurrentUser, Context.User);

                foreach (Data.Game game in Games)
                {
                    string st = game.IsOnline ? "Online " : "Offline ";
                    builder.AddInlineField(game.Name, st + $"on: {game.Port}, v: {game.Version}");
                }

                await ReplyAsync(Message.Info.Generic, false, builder.Build()); 
            } else
            {
                await ReplyAsync(Message.Error.NoContent(Context.User));
            }
        }

        [Command("jokes")]
        [Alias("jokes by")]
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
                    embed.AddField($"{joke.Text}", $"Submitted {joke.CreatedAt}");
                }

                await ReplyAsync(Message.Info.Generic, false, embed.Build()); 
            } else
            {
                await ReplyAsync(Message.Error.NoContent(local));
            }
        }

        [Command("memes")]
        [Alias("memes by")]
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

                int i = 1;

                foreach (Meme meme in Memes)
                {
                    embed.AddField($"Shitpost nr {i}:", $"{meme.Url}");
                    i++;
                }

                await ReplyAsync(Message.Info.Generic, false, embed.Build()); 
            } else
            {
                await ReplyAsync(Message.Error.NoContent(local));
            }
        }

        [Command("reminders")]
        [Summary("Returns a list of all set reminders")]
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
            } else
            {
                await ReplyAsync(Message.Error.NoContentGeneric);
            }
        }

        [Command("notifications")]
        [Summary("Returns a list of all notifications")]
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
            } else
            {
                await ReplyAsync(Message.Error.NoContentGeneric);
            }
        }
    }
}
