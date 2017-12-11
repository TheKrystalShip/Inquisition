using Discord.WebSocket;

namespace Inquisition.Data
{
    public class Message
    {
        public class Error
        {
            public static string Generic = $"Something went wrong, please let the Admin know about this, thanks";
            public static string DatabaseAccess = $"There was an error while trying to access the database, please let the Admin know about this, thanks";
            public static string NoContent(SocketGuildUser user) => $"{user.Username} doesn't have anything in the database";
            public static string NoContent(SocketUser user) => $"{user.Username} doesn't have anything in the database";
            public static string NoContent(User user) => $"{user.Username} doesn't have anything in the database";
            public static string NoContentGeneric = $"You don't have anything in the database";

            #region AlreadyExists

            public static string AlreadyExists(Game game)
            {
                return $"{game.Name} already exists in the database";
            }

            public static string AlreadyExists(User user)
            {
                return $"{user.Username} already exists in the database";
            }

            public static string AlreadyExists(SocketUser user)
            {
                return $"{user.Username} already exists in the database";
            }

            public static string AlreadyExists(SocketGuildUser user)
            {
                return $"{user.Username} already exists in the database";
            }

            #endregion

            #region Game

            public static string GameNotFound(Game game)
            {
                return $"Sorry, could't find {game.Name} in the database. Please make sure the name is correctly written.";
            }

            public static string GameNotRunning(Game game)
            {
                return $"{game.Name} doesn't seem to be running";
            }

            public static string GameAlreadyRunning(Game game)
            {
                return $"{game.Name} server seems to already be running version {game.Version}, on port {game.Port}";
            }

            public static string UnableToStopGameServer(Game game)
            {
                return $"Something went wrong, couldn't stop {game.Name} server, please let the Admin know about this.";
            }

            public static string UnableToStartGameServer(Game game)
            {
                return $"Something went wrong, couldn't start {game.Name} server, please let the Admin know about this.";
            }

            #endregion

            #region Incorrect command structure

            private static string Common = "Incorrect structure, please use: ";

            public static string IncorrectStructure(Game game)
            {
                return Common + "\"[Name]\" \"[Version]\" \"[Port]\"";
            }

            public static string IncorrectStructure(Joke joke)
            {
                return Common + "\"[Joke text]\"";
            }

            public static string IncorrectStructure(Meme meme)
            {
                return Common + "[Url/Link]";
            }

            public static string IncorrectStructure(Reminder reminder)
            {
                return Common + "\"[dd/mm/yyyy hh:mm:ss]\" \"[Message]\"";
            }

            #endregion
        }

        public class Info
        {
            public static string Generic = $"Here you go:";

            #region Successfully Added

            public static string SuccessfullyAdded(string data) => $"Your {data} has been successfully added";
            public static string SuccessfullyAdded(Meme meme) => SuccessfullyAdded("meme");
            public static string SuccessfullyAdded(Joke joke) => SuccessfullyAdded("joke");
            public static string SuccessfullyAdded(Game game) => $"{game.Name} has been successfully added, version {game.Version}, on port {game.Port}";
            public static string SuccessfullyAdded(Reminder reminder) => SuccessfullyAdded("reminder");
            public static string SuccessfullyAdded(SocketGuildUser user) => $"{user} has been successfully added";
            public static string SuccessfullyAdded(Notification notification) => SuccessfullyAdded("notification");

            #endregion

            #region Successfully Removed

            public static string SuccessfullyRemoved(string data) => $"Your {data} has been successfully removed";
            public static string SuccessfullyRemoved(Meme meme) => SuccessfullyRemoved("meme");
            public static string SuccessfullyRemoved(Joke joke) => SuccessfullyRemoved("joke");
            public static string SuccessfullyRemoved(Game game) => SuccessfullyRemoved("game");
            public static string SuccessfullyRemoved(Reminder reminder) => SuccessfullyRemoved("reminder");

            #endregion
            
            #region Game

            public static string GameStartingUp(Game game) => $"{game.Name} server should be online in a few moments, version {game.Version} on port {game.Port}";
            public static string GameShuttingDown(string game) => $"{game} server is shutting down";

            #endregion

            #region Users

            public static string UsersPruned(int users, int days) => $"{users} users were pruned for {days} days of innactivity";
            public static string UserLeft(string user) => $"{user} left the server";
            public static string UserBanned(string user) => $"{user} has been banned";
            public static string UserUnbanned(string user) => $"{user} has been unbanned";

            #endregion
        }
    }
}
