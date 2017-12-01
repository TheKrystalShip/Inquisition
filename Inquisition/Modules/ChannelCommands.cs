using Discord;
using Discord.Commands;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Inquisition.Modules
{
    [Group("channel")]
    public class ChannelCommands : ModuleBase<SocketCommandContext>
    {
        [Command("wipe")]
        [Summary("Wipes a text channel")]
        [RequireUserPermission(GuildPermission.Administrator)]
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
}
