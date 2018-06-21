using Inquisition.Database.Models;

namespace Inquisition.Database.Repositories
{
    public class UserRepository : Repository<User>, IUserRepository
    {
        public UserRepository(DatabaseContext dbContext) : base(dbContext)
        {

        }
    }
}
