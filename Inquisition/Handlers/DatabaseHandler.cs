using System;
using System.Collections.Generic;
using System.Linq;
using Discord.WebSocket;
using Microsoft.EntityFrameworkCore;
using Inquisition.Data;

namespace Inquisition.Handlers
{
    public class DbHandler
    {
        private static DatabaseContext db = new DatabaseContext();

        public static User ConvertToUser(SocketGuildUser user)
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
        public static User ConvertToUser(SocketUser user)
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
        public static Result Save()
        {
            try
            {
                db.SaveChanges();
                return Result.Successful;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return Result.Failed;
            }
        }

        public static class Select
        {
            #region Single
            public static User User(SocketGuildUser user)
            {
                User local = ConvertToUser(user);
                User u = db.Users
                           .Where(x => x == local)
                           .FirstOrDefault();
                return u;
            }
            public static User User(SocketUser user)
            {
                User local = ConvertToUser(user);
                User u = db.Users
                           .Where(x => x == local)
                           .FirstOrDefault();
                return u;
            }
            public static User User(User user)
            {
                User u = db.Users
                           .Where(x => x == user)
                           .FirstOrDefault();
                return u;
            }
            public static Game Game(string name)
            {
                Game g = db.Games
                           .Where(x => x.Name == name)
                           .FirstOrDefault();
                return g;
            }
            public static Joke Joke(int id, User user)
            {
                Joke j = db.Jokes
                           .Where(x => x.Id == id && x.User == user)
                           .FirstOrDefault();
                return j;
            }
            public static Meme Meme(int id, User user)
            {
                Meme m = db.Memes
                           .Where(x => x.Id == id && x.User == user)
                           .FirstOrDefault();
                return m;
            }
            public static Reminder Reminder(int id, User user)
            {
                Reminder r = db.Reminders
                               .Where(x => x.Id == id && x.User == user)
                               .FirstOrDefault();
                return r;
            }
            public static Alert Alert(int id, User user)
            {
                Alert a = db.Alerts
                            .Where(x => x.Id == id && x.User == user)
                            .FirstOrDefault();
                return a;
            }
            public static Alert Alert(User author, User target)
            {
                Alert a = db.Alerts
                            .Where(x => x.User == author && x.TargetUser == target)
                            .FirstOrDefault();
                return a;
            }
            public static Playlist Playlist(int id)
            {
                Playlist p = db.Playlists
                               .Where(x => x.Id == id)
                               .Include(x => x.User)
                               .Include(x => x.Songs)
                               .FirstOrDefault();
                return p;
            }
            public static Playlist Playlist(int id, User user)
            {
                Playlist p = db.Playlists
                               .Where(x => x.Id == id && x.User == user)
                               .Include(x => x.User)
                               .Include(x => x.Songs)
                               .FirstOrDefault();
                return p;
            }
            public static Song Song(int id)
            {
                Song s = db.Songs
                           .Where(x => x.Id == id)
                           .Include(x => x.Playlists)
                           .Include(x => x.User)
                           .FirstOrDefault();
                return s;
            }
            public static Song Song(int id, User user)
            {
                Song s = db.Songs
                           .Where(x => x.Id == id && x.User == user)
                           .Include(x => x.Playlists)
                           .Include(x => x.User)
                           .FirstOrDefault();
                return s;
            }
            #endregion

