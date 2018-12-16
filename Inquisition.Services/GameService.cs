using Discord.Commands;

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;

using TheKrystalShip.Inquisition.Domain;
using TheKrystalShip.Logging;

namespace TheKrystalShip.Inquisition.Services
{
    public class GameService
    {
        private readonly ConcurrentDictionary<string, Process> _runningServers;
        private readonly string _path = $"";
        private readonly ILogger<GameService> _logger;

        public GameService()
        {
            _runningServers = new ConcurrentDictionary<string, Process>();
            _logger = new Logger<GameService>();
        }

        public async Task StartServer(Game game, SocketCommandContext context)
        {
            try
            {
                if (_runningServers.TryGetValue(game.Name, out Process temp))
                {
                    return;
                }

                Process p = new Process();
                p.StartInfo.FileName = $"C:\\Windows\\system32\\notepad.exe";
                p.StartInfo.Arguments = game.Arguments;
                p.Start();

                _runningServers.TryAdd(game.Name, p);

                game.IsOnline = true;

                await context.Channel.SendMessageAsync("Starting game...");
            }
            catch (Exception e)
            {
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

                    await context.Channel.SendMessageAsync("Stopping game...");
                    return;
                }

                await context.Channel.SendMessageAsync("Game server not running");
            }
            catch (Exception e)
            {
                _logger.LogError(e);
            }
        }
    }
}
