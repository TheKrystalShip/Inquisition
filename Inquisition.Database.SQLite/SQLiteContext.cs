using Microsoft.EntityFrameworkCore;

using System.Threading.Tasks;

using TheKrystalShip.Inquisition.Domain;
using TheKrystalShip.Inquisition.Tools;

namespace TheKrystalShip.Inquisition.Database.SQLite
{
    public class SQLiteContext : DbContext, IDbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Joke> Jokes { get; set; }
        public DbSet<Reminder> Reminders { get; set; }
        public DbSet<Alert> Alerts { get; set; }
        public DbSet<Deal> Deals { get; set; }
        public DbSet<Game> Games { get; set; }
        public DbSet<Guild> Guilds { get; set; }
        public DbSet<Activity> Activities { get; set; }

        public SQLiteContext(DbContextOptions options) : base(options)
        {

        }

        public void Migrate() => Database.Migrate();

        protected override void OnConfiguring(DbContextOptionsBuilder builder)
        {
            base.OnConfiguring(builder);

            builder.UseSqlite(Configuration.GetConnectionString("SQLite"));
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfiguration(new ActivityConfiguration());
            modelBuilder.ApplyConfiguration(new AlertConfiguration());
            modelBuilder.ApplyConfiguration(new DealConfiguration());
            modelBuilder.ApplyConfiguration(new GameConfiguration());
            modelBuilder.ApplyConfiguration(new GuildConfiguration());
            modelBuilder.ApplyConfiguration(new JokeConfiguration());
            modelBuilder.ApplyConfiguration(new ReminderConfiguration());
            modelBuilder.ApplyConfiguration(new ServerConfiguration());
            modelBuilder.ApplyConfiguration(new UserConfiguration());
        }

        void IDbContext.SaveChanges()
        {
            base.SaveChanges();
        }

        public Task SaveChangesAsync()
        {
            return SaveChangesAsync();
        }
    }
}
