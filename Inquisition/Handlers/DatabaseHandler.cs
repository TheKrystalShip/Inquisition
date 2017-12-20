using System;
using System.Collections.Generic;
using System.Linq;
using Discord.WebSocket;
using Microsoft.EntityFrameworkCore;
using Inquisition.Data;

namespace Inquisition.Handlers
{
    public class DatabaseHandler
    {
        public enum Result
        {
            Failed,
            Successful,
            AlreadyExists,
            DoesNotExist
        }

        private static DatabaseContext db = new DatabaseContext();

        public static User ConvertToLocalUser(SocketGuildUser user)
        {
            User local = new User
            {
                Username = user.Username,
                Id = $"{user.Id}",
                Nickname = user.Nickname,
                AvatarUrl = user.GetAvatarUrl(),
                Discriminator = user.Discriminator,
                JoinedAt = user.JoinedAt,
                LastSeenOnline = DateTimeOffset.UtcNow
            };

            return local;
        }

        public static User ConvertToLocalUser(SocketUser user)
        {
            User local = new User
            {
                Username = user.Username,
                Id = $"{user.Id}",
                AvatarUrl = user.GetAvatarUrl(),
                Discriminator = user.Discriminator,
                LastSeenOnline = DateTimeOffset.UtcNow
            };

            return local;
        }

        public static void Save()
        {
            db.SaveChanges();
        }

        #region AddToDb

