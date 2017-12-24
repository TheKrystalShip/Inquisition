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
        private SocketTextChannel MembersLogChannel;
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

            MembersLogChannel = Client.GetChannel(ChannelId) as SocketTextChannel;
        }

        private Task Log(LogMessage msg)
        {
            Console.WriteLine(msg.ToString());
            return Task.CompletedTask;
        }

        private Task UserJoined(SocketGuildUser guildUser)
        {
            if (!guildUser.IsBot)
            {
                if (!DbHandler.Exists(guildUser))
                {
                    DbHandler.Insert.User(guildUser);
                    return Task.CompletedTask;
                }
            }
            return null;
        }

        private async Task UserBannedAsync(SocketUser user, SocketGuild guild)
        {
            await MembersLogChannel.SendMessageAsync(Reply.Info.UserBanned(user));
        }

        private async Task UserLeftAsync(SocketGuildUser guildUser)
        {
            await MembersLogChannel.SendMessageAsync(Reply.Info.UserLeft(guildUser));
        }

        private async Task OnGuildMemberUpdated(SocketGuildUser beforeGuildUser, SocketGuildUser afterGuildUser)
        {
            User beforeLocalUser = DbHandler.Select.User(beforeGuildUser);

            if (beforeGuildUser.Username != afterGuildUser.Username)
            {
                beforeLocalUser.Username = afterGuildUser.Username;
            }

            if (beforeGuildUser.Nickname != beforeGuildUser.Nickname)
            {
                beforeLocalUser.Nickname = afterGuildUser.Nickname;
            }

            if (beforeGuildUser.GetAvatarUrl() != afterGuildUser.GetAvatarUrl())
            {
                beforeLocalUser.AvatarUrl = afterGuildUser.GetAvatarUrl();
            }

            DbHandler.Save();

            if (beforeGuildUser.Status == UserStatus.Offline && afterGuildUser.Status == UserStatus.Online)
            {
                List<Alert> Alerts =
                    DbHandler.Select.TargetAlerts(beforeLocalUser);

                beforeLocalUser.LastSeenOnline = DateTimeOffset.UtcNow;

                foreach (Alert n in Alerts)
                {
                    SocketUser AlertAuthor = Client.GetUser(Convert.ToUInt64(n.User.Id));
                    await AlertAuthor.SendMessageAsync($"Notification: {beforeLocalUser.Username} is now online");
                }
            }
        }

        public void ReminderLoop()
        {
            while (true)
            {
                int sleepTime = 10000;

                List<Reminder> RemindersList = DbHandler.Select.Reminders(10);

                if (RemindersList.Count > 0)
                {
                    sleepTime = 1000;
                }

                foreach (Reminder r in RemindersList)
                {
                    Client.GetUser(Convert.ToUInt64(r.User.Id)).SendMessageAsync($"Reminder: {r.Message}");
                }

                DbHandler.Delete.ReminderList(RemindersList);

                Thread.Sleep(sleepTime);
            }
        }
    }
}
