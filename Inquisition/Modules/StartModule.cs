using Discord.Commands;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Inquisition.Data;
using System.Linq;
using System.Diagnostics;

namespace Inquisition.Modules
{
    [Group("start")]
    public class StartModule : ModuleBase<SocketCommandContext>
    {
        InquisitionContext db = new InquisitionContext();
        string Path = ProcessDictionary.Path;

        [Command("game")]
        [Summary("Starts up a game server")]
        public async Task StartGameAsync(string name)
        {
            Data.Game game = db.Games.Where(x => x.Name == name).FirstOrDefault();
            if (game is null)
            {
                await ReplyAsync($"Sorry, {name} not found in the database");
                return;
            }

            try
            {
                if (ProcessDictionary.Instance.TryGetValue(game.Name, out Process temp))
                {
                    await ReplyAsync($"{game.Name} is already running, version {game.Version} on port {game.Port}. " +
                        $"If you want to stop it, use the command: game stop \"{game.Name}\".");
                    return;
                }

                Process p = new Process();
                p.StartInfo.FileName = Path + game.ExeDir;
                p.StartInfo.Arguments = game.Args;
                p.Start();

                ProcessDictionary.Instance.Add(game.Name, p);

                game.IsOnline = true;
                await db.SaveChangesAsync();

                await ReplyAsync($"{game.Name} server should be online in a few seconds, version {game.Version} on port {game.Port}");
            }
            catch (System.Exception ex)
            {
                await ReplyAsync($"Something went wrong, couldn't start {game.Name} server, please let the Admin know about this.");
                System.Console.WriteLine(ex.Message);
                throw;
            }
        }
    }
}
