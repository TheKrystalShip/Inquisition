using Inquisition.Database.Models;

namespace Inquisition.Database.Repositories
{
    public class AlertRepository : Repository<Alert>, IAlertRepository
    {
        public AlertRepository(DatabaseContext context) : base(context)
        {

        }
    }
}
