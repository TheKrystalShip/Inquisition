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
using Inquisition.Services;

/*
 * Required packages for the porject:
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
     * [Bugs]
     * Reminders are only based on UTC 0 timezone, add timezone
     *  field in User model and automate the reminder config upon creation
     *  based on the user's timezone.
     *  Ask the user to specify timezone if field is null in db.
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

        private DiscordSocketClient _client;
        private CommandService _commands;
        private IServiceProvider _services;

        private string token;
        private ulong channel;

        private SocketTextChannel members_log;

        private Thread reminderLoopThread;

        #endregion

        #region Main Execution

        public static void Main(string[] args)
            => new Program().MainAsync().GetAwaiter().GetResult();

        public async Task MainAsync()
        {

            #region Properties assignment

            _client = new DiscordSocketClient();
            _commands = new CommandService();

            _services = new ServiceCollection()
                .AddSingleton(_client)
                .AddSingleton(_commands)
                .AddSingleton(new AudioService())
                .BuildServiceProvider();

            #endregion

            #region FileReader

            try
            {
                System.IO.StreamReader file = new System.IO.StreamReader("Data/TextFiles/token.txt");
                token = file.ReadLine();

                file = new System.IO.StreamReader("Data/TextFiles/channel.txt");
                channel = ulong.Parse(file.ReadLine());

                members_log = _client.GetChannel(channel) as SocketTextChannel;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }

            #endregion

            #region Events

            _client.Log += Log;
            _client.UserJoined += UserJoined;
            _client.UserLeft += UserLeftAsync;
            _client.UserBanned += UserBannedAsync;
            _client.GuildMemberUpdated += OnGuildMemberUpdated;
            await _client.SetGameAsync($"@Inquisition help");

            #endregion

            await RegisterCommandsAsync();
            HelpModule.Create(_commands);

            await _client.LoginAsync(TokenType.Bot, token);
            await _client.StartAsync();

            #region Reminder loop thread

            reminderLoopThread = new Thread(ReminderLoop);
            reminderLoopThread.IsBackground = true;
            reminderLoopThread.Start();

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

        private async Task UserBannedAsync(SocketUser arg1, SocketGuild arg2)
        {
            await members_log.SendMessageAsync(Message.Info.UserBanned(arg1.Mention));
        }

        private async Task UserLeftAsync(SocketGuildUser arg)
        {
            await members_log.SendMessageAsync(Message.Info.UserLeft(arg.Mention));
        }

        private async Task OnGuildMemberUpdated(SocketGuildUser before, SocketGuildUser after)
        {
            if (before.Status == UserStatus.Offline && after.Status == UserStatus.Online)
            {
                List<Notification> nList = DbHandler.ListAll(new Notification());
                List<Notification> finished = new List<Notification>();
                User target = DbHandler.GetFromDb(after);

                DbHandler.GetFromDb(after).LastSeenOnline = DateTimeOffset.UtcNow;
                foreach (var n in nList)
                {
                    if (n.TargetUser == target && _client.GetUser(Convert.ToUInt64(n.User.Id)).Status == UserStatus.Online)
                    {
                        SocketUser socketUser = _client.GetUser(Convert.ToUInt64(n.User.Id));
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

            if (before.Nickname != before.Nickname)
            {
                DbHandler.GetFromDb(before).Nickname = after.Nickname;
            }

            if (before.GetAvatarUrl() != after.GetAvatarUrl())
            {
                DbHandler.GetFromDb(before).AvatarUrl = after.GetAvatarUrl();
            }

            DbHandler.Save();
        }

        #endregion

        #region CommandHandling

        private async Task RegisterCommandsAsync()
        {
            _client.MessageReceived += HandleCommands;

            await _commands.AddModulesAsync(Assembly.GetEntryAssembly());
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

            if (message.HasMentionPrefix(_client.CurrentUser, ref argPos) ||
                message.HasStringPrefix(prefix, ref argPos) ||
                message.Channel.GetType() == typeof(SocketDMChannel))
            {
                Console.WriteLine($"{DateTimeOffset.UtcNow} - {message.Author}: {message.Content}");

                SocketCommandContext context = new SocketCommandContext(_client, message);
                IResult result = await _commands.ExecuteAsync(context, argPos, _services);

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
            List<Reminder> Reminders;

            while (true)
            {
                Reminders = DbHandler.ListAll(new Reminder());
                List<Reminder> FinishedReminders = new List<Reminder>();

                foreach (Reminder r in Reminders)
                {
                    if (DateTimeOffset.UtcNow >= r.DueDate)
                    {
                        _client
                            .GetUser(Convert.ToUInt64(r.User.Id))
                            .SendMessageAsync($"Reminder: {r.Message}");
                        FinishedReminders.Add(r);
                    }
                }

                if (FinishedReminders.Count != 0)
                {
                    DbHandler.RemoveRangeFromDb(FinishedReminders);
                }

                Thread.Sleep(5000);
            }
        }

        #endregion
    }
}
