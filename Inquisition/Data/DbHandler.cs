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
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }

        #endregion

        #region RemoveFromDb

        public static void RemoveFromDb(SocketGuildUser user)
        {
            User temp = GetFromDb(user);
            db.Users.Remove(temp);
            db.SaveChanges();
        }

        public static void RemoveFromDb(SocketUser user)
        {
            User temp = GetFromDb(user);
            db.Users.Remove(temp);
            db.SaveChanges();
        }

        public static void RemoveFromDb(User user)
        {
            db.Users.Remove(user);
            db.SaveChanges();
        }

        public static void RemoveFromDb(Game game)
        {
            db.Games.Remove(game);
            db.SaveChanges();
        }

        public static void RemoveFromDb(Joke joke)
        {
            db.Jokes.Remove(joke);
            db.SaveChanges();
        }

        public static void RemoveFromDb(Meme meme)
        {
            db.Memes.Remove(meme);
            db.SaveChanges();
        }

        public static void RemoveFromDb(Reminder reminder)
        {
            db.Reminders.Remove(reminder);
            db.SaveChanges();
        }

        public static void RemoveFromDb(Notification notification)
        {
            db.Notifications.Remove(notification);
            db.SaveChanges();
        }

        #endregion

        #region RemoveRangeFromDb

        public static void RemoveRangeFromDb(List<User> users)
        {
            db.RemoveRange(users);
            db.SaveChanges();
        }

        public static void RemoveRangeFromDb(List<Game> games)
        {
            db.RemoveRange(games);
            db.SaveChanges();
        }

        public static void RemoveRangeFromDb(List<Joke> jokes)
        {
            db.RemoveRange(jokes);
            db.SaveChanges();
        }

        public static void RemoveRangeFromDb(List<Meme> memes)
        {
            db.RemoveRange(memes);
            db.SaveChanges();
        }

        public static void RemoveRangeFromDb(List<Reminder> reminders)
        {
            db.RemoveRange(reminders);
            db.SaveChanges();
        }

        public static void RemoveRangeFromDb(List<Notification> notifications)
        {
            db.RemoveRange(notifications);
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
            List<Notification> Notifications = db.Notifications.Include(x => x.User).Include(x => x.TargetUser).ToList();
            return Notifications;
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

        #endregion

        #region GetFromDb

        public static User GetFromDb(SocketUser user)
        {
            User local = ConvertToLocalUser(user);
            User u = db.Users.Where(x => x.Id == local.Id).FirstOrDefault();
            return u;
        }

        public static User GetFromDb(SocketGuildUser user)
        {
            User local = ConvertToLocalUser(user);
            User u = db.Users.Where(x => x.Id == local.Id).FirstOrDefault();
            return u;
        }

        public static User GetFromDb(User user)
        {
            User u = db.Users.Where(x => x.Id == user.Id).FirstOrDefault();
            return u;
        }

        public static Game GetFromDb(Game game)
        {
            Game g = db.Games.Where(x => x.Name == game.Name).FirstOrDefault();
            return g;
        }

        #endregion
    }
}
