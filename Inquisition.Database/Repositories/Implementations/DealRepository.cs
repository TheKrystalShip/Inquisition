using Inquisition.Database.Models;

namespace Inquisition.Database.Repositories
{
    public class DealRepository : Repository<Deal>, IDealRepository
    {
        public DealRepository(DatabaseContext context): base(context)
        {

        }
    }
}
