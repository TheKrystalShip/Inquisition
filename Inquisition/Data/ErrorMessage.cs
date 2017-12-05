namespace Inquisition.Data
{
    public class ErrorMessage
    {
        public static string Generic()
        {
            return $"Something went wrong, please let the Admin know about this, thanks";
        }

        public static string DatabaseAccess()
        {
            return $"There was an error while trying to access the database, please let the Admin know about this, thanks";
        }

        public static string UnableToStopGameServer(string game)
        {
            return $"Something went wrong, couldn't stop {game} server, please let the Admin know about this.";
        }

        public static string UnableToStartGameServer(string game)
        {
            return $"Something went wrong, couldn't start {game} server, please let the Admin know about this.";
        }

        public static string GameNotFound(string game)
        {
            return $"Sorry, could't find {game} in the database. Please make sure the name is correctly written.";
        }

        public static string GameNotRunning(string mention, string game)
        {
            return $"{mention}, {game} doesn't seem to be running";
        }

        public static string GameAlreadyRunning(string mention, string game, string version, string port)
        {
            return $"{mention}, {game} server seems to already be running version {version}, on port {port}";
        }
    }
}
