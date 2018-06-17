using Discord;
using Discord.WebSocket;

using Inquisition.Handlers;
using Inquisition.Properties;

using System;
using System.Threading.Tasks;

namespace Inquisition
{
    public class Program
    {
		private static string _token;
		private static DiscordSocketClient _client;
		private static CommandHandler _commandHandler;

        public static async Task Main(string[] args)
		{
            Console.Title = "Inquisition";

            _token = BotInfo.Token;

			_client = new DiscordSocketClient(new DiscordSocketConfig()
				{
					LogLevel = LogSeverity.Debug,
					DefaultRetryMode = RetryMode.AlwaysRetry,
					ConnectionTimeout = 5000,
					AlwaysDownloadUsers = true
				}
			);

			_commandHandler = new CommandHandler(_client);

            await _client.LoginAsync(TokenType.Bot, _token);
            await _client.StartAsync();
            await _client.SetGameAsync($"God");

            await Task.Delay(-1);
        }
    }
}
