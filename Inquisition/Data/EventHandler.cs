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

            User beforeLocalUser = DbHandler.GetFromDb(before);
            User afterLocalUser = DbHandler.GetFromDb(after);

            if (beforeLocalUser != afterLocalUser)
            {
                DbHandler.UpdateInDb(afterLocalUser);
            }

            if (before.Username != after.Username)
            {
                DbHandler.GetFromDb(before).Username = after.Username;
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
