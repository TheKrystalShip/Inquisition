using Discord.Commands;

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;

using TheKrystalShip.Inquisition.Data.Models;
using TheKrystalShip.Inquisition.Domain;
using TheKrystalShip.Inquisition.Handlers;
using TheKrystalShip.Logging;

namespace TheKrystalShip.Inquisition.Services
{
    public class GameService
    {
        private readonly ConcurrentDictionary<string, Process> _runningServers;
        private readonly string _path = $"";
        private readonly ReportHandler _reportHandler;
        private readonly ILogger<GameService> _logger;

        public GameService(ReportHandler reportHandler, ILogger<GameService> logger)
        {
            _runningServers = new ConcurrentDictionary<string, Process>();
            _reportHandler = reportHandler;
            _logger = logger;
        }

        public async Task StartServer(Game game, SocketCommandContext context)
        {
            try
            {
                if (_runningServers.TryGetValue(game.Name, out Process temp))
                {
                    _reportHandler.ReportAsync(ReplyHandler.Error.GameAlreadyRunning(game), context.Message);
                    return;
                }

                Process p = new Process();
                p.StartInfo.FileName = $"C:\\Windows\\system32\\notepad.exe";
                p.StartInfo.Arguments = game.Arguments;
                p.Start();

                _runningServers.TryAdd(game.Name, p);

                game.IsOnline = true;
                //Update.Game(game);

                await context.Channel.SendMessageAsync(ReplyHandler.Info.GameStarting(game));
            }
            catch (Exception e)
            {
                await context.Channel.SendMessageAsync(ReplyHandler.Context(Result.Failed));
                _reportHandler.ReportAsync(context, e);
                _logger.LogError(e);
            }
        }

        public async Task StopServer(Game game, SocketCommandContext context)
        {
            try
            {
                if (_runningServers.TryGetValue(game.Name, out Process p))
                {
                    p.CloseMainWindow();
                    p.Close();
                    _runningServers.Remove(game.Name, out Process _);

                    game.IsOnline = false;
                    //Update.Game(game);

                    await context.Channel.SendMessageAsync(ReplyHandler.Info.GameStopping(game));
                    return;
                }

                await context.Channel.SendMessageAsync(ReplyHandler.Error.GameNotRunning(game));
            }
            catch (Exception e)
            {
                await context.Channel.SendMessageAsync(ReplyHandler.Context(Result.Failed));
                _reportHandler.ReportAsync(context, e);
                _logger.LogError(e);
            }
        }

        public Result ServerStatus(Game game)
        {
            bool ProcessRunning = _runningServers.TryGetValue(game.Name, out Process p);
            bool GameMarkedOnline = game.IsOnline;

            if (ProcessRunning && GameMarkedOnline)
            {
                return Result.Online;
            }
            else if (!ProcessRunning && !GameMarkedOnline)
            {
                return Result.Offline;
            }
            else
            {
                return Result.GenericError;
            }
        }
    }
}
