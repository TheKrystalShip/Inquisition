using Discord;
using Discord.Commands;
using Discord.WebSocket;

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using System.Threading.Tasks;

using TheKrystalShip.Inquisition.Handlers;

namespace TheKrystalShip.Inquisition.Modules
{
    [RequireUserPermission(GuildPermission.Administrator)]
    public class AdminModule : Module
    {
        public AdminModule(Tools tools) : base(tools)
        {

        }

        [Command("prune")]
        [Alias("purge")]
        [Summary("[Admin] Prunes all inactive members from the server")]
        public async Task PruneMembersAsync(int days)
        {
            if (days < 7)
            {
                await ReplyAsync("Minimum is 7 days of innactivity");
                return;
            }

            int members = await Context.Guild.PruneUsersAsync(days);
            await ReplyAsync(ReplyHandler.Info.UsersPruned(members, days));
        }

        [Command("ban")]
        [Summary("[Admin] Bans a user from the server")]
        public async Task BanMemberAsync(SocketGuildUser user, [Remainder] string reason = "")
        {
            await user.SendMessageAsync($"You've been banned from {Context.Guild}, reason: {reason}.");
            await Context.Guild.AddBanAsync(user, 0, reason);
            await ReplyAsync(ReplyHandler.Info.UserBanned(user));
        }

        [Command("wipe")]
        [Alias("wipe last", "wipe the last")]
        [Summary("[Admin] Wipes X number of messages from a text channel")]
        public async Task WipeChannelAsync(uint amount = 1, [Remainder] string s = "")
        {
            IEnumerable<IMessage> messages = await Context.Channel
                .GetMessagesAsync((int)amount + 1)
                .Flatten();

            await Context.Channel
                .DeleteMessagesAsync(messages);

            const int delay = 5000;
            IUserMessage reply = await ReplyAsync($"Deleted {amount} messages. _This message will be deleted in {delay / 1000} seconds._");

            await Task.Delay(delay);
            await reply.DeleteAsync();
        }

        [Command("hello there")]
        public async Task TestCommandAsync()
        {
            await ReplyAsync($"General {User.Username} ⚔️⚔️");
        }

        [Command("restart")]
        [Alias("reboot")]
        public async Task RebootAsync()
        {
            await ReplyAsync("Rebooting...");

            Process.Start(new ProcessStartInfo()
                {
                    FileName = "dotnet",
                    Arguments = "\"" + Assembly.GetExecutingAssembly().Location + "\"",
                    WindowStyle = ProcessWindowStyle.Normal,
                    CreateNoWindow = false
                }
            );

            Environment.Exit(0);
        }

        [Command("shutdown")]
        [Alias("quit")]
        public async Task Shutdown()
        {
            await ReplyAsync("Shutting down...");
            Environment.Exit(0);
        }
    }

    [Group("stop")]
    public class StopAdminModule : Module
    {
        private readonly ServiceHandler _serviceHandler;

        public StopAdminModule(ServiceHandler serviceHandler, Tools tools) : base(tools)
        {
            _serviceHandler = serviceHandler;
        }

        [Command("loops")]
        [Alias("all loops")]
        public async Task StopAllLoopsAsync()
        {
            _serviceHandler.StopAllLoops();
            await ReplyAsync("All loops stopped");
            Tools.Logger.LogInformation("Stopped all loops");
        }
    }

    [Group("start")]
    public class StartAdminModule : Module
    {
        private readonly ServiceHandler _serviceHandler;

        public StartAdminModule(ServiceHandler serviceHandler, Tools tools) : base(tools)
        {
            _serviceHandler = serviceHandler;
        }

        [Command("loops")]
        [Alias("all loops")]
        public async Task StartAllLoopsAsync()
        {
            _serviceHandler.StartAllLoops();
            await ReplyAsync("All loops started");
        }
    }
}
