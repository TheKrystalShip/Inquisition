using Discord.Commands;
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

        public async Task StartServer(Game game, SocketCommandContext context)
        {
            try
            {
                if (RunningServers.TryGetValue(game.Name, out Process temp))
                {
                    await ExceptionService.SendErrorAsync(context, Reply.Error.GameAlreadyRunning(game));
                    return;
                }

                Process p = new Process();
                p.StartInfo.FileName = $"C:\\Windows\\system32\\notepad.exe";
                p.StartInfo.Arguments = game.LaunchArgs;
                p.Start();

                RunningServers.Add(game.Name, p);

                game.IsOnline = true;
                DbHandler.Update.Game(game);

                await context.Channel.SendMessageAsync(Reply.Info.GameStarting(game));
            }
            catch (Exception e)
            {
                await ExceptionService.SendErrorAsync(context, e);
            }
        }

        public async Task StopServer(Game game, SocketCommandContext context)
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

                    await context.Channel.SendMessageAsync(Reply.Info.GameStopping(game));
                    return;
                }

                await context.Channel.SendMessageAsync(Reply.Error.GameNotRunning(game));
            }
            catch (Exception e)
            {
                await ExceptionService.SendErrorAsync(context, e);
            }
        }

        public Result ServerStatus(Game game, SocketCommandContext context)
        {
            bool ProcessRunning = RunningServers.TryGetValue(game.Name, out Process p);
            bool GameMarkedOnline = game.IsOnline;

            if (ProcessRunning && GameMarkedOnline)
            {
                return Result.Online;
            }
            else if (!ProcessRunning && !GameMarkedOnline)
            {
                return Result.Offline;
            }
            else if (ProcessRunning && !GameMarkedOnline)
            {
                return Result.ProcessRunningButOfflineInDb;
            }
            else if (!ProcessRunning && GameMarkedOnline)
            {
                return Result.ProcessNotRunningButOnlineInDb;
            }
            else
            {
                return Result.GenericError;
            }
        }
    }
}
