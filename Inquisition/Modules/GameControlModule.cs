using Discord;
using Discord.Commands;
using Inquisition.Data;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;

namespace Inquisition.Modules
{
    [Group("game")]
    public class GameControlModule : ModuleBase<SocketCommandContext>
    {
        [Command("start", RunMode = RunMode.Async)]
        [Summary("Starts up a game server")]
        public async Task StartGameAsync(string name)
        {
            Data.Game game = DbHandler.GetFromDb(new Data.Game { Name = name });
            string Path = ProcessDictionary.Path;

            if (game is null)
            {
                await ReplyAsync(Message.Error.GameNotFound(game));
                return;
            }

            try
            {
                if (ProcessDictionary.Instance.TryGetValue(game.Name, out Process temp))
                {
                    await ReplyAsync(Message.Error.GameAlreadyRunning(game));
                    return;
                }

                Process p = new Process();
                p.StartInfo.FileName = $"C:\\Windows\\system32\\notepad.exe";
                p.StartInfo.Arguments = game.LaunchArgs;
                p.Start();

                ProcessDictionary.Instance.Add(game.Name, p);

                game.IsOnline = true;
                DbHandler.UpdateInDb(game);

                await ReplyAsync(Message.Info.GameStartingUp(game));
            }
            catch (Exception ex)
            {
                await ReplyAsync(Message.Error.UnableToStartGameServer(game));
                Console.WriteLine(ex.Message);
            }
        }

        [Command("stop", RunMode = RunMode.Async)]
        [Summary("Stops a game server")]
        public async Task StopGameAsync(string name)
        {
            Data.Game game = DbHandler.GetFromDb(new Data.Game { Name = name });
            if (game is null)
            {
                await ReplyAsync(Message.Error.GameNotFound(game));
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
                    DbHandler.UpdateInDb(game);

                    await ReplyAsync(Message.Info.GameShuttingDown(game.Name));
                    return;
                }

                await ReplyAsync(Message.Error.GameNotRunning(game));
            }
            catch (Exception ex)
            {
                await ReplyAsync(Message.Error.UnableToStopGameServer(game));
                Console.WriteLine(ex.Message);
            }
        }

        [Command("status", RunMode = RunMode.Async)]
        [Alias("info")]
        [Summary("Returns if a game server is online")]
        public async Task StatusAsync(string name)
        {
            Data.Game game = DbHandler.GetFromDb(new Data.Game { Name = name });
            if (game is null)
            {
                await ReplyAsync(Message.Error.GameNotFound(new Data.Game { Name = name }));
                return;
            }

            bool ProcessRunning = ProcessDictionary.Instance.TryGetValue(game.Name, out Process p);
            bool GameMarkedOnline = game.IsOnline;

            if (!ProcessRunning && !GameMarkedOnline)
            {
                await ReplyAsync($"{game.Name} server is offline. If you wish to start it up use: game start \"{game.Name}\"");
                return;
            }
            else if (ProcessRunning && !GameMarkedOnline)
            {
                await ReplyAsync($"{game.Name} has a process running, but is marked as offline in the database, " +
                    $"please let the knobhead who programmed this know abut this error, thanks");
                return;
            }
            else if (!ProcessRunning && GameMarkedOnline)
            {
                await ReplyAsync($"{game.Name} server is not running, but is marked as online in the database, " +
                    $"please let the knobhead who programmed this know abut this error, thanks");
                return;
            }

            await ReplyAsync($"{game.Name} server is online, version {game.Version} on port {game.Port}");
        }

        [Command("version", RunMode = RunMode.Async)]
        [Summary("Returns a game's version")]
        public async Task GameVersionAsync(string name)
        {
            Data.Game game = DbHandler.GetFromDb(new Data.Game { Name = name });
            if (game is null)
            {
                await ReplyAsync(Message.Error.GameNotFound(new Data.Game { Name = name }));
                return;
            }

            await ReplyAsync($"{game.Name}'s version is {game.Version}");
        }

        [Command("port", RunMode = RunMode.Async)]
        [Summary("Returns a game's port")]
        public async Task GamePortAsync(string name)
        {
            Data.Game game = DbHandler.GetFromDb(new Data.Game { Name = name });
            if (game is null)
            {
                await ReplyAsync(Message.Error.GameNotFound(new Data.Game { Name = name }));
                return;
            }

            await ReplyAsync($"{game.Name}'s port is {game.Port}");
        }

        [Command("list", RunMode = RunMode.Async)]
        [Summary("Returns list of all games in the database")]
        public async Task ListAllGamesAsync()
        {
            List<Data.Game> Games = DbHandler.ListAll(new Data.Game());

            if (Games.Count > 0)
            {
                EmbedBuilder builder = EmbedTemplate.Create(Context.Client.CurrentUser, Context.User);

                foreach (Data.Game game in Games)
                {
                    string st = game.IsOnline ? "Online " : "Offline ";
                    builder.AddInlineField(game.Name, st + $"on: {game.Port}, v: {game.Version}");
                }

                await ReplyAsync(Message.Info.Generic, false, builder.Build());
            }
            else
            {
                await ReplyAsync(Message.Error.NoContent(Context.User));
            }
        }
    }
}
