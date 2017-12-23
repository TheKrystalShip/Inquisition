using Discord;
using Discord.Commands;
using Inquisition.Handlers;
using System.Collections.Generic;
using System.Threading.Tasks;
using Inquisition.Data;
using Inquisition.Services;

namespace Inquisition.Modules
{
    public class GameModule : ModuleBase<SocketCommandContext>
    {
        private GameService GameServerService;

        public GameModule(GameService gameServerService)
        {
            GameServerService = gameServerService;
        }

        [Command("start", RunMode = RunMode.Async)]
        [Summary("Starts up a game server")]
        public async Task StartGameAsync(string name)
        {
            Data.Game game = DbHandler.Select.Game(name);

            if (game is null)
            {
                await ReplyAsync(Message.Error.GameNotFound(game));
                return;
            }

            await GameServerService.StartServer(game, Context.Message.Channel);
        }

        [Command("stop", RunMode = RunMode.Async)]
        [Summary("Stops a game server")]
        public async Task StopGameAsync(string name)
        {
            Data.Game game = DbHandler.Select.Game(name);
            if (game is null)
            {
                await ReplyAsync(Message.Error.GameNotFound(game));
                return;
            }

            await GameServerService.StopServer(game, Context.Message.Channel);
        }

        [Command("status", RunMode = RunMode.Async)]
        [Alias("info")]
        [Summary("Returns if a game server is online")]
        public async Task StatusAsync(string name)
        {
            Data.Game game = DbHandler.Select.Game(name);
            if (game is null)
            {
                await ReplyAsync(Message.Error.GameNotFound(new Data.Game { Name = name }));
                return;
            }

            await GameServerService.ServerStatus(game, Context.Message.Channel);
        }

        [Command("version", RunMode = RunMode.Async)]
        [Summary("Returns a game's version")]
        public async Task GameVersionAsync(string name)
        {
            Data.Game game = DbHandler.Select.Game(name);
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
            Data.Game game = DbHandler.Select.Game(name);
            if (game is null)
            {
                await ReplyAsync(Message.Error.GameNotFound(new Data.Game { Name = name }));
                return;
            }

            await ReplyAsync($"{game.Name}'s port is {game.Port}");
        }

        [Command("games", RunMode = RunMode.Async)]
        [Summary("Returns list of all games in the database")]
        public async Task ListAllGamesAsync()
        {
            List<Data.Game> Games = DbHandler.Select.Games(0);

            if (Games.Count == 0)
            {
                await ReplyAsync(Message.Error.NoContent(Context.User));
                return;
            }

            EmbedBuilder builder = EmbedTemplate.Create(Context.Client.CurrentUser, Context.User);

            foreach (Data.Game game in Games)
            {
                string st = game.IsOnline ? "Online " : "Offline ";
                builder.AddInlineField(game.Name, st + $"on port {game.Port}, version {game.Version}");
            }

            await ReplyAsync(Message.Info.Generic, false, builder.Build());
        }
    }
}
