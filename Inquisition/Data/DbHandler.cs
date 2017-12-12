using System;
using System.Collections.Generic;
using System.Linq;
using Discord.WebSocket;
using Microsoft.EntityFrameworkCore;

namespace Inquisition.Data
{
    public class DbHandler
    {
        private static InquisitionContext db = new InquisitionContext();

        private static User ConvertToLocalUser(SocketGuildUser user)
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

        private static User ConvertToLocalUser(SocketUser user)
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

        public static bool AddToDb(SocketGuildUser user)
        {
            try
            {
                User local = ConvertToLocalUser(user);
                if (Exists(local))
                {
                    return false;
                } else
                {
                    db.Users.Add(local);
                    db.SaveChanges();
                    return true;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }
        
        public static bool AddToDb(SocketUser user)
        {
            try
            {
                User local = ConvertToLocalUser(user);
                if (Exists(local))
                {
                    return false;
                } else
                {
                    db.Users.Add(local);
                    db.SaveChanges();
                    return true;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }
        
        public static bool AddToDb(User user)
        {
            try
            {
                if (Exists(user))
                {
                    return false;
                } else
                {
                    db.Users.Add(user);
                    db.SaveChanges();
                    return true;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }

        public static bool AddToDb(Game game)
        {
            try
            {
                if (Exists(game))
                {
                    return false;
                } else
                {
                    db.Games.Add(game);
                    db.SaveChanges();
                    return true;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }

        public static bool AddToDb(Joke joke)
        {
            try
            {
                db.Jokes.Add(joke);
                db.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }

        public static bool AddToDb(Meme meme)
        {
            try
            {
                db.Memes.Add(meme);
                db.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }

        public static bool AddToDb(Reminder reminder)
        {
            try
            {
                db.Reminders.Add(reminder);
                db.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }

        public static bool AddToDb(Notification notification)
        {
            try
            {
                db.Notifications.Add(notification);
                db.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }

        public static bool AddToDb(Playlist playlist)
        {
            try
            {
                db.Playlists.Add(playlist);
                db.SaveChanges();
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return false;
            }
        }

        public static bool AddToDb(Song song)
        {
            try
            {
                db.Songs.Add(song);
                db.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return false;
            }
        }

        #endregion

        #region AddRangeToDb

        public static bool AddRangeToDb(List<SocketGuildUser> users)
        {
            try
            {
                List<User> list = new List<User>();
                foreach (SocketGuildUser user in users)
                {
                    User local = ConvertToLocalUser(user);
                    if (Exists(local))
                    {
                        continue;
                    }
                    else
                    {
                        list.Add(local);
                    }
                }

                db.Users.AddRange(list);
                db.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        } 

        public static bool AddRangeToDb(List<SocketUser> users)
        {
            try
            {
                List<User> list = new List<User>();
                foreach (SocketUser user in users)
                {
                    User local = ConvertToLocalUser(user);
                    if (Exists(local))
                    {
                        continue;
                    }
                    else
                    {
                        list.Add(local);
                    }
                }

                db.Users.AddRange(list);
                db.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }

        public static bool AddRangeToDb(List<User> users)
        {
            try
            {
                List<User> list = new List<User>();
                foreach (User user in users)
                {
                    if (Exists(user))
                    {
                        continue;
                    }
                    else
                    {
                        list.Add(user);
                    }
                }

                db.Users.AddRange(list);
                db.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }

        public static bool AddRangeToDb(List<Game> games)
        {
            try
            {
                List<Game> list = new List<Game>();
                foreach (Game game in games)
                {
                    if (Exists(game))
                    {
                        continue;
                    } else
                    {
                        list.Add(game);
                    }
                }

                db.Games.AddRange(list);
                db.SaveChanges();
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return false;
            }
        }

        public static bool AddRangeToDb(List<Playlist> playlists)
        {
            try
            {
                List<Playlist> list = new List<Playlist>();
                foreach (Playlist p in playlists)
                {
                    if (Exists(p))
                    {
                        continue;
                    } else
                    {
                        list.Add(p);
                    }
                }

                db.Playlists.AddRange(list);
                db.SaveChanges();
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return false;
            }
        }

        public static bool AddRangeToDb(List<Song> songs)
        {
            try
            {
                List<Song> list = new List<Song>();
                foreach (Song s in songs)
                {
                    if (Exists(s))
                    {
                        continue;
                    } else
                    {
                        list.Add(s);
                    }
                }

                db.Songs.AddRange(list);
                db.SaveChanges();
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return false;
            }
        }

        #endregion

        #region RemoveFromDb

        public static bool RemoveFromDb(SocketGuildUser user)
        {
            try
            {
                User temp = ConvertToLocalUser(user);
                db.Users.Remove(temp);
                db.SaveChanges();
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return false;
            }
        }

        public static bool RemoveFromDb(SocketUser user)
        {
            try
            {
                User temp = ConvertToLocalUser(user);
                db.Users.Remove(temp);
                db.SaveChanges();
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return false;
            }
        }

        public static bool RemoveFromDb(User user)
        {
            try
            {
                db.Users.Remove(user);
                db.SaveChanges();
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return false;
            }
        }

        public static bool RemoveFromDb(Game game)
        {
            try
            {
                db.Games.Remove(game);
                db.SaveChanges();
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return false;
            }
        }

        public static bool RemoveFromDb(Joke joke)
        {
            try
            {
                db.Jokes.Remove(joke);
                db.SaveChanges();
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return false;
            }
        }

        public static bool RemoveFromDb(Meme meme)
        {
            try
            {
                db.Memes.Remove(meme);
                db.SaveChanges();
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return false;
            }
        }

        public static bool RemoveFromDb(Reminder reminder)
        {
            try
            {
                Reminder temp = db.Reminders.Where(x => x.DueDate == reminder.DueDate).FirstOrDefault();
                db.Reminders.Remove(temp);
                db.SaveChanges();
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return false;
            }
        }

        public static bool RemoveFromDb(Notification notification)
        {
            try
            {
                Notification temp = 
                    db.Notifications
                    .Where(x => x.TargetUser == notification.TargetUser && x.User == notification.User)
                    .FirstOrDefault();

                db.Notifications.Remove(temp);
                db.SaveChanges();
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return false;
            }
        }

        public static bool RemoveFromDb(Playlist playlist)
        {
            try
            {
                db.Playlists.Remove(playlist);
                db.SaveChanges();
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return false;
            }
        }

        public static bool RemoveFromDb(Song song)
        {
            try
            {
                db.Songs.Remove(song);
                db.SaveChanges();
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return false;
            }
        }

        #endregion

        #region RemoveRangeFromDb

        public static void RemoveRangeFromDb(List<User> users)
        {
            db.Users.RemoveRange(users);
            db.SaveChanges();
        }

        public static void RemoveRangeFromDb(List<Game> games)
        {
            db.Games.RemoveRange(games);
            db.SaveChanges();
        }

        public static void RemoveRangeFromDb(List<Joke> jokes)
        {
            db.Jokes.RemoveRange(jokes);
            db.SaveChanges();
        }

        public static void RemoveRangeFromDb(List<Meme> memes)
        {
            db.Memes.RemoveRange(memes);
            db.SaveChanges();
        }

        public static void RemoveRangeFromDb(List<Reminder> reminders)
        {
            db.Reminders.RemoveRange(reminders);
            db.SaveChanges();
        }

        public static void RemoveRangeFromDb(List<Notification> notifications)
        {
            db.Notifications.RemoveRange(notifications);
            db.SaveChanges();
        }

        public static void RemoveRangeFromDb(List<Playlist> playlists)
        {
            db.Playlists.RemoveRange(playlists);
            db.SaveChanges();
        }

        public static void RemoveRangeFromDb(List<Song> songs)
        {
            db.Songs.RemoveRange(songs);
            db.SaveChanges();
        }

        #endregion

        #region UpdateInDb

        public static void UpdateInDb(Game game)
        {
            if (!Exists(game))
                AddToDb(game);

            db.Games.Update(game);
            db.SaveChanges();
        }

        public static void UpdateInDb(Playlist playlist)
        {
            if (!Exists(playlist))
                AddToDb(playlist);

            db.Playlists.Update(playlist);
            db.SaveChanges();
        }

        public static void UpdateInDb(Song song)
        {
            if (!Exists(song))
                AddToDb(song);

            db.Songs.Update(song);
            db.SaveChanges();
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
            bool exists = db.Playlists.Any(x => x.Name == playlist.Name && x.Author == playlist.Author);
            return exists;
        }

        public static bool Exists(Song song)
        {
            bool exists = db.Songs.Any(x => x.Name == song.Name && x.Author == song.Author);
            return exists;
        }

        #endregion

        #region List (No user specified)

        public static List<Game> ListAll(Game game)
        {
            List<Game> Games = db.Games.ToList();
            return Games;
        }

        public static List<Joke> ListAll(Joke joke)
        {
            List<Joke> Jokes = db.Jokes.Include(x => x.User).ToList();
            return Jokes;
        }

        public static List<Meme> ListAll(Meme meme)
        {
            List<Meme> Memes = db.Memes.Include(x => x.User).ToList();
            return Memes;
        }

        public static List<Reminder> ListAll(Reminder reminder)
        {
            List<Reminder> Reminders = db.Reminders.Include(x => x.User).ToList();
            return Reminders;
        }

        public static List<Notification> ListAll(Notification notification)
        {
            List<Notification> Notifications = 
                db.Notifications
                .Include(x => x.User)
                .Include(x => x.TargetUser)
                .ToList();

            return Notifications;
        }

        public static List<Playlist> ListAll(Playlist playlist)
        {
            List<Playlist> Playlists =
                db.Playlists
                .Include(x => x.Songs)
                .Include(x => x.Author)
                .ToList();

            return Playlists;
        }

        public static List<Song> ListAll(Song song)
        {
            List<Song> Songs =
                db.Songs
                .Include(x => x.Playlists)
                .Include(x => x.Author)
                .ToList();

            return Songs;
        }

        #endregion

        #region List (User specified)

        public static List<Joke> ListAll(Joke joke, User user)
        {
            if (user is null)
                return ListAll(joke);

            List<Joke> Jokes = db.Jokes.Where(x => x.User == user).Include(x => x.User).ToList();
            return Jokes;
        }

        public static List<Meme> ListAll(Meme meme, User user)
        {
            if (user is null)
                return ListAll(meme);

            List<Meme> Memes = db.Memes.Where(x => x.User == user).Include(x => x.User).ToList();
            return Memes;
        }

        public static List<Reminder> ListAll(Reminder reminder, User user)
        {
            if (user is null)
                return ListAll(reminder);

            List<Reminder> Reminders = db.Reminders.Where(x => x.User == user).Include(x => x.User).ToList();
            return Reminders;
        }

        public static List<Notification> ListAll(Notification notification, User user)
        {
            if (user is null)
                return ListAll(notification);
            
            List<Notification> Notifications = 
                db.Notifications
                .Where(x => x.User == user)
                .Include(x => x.User)
                .Include(x => x.TargetUser)
                .ToList();

            return Notifications;
        }

        public static List<Playlist> ListAll(Playlist playlist, User user)
        {
            if (user is null)
                return ListAll(playlist);

            List<Playlist> Playlists =
                db.Playlists
                .Where(x => x.Author == user)
                .Include(x => x.Songs)
                .Include(x => x.Author)
                .ToList();

            return Playlists;
        }

        public static List<Song> ListAll(Song song, User user)
        {
            if (user is null)
                return ListAll(song);

            List<Song> Songs =
                db.Songs.Where(x => x.Author == user)
                .Include(x => x.Playlists)
                .Include(x => x.Author)
                .ToList();

            return Songs;
        }

        #endregion

        #region GetFromDb

        public static User GetFromDb(SocketUser user)
        {
            User local = ConvertToLocalUser(user);
            User u = db.Users.Where(x => x == local).FirstOrDefault();
            return u;
        }

        public static User GetFromDb(SocketGuildUser user)
        {
            User local = ConvertToLocalUser(user);
            User u = db.Users.Where(x => x == local).FirstOrDefault();
            return u;
        }

        public static User GetFromDb(User user)
        {
            User u = db.Users.Where(x => x == user).FirstOrDefault();
            return u;
        }

        public static Game GetFromDb(Game game)
        {
            Game g = db.Games.Where(x => x.Name == game.Name).FirstOrDefault();
            return g;
        }

        public static Playlist GetFromDb(Playlist playlist)
        {
            Playlist local = db.Playlists.Where(x => x.Name == playlist.Name).FirstOrDefault();
            return local;
        }

        public static Song GetFromDb(Song song)
        {
            Song local = db.Songs.Where(x => x.Name == song.Name).FirstOrDefault();
            return local;
        }

        #endregion
    }
}
