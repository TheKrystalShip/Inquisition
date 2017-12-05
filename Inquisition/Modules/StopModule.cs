using Discord.Commands;
using System;
using System.Collections.Generic;
using System.Text;
using Inquisition.Data;
using System.Threading.Tasks;
using System.Linq;
using System.Diagnostics;

namespace Inquisition.Modules
{
    [Group("stop")]
    public class StopModule : ModuleBase<SocketCommandContext>
    {
        InquisitionContext db = new InquisitionContext();

        [Command("game")]
        [Summary("Stops a game server")]
        public async Task StopGameAsync(string name)
        {
            Data.Game game = db.Games.Where(x => x.Name == name).FirstOrDefault();
            if (game is null)
            {
                await ReplyAsync(ErrorMessage.GameNotFound(game.Name));
                return;
            }

            try
            {
                if (ProcessDictionary.Instance.TryGetValue(game.Name, out Process p))
                {
                    p.CloseMainWindow();
                    p.Close();
                    ProcessDictionary.Instance.Remove(game.Name);

                    game.IsOnline = false;
                    await db.SaveChangesAsync();

                    await ReplyAsync(InfoMessage.GameShuttingDown(game.Name));
                    return;
                }

                await ReplyAsync(ErrorMessage.GameNotRunning(Context.User.Mention, game.Name));
            }
            catch (Exception ex)
            {
                await ReplyAsync(ErrorMessage.UnableToStopGameServer(game.Name));
                Console.WriteLine(ex.Message);
                throw;
            }
        }
    }
}
