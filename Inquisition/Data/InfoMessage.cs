namespace Inquisition.Data
{
    public class InfoMessage
    {
#region Successfully Added
        public static string SuccessfullyAdded(string data)
        {
            return $"Your {data} has been successfully added";
        }

        public static string SuccessfullyAdded(Meme meme)
        {
            return SuccessfullyAdded("meme");
        }

        public static string SuccessfullyAdded(Joke joke)
        {
            return SuccessfullyAdded("joke");
        }

        public static string SuccessfullyAdded(Game game)
        {
            return $"{game.Name} has been successfully added, version {game.Version}, on port {game.Port}";
        }

        public static string SuccessfullyAdded(Reminder reminder)
        {
            return SuccessfullyAdded("reminder");
        }

#endregion
#region Successfully Removed
        public static string SuccessfullyRemoved(string data)
        {
            return $"Your {data} has been successfully removed";
        }

        public static string SuccessfullyRemoved(Meme meme)
        {
            return SuccessfullyRemoved("meme");
        }

        public static string SuccessfullyRemoved(Joke joke)
        {
            return SuccessfullyRemoved("joke");
        }

        public static string SuccessfullyRemoved(Game game)
        {
            return SuccessfullyRemoved("game");
        }

        public static string SuccessfullyRemoved(Reminder reminder)
        {
            return SuccessfullyRemoved("reminder");
        }

#endregion
#region Game
        public static string GameStartingUp(string mention, string game, string version, string port)
        {
            return $"{mention}, {game} server should be online in a few moments, version {version} on port {port}";
        }

        public static string GameShuttingDown(string game)
        {
            return $"{game} server is shutting down";
        }
#endregion
#region Users
        public static string UsersPruned(int users, int days)
        {
            return $"{users} were pruned for inactivity in the last {days} days";
        }

        public static string UserLeft(string user)
        {
            return $"{user} left the server";
        }

        public static string UserBanned(string user)
        {
            return $"{user} has been banned";
        }

        public static string UserUnbanned(string user)
        {
            return $"{user} has been unbanned";
        }
#endregion
    }
}
