using Discord;
using Discord.Commands;

using Microsoft.EntityFrameworkCore;

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
        [Command("reminders")]
        [Summary("Displays a list with all of your reminders")]
        public async Task<RuntimeResult> ListRemindersAsync()
        {
            List<Reminder> reminderList = await Database.Reminders
                .Where(x => x.User.Id == User.Id)
                .ToListAsync();

            if (reminderList.Count is 0)
            {
                return new ErrorResult(CommandError.ObjectNotFound, "No reminders in the database");
            }

            EmbedBuilder embedBuilder = new EmbedBuilder().Create(Context.User);

            foreach (Reminder reminder in reminderList)
            {
                embedBuilder.AddField($"{reminder.Id} - {reminder.Message ?? "No message"}", $"{reminder.DueDate}");
            }

            return new InfoResult("Info", embedBuilder);
        }

        [Command("add reminder")]
        [Summary("Add a new reminder")]
        public async Task<RuntimeResult> AddReminderAsync(string dueDate, [Remainder] string remainder = "")
        {
            if (User.TimezoneOffset is null)
            {
                return new ErrorResult(CommandError.UnmetPrecondition, "Timezone not set");
            }

            DateTimeOffset dueDateUtc;

            try
            {
                dueDateUtc = new DateTimeOffset(DateTime.Parse(dueDate),
                    new TimeSpan((int)User.TimezoneOffset, 0, 0));
            }
            catch (Exception)
            {
                return new ErrorResult(CommandError.ParseFailed, "DateTime parse error");
            }

            Reminder reminder = new Reminder
            {
                DueDate = dueDateUtc,
                Message = remainder,
                User = User
            };

            Database.Reminders.Add(reminder);

            return new SuccessResult("Success");
        }

        [Command("delete reminder")]
        [Alias("remove reminder")]
        [Summary("Remove a reminder")]
        public async Task<RuntimeResult> RemoveReminderAsync(int id)
        {
            Reminder reminder = await Database.Reminders
                .FirstOrDefaultAsync(x => x.Id == id && x.User.Id == User.Id);

            if (reminder is null)
            {
                return new ErrorResult(CommandError.ObjectNotFound, "Reminder not found");
            }

            Database.Reminders.Remove(reminder);

            return new SuccessResult("Success");
        }
    }
}
