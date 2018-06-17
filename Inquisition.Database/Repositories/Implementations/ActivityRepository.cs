using Inquisition.Database.Models;

namespace Inquisition.Database.Repositories
{
    public class ActivityRepository : Repository<Activity>, IActivityRepository
    {
        public ActivityRepository(DatabaseContext context) : base(context)
        {

        }
    }
}
