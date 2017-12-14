using Discord;
using Discord.Commands;
using Discord.WebSocket;
using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using System.Collections.Generic;
using Inquisition.Data;
using System.Threading;
using Inquisition.Modules;

/*
 * Required packages for the porject just in case they don't automatically download:
 *  1. Discord.Net v1.0.2
 *  2. Discord.Net.Commands v1.0.2
 *  3. Discord.Net.WebSocket v1.0.2
 *  4. Microsoft.EntityFrameworkCore.SqlServer
 *  5. Microsoft.EntityFrameworkCore.Tools
 */

namespace Inquisition
{
    /*
     * TODO: 
     * 
     * [Developer branch]
     * Make Inquisition play music, create and save playlists
     *  in the database.
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
        #region Properties

        private DiscordSocketClient Client;
        private CommandService Commands;
        private IServiceProvider Services;

        private string Token;
        private ulong ChannelId;

        private SocketTextChannel MembersLogTextChannel;

        private Thread ReminderLoopThread;

        #endregion

        #region Main Execution

        public static void Main(string[] args) => new Program().RunAsync().GetAwaiter().GetResult();

        public async Task RunAsync()
        {
            using (InquisitionContext db = new InquisitionContext())
            {
                db.Database.EnsureCreated();
            }

            #region Properties assignment

            Client = new DiscordSocketClient();
            Commands = new CommandService();

            Services = new ServiceCollection()
                .AddSingleton(Client)
                .AddSingleton(Commands)
                .BuildServiceProvider();

            #endregion

            #region FileReader

            try
            {
                System.IO.StreamReader file = new System.IO.StreamReader("Data/TextFiles/token.txt");
                Token = file.ReadLine();

                file = new System.IO.StreamReader("Data/TextFiles/channel.txt");
                ChannelId = ulong.Parse(file.ReadLine());

                MembersLogTextChannel = Client.GetChannel(ChannelId) as SocketTextChannel;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }

            #endregion

            #region Events

            Client.Log += Log;
            Client.UserJoined += UserJoined;
            Client.UserLeft += UserLeftAsync;
            Client.UserBanned += UserBannedAsync;
            Client.GuildMemberUpdated += OnGuildMemberUpdated;
            await Client.SetGameAsync($"@Inquisition help");

            #endregion

            await Client.SetGameAsync($"@Inquisition help");
            await RegisterCommandsAsync();
            HelpModule.Create(Commands);

            #region Login

            try
            {
                await Client.LoginAsync(TokenType.Bot, Token);
                await Client.StartAsync();
            }
            catch (Exception e)
            {
                Console.WriteLine($"{DateTimeOffset.Now} - CONNECTION FAILED");
                Console.WriteLine(e);
            }

            #endregion

            #region Reminder loop thread

            try
            {
                ReminderLoopThread = new Thread(ReminderLoop)
                {
                    IsBackground = true
                };
                ReminderLoopThread.Start();
            }
            catch (Exception e)
            {
                Console.WriteLine($"{DateTimeOffset.Now} - FAILED TO CREATE REMINDER LOOP THREAD");
                Console.WriteLine(e);
            }

            #endregion

            await Task.Delay(-1);
        }

        #endregion

        #region Events

        private Task UserJoined(SocketGuildUser user)
        {
            if (!user.IsBot)
            {
                if (!DbHandler.Exists(user))
                {
                    DbHandler.AddToDb(user);
                    return Task.CompletedTask;
                }
            }
            return null;
        }

        private async Task UserBannedAsync(SocketUser user, SocketGuild guild)
        {
            await MembersLogTextChannel.SendMessageAsync(Message.Info.UserBanned(user.Mention));
        }

        private async Task UserLeftAsync(SocketGuildUser user)
        {
            await MembersLogTextChannel.SendMessageAsync(Message.Info.UserLeft(user.Mention));
        }

        private async Task OnGuildMemberUpdated(SocketGuildUser userBefore, SocketGuildUser userAfter)
        {
            if (userBefore.Status == UserStatus.Offline && userAfter.Status == UserStatus.Online)
            {
                List<Notification> nList = DbHandler.ListAll(new Notification());
                List<Notification> finished = new List<Notification>();
                User target = DbHandler.GetFromDb(userAfter);

                DbHandler.GetFromDb(userAfter).LastSeenOnline = DateTimeOffset.UtcNow;
                foreach (var n in nList)
                {
                    if (n.TargetUser == target && Client.GetUser(Convert.ToUInt64(n.User.Id)).Status == UserStatus.Online)
                    {
                        SocketUser socketUser = Client.GetUser(Convert.ToUInt64(n.User.Id));
                        await socketUser.SendMessageAsync($"Notification: {target.Username} is now online");
                        if (!n.IsPermanent)
                        {
                            finished.Add(n); 
                        }
                    }
                }

                if (finished.Count > 0)
                {
                    DbHandler.RemoveRangeFromDb(finished);
                }
            }

            if (userBefore.Nickname != userAfter.Nickname)
            {
                DbHandler.GetFromDb(userBefore).Nickname = userAfter.Nickname;
            }

            if (userBefore.GetAvatarUrl() != userAfter.GetAvatarUrl())
            {
                DbHandler.GetFromDb(userBefore).AvatarUrl = userAfter.GetAvatarUrl();
            }

            DbHandler.Save();
        }

        #endregion

        #region CommandHandling

        private async Task RegisterCommandsAsync()
        {
            Client.MessageReceived += HandleCommands;

            await Commands.AddModulesAsync(Assembly.GetEntryAssembly());
        }

        private async Task HandleCommands(SocketMessage msg)
        {
            var message = msg as SocketUserMessage;

            if (message is null || message.Author.IsBot) return;

            if (!DbHandler.Exists(msg.Author))
            {
                DbHandler.AddToDb(msg.Author);
            }

            string prefix = "rip ";

            int argPos = 0;

            if (message.HasMentionPrefix(Client.CurrentUser, ref argPos) ||
                message.HasStringPrefix(prefix, ref argPos) ||
                message.Channel.GetType() == typeof(SocketDMChannel))
            {
                Console.WriteLine($"{DateTimeOffset.UtcNow} - {message.Author}: {message.Content}");

                SocketCommandContext context = new SocketCommandContext(Client, message);
                IResult result = await Commands.ExecuteAsync(context, argPos, Services);

                if (!result.IsSuccess)
                {
                    Console.WriteLine($"{DateTimeOffset.UtcNow} - {result.ErrorReason}");
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

        #region Reminder loop thread

        public void ReminderLoop()
        {
            while (true)
            {
                List<Reminder> RemindersList = DbHandler.ListAll(new Reminder());

                foreach (Reminder r in RemindersList)
                {
                    Client.GetUser(Convert.ToUInt64(r.User.Id)).SendMessageAsync($"Reminder: {r.Message}");
                }

                DbHandler.RemoveRangeFromDb(RemindersList);

                Thread.Sleep(5000);
            }
        }

        #endregion
    }
}