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
        [Command("game")]
        [Summary("Remove a game from db")]
        public async Task DeleteGameAsync(string name)
        {
            Data.Game game = DbHandler.GetFromDb(new Data.Game { Name = name });

            if (game is null)
            {
                await ReplyAsync(Message.Error.GameNotFound(game));
            } else
            {
                DbHandler.RemoveFromDb(game);
                await ReplyAsync(Message.Info.SuccessfullyRemoved(game));
            }
        }
    }
}