            #region List
            public static List<Game> Games()
            {
                List<Game> Games = db.Games
                                     .ToList();
                return Games;
            }
            public static List<Game> Games(int amount)
            {
                List<Game> Games = db.Games
                                     .Take(amount)
                                     .ToList();
                return Games;
            }
            public static List<Joke> Jokes()
            {
                List<Joke> Jokes = db.Jokes
                                     .Include(x => x.User)
                                     .ToList();
                return Jokes;
            }
            public static List<Joke> Jokes(int amount)
            {
                List<Joke> Jokes = db.Jokes
                                     .Include(x => x.User)
                                     .Take(amount)
                                     .ToList();
                return Jokes;
            }
            public static List<Joke> Jokes(User user)
            {
                List<Joke> Jokes = db.Jokes
                                     .Where(x => x.User == user)
                                     .Include(x => x.User)
                                     .ToList();
                return Jokes;
            }
            public static List<Joke> Jokes(int amount, User user)
            {
                List<Joke> Jokes = db.Jokes
                                     .Where(x => x.User == user)
                                     .Include(x => x.User)
                                     .Take(amount)
                                     .ToList();
                return Jokes;
            }
            public static List<Meme> Memes()
            {
                List<Meme> Memes = db.Memes
                                     .Include(x => x.User)
                                     .ToList();
                return Memes;
            }
            public static List<Meme> Memes(int amount)
            {
                List<Meme> Memes = db.Memes
                                     .Include(x => x.User)
                                     .Take(amount)
                                     .ToList();
                return Memes;
            }
            public static List<Meme> Memes(User user)
            {
                List<Meme> Memes = db.Memes
                                     .Where(x => x.User == user)
                                     .Include(x => x.User)
                                     .ToList();
                return Memes;
            }
            public static List<Meme> Memes(int amount, User user)
            {
                List<Meme> Memes = db.Memes
                                     .Where(x => x.User == user)
                                     .Include(x => x.User)
                                     .Take(amount)
                                     .ToList();
                return Memes;
            }
            public static List<Reminder> Reminders()
            {
                List<Reminder> Reminders = db.Reminders
                                             .Where(x => x.DueDate <= DateTimeOffset.UtcNow)
                                             .Include(x => x.User)
                                             .ToList();
                return Reminders;
            }
            public static List<Reminder> Reminders(int amount)
            {
                List<Reminder> Reminders = db.Reminders
                                             .Where(x => x.DueDate <= DateTimeOffset.UtcNow)
                                             .Include(x => x.User)
                                             .Take(amount)
                                             .ToList();
                return Reminders;
            }
            public static List<Reminder> Reminders(User user)
            {
                List<Reminder> Reminders = db.Reminders
                                             .Where(x => x.User == user)
                                             .Include(x => x.User)
                                             .ToList();
                return Reminders;
            }
            public static List<Reminder> Reminders(int amount, User user)
            {
                List<Reminder> Reminders = db.Reminders
                                             .Where(x => x.User == user)
                                             .Include(x => x.User)
                                             .Take(amount)
                                             .ToList();
                return Reminders;
            }
            public static List<Alert> Alerts()
            {
                List<Alert> Alerts = db.Alerts
                                       .Include(x => x.User)
                                       .Include(x => x.TargetUser)
                                       .ToList();
                return Alerts;
            }
            public static List<Alert> Alerts(int amount)
            {
                List<Alert> Alerts = db.Alerts
                                       .Include(x => x.User)
                                       .Include(x => x.TargetUser)
                                       .Take(amount)
                                       .ToList();
                return Alerts;
            }
            public static List<Alert> Alerts(User user)
            {
                List<Alert> Alerts = db.Alerts
                                       .Where(x => x.User == user)
                                       .Include(x => x.User)
                                       .Include(x => x.TargetUser)
                                       .ToList();
                return Alerts;
            }
            public static List<Alert> Alerts(int amount, User user)
            {
                List<Alert> Alerts = db.Alerts
                                       .Where(x => x.User == user)
                                       .Include(x => x.User)
                                       .Include(x => x.TargetUser)
                                       .Take(amount)
                                       .ToList();
                return Alerts;
            }
            public static List<Alert> TargetAlerts(User targetUser)
            {
                List<Alert> Alerts = db.Alerts
                                       .Where(x => x.TargetUser == targetUser)
                                       .Include(x => x.User)
                                       .Include(x => x.TargetUser)
                                       .ToList();
                return Alerts;
            }
            public static List<Alert> TargetAlerts(int amount, User targetUser)
            {
                List<Alert> Alerts = db.Alerts
                                       .Where(x => x.TargetUser == targetUser)
                                       .Include(x => x.User)
                                       .Include(x => x.TargetUser)
                                       .Take(amount)
                                       .ToList();
                return Alerts;
            }
            public static List<Playlist> Playlists()
            {
                List<Playlist> Playlists = db.Playlists
                                             .Include(x => x.Songs)
                                             .Include(x => x.User)
                                             .ToList();
                return Playlists;
            }
            public static List<Playlist> Playlists(int amount)
            {
                List<Playlist> Playlists = db.Playlists
                                             .Include(x => x.Songs)
                                             .Include(x => x.User)
                                             .Take(amount)
                                             .ToList();
                return Playlists;
            }
            public static List<Playlist> Playlists(User user)
            {
                List<Playlist> Playlists = db.Playlists
                                             .Where(x => x.User == user)
                                             .Include(x => x.Songs)
                                             .Include(x => x.User)
                                             .ToList();
                return Playlists;
            }
            public static List<Playlist> Playlists(int amount, User user)
            {
                List<Playlist> Playlists = db.Playlists
                                             .Where(x => x.User == user)
                                             .Include(x => x.Songs)
                                             .Include(x => x.User)
                                             .Take(amount)
                                             .ToList();
                return Playlists;
            }
            public static List<Song> Songs()
            {
                List<Song> Songs = db.Songs
                                     .Include(x => x.Playlists)
                                     .Include(x => x.User)
                                     .ToList();
                return Songs;
            }
            public static List<Song> Songs(int amount)
            {
                List<Song> Songs = db.Songs
                                     .Include(x => x.Playlists)
                                     .Include(x => x.User)
                                     .Take(amount)
                                     .ToList();
                return Songs;
            }
            public static List<Song> Songs(User user)
            {
                List<Song> Songs = db.Songs
                                     .Where(x => x.User == user)
                                     .Include(x => x.Playlists)
                                     .Include(x => x.User)
                                     .ToList();
                return Songs;
            }
            public static List<Song> Songs(int amount, User user)
            {
                List<Song> Songs = db.Songs
                                     .Where(x => x.User == user)
                                     .Include(x => x.Playlists)
                                     .Include(x => x.User)
                                     .Take(amount)
                                     .ToList();
                return Songs;
            }
            #endregion
        }

