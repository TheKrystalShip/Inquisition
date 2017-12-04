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
                await ReplyAsync($"Sorry, {name} not found in the database");
                return;
            }
            try
            {
                db.Remove(game);
                await db.SaveChangesAsync();
                await ReplyAsync($"{game.Name} successfully deleted from the server list");
            }
            catch (Exception ex)
            {
                await ReplyAsync($"Something went wrong, check the console for the error message");
                Console.WriteLine(ex.Message);
                throw;
            }
        }
    }
}
