using Discord.Commands;
using Discord;
using System;
using System.Threading.Tasks;
using Inquisition.Data;
using System.Linq;

namespace Inquisition.Modules
{
    [Group("remove")]
    [Alias("remove a", "delete", "delete a")]
    [RequireUserPermission(GuildPermission.Administrator)]
    public class RemoveModule : ModuleBase<SocketCommandContext>
    {
        InquisitionContext db = new InquisitionContext();

        [Command("game")]
        [Summary("Remove a game from db")]
        public async Task DeleteGameAsync(string name)
        {
            if (!DbHandler.Exists(new Data.Game { Name = name }))
            {
                await ReplyAsync(Message.Error.GameNotFound(new Data.Game { Name = name }));
            } else
            {
                DbHandler.RemoveFromDb(new Data.Game { Name = name });
                await ReplyAsync(Message.Info.SuccessfullyRemoved(new Data.Game { Name = name }));
            }
        }
    }
}