        public static class Insert
        {
            #region Single
            public static Result User(SocketGuildUser user)
            {
                try
                {
                    User local = ConvertToUser(user);
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
            public static Result User(SocketUser user)
            {
                try
                {
                    User local = ConvertToUser(user);
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
            public static Result User(User user)
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
            public static Result Game(Game game)
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
            public static Result Joke(Joke joke)
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
            public static Result Meme(Meme meme)
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
            public static Result Reminder(Reminder reminder)
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
            public static Result Alert(Alert alert)
            {
                try
                {
                    db.Alerts.Add(alert);
                    db.SaveChanges();
                    return Result.Successful;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    return Result.Failed;
                }
            }
            public static Result Playlist(Playlist playlist)
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
            public static Result Song(Song song)
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

            #region List
            public static Result UserList(List<SocketGuildUser> users)
            {
                try
                {
                    List<User> list = new List<User>();
                    foreach (SocketGuildUser user in users)
                    {
                        User local = ConvertToUser(user);
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
            public static Result UserList(List<SocketUser> users)
            {
                try
                {
                    List<User> list = new List<User>();
                    foreach (SocketUser user in users)
                    {
                        User local = ConvertToUser(user);
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
            public static Result UserList(List<User> users)
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
            public static Result GameList(List<Game> games)
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
            public static Result PlaylistList(List<Playlist> playlists)
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
            public static Result SongList(List<Song> songs)
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
        }

        public static class Delete
        {
            #region Single
            public static Result SocketGuildUser(SocketGuildUser user)
            {
                try
                {
                    User temp = ConvertToUser(user);
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
            public static Result SoketUser(SocketUser user)
            {
                try
                {
                    User temp = ConvertToUser(user);
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
            public static Result User(User user)
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
            public static Result Game(Game game)
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
            public static Result Joke(Joke joke)
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
            public static Result Meme(Meme meme)
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
            public static Result Reminder(Reminder reminder)
            {
                try
                {
                    db.Reminders.Remove(reminder);
                    db.SaveChanges();
                    return Result.Successful;
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    return Result.Failed;
                }
            }
            public static Result Alert(Alert alert)
            {
                try
                {
                    Alert temp =
                        db.Alerts
                        .Where(x => x.TargetUser == alert.TargetUser && x.User == alert.User)
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
            public static Result Playlist(Playlist playlist)
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
            public static Result Song(Song song)
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

            #region List
            public static Result UserList(List<User> users)
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
            public static Result GameList(List<Game> games)
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
            public static Result JokeList(List<Joke> jokes)
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
            public static Result MemeList(List<Meme> memes)
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
            public static Result ReminderList(List<Reminder> reminders)
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
            public static Result AlertList(List<Alert> notifications)
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
            public static Result PlaylistList(List<Playlist> playlists)
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
            public static Result SongList(List<Song> songs)
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
        }

        public static class Update
        {
            public static Result Game(Game game)
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
            public static Result User(User user)
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
            public static Result Playlist(Playlist playlist)
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
            public static Result Song(Song song)
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
        }

        #region Exists
        public static bool Exists(SocketGuildUser user)
        {
            User local = ConvertToUser(user);
            bool exists = db.Users.Any(x => x.Id == local.Id);
            return exists;
        }
        public static bool Exists(SocketUser user)
        {
            User local = ConvertToUser(user);
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
    }
}