        public static Result AddToDb(SocketGuildUser user)
        {
            try
            {
                User local = ConvertToLocalUser(user);
                if (Exists(local))
                {
                    return Result.AlreadyExists;
                }
                else
                {
                    db.Users.Add(local);
                    db.SaveChanges();
                    return Result.Successful;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return Result.Failed;
            }
        }

        public static Result AddToDb(SocketUser user)
        {
            try
            {
                User local = ConvertToLocalUser(user);
                if (Exists(local))
                {
                    return Result.AlreadyExists;
                }
                else
                {
                    db.Users.Add(local);
                    db.SaveChanges();
                    return Result.Successful;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return Result.Failed;
            }
        }

        public static Result AddToDb(User user)
        {
            try
            {
                if (Exists(user))
                {
                    return Result.AlreadyExists;
                }
                else
                {
                    db.Users.Add(user);
                    db.SaveChanges();
                    return Result.Successful;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return Result.Failed;
            }
        }

        public static Result AddToDb(Game game)
        {
            try
            {
                if (Exists(game))
                {
                    return Result.AlreadyExists;
                }
                else
                {
                    db.Games.Add(game);
                    db.SaveChanges();
                    return Result.Successful;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return Result.Failed;
            }
        }

        public static Result AddToDb(Joke joke)
        {
            try
            {
                db.Jokes.Add(joke);
                db.SaveChanges();
                return Result.Successful;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return Result.Failed;
            }
        }

        public static Result AddToDb(Meme meme)
        {
            try
            {
                db.Memes.Add(meme);
                db.SaveChanges();
                return Result.Successful;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return Result.Failed;
            }
        }

        public static Result AddToDb(Reminder reminder)
        {
            try
            {
                db.Reminders.Add(reminder);
                db.SaveChanges();
                return Result.Successful;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return Result.Failed;
            }
        }

        public static Result AddToDb(Alert notification)
        {
            try
            {
                db.Alerts.Add(notification);
                db.SaveChanges();
                return Result.Successful;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return Result.Failed;
            }
        }

        public static Result AddToDb(Playlist playlist)
        {
            try
            {
                db.Playlists.Add(playlist);
                db.SaveChanges();
                return Result.Successful;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return Result.Failed;
            }
        }

        public static Result AddToDb(Song song)
        {
            try
            {
                db.Songs.Add(song);
                db.SaveChanges();
                return Result.Successful;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return Result.Failed;
            }
        }

        #endregion

        #region AddRangeToDb

        public static Result AddRangeToDb(List<SocketGuildUser> users)
        {
            try
            {
                List<User> list = new List<User>();
                foreach (SocketGuildUser user in users)
                {
                    User local = ConvertToLocalUser(user);
                    if (!Exists(local))
                        list.Add(local);
                }

                db.Users.AddRange(list);
                db.SaveChanges();
                return Result.Successful;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return Result.Failed;
            }
        }

        public static Result AddRangeToDb(List<SocketUser> users)
        {
            try
            {
                List<User> list = new List<User>();
                foreach (SocketUser user in users)
                {
                    User local = ConvertToLocalUser(user);
                    if (!Exists(local))
                        list.Add(local);
                }

                db.Users.AddRange(list);
                db.SaveChanges();
                return Result.Successful;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return Result.Failed;
            }
        }

        public static Result AddRangeToDb(List<User> users)
        {
            try
            {
                List<User> list = new List<User>();
                foreach (User user in users)
                {
                    if (!Exists(user))
                        list.Add(user);
                }

                db.Users.AddRange(list);
                db.SaveChanges();
                return Result.Successful;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return Result.Failed;
            }
        }

        public static Result AddRangeToDb(List<Game> games)
        {
            try
            {
                List<Game> list = new List<Game>();
                foreach (Game game in games)
                {
                    if (!Exists(game))
                        list.Add(game);
                }

                db.Games.AddRange(list);
                db.SaveChanges();
                return Result.Successful;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return Result.Failed;
            }
        }

        public static Result AddRangeToDb(List<Playlist> playlists)
        {
            try
            {
                List<Playlist> list = new List<Playlist>();
                foreach (Playlist p in playlists)
                {
                    if (!Exists(p))
                        list.Add(p);
                }

                db.Playlists.AddRange(list);
                db.SaveChanges();
                return Result.Successful;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return Result.Failed;
            }
        }

        public static Result AddRangeToDb(List<Song> songs)
        {
            try
            {
                List<Song> list = new List<Song>();
                foreach (Song s in songs)
                {
                    if (!Exists(s))
                        list.Add(s);
                }

                db.Songs.AddRange(list);
                db.SaveChanges();
                return Result.Successful;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return Result.Failed;
            }
        }

        #endregion

        #region RemoveFromDb

        public static Result RemoveFromDb(SocketGuildUser user)
        {
            try
            {
                User temp = ConvertToLocalUser(user);
                db.Users.Remove(temp);
                db.SaveChanges();
                return Result.Successful;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return Result.Failed;
            }
        }

        public static Result RemoveFromDb(SocketUser user)
        {
            try
            {
                User temp = ConvertToLocalUser(user);
                db.Users.Remove(temp);
                db.SaveChanges();
                return Result.Successful;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return Result.Failed;
            }
        }

        public static Result RemoveFromDb(User user)
        {
            try
            {
                db.Users.Remove(user);
                db.SaveChanges();
                return Result.Successful;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return Result.Failed;
            }
        }

        public static Result RemoveFromDb(Game game)
        {
            try
            {
                db.Games.Remove(game);
                db.SaveChanges();
                return Result.Successful;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return Result.Failed;
            }
        }

        public static Result RemoveFromDb(Joke joke)
        {
            try
            {
                db.Jokes.Remove(joke);
                db.SaveChanges();
                return Result.Successful;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return Result.Failed;
            }
        }

        public static Result RemoveFromDb(Meme meme)
        {
            try
            {
                db.Memes.Remove(meme);
                db.SaveChanges();
                return Result.Successful;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return Result.Failed;
            }
        }

        public static Result RemoveFromDb(Reminder reminder)
        {
            try
            {
                Reminder temp = db.Reminders.Where(x => x.Id == reminder.Id).FirstOrDefault();
                db.Reminders.Remove(temp);
                db.SaveChanges();
                return Result.Successful;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return Result.Failed;
            }
        }

        public static Result RemoveFromDb(Alert notification)
        {
            try
            {
                Alert temp =
                    db.Alerts
                    .Where(x => x.TargetUser == notification.TargetUser && x.User == notification.User)
                    .FirstOrDefault();

                db.Alerts.Remove(temp);
                db.SaveChanges();
                return Result.Successful;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return Result.Failed;
            }
        }

        public static Result RemoveFromDb(Playlist playlist)
        {
            try
            {
                db.Playlists.Remove(playlist);
                db.SaveChanges();
                return Result.Successful;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return Result.Failed;
            }
        }

        public static Result RemoveFromDb(Song song)
        {
            try
            {
                db.Songs.Remove(song);
                db.SaveChanges();
                return Result.Successful;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return Result.Failed;
            }
        }

        #endregion

        #region RemoveRangeFromDb

        public static Result RemoveRangeFromDb(List<User> users)
        {
            try
            {
                db.Users.RemoveRange(users);
                db.SaveChanges();
                return Result.Successful;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return Result.Failed;
            }
        }

        public static Result RemoveRangeFromDb(List<Game> games)
        {
            try
            {
                db.Games.RemoveRange(games);
                db.SaveChanges();
                return Result.Successful;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return Result.Failed;
            }
        }

        public static Result RemoveRangeFromDb(List<Joke> jokes)
        {
            try
            {
                db.Jokes.RemoveRange(jokes);
                db.SaveChanges();
                return Result.Successful;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return Result.Failed;
            }
        }

        public static Result RemoveRangeFromDb(List<Meme> memes)
        {
            try
            {
                db.Memes.RemoveRange(memes);
                db.SaveChanges();
                return Result.Successful;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return Result.Failed;
            }
        }

        public static Result RemoveRangeFromDb(List<Reminder> reminders)
        {
            try
            {
                db.Reminders.RemoveRange(reminders);
                db.SaveChanges();
                return Result.Successful;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return Result.Failed;
            }
        }

        public static Result RemoveRangeFromDb(List<Alert> notifications)
        {
            try
            {
                db.Alerts.RemoveRange(notifications);
                db.SaveChanges();
                return Result.Successful;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return Result.Failed;
            }
        }

        public static Result RemoveRangeFromDb(List<Playlist> playlists)
        {
            try
            {
                db.Playlists.RemoveRange(playlists);
                db.SaveChanges();
                return Result.Successful;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return Result.Failed;
            }
        }

        public static Result RemoveRangeFromDb(List<Song> songs)
        {
            try
            {
                db.Songs.RemoveRange(songs);
                db.SaveChanges();
                return Result.Successful;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return Result.Failed;
            }
        }

        #endregion

        #region UpdateInDb

        public static Result UpdateInDb(Game game)
        {
            try
            {
                if (!Exists(game))
                    return Result.DoesNotExist;

                db.Games.Update(game);
                db.SaveChanges();
                return Result.Successful;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return Result.Failed;
            }
        }

        public static Result UpdateInDb(User user)
        {
            try
            {
                if (!Exists(user))
                    return Result.DoesNotExist;

                db.Users.Update(user);
                db.SaveChanges();
                return Result.Successful;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return Result.Failed;
            }
        }

        public static Result UpdateInDb(Playlist playlist)
        {
            try
            {
                if (!Exists(playlist))
                    return Result.DoesNotExist;

                db.Playlists.Update(playlist);
                db.SaveChanges();
                return Result.Successful;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return Result.Failed;
            }
        }

        public static Result UpdateInDb(Song song)
        {
            try
            {
                if (!Exists(song))
                    return Result.DoesNotExist;

                db.Songs.Update(song);
                db.SaveChanges();
                return Result.Successful;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return Result.Failed;
            }
        }

        #endregion

        #region Exists

        public static bool Exists(SocketGuildUser user)
        {
            User local = ConvertToLocalUser(user);
            bool exists = db.Users.Any(x => x.Id == local.Id);
            return exists;
        }

        public static bool Exists(SocketUser user)
        {
            User local = ConvertToLocalUser(user);
            bool exists = db.Users.Any(x => x.Id == local.Id);
            return exists;
        }

        public static bool Exists(User user)
        {
            bool exists = db.Users.Any(x => x.Id == user.Id);
            return exists;
        }

        public static bool Exists(Game game)
        {
            bool exists = db.Games.Any(x => x.Name == game.Name);
            return exists;
        }

        public static bool Exists(Playlist playlist)
        {
            bool exists = db.Playlists.Any(x => x.Name == playlist.Name && x.User == playlist.User);
            return exists;
        }

        public static bool Exists(Song song)
        {
            bool exists = db.Songs.Any(x => x.Name == song.Name && x.User == song.User);
            return exists;
        }

        #endregion

        #region List (No user specified)

        public static List<Game> ListAll(Game game)
        {
            List<Game> Games = db.Games
                                 .Take(10)
                                 .ToList();
            return Games;
        }

        public static List<Joke> ListAll(Joke joke)
        {
            List<Joke> Jokes = db.Jokes
                                 .Include(x => x.User)
                                 .Take(10)
                                 .ToList();
            return Jokes;
        }

        public static List<Meme> ListAll(Meme meme)
        {
            List<Meme> Memes = db.Memes
                                 .Include(x => x.User)
                                 .Take(10)
                                 .ToList();
            return Memes;
        }

        public static List<Reminder> ListAll(Reminder reminder)
        {
            List<Reminder> Reminders = db.Reminders
                                         .Where(x => x.DueDate <= DateTimeOffset.UtcNow)
                                         .Include(x => x.User)
                                         .ToList();
            return Reminders;
        }

        public static List<Reminder> ListLastTen(Reminder reminder)
        {
            List<Reminder> Reminders = db.Reminders
                                         .Where(x => x.DueDate <= DateTimeOffset.UtcNow)
                                         .Include(x => x.User)
                                         .Take(10)
                                         .ToList();
            return Reminders;
        }

        public static List<Alert> ListAll(Alert alert)
        {
            List<Alert> Alerts = db.Alerts
                                   .Include(x => x.User)
                                   .Include(x => x.TargetUser)
                                   .ToList();
            return Alerts;
        }

        public static List<Alert> ListAllTargetAlerts(Alert notification, User target)
        {
            List<Alert> Alerts = db.Alerts
                                   .Where(x => x.TargetUser == target)
                                   .Include(x => x.User)
                                   .Include(x => x.TargetUser)
                                   .ToList();
            return Alerts;
        }

        public static List<Playlist> ListAll(Playlist playlist)
        {
            List<Playlist> Playlists = db.Playlists
                                         .Include(x => x.Songs)
                                         .Include(x => x.User)
                                         .ToList();
            return Playlists;
        }

        public static List<Song> ListAll(Song song)
        {
            List<Song> Songs = db.Songs
                                 .Include(x => x.Playlists)
                                 .Include(x => x.User)
                                 .ToList();
            return Songs;
        }

        #endregion

        #region List (User specified)

        public static List<Joke> ListAll(Joke joke, User user)
        {
            if (user is null)
                return ListAll(joke);

            List<Joke> Jokes = db.Jokes
                                 .Where(x => x.User == user)
                                 .Include(x => x.User)
                                 .Take(10)
                                 .ToList();
            return Jokes;
        }

        public static List<Meme> ListAll(Meme meme, User user)
        {
            if (user is null)
                return ListAll(meme);

            List<Meme> Memes = db.Memes
                                 .Where(x => x.User == user)
                                 .Include(x => x.User)
                                 .Take(10)
                                 .ToList();
            return Memes;
        }

        public static List<Reminder> ListAll(Reminder reminder, User user)
        {
            if (user is null)
                return ListAll(reminder);

            List<Reminder> Reminders = db.Reminders
                                         .Where(x => x.User == user)
                                         .Include(x => x.User)
                                         .ToList();
            return Reminders;
        }

        public static List<Alert> ListAll(Alert alert, User user)
        {
            if (user is null)
                return ListAll(alert);

            List<Alert> Alerts = db.Alerts
                                   .Where(x => x.User == user)
                                   .Include(x => x.User)
                                   .Include(x => x.TargetUser)
                                   .ToList();
            return Alerts;
        }

        public static List<Playlist> ListAll(Playlist playlist, User user)
        {
            if (user is null)
                return ListAll(playlist);

            List<Playlist> Playlists = db.Playlists
                                         .Where(x => x.User == user)
                                         .Include(x => x.Songs)
                                         .Include(x => x.User)
                                         .ToList();
            return Playlists;
        }

        public static List<Song> ListAll(Song song, User user)
        {
            if (user is null)
                return ListAll(song);

            List<Song> Songs = db.Songs
                                 .Where(x => x.User == user)
                                 .Include(x => x.Playlists)
                                 .Include(x => x.User)
                                 .ToList();
            return Songs;
        }

        #endregion

        #region GetFromDb

        public static User GetFromDb(SocketGuildUser user)
        {
            User local = ConvertToLocalUser(user);
            User u = db.Users
                       .Where(x => x == local)
                       .FirstOrDefault();
            return u;
        }

        public static User GetFromDb(SocketUser user)
        {
            User local = ConvertToLocalUser(user);
            User u = db.Users
                       .Where(x => x == local)
                       .FirstOrDefault();
            return u;
        }

        public static Game GetFromDb(Game game)
        {
            Game g = db.Games
                       .Where(x => x.Name == game.Name)
                       .FirstOrDefault();
            return g;
        }

        public static User GetFromDb(User user)
        {
            User u = db.Users
                       .Where(x => x == user)
                       .FirstOrDefault();
            return u;
        }

        public static Joke GetFromDb(Joke joke, User user)
        {
            Joke j = db.Jokes
                       .Where(x => x.Id == joke.Id && x.User == user)
                       .FirstOrDefault();
            return j;
        }

        public static Meme GetFromDb(Meme meme, User user)
        {
            Meme m = db.Memes
                       .Where(x => x.Id == meme.Id && x.User == user)
                       .FirstOrDefault();
            return m;
        }

        public static Reminder GetFromDb(Reminder reminder, User user)
        {
            Reminder r = db.Reminders
                           .Where(x => x.Id == reminder.Id && x.User == user)
                           .FirstOrDefault();
            return r;
        }

        public static Playlist GetFromDb(Playlist playlist, User user)
        {
            Playlist p = db.Playlists
                           .Where(x => x.Name == playlist.Name && x.User == user)
                           .FirstOrDefault();
            return p;
        }

        public static Song GetFromDb(Song song, User user)
        {
            Song s = db.Songs
                       .Where(x => x.Name == song.Name && x.User == user)
                       .FirstOrDefault();
            return s;
        }

        #endregion
    }
}
