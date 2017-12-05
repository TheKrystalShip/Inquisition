using Discord;
using Discord.Commands;
using Discord.WebSocket;
using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using System.Collections.Generic;
using Inquisition.Data;

/*
 * Required packages for the porject:
 *  1. Discord.Net v1.0.2
 *  2. Discord.Net.Commands v1.0.2
 *  3. Discord.Net.Rest v1.0.2
 *  4. Discord.Net.WebSocket v1.0.2
 *  5. Microsoft.EntityFrameworkCore.SqlServer
 *  6. Microsoft.EntityFrameworkCore.Tools
 */

namespace Inquisition
{
    /*
     * TODO: 
     * 
     * [EndGoal]
     * Track member activity and store it in db, removing inactive members
     *  regardless of role. Some exceptions can be set in place.
     *  This can be done by tracking user activity on the server using
     *  (UserIsTyping && UserVoiceStateUpdated) events to track time between
     *  activities, increasing a counter on the db ofr every 24h of inactivity.
     *  
     */

    class Program
    {
        private DiscordSocketClient _client;
        private CommandService _commands;
        private IServiceProvider _services;

        private string token;
        private ulong channel;

#region Main Execution

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
                System.IO.StreamReader file = new System.IO.StreamReader("Data/t_token.txt");
                token = file.ReadLine();

                file = new System.IO.StreamReader("Data/t_channel.txt");
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
            _client.UserUpdated += UserUpdated;
            
            await RegisterCommandsAsync();

            /* 
             * Uncomment and call this method ONCE to populate the database with the games info.
             * Once the database is populated, this method can be either commented or completly 
             * removed to avoid creating duplicate information in the database since this is done
             * at runtime everytime the orgram starts.
             */
            // await PopulateDbAsync();

            await _client.LoginAsync(TokenType.Bot, token);
            await _client.StartAsync();
            
            await Task.Delay(-1);
        }

#endregion
#region Listeners

        private async Task UserUpdated(SocketUser arg1, SocketUser arg2)
        {
            if (arg1.Status == UserStatus.Offline && arg2.Status == UserStatus.Online)
            {
                // Does fuck all for now.
            }
        }

        private async Task UserBannedAsync(SocketUser arg1, SocketGuild arg2)
        {
            await arg2.GetTextChannel(channel).SendMessageAsync(InfoMessage.UserBanned(arg1.Mention));
        }

        private async Task UserLeftAsync(SocketGuildUser arg)
        {
            await arg.Guild.GetTextChannel(channel).SendMessageAsync(InfoMessage.UserLeft(arg.Mention));
        }

        private async Task RegisterCommandsAsync()
        {
            _client.MessageReceived += HandleCommands;

            await _commands.AddModulesAsync(Assembly.GetEntryAssembly());
        }

#endregion
#region CommandHandling

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
                    Console.WriteLine($"{DateTime.Now.Minute}:{DateTime.Now.Second} - {result.ErrorReason}");
                }
            }
        }

#endregion
#region Logging data

        private Task Log(LogMessage msg)
        {
            Console.WriteLine(msg.ToString());
            return Task.CompletedTask;
        }

#endregion
#region Database data

        private async Task PopulateDbAsync()
        {
            List<Data.Game> Games = new List<Data.Game>
            {
                new Data.Game { Name = "Space Engineers", Port = "3080", Version = "?" },
                new Data.Game { Name = "StarMade", Port = "3070", Version = ".654" },
                new Data.Game { Name = "Project Zomboid", Port = "3050", Version = "37.14" },
                new Data.Game { Name = "Starbound", Port = "3040", Version = "1.2.2" },
                new Data.Game { Name = "Terraria", Port = "3030", Version = "1.3.5.3" },
                new Data.Game { Name = "Factorio", Port = "3020", Version = "15.18" },
                new Data.Game { Name = "7 Days to die", Port = "3010", Version = "16 b138" },
                new Data.Game { Name = "GMod - Sandbox", Port = "3003", Version = "?" },
                new Data.Game { Name = "GMod - Murder", Port = "3000", Version = "?" }
            };

            InquisitionContext db = new InquisitionContext();
            await db.Games.AddRangeAsync(Games);
            await db.SaveChangesAsync();
        }

#endregion
    }
}
