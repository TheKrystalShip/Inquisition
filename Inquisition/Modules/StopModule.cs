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
                await ReplyAsync($"Sorry, {name} not found in the database");
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

                    await ReplyAsync($"{game.Name} server is shutting down");
                    return;
                }

                await ReplyAsync($"{Context.User.Mention}, {game.Name} doesn't seem to be running. " +
                    $"If you wish to start it up, use the command: game start \"{game.Name}\"");
            }
            catch (System.Exception ex)
            {
                await ReplyAsync($"Something went wrong, couldn't stop {game.Name} server, please let the Admin know about this");
                System.Console.WriteLine(ex.Message);
                throw;
            }
        }
    }
}
