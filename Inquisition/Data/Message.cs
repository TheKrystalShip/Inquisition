using Discord.WebSocket;
using Inquisition.Handlers;

namespace Inquisition.Data
{
    public class Message
    {
        public static string Context(DbHandler.Result result)
        {
            switch (result)
            {
                case DbHandler.Result.Failed:
                    return $"Operation failed";
                case DbHandler.Result.Successful:
                    return $"Operation was successful";
                case DbHandler.Result.AlreadyExists:
                    return $"It already exists";
                case DbHandler.Result.DoesNotExist:
                    return $"It doesn't exist";
                default:
                    return $"";
            }
        }

        public class Error
        {
            public static string Generic = $"Something went wrong, please let the Admin know about this, thanks";
            public static string DatabaseAccess = $"There was an error while trying to access the database, please let the Admin know about this, thanks";
            public static string NotInVoiceChannel = $"You're not in a voice channel";
            public static string TimezoneNotSet = $"You have to set your timezone relative to UTC, use:\n`@Inquisition set timezone n`";
            public static string InvalidDateTime = $"The DateTime specified is not valid";
            public static string NotTheOwner = $"You're not the author";

            #region NoContent
            public static string NoContent(SocketGuildUser user) => $"{user.Username} doesn't have anything in the database";
            public static string NoContent(SocketUser user) => $"{user.Username} doesn't have anything in the database";
            public static string NoContent(User user) => $"{user.Username} doesn't have anything in the database";
            public static string NoContentGeneric = $"You don't have anything in the database";
            #endregion

            #region AlreadyExists
            public static string AlreadyExists(Game game) => $"{game.Name} already exists in the database";
            public static string AlreadyExists(User user) => $"{user.Username} already exists in the database";
            public static string AlreadyExists(SocketUser user) => $"{user.Username} already exists in the database";
            public static string AlreadyExists(SocketGuildUser user) => $"{user.Username} already exists in the database";
            #endregion

            #region Game
            public static string GameNotFound(Game game) => $"Sorry, could't find {game.Name} in the database. Please make sure the name is correctly written.";
            public static string GameNotRunning(Game game) => $"{game.Name} doesn't seem to be running";
            public static string GameAlreadyRunning(Game game) => $"{game.Name} server seems to already be running version {game.Version}, on port {game.Port}";
            public static string UnableToStopGameServer(Game game) => $"Something went wrong, couldn't stop {game.Name} server, please let the Admin know about this.";
            public static string UnableToStartGameServer(Game game) => $"Something went wrong, couldn't start {game.Name} server, please let the Admin know about this.";
            #endregion

            #region Incorrect command structure
            private static string Common = "Incorrect structure, please use: ";
            public static string IncorrectStructure(Game game) => Common + "\"[Name]\"* \"[_Version_]\" \"[_Port_]\"";
            public static string IncorrectStructure(Joke joke) => Common + "\"[Joke text]\"*";
            public static string IncorrectStructure(Meme meme) => Common + "[Url/Link]*";
            public static string IncorrectStructure(Reminder reminder) => Common + "\"[dd/mm/yyyy hh:mm:ss]\"* \"[_Message_]\"";
            public static string IncorrectStructure(Alert alert) => $"[Username]* [_permanent_]";
            public static string IncorrectStructure(Playlist playlist) => $"[playlist]*/[song-name]*";
            #endregion
        }

        public class Info
        {
            public static string Generic = $"Here you go:";
            public static string Timezone(User user) => $"Your timezone is set to UTC+{user.TimezoneOffset}";

            #region Successfully Added
            public static string SuccessfullyAdded(string data) => $"Your {data} has been successfully added";
            public static string SuccessfullyAdded(Meme meme) => SuccessfullyAdded("meme");
            public static string SuccessfullyAdded(Joke joke) => SuccessfullyAdded("joke");
            public static string SuccessfullyAdded(Game game) => $"{game.Name} has been successfully added, version {game.Version}, on port {game.Port}";
            public static string SuccessfullyAdded(Reminder reminder) => SuccessfullyAdded("reminder");
            public static string SuccessfullyAdded(SocketGuildUser user) => $"{user} has been successfully added";
            public static string SuccessfullyAdded(Alert alert) => SuccessfullyAdded("alert");
            public static string SuccessfullyAdded(Playlist playlist) => SuccessfullyAdded("playlist");
            #endregion

            #region Successfully Removed
            public static string SuccessfullyRemoved(string data) => $"Your {data} has been successfully removed";
            public static string SuccessfullyRemoved(Meme meme) => SuccessfullyRemoved("meme");
            public static string SuccessfullyRemoved(Joke joke) => SuccessfullyRemoved("joke");
            public static string SuccessfullyRemoved(Game game) => SuccessfullyRemoved("game");
            public static string SuccessfullyRemoved(Reminder reminder) => SuccessfullyRemoved("reminder");
            public static string SuccessfullyRemoved(Alert notification) => SuccessfullyRemoved("alert");
            public static string SuccessfullyRemoved(Playlist playlist) => SuccessfullyRemoved("playlist");
            public static string SuccessfullyRemoved(Song song) => SuccessfullyRemoved("song");
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
