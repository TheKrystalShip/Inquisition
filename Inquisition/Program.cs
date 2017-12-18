using Discord;
using Discord.WebSocket;
using System;
using System.Threading.Tasks;
using Inquisition.Data;
using System.IO;

namespace Inquisition
{
    class Program
    {
        private DiscordSocketClient Client;
        private CommandHandler Commands;
        private EventHandler EventHandler;
        private string Token;

        public static void Main(string[] args)
            => new Program().MainAsync().GetAwaiter().GetResult();

        public async Task MainAsync()
        {
            using (InquisitionContext db = new InquisitionContext())
            {
                db.Database.EnsureCreated();
            }

            Client = new DiscordSocketClient(new DiscordSocketConfig() {
                LogLevel = LogSeverity.Debug,
            });

            Commands = new CommandHandler(Client);
            EventHandler = new EventHandler(Client);

            try
            {
                using (StreamReader file = new StreamReader("Data/TextFiles/token.txt"))
                {
                    Token = file.ReadLine();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

            await Client.SetGameAsync($"@Inquisition help");
            
            await Client.LoginAsync(TokenType.Bot, Token);
            await Client.StartAsync();

            await Task.Delay(-1);
        }
    }
}