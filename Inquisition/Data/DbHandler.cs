using System;
using System.Collections.Generic;
using System.Linq;
using Discord.WebSocket;

namespace Inquisition.Data
{
    public class DbHandler
    {
        private static InquisitionContext db = new InquisitionContext();

        #region AddToDb

        /*Server member*/
        public static bool AddToDb(SocketGuildUser user)
        {
            try
            {
                if (Exists(user))
                {
                    return false;
                } else
                {
                    User local = new User
                    {
                        Id = $"{user.Id}",
                        Discriminator = user.Discriminator,
                        Username = user.Username,
                        JoinedAt = user.JoinedAt,
                        Nickname = user.Nickname
                    };

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

        /*Discord member*/
        public static bool AddToDb(SocketUser user)
        {
            try
            {
                if (Exists(user))
                {
                    return false;
                } else
                {
                    User local = new User
                    {
                        Id = $"{user.Id}",
                        Discriminator = user.Discriminator,
                        Username = user.Username
                    };

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

        /*Local member*/
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
                    if (Exists(user))
                    {
                        continue;
                    }
                    else
                    {
                        User local = new User
                        {
                            Id = $"{user.Id}",
                            Discriminator = user.Discriminator,
                            Username = user.Username,
                            JoinedAt = user.JoinedAt,
                            Nickname = user.Nickname
                        };

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
                    if (Exists(user))
                    {
                        continue;
                    }
                    else
                    {
                        User local = new User
                        {
                            Id = $"{user.Id}",
                            Discriminator = user.Discriminator,
                            Username = user.Username
                        };

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
            User temp = GetUser(user);
            db.Users.Remove(temp);
            db.SaveChanges();
        }

        public static void RemoveFromDb(SocketUser user)
        {
            User temp = GetUser(user);
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

        #region Exists

        /*Server member*/
        public static bool Exists(SocketGuildUser user)
        {
            return db.Users.Any(x => ulong.Parse(x.Id) == user.Id);
        }

        /*Discord member*/
        public static bool Exists(SocketUser user)
        {
            return db.Users.Any(x => ulong.Parse(x.Id) == user.Id);
        }

        /*Local member*/
        public static bool Exists(User user)
        {
            return db.Users.Any(x => x.Id == user.Id);
        }

        public static bool Exists(Game game)
        {
            return db.Games.Any(x => x.Name == game.Name);
        }

        #endregion

        #region List (Context.User)

        public static List<Game> ListAll(Game game)
        {
            return db.Games.ToList();
        }

        public static List<Joke> ListAll(Joke joke)
        {
            return db.Jokes.ToList();
        }

        public static List<Meme> ListAll(Meme meme)
        {
            return db.Memes.ToList();
        }

        public static List<Reminder> ListAll(Reminder reminder)
        {
            return db.Reminders.ToList();
        }

        public static List<Notification> ListAll(Notification notification)
        {
            return db.Notifications.ToList();
        }

        #endregion

        #region List (SocketUser)

        public static List<Joke> ListAll(Joke joke, SocketUser user)
        {
            if (user is null)
            {
                return ListAll(joke);
            } else if (!Exists(user))
                AddToDb(user);

            return db.Jokes.Where(x => x.User.Id == $"{user.Id}").ToList();
        }

        public static List<Meme> ListAll(Meme meme, SocketUser user)
        {
            if (user is null)
            {
                return ListAll(meme);
            }
            else if (!Exists(user))
                AddToDb(user);

            return db.Memes.Where(x => x.User.Id == $"{user.Id}").ToList();
        }

        public static List<Reminder> ListAll(Reminder reminder, SocketUser user)
        {
            if (user is null)
            {
                return ListAll(reminder);
            }
            else if (!Exists(user))
                AddToDb(user);

            return db.Reminders.Where(x => x.User.Id == $"{user.Id}").ToList();

        }

        public static List<Notification> ListAll(Notification notification, SocketUser user)
        {
            if (user is null)
            {
                return ListAll(notification);
            } else if (!Exists(user))
                AddToDb(user);

            return db.Notifications.Where(x => x.User.Id == $"{user.Id}").ToList();
        }

        #endregion

        #region GetUser

        public static User GetUser(SocketUser user)
        {
            if (!Exists(user))
                AddToDb(user);
            return db.Users.Where(x => Convert.ToUInt64(x.Id) == user.Id).FirstOrDefault();
        }

        public static User GetUser(SocketGuildUser user)
        {
            if (!Exists(user))
                AddToDb(user);
            return db.Users.Where(x => Convert.ToUInt64(x.Id) == user.Id).FirstOrDefault();
        }

        public static User GetUser(User user)
        {
            if (!Exists(user))
                AddToDb(user);
            return db.Users.Where(x => x.Id == user.Id).FirstOrDefault();
        }

        #endregion
    }
}
