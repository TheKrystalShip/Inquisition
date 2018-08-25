using Microsoft.EntityFrameworkCore;

using TheKrystalShip.Inquisition.Database.Models;

namespace TheKrystalShip.Inquisition.Database
{
    public class DatabaseContext : DbContext
    {
		public DbSet<User> Users { get; set; }
		public DbSet<Joke> Jokes { get; set; }
		public DbSet<Reminder> Reminders { get; set; }
		public DbSet<Alert> Alerts { get; set; }
		public DbSet<Deal> Deals { get; set; }
		public DbSet<Game> Games { get; set; }
		public DbSet<Guild> Guilds { get; set; }
		public DbSet<Activity> Activities { get; set; }

        public DatabaseContext() : base()
        {

        }

        public DatabaseContext(DbContextOptions options) : base(options)
        {

        }

		public void Migrate() => Database.Migrate();

        protected override void OnConfiguring(DbContextOptionsBuilder builder)
        {
            builder.UseSqlite("Data source=.\\Properties\\Inquisition.db");
            base.OnConfiguring(builder);
        }

		protected override void OnModelCreating(ModelBuilder mb)
		{
			base.OnModelCreating(mb);

			mb.Entity<User>()
				.HasMany(x => x.Jokes)
				.WithOne(x => x.User);

			mb.Entity<User>()
				.HasMany(x => x.Reminders)
				.WithOne(x => x.User);

			mb.Entity<User>()
				.HasMany(x => x.Alerts)
				.WithOne(x => x.User);

			mb.Entity<User>()
				.HasMany(x => x.TargetAlerts)
				.WithOne(x => x.TargetUser);

			mb.Entity<User>()
				.HasMany(x => x.Deals)
				.WithOne(x => x.User);

			mb.Entity<User>()
				.HasMany(x => x.Servers)
				.WithOne(x => x.User);

			mb.Entity<User>()
				.HasMany(x => x.Activities)
				.WithOne(x => x.User);

			mb.Entity<Joke>()
				.HasOne(x => x.User)
				.WithMany(x => x.Jokes)
				.OnDelete(DeleteBehavior.Cascade);

			mb.Entity<Reminder>()
				.HasOne(x => x.User)
				.WithMany(x => x.Reminders)
				.OnDelete(DeleteBehavior.Cascade);

			mb.Entity<Alert>()
				.HasOne(x => x.User)
				.WithMany(x => x.Alerts)
				.OnDelete(DeleteBehavior.Cascade);

			mb.Entity<Alert>()
				.HasOne(x => x.TargetUser)
				.WithMany(x => x.TargetAlerts);

			mb.Entity<Deal>()
				.HasOne(x => x.User)
				.WithMany(x => x.Deals)
				.OnDelete(DeleteBehavior.Cascade);

			mb.Entity<Guild>()
				.HasMany(x => x.Servers)
				.WithOne(x => x.Guild);

			mb.Entity<Activity>()
				.HasOne(x => x.User)
				.WithMany(x => x.Activities)
				.OnDelete(DeleteBehavior.Cascade);

            mb.Entity<Server>()
                .HasOne(x => x.User)
                .WithMany(x => x.Servers);

            mb.Entity<Server>()
                .HasOne(x => x.Guild)
                .WithMany(x => x.Servers);
		}
	}
}
