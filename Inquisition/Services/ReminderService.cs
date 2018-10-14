using Discord;

using Microsoft.EntityFrameworkCore;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

using TheKrystalShip.Inquisition.Domain;

namespace TheKrystalShip.Inquisition.Services
{
    public class ReminderService : Service
    {
        public override Timer Timer { get; protected set; }
        public override event Action<Service> Start;
        public override event Action<Service> Stop;
        public override event Action<Service> Tick;

        public ReminderService(Tools tools) : base(tools)
        {
            
        }

        public override void Init(int startDelay = 0, int interval = 1000)
        {
            Timer = new Timer(Loop, null, startDelay, interval);
            Start?.Invoke(this);
        }

        protected override void Loop(object state)
        {
            List<Reminder> reminderList = GetReminderList(10);

            foreach (Reminder reminder in reminderList)
            {
                Tools.Client.GetUser(reminder.User.Id).SendMessageAsync($"Reminder: {reminder.Message}");
            }

            RemoveReminderList(reminderList);
            Tick?.Invoke(this);
        }

        public override void Dispose()
        {
            Stop?.Invoke(this);
        }

        private List<Reminder> GetReminderList(int amount)
        {
            return Tools.Database.Reminders
                .Where(x => x.DueDate <= DateTimeOffset.UtcNow)
                .Include(x => x.User)
                .Take(amount)
                .ToList() ?? new List<Reminder>();
        }

        private void RemoveReminderList(List<Reminder> reminderList)
        {
            Tools.Database.Reminders.RemoveRange(reminderList);
            Tools.Database.SaveChanges();
        }
    }
}
