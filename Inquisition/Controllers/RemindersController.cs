using System;
using System.Collections.Generic;
using System.Text;
using Inquisition.Data;
using System.Threading.Tasks;
using System.Threading;
using System.Linq;

namespace Inquisition.Controllers
{
    public class RemindersController
    {
        List<Reminder> Reminders;
        InquisitionContext db = new InquisitionContext();
        CancellationToken CancellationToken = new CancellationToken();

        public async Task ReminderLoopAsync()
        {
            Reminders = db.Reminders.ToList();

            foreach (Reminder r in Reminders)
            {
                if (r.DueDate >= DateTime.Now)
                {
                }
            }

            await Task.Delay(1000, CancellationToken);
        }
    }
}
