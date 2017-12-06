using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Inquisition.Data;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Inquisition.Modules
{
    [RequireUserPermission(GuildPermission.Administrator)]
    public class UngroupedAdminModule : ModuleBase<SocketCommandContext>
    {
        [Command("prune")]
        [Summary("Prunes all inactive members from the server")]
        public async Task PruneMembersAsync(int d)
        {
            if (d < 7)
            {
                await ReplyAsync("Minimum is 7 days of innactivity");
                return;
            }

            var n = await Context.Guild.PruneUsersAsync(d);
            await ReplyAsync(Message.Info.UsersPruned(n, d));
        }

        [Command("ban")]
        [Summary("Bans a user from the server")]
        public async Task KickMemberAsync(SocketUser user)
        {
            await Context.Guild.AddBanAsync(user);
            await ReplyAsync(Message.Info.UserBanned(user.Username));
        }

        [Command("unban")]
        [Summary("Unbans a user")]
        public async Task UnbanMemberAsync(SocketUser user)
        {
            await Context.Guild.RemoveBanAsync(user);
            await ReplyAsync(Message.Info.UserUnbanned(user.Username));
        }

        [Command("wipe")]
        [Summary("Wipes a text channel")]
        [RequireBotPermission(ChannelPermission.ManageMessages)]
        public async Task WipeChannelAsync(uint amount = 1)
        {
            var messages = await Context.Channel.GetMessagesAsync((int)amount + 1).Flatten();
            await Context.Channel.DeleteMessagesAsync(messages);
            const int delay = 5000;
            var m = await ReplyAsync($"Deleted {amount} messages. _This message will be deleted in {delay / 1000} seconds._");
            await Task.Delay(delay);
            await m.DeleteAsync();
        }
    }

    public class UngroupedModule : ModuleBase<SocketCommandContext>
    {
        [Command("poll:")]
        public async Task AddReactionAsync(string a = "", [Remainder] string r = "")
        {
            SocketUserMessage msg = Context.Message;
            List<Emoji> reactions = new List<Emoji> { new Emoji("👍🏻"), new Emoji("👎🏻"), new Emoji("🤷🏻") };

            foreach (var item in reactions)
            {
                await msg.AddReactionAsync(item);
            }
        }
    }
}
