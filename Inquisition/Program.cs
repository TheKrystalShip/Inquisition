using Discord;
using Discord.Commands;
using Discord.WebSocket;
using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Inquisition
{
    class Program
    {
        private DiscordSocketClient _client;
        private CommandService _commands;
        private IServiceProvider _services;

        private string token;
        private ulong channel;

        public static void Main(string[] args)
            => new Program().MainAsync().GetAwaiter().GetResult();

        public async Task MainAsync()
        {
            _client = new DiscordSocketClient();
            _commands = new CommandService();

            _services = new ServiceCollection()
                .AddSingleton(_client)
                .AddSingleton(_commands)
                .BuildServiceProvider();

            try
            {
                System.IO.StreamReader file = new System.IO.StreamReader("token.txt");
                token = file.ReadLine();

                file = new System.IO.StreamReader("channel.txt");
                channel = ulong.Parse(file.ReadLine());
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }

            _client.Log += Log;
            _client.UserLeft += UserLeftAsync;
            _client.UserBanned += UserBannedAsync;

            await RegisterCommandsAsync();

            await _client.LoginAsync(TokenType.Bot, token);
            await _client.StartAsync();
            
            await Task.Delay(-1);
        }

        private async Task UserBannedAsync(SocketUser arg1, SocketGuild arg2)
        {
            await arg2.GetTextChannel(channel).SendMessageAsync(arg1.Mention + " was banned from the server.");
        }

        private async Task UserLeftAsync(SocketGuildUser arg)
        {
            await arg.Guild.GetTextChannel(channel).SendMessageAsync(arg.Mention + " left the server.");
        }

        private async Task RegisterCommandsAsync()
        {
            _client.MessageReceived += HandleCommands;

            await _commands.AddModulesAsync(Assembly.GetEntryAssembly());
        }

        private async Task HandleCommands(SocketMessage arg)
        {
            var message = arg as SocketUserMessage;

            if (message is null || message.Author.IsBot) return;

            int argPos = 0;

            if(message.HasMentionPrefix(_client.CurrentUser, ref argPos))
            {
                var context = new SocketCommandContext(_client, message);

                var result = await _commands.ExecuteAsync(context, argPos, _services);

                if (!result.IsSuccess)
                {
                    Console.WriteLine(result.ErrorReason);
                }
            }
        }

        private Task Log(LogMessage msg)
        {
            Console.WriteLine(msg.ToString());
            return Task.CompletedTask;
        }
    }
}
