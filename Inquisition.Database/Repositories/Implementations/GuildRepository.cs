using Inquisition.Database.Models;

namespace Inquisition.Database.Repositories
{
    public class GuildRepository : Repository<Guild>, IGuildRepository
    {
        public GuildRepository(DatabaseContext context) : base(context)
        {

        }
    }
}
