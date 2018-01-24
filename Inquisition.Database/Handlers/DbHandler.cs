using Inquisition.Database.Models;
using Inquisition.Database.Properties;

using Microsoft.EntityFrameworkCore;

using System.Reflection;

namespace Inquisition.Database.Handlers
{
	public class DbHandler : DbContext
    {
		private string ConnectionString = Resources.ConnectionStringTesting;

		public DbSet<Guild> Guilds { get; set; }
		public DbSet<User> Users { get; set; }
		public DbSet<Joke> Jokes { get; set; }
		public DbSet<Reminder> Reminders { get; set; }
        public DbSet<Alert> Alerts { get; set; }

		protected override void OnConfiguring(DbContextOptionsBuilder ob)
        {
            ob.UseSqlServer(ConnectionString, 
				b => b.MigrationsAssembly(Assembly.GetExecutingAssembly().GetName().Name));

            ob.EnableSensitiveDataLogging(true);
            base.OnConfiguring(ob);
        }

        protected override void OnModelCreating(ModelBuilder mb)
        {
			mb.Entity<Guild>()
				.HasMany(x => x.Users)
				.WithOne(x => x.Guild);

			mb.Entity<User>()
				.HasOne(x => x.Guild)
				.WithMany(x => x.Users);

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
        }

		public void MigrateDatabase() => Database.Migrate();
	}
}
