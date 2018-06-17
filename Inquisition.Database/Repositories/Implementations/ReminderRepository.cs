using Inquisition.Database.Models;

namespace Inquisition.Database.Repositories
{
    public class ReminderRepository : Repository<Reminder>, IReminderRepository
    {
        public ReminderRepository(DatabaseContext context) : base(context)
        {

        }
    }
}
