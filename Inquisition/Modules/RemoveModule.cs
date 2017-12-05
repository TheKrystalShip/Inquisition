using Discord.Commands;
using Discord;
using System;
using System.Threading.Tasks;
using Inquisition.Data;
using System.Linq;

namespace Inquisition.Modules
{
    [Group("remove")]
    [Alias("delete")]
    [RequireUserPermission(GuildPermission.Administrator)]
    public class RemoveModule : ModuleBase<SocketCommandContext>
    {
        InquisitionContext db = new InquisitionContext();

        [Command("game")]
        [Summary("Remove a game from db")]
        public async Task DeleteGameAsync(string name)
        {
            Data.Game game = db.Games.Where(x => x.Name == name).FirstOrDefault();
            if (game is null)
            {
                await ReplyAsync(ErrorMessage.GameNotFound(game.Name));
                return;
            }
            try
            {
                db.Remove(game);
                await db.SaveChangesAsync();
                await ReplyAsync(InfoMessage.SuccessfullyRemoved(game));
            }
            catch (Exception ex)
            {
                await ReplyAsync(ErrorMessage.DatabaseAccess());
                Console.WriteLine(ex.Message);
                throw;
            }
        }
    }
}
