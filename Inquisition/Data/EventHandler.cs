using Discord;
using Discord.WebSocket;
using Inquisition.Data;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Inquisition
{
    public class EventHandler
    {
        private DiscordSocketClient Client;
        private SocketTextChannel MembersLogTextChannel;
        private Thread ReminderLoopThread;
        private ulong ChannelId;

        public EventHandler(DiscordSocketClient client)
        {
            Client = client;

            Client.Log += Log;
            Client.UserJoined += UserJoined;
            Client.UserLeft += UserLeftAsync;
            Client.UserBanned += UserBannedAsync;
            Client.GuildMemberUpdated += OnGuildMemberUpdated;

            ReminderLoopThread = new Thread(ReminderLoop);
            ReminderLoopThread.IsBackground = true;
            ReminderLoopThread.Start();

            try
            {
                using (StreamReader file = new StreamReader("Data/TextFiles/channel.txt"))
                {
                    ChannelId = ulong.Parse(file.ReadLine());
                    MembersLogTextChannel = Client.GetChannel(ChannelId) as SocketTextChannel;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private Task Log(LogMessage msg)
        {
            Console.WriteLine(msg.ToString());
            return Task.CompletedTask;
        }

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
            await MembersLogTextChannel.SendMessageAsync(Message.Info.UserBanned(arg1.Mention));
        }

        private async Task UserLeftAsync(SocketGuildUser arg)
        {
            await MembersLogTextChannel.SendMessageAsync(Message.Info.UserLeft(arg.Mention));
        }

        private async Task OnGuildMemberUpdated(SocketGuildUser BeforeGuildUser, SocketGuildUser AfterGuildUser)
        {
            User BeforeLocalUser = DbHandler.GetFromDb(BeforeGuildUser);

            if (BeforeGuildUser.Username != AfterGuildUser.Username)
            {
                BeforeLocalUser.Username = AfterGuildUser.Username;
            }

            if (BeforeGuildUser.Nickname != BeforeGuildUser.Nickname)
            {
                BeforeLocalUser.Nickname = AfterGuildUser.Nickname;
            }

            if (BeforeGuildUser.GetAvatarUrl() != AfterGuildUser.GetAvatarUrl())
            {
                BeforeLocalUser.AvatarUrl = AfterGuildUser.GetAvatarUrl();
            }

            DbHandler.Save();

            if (BeforeGuildUser.Status == UserStatus.Offline && AfterGuildUser.Status == UserStatus.Online)
            {
                List<Alert> Notifications = 
                    DbHandler.ListAllTargetAlerts(new Alert(), BeforeLocalUser);

                BeforeLocalUser.LastSeenOnline = DateTimeOffset.UtcNow;

                foreach (Alert n in Notifications)
                {
                    SocketUser NotificationAuthor = Client.GetUser(Convert.ToUInt64(n.User.Id));
                    await NotificationAuthor.SendMessageAsync($"Notification: {BeforeLocalUser.Username} is now online");
                }
            }
        }

        public void ReminderLoop()
        {
            while (true)
            {
                int SleepTime = 10000;

                List<Reminder> RemindersList = DbHandler.ListLastTen(new Reminder());

                if (RemindersList.Count > 0)
                {
                    SleepTime = 1000;
                }

                foreach (Reminder r in RemindersList)
                {
                    Client.GetUser(Convert.ToUInt64(r.User.Id)).SendMessageAsync($"Reminder: {r.Message}");
                }

                DbHandler.RemoveRangeFromDb(RemindersList);

                Thread.Sleep(SleepTime);
            }
        }
    }
}
