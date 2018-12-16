using Discord;
using Discord.Commands;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using TheKrystalShip.Inquisition.Domain;
using TheKrystalShip.Inquisition.Extensions;

namespace TheKrystalShip.Inquisition.Core.Modules
{
    public class ReminderModule : Module
    {
        public ReminderModule()
        {

        }

        [Command("reminders")]
        [Summary("Displays a list with all of your reminders")]
        public async Task ListRemindersAsync()
        {
            List<Reminder> Reminders = Database.Reminders
                .Where(x => x.User.Id == User.Id)
                .ToList();

            if (Reminders.Count is 0)
            {
                await ReplyAsync("No content");
                return;
            }

            EmbedBuilder embed = new EmbedBuilder().Create(Context.User);

            foreach (Reminder reminder in Reminders)
            {
                embed.AddField($"{reminder.Id} - {reminder.Message ?? "No message"}", $"{reminder.DueDate}");
            }

            await ReplyAsync(embed);
        }

        [Command("add reminder")]
        [Summary("Add a new reminder")]
        public async Task AddReminderAsync(string dueDate, [Remainder] string remainder = "")
        {
            if (User.TimezoneOffset is null)
            {
                await ReplyAsync("Timezone not set");
                return;
            }

            DateTimeOffset dueDateUtc;

            try
            {
                dueDateUtc = new DateTimeOffset(DateTime.Parse(dueDate),
                    new TimeSpan((int)User.TimezoneOffset, 0, 0));
            }
            catch (Exception)
            {
                await ReplyAsync("Datetime parse error");
                return;
            }

            Reminder reminder = new Reminder
            {
                DueDate = dueDateUtc,
                Message = remainder,
                User = User
            };

            Database.Reminders.Add(reminder);

            await ReplyAsync("Success");
        }

        [Command("delete reminder")]
        [Alias("remove reminder")]
        [Summary("Remove a reminder")]
        public async Task RemoveReminderAsync(int id)
        {
            Reminder reminder = Database.Reminders
                .FirstOrDefault(x => x.Id == id && x.User.Id == User.Id);

            if (reminder is null)
            {
                await ReplyAsync("Reminder not found");
                return;
            }

            Database.Reminders.Remove(reminder);

            await ReplyAsync("Success");
        }
    }
}
