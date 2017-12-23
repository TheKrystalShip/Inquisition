using Discord;
using Discord.WebSocket;
using Inquisition.Data;
using Inquisition.Properties;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Inquisition.Handlers
{
    public class EventHandler
    {
        private DiscordSocketClient Client;
        private SocketTextChannel MembersLogTextChannel;
        private Thread ReminderLoopThread;
        private ulong ChannelId = Convert.ToUInt64(Resources.MembersLogChannel);

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

            MembersLogTextChannel = Client.GetChannel(ChannelId) as SocketTextChannel;
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
                    DbHandler.Insert.User(user);
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
            User BeforeLocalUser = DbHandler.Select.User(BeforeGuildUser);

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
                List<Alert> Alerts =
                    DbHandler.Select.TargetAlerts(0, BeforeLocalUser);

                BeforeLocalUser.LastSeenOnline = DateTimeOffset.UtcNow;

                foreach (Alert n in Alerts)
                {
                    SocketUser AlertAuthor = Client.GetUser(Convert.ToUInt64(n.User.Id));
                    await AlertAuthor.SendMessageAsync($"Notification: {BeforeLocalUser.Username} is now online");
                }
            }
        }

        public void ReminderLoop()
        {
            while (true)
            {
                int SleepTime = 10000;

                List<Reminder> RemindersList = DbHandler.Select.Reminders(10);

                if (RemindersList.Count > 0)
                {
                    SleepTime = 1000;
                }

                foreach (Reminder r in RemindersList)
                {
                    Client.GetUser(Convert.ToUInt64(r.User.Id)).SendMessageAsync($"Reminder: {r.Message}");
                }

                DbHandler.Delete.ReminderList(RemindersList);

                Thread.Sleep(SleepTime);
            }
        }
    }
}
