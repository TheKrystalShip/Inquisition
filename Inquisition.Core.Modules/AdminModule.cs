using Discord;
using Discord.Commands;
using Discord.WebSocket;

using System;
using System.Threading.Tasks;

namespace TheKrystalShip.Inquisition.Core.Modules
{
    [RequireUserPermission(GuildPermission.Administrator)]
    public class AdminModule : Module
    {
        [Command("prune")]
        [Alias("purge")]
        [Summary("[Admin] Prunes all inactive members from the server")]
        public async Task<RuntimeResult> PruneMembersAsync(int days)
        {
            if (days < 7)
            {
                return new ErrorResult("Minimum is 7 days of innactivity");
            }

            int members = await Context.Guild.PruneUsersAsync(days);

            return new SuccessResult($"Purged {members} members");
        }

        [Command("ban")]
        [Summary("[Admin] Bans a user from the server")]
        public async Task<RuntimeResult> BanMemberAsync(SocketGuildUser user, [Remainder] string reason = "")
        {
            await user.SendMessageAsync($"You've been banned from {Context.Guild}, reason: {reason}.");
            await Context.Guild.AddBanAsync(user, 0, reason);
            return new SuccessResult($"Banned {user.Username}");
        }

        [Command("hello there")]
        public async Task TestCommandAsync()
        {
            await ReplyAsync($"General {User.Username} ⚔️⚔️");
        }

        [Command("shutdown")]
        [Alias("quit")]
        public async Task Shutdown()
        {
            await ReplyAsync("Shutting down...");
            Environment.Exit(0);
        }
    }
}
