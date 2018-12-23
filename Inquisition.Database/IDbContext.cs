using Microsoft.EntityFrameworkCore;

using System.Threading.Tasks;

using TheKrystalShip.Inquisition.Domain;

namespace TheKrystalShip.Inquisition.Database
{
    public interface IDbContext
    {
        DbSet<Activity> Activities { get; set; }
        DbSet<Alert> Alerts { get; set; }
        DbSet<Deal> Deals { get; set; }
        DbSet<Game> Games { get; set; }
        DbSet<Guild> Guilds { get; set; }
        DbSet<Joke> Jokes { get; set; }
        DbSet<Reminder> Reminders { get; set; }
        DbSet<User> Users { get; set; }

        void SaveChanges();
        Task SaveChangesAsync();
    }
}