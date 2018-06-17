using Inquisition.Database.Models;

namespace Inquisition.Database.Repositories
{
    public class GameRepository : Repository<Game>, IGameRepository
    {
        public GameRepository(DatabaseContext context) : base(context)
        {

        }
    }
}
