using Discord;
using Discord.Commands;
using Discord.WebSocket;
using System;
using System.Threading.Tasks;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;

namespace Inquisition
{
    class Program
    {

        private CommandService CommandService;
        private DiscordSocketClient DiscordSocketClient;
        private IServiceProvider ServiceProvider;
        private SocketUser SocketUser;

        public static void Main(string[] args)
                    => 
            new Program().MainAsync().GetAwaiter().GetResult();

        public async Task MainAsync()
        {
            DiscordSocketClient = new DiscordSocketClient();

            DiscordSocketClient.Log += Log;
            DiscordSocketClient.MessageReceived += MessageReceived;

            string token;
            try
            {
                System.IO.StreamReader file = new System.IO.StreamReader("token.txt");
                token = file.ReadLine();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }

            await DiscordSocketClient.LoginAsync(TokenType.Bot, token);
            await DiscordSocketClient.StartAsync();

            // Block this task until the program is closed.
            await Task.Delay(-1);
        }

        private Task Log(LogMessage msg)
        {
            Console.WriteLine(msg.ToString());
            return Task.CompletedTask;
        }

        private async Task MessageReceived(SocketMessage message)
        {
            var channel = message.Channel;

            switch (message.Content)
            {
                case "!hello":
                    await message.Channel.SendMessageAsync("Hello " + message.Author.Mention);
                    break;
            }
        }
    }
}
