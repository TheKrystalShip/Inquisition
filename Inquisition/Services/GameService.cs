using Discord.WebSocket;
using Inquisition.Data;
using Inquisition.Handlers;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;

namespace Inquisition.Services
{
    public class GameService
    {
        public static Dictionary<string, Process> RunningServers { get; set; } = new Dictionary<string, Process>();
        public static string Path = $"";

        public async Task StartServer(Game game, ISocketMessageChannel channel)
        {
            try
            {
                if (RunningServers.TryGetValue(game.Name, out Process temp))
                {
                    await channel.SendMessageAsync(Message.Error.GameAlreadyRunning(game));
                    return;
                }

                Process p = new Process();
                p.StartInfo.FileName = $"C:\\Windows\\system32\\notepad.exe";
                p.StartInfo.Arguments = game.LaunchArgs;
                p.Start();

                RunningServers.Add(game.Name, p);

                game.IsOnline = true;
                DbHandler.Update.Game(game);

                await channel.SendMessageAsync(Message.Info.GameStartingUp(game));
            }
            catch (Exception e)
            {
                await channel.SendMessageAsync(Message.Error.UnableToStartGameServer(game));
                Console.WriteLine(e.Message);
            }
        }

        public async Task StopServer(Game game, ISocketMessageChannel channel)
        {
            try
            {
                if (RunningServers.TryGetValue(game.Name, out Process p))
                {
                    p.CloseMainWindow();
                    p.Close();
                    RunningServers.Remove(game.Name);

                    game.IsOnline = false;
                    DbHandler.Update.Game(game);

                    await channel.SendMessageAsync(Message.Info.GameShuttingDown(game.Name));
                    return;
                }

                await channel.SendMessageAsync(Message.Error.GameNotRunning(game));
            }
            catch (Exception ex)
            {
                await channel.SendMessageAsync(Message.Error.UnableToStopGameServer(game));
                Console.WriteLine(ex.Message);
            }
        }

        public async Task ServerStatus(Game game, ISocketMessageChannel channel)
        {
            bool ProcessRunning = RunningServers.TryGetValue(game.Name, out Process p);
            bool GameMarkedOnline = game.IsOnline;

            if (!ProcessRunning && !GameMarkedOnline)
            {
                await channel.SendMessageAsync($"{game.Name} server is offline. If you wish to start it up use: game start \"{game.Name}\"");
                return;
            }
            else if (ProcessRunning && !GameMarkedOnline)
            {
                await channel.SendMessageAsync($"{game.Name} has a process running, but is marked as offline in the database, " +
                    $"please let the knobhead who programmed this know abut this error, thanks");
                return;
            }
            else if (!ProcessRunning && GameMarkedOnline)
            {
                await channel.SendMessageAsync($"{game.Name} server is not running, but is marked as online in the database, " +
                    $"please let the knobhead who programmed this know abut this error, thanks");
                return;
            }

            await channel.SendMessageAsync($"{game.Name} server is online, version {game.Version} on port {game.Port}");
        }
    }
}
