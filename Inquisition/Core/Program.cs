using Discord;
using Discord.WebSocket;

using Inquisition.Extensions;
using Inquisition.Handlers;

using Microsoft.Extensions.Configuration;

using System;
using System.IO;
using System.Threading.Tasks;

namespace Inquisition
{
    public class Program
    {
        private static IConfiguration _config;
		private static DiscordSocketClient _client;
		private static CommandHandler _commandHandler;
		private static string _token;

        public static async Task Main(string[] args)
		{
            _config = new ConfigurationBuilder()
                .AddJsonFile(Path.Combine("Properties", "settings.json"), true, true)
                .Build();

            Console.Title = _config.GetName();

            _token = _config.GetToken();
            
            _client = new DiscordSocketClient(new DiscordSocketConfig()
                {
                    LogLevel = LogSeverity.Debug,
                    DefaultRetryMode = RetryMode.AlwaysRetry,
                    ConnectionTimeout = 5000,
                    AlwaysDownloadUsers = true
                }
            );

            _commandHandler = new CommandHandler(ref _client, ref _config);

            await _client.LoginAsync(TokenType.Bot, _token);
            await _client.StartAsync();
            await _client.SetGameAsync("God");

            await Task.Delay(-1);
        }
    }
}
