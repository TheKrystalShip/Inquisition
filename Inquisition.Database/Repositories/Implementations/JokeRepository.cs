using Inquisition.Database.Models;

namespace Inquisition.Database.Repositories
{
    public class JokeRepository : Repository<Joke>, IJokeRepository
    {
        public JokeRepository(DatabaseContext context) : base(context)
        {

        }
    }
}
