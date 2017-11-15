using Discord;
using Discord.Commands;
using Discord.WebSocket;
using System;
using System.Threading.Tasks;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;

namespace Inquisition
{
    class Program
    {

        private CommandService CommandService;
        private DiscordSocketClient DiscordSocketClient;
        private IServiceProvider ServiceProvider;
        

        private string token = "MzA0MzUzMTIyMDE5NzA0ODQy.DOdzuQ.y-fp-cRx4gno1bhyEjna6YOadeo";

        public static void Main(string[] args)
                    => 
            new Program().MainAsync().GetAwaiter().GetResult();

        public async Task MainAsync()
        {
            DiscordSocketClient = new DiscordSocketClient();

            DiscordSocketClient.Log += Log;
            DiscordSocketClient.MessageReceived += MessageReceived;

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
            switch (message.Content)
            {
                case "!assface":
                    await message.Channel.SendMessageAsync("You're an assface, " + message.Author.Mention);
                    break;

                case "!prune":
                    break;

                case "!help":
                    await message.Author.SendMessageAsync("!assface, !help, !commands, !prune");
                    break;

                case "!commands":
                    await message.Channel.SendMessageAsync("I don't do much yet");
                    break;
            }
        }
    }
}
