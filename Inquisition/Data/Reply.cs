using Discord.WebSocket;

namespace Inquisition.Data
{
    public class Reply
    {
        public static string Generic = $"Here you go:";

        public static string Context(Result result)
        {
            switch (result)
            {
                case Result.Failed:
                    return $"Operation failed";
                case Result.Successful:
                    return $"Operation was successful";
                case Result.AlreadyExists:
                    return $"It already exists";
                case Result.DoesNotExist:
                    return $"It doesn't exist";
                case Result.AlreadyRunning:
                    return "Server is already running";
                case Result.Offline:
                    return "Server is offline";
                case Result.Online:
                    return "Server is online";
                case Result.ProcessRunningButOfflineInDb:
                    return "Server has a process running, but is marked as offline";
                case Result.ProcessNotRunningButOnlineInDb:
                    return $"Server is not running, but is marked as online";
                case Result.GenericError:
                    return "Generic error";
                default:
                    return "";
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
            
            public static string NoContent(SocketGuildUser user) => $"{user.Username} doesn't have anything in the database";
            public static string NoContent(SocketUser user) => $"{user.Username} doesn't have anything in the database";
            public static string NoContent(User user) => $"{user.Username} doesn't have anything in the database";
            public static string NoContentGeneric = $"You don't have anything in the database";

            public static string GameNotRunning(Game game) => $"{game.Name} doesn't seem to be running";
            public static string GameAlreadyRunning(Game game) => $"{game.Name} server seems to already be running version {game.Version}, on port {game.Port}";
            public static string UnableToStopGameServer(Game game) => $"Something went wrong, couldn't stop {game.Name} server, please let the Admin know about this.";
            public static string UnableToStartGameServer(Game game) => $"Something went wrong, couldn't start {game.Name} server, please let the Admin know about this.";

            public struct Command
            {
                private static string Common(string data) => $"Incorrect command structure, use: {data}";

                public static string Game = Common("\"Name (Terraria)\" \"Version (1.3.4)\" \"Port (3030)\"");
                public static string Joke = Common("Text");
                public static string Meme = Common("Url/Link");
                public static string Reminder = Common("\"dd/mm/yyyy hh:mm:ss\" \"Message\"");
                public static string Alert = Common("User");
                public static string Playlist = Common("Name");
            }

            public struct NotFound
            {
                private static string Common(string data) => $"{data} not found";

                public static string Game = Common("Game");
                public static string Joke = Common("Joke");
                public static string Meme = Common("Meme");
                public static string Reminder = Common("Reminder");
                public static string User = Common("User");
                public static string Alert = Common("Alert");
                public static string Playlist = Common("Playlist");
                public static string Song = Common("Song");
            }

            public struct Duplicate
            {
                private static string Common(string data) => $"{data} already exists";

                public static string Game = Common("Game");
                public static string Joke = Common("Joke");
                public static string Meme = Common("Meme");
                public static string Reminder = Common("Reminder");
                public static string User = Common("User");
                public static string Alert = Common("Alert");
                public static string Playlist = Common("Playlist");
                public static string Song = Common("Song");
            }
        }

        public class Info
        {
            public static string GameStarting(Game game) => $"{game.Name} is starting up";
            public static string GameStopping(Game game) => $"{game.Name} is stopping";

            public static string UsersPruned(int users, int days) => $"{users} users were pruned for innactivity in the last {days} days";
            public static string UserLeft(SocketGuildUser user) => $"{user.Username} left the server";
            public static string UserLeft(SocketUser user) => $"{user.Username} left the server";
            public static string UserLeft(User user) => $"{user.Username} left the server";
            public static string UserBanned(SocketGuildUser user) => $"{user.Username} has been banned";
            public static string UserBanned(SocketUser user) => $"{user.Username} has been banned";
            public static string UserBanned(User user) => $"{user.Username} has been banned";
            public static string UserUnbanned(User user) => $"{user.Username} has been unbanned";
            public static string UserTimezone(User user) => $"Your timezone is set to UTC+{user.TimezoneOffset}";
            
            public struct Added
            {
                private static string Common(string data) => $"{data} was successfully added";

                public static string Game = Common("Game");
                public static string Joke = Common("Joke");
                public static string Meme = Common("Meme");
                public static string Reminder = Common("Reminder");
                public static string User = Common("User");
                public static string Alert = Common("Alert");
                public static string Playlist = Common("Playlist");
                public static string Song = Common("Song");
            }

            public struct Removed
            {
                private static string Common(string data) => $"{data} was successfully removed";

                public static string Game = Common("Game");
                public static string Joke = Common("Joke");
                public static string Meme = Common("Meme");
                public static string Reminder = Common("Reminder");
                public static string User = Common("User");
                public static string Alert = Common("Alert");
                public static string Playlist = Common("Playlist");
                public static string Song = Common("Song");
            }
        }
    }
}
