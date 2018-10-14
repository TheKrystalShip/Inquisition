using Discord;
using Discord.Commands;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using TheKrystalShip.Inquisition.Data.Models;
using TheKrystalShip.Inquisition.Domain;
using TheKrystalShip.Inquisition.Extensions;
using TheKrystalShip.Inquisition.Handlers;

namespace TheKrystalShip.Inquisition.Modules
{
    public class ReminderModule : Module
    {
        public ReminderModule(Tools tools) : base(tools)
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
                await ReplyAsync(ReplyHandler.Error.NoContentGeneric);
                return;
            }

            EmbedBuilder embed = new EmbedBuilder().Create(Context.User);

            foreach (Reminder reminder in Reminders)
            {
                embed.AddField($"{reminder.Id} - {reminder.Message ?? "No message"}", $"{reminder.DueDate}");
            }

            await ReplyAsync(ReplyHandler.Generic, false, embed.Build());
        }

        [Command("add reminder")]
        [Summary("Add a new reminder")]
        public async Task AddReminderAsync(string dueDate, [Remainder] string remainder = "")
        {
            if (User.TimezoneOffset is null)
            {
                await ReplyAsync(ReplyHandler.Error.TimezoneNotSet);
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
                await ReplyAsync(ReplyHandler.Error.Command.Reminder);
                return;
            }

            Reminder reminder = new Reminder
            {
                DueDate = dueDateUtc,
                Message = remainder,
                User = User
            };

            Database.Reminders.Add(reminder);

            await ReplyAsync(ReplyHandler.Context(Result.Successful));
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
                await ReplyAsync(ReplyHandler.Error.NotFound.Reminder);
                return;
            }

            Database.Reminders.Remove(reminder);

            await ReplyAsync(ReplyHandler.Context(Result.Successful));
        }
    }
}
