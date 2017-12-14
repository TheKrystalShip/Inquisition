using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Inquisition.Data
{
    /*
     * All info is stored locally for now, maybe will end up using azure database
     * at some point if needed, for now it works like this.
     * 
     * To be able to store info in the database you need to have installed the two
     * EntityFramework packages (See Program.cs for info)
     * 
     * Once the packages installed, open up the Package Manager Console and type:
     *      update-database
     *      
     * This will create a local database for the program to use.
     */
    public class InquisitionContext : DbContext
    {
        public DbSet<Game> Games { get; set; }
        public DbSet<Joke> Jokes { get; set; }
        public DbSet<Meme> Memes { get; set; }
        public DbSet<Reminder> Reminders { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Notification> Notifications { get; set; }
        public DbSet<Playlist> Playlists { get; set; }
        public DbSet<Song> Songs { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Server=(localdb)\mssqllocaldb;Database=InquisitionDB;Trusted_Connection=True;");
            optionsBuilder.EnableSensitiveDataLogging(true);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Joke>()
                .HasOne(x => x.User)
                .WithMany(x => x.Jokes)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Meme>()
                .HasOne(x => x.User)
                .WithMany(x => x.Memes)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Reminder>()
                .HasOne(x => x.User)
                .WithMany(x => x.Reminders)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Notification>()
                .HasOne(x => x.User)
                .WithMany(x => x.Notifications)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Notification>()
                .HasOne(x => x.TargetUser)
                .WithMany(x => x.TargetNotifications);

            modelBuilder.Entity<Playlist>()
                .HasOne(x => x.Author)
                .WithMany(x => x.Playlists)
                .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<Playlist>()
                .HasMany(x => x.Songs);

            modelBuilder.Entity<Song>()
                .HasOne(x => x.Author)
                .WithMany(x => x.Songs)
                .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<Song>()
                .HasMany(x => x.Playlists);

            modelBuilder.Entity<User>()
                .HasMany(x => x.Playlists)
                .WithOne(x => x.Author);

            modelBuilder.Entity<User>()
                .HasMany(x => x.Songs)
                .WithOne(x => x.Author);
        }
    }

    public class Game
    {
        [Key]
        [StringLength(100)]
        public string Name { get; set; }

        [StringLength(10)]
        public string Port { get; set; }

        [StringLength(10)]
        public string Version { get; set; }

        public bool IsOnline { get; set; }

        public string Exe { get; set; }

        public string LaunchArgs { get; set; }
    }

    public class Joke
    {
        [Key]
        public int Id { get; set; }
        
        public virtual User User { get; set; }

        public string Text { get; set; }

        public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;
    }

    public class Meme
    {
        [Key]
        public int Id { get; set; }

        public virtual User User { get; set; }

        public string Url { get; set; }

        public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;
    }

    public class Reminder
    {
        [Key]
        public int Id { get; set; }

        public virtual User User { get; set; }

        public string Message { get; set; }

        public DateTimeOffset CreateDate { get; set; } = DateTimeOffset.UtcNow;

        public TimeSpan Duration { get; set; }

        public DateTimeOffset DueDate { get; set; }
    }

    public class User
    {
        [Key]
        [StringLength(100)]
        public string Id { get; set; }

        public string Username { get; set; }

        public string Nickname { get; set; }

        public string Discriminator { get; set; }

        public DateTimeOffset? JoinedAt { get; set; } = DateTimeOffset.UtcNow;

        public DateTimeOffset LastSeenOnline { get; set; } = DateTimeOffset.UtcNow;

        public int? TimezoneOffset { get; set; } = null;

        public string AvatarUrl { get; set; }

        public virtual List<Joke> Jokes { get; set; } = new List<Joke>();

        public virtual List<Meme> Memes { get; set; } = new List<Meme>();

        public virtual List<Reminder> Reminders { get; set; } = new List<Reminder>();

        public virtual List<Notification> Notifications { get; set; } = new List<Notification>();

        public virtual List<Notification> TargetNotifications { get; set; } = new List<Notification>();

        public virtual List<Playlist> Playlists { get; set; } = new List<Playlist>();

        public virtual List<Song> Songs { get; set; } = new List<Song>();
    }

    public class Notification
    {
        [Key]
        public int Id { get; set; }

        public virtual User User { get; set; }

        public virtual User TargetUser { get; set; }

        public bool IsPermanent { get; set; }

        public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;
    }

    public class Playlist
    {
        [Key]
        public int Id { get; set; }

        public string Name { get; set; }

        public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;

        public virtual User Author { get; set; }

        public virtual List<Song> Songs { get; set; } = new List<Song>();
    }

    public class Song
    {
        [Key]
        public int Id { get; set; }

        public string Name { get; set; }

        public string Duration { get; set; }

        public string Url { get; set; } = null;

        public virtual User Author { get; set; }

        public virtual List<Playlist> Playlists { get; set; } = new List<Playlist>();
    }
}
