using Discord;
using Discord.Commands;
using Discord.WebSocket;
using System;
using System.Threading.Tasks;

namespace Inquisition
{
    class Program
    {

        // private CommandService CommandService;
        private DiscordSocketClient DiscordSocketClient;
        // private IServiceProvider ServiceProvider;
        private string token;

        public async Task MainAsync()
        {
            DiscordSocketClient = new DiscordSocketClient();

            DiscordSocketClient.Log += Log;
            DiscordSocketClient.UserLeft += UserLeft;
            DiscordSocketClient.UserBanned += UserBanned;

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

        private async Task UserBanned(SocketUser arg1, SocketGuild arg2)
        {
            ulong channel;
            try
            {
                System.IO.StreamReader file = new System.IO.StreamReader("channel.txt");
                channel = ulong.Parse(file.ReadLine());
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }

            await arg2.GetTextChannel(channel).SendMessageAsync(arg1.Mention + " was banned from the server.");
        }

        private async Task UserLeft(SocketGuildUser arg)
        {
            ulong channel;
            try
            {
                System.IO.StreamReader file = new System.IO.StreamReader("channel.txt");
                channel = ulong.Parse(file.ReadLine());
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }

            await arg.Guild.GetTextChannel(channel).SendMessageAsync(arg.Mention + " left the server.");
        }

        private Task Log(LogMessage msg)
        {
            Console.WriteLine(msg.ToString());
            return Task.CompletedTask;
        }
    }
}
