using Discord;
using Discord.Commands;
using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inquisition.Modules
{
    [Group("reminder")]
    public class ReminderCommands : ModuleBase<SocketCommandContext>
    {
        Data.InquisitionContext db = new Data.InquisitionContext();

        [Command("add")]
        [Summary("Creates a reminder")]
        public async Task SetReminderAsync(DateTime dateTime, [Remainder] string remainder)
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

        [Command("delete")]
        [Alias("remove")]
        [Summary("Deletes a reminder")]
        public async Task DeleteReminderAsync(int id)
        {

            await ReplyAsync($"Reminder removed {Context.Message.Author.Mention}");
        }

        [Command("list")]
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
}
