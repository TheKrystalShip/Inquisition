using Discord;
using Discord.Commands;

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using TheKrystalShip.Inquisition.Data.Models;
using TheKrystalShip.Inquisition.Extensions;
using TheKrystalShip.Inquisition.Handlers;
using TheKrystalShip.Inquisition.Services;

namespace TheKrystalShip.Inquisition.Modules
{
    public class GameModule : Module
    {
        private readonly GameService _gameService;

        public GameModule(GameService gameService, Tools tools) : base(tools)
        {
            _gameService = gameService;
        }

        [Command("start")]
        [Summary("Starts up a game server")]
        public async Task StartGameAsync(string name)
        {
            Domain.Game game = Database.Games
                .FirstOrDefault(x => x.Name.ToLower() == name.ToLower());

            if (game is null)
            {
                await ReplyAsync(ReplyHandler.Error.NotFound.Game);
                return;
            }

            await _gameService.StartServer(game, Context);
        }

        [Command("stop")]
        [Summary("Stops a game server")]
        public async Task StopGameAsync(string name)
        {
            Domain.Game game = Database.Games
                .FirstOrDefault(x => x.Name.ToLower() == name.ToLower());

            if (game is null)
            {
                await ReplyAsync(ReplyHandler.Error.NotFound.Game);
                return;
            }

            await _gameService.StopServer(game, Context);
        }

        [Command("status")]
        [Alias("info")]
        [Summary("Returns if a game server is online")]
        public async Task StatusAsync(string name)
        {
            Domain.Game game = Database.Games
            .FirstOrDefault(x => x.Name.ToLower() == name.ToLower());

            if (game is null)
            {
                await ReplyAsync(ReplyHandler.Error.NotFound.Game);
                return;
            }

            Result result = _gameService.ServerStatus(game);
            await ReplyAsync(ReplyHandler.Context(result));
        }

        [Command("version")]
        [Summary("Returns a game's version")]
        public async Task GameVersionAsync(string name)
        {
            Domain.Game game = Database.Games
                .FirstOrDefault(x => x.Name.ToLower() == name.ToLower());

            if (game is null)
            {
                await ReplyAsync(ReplyHandler.Error.NotFound.Game);
                return;
            }

            await ReplyAsync($"{game.Name}'s version is {game.Version}");
        }

        [Command("port")]
        [Summary("Returns a game's port")]
        public async Task GamePortAsync(string name)
        {
            Domain.Game game = Database.Games
            .FirstOrDefault(x => x.Name.ToLower() == name.ToLower());

            if (game is null)
            {
                await ReplyAsync(ReplyHandler.Error.NotFound.Game);
                return;
            }

            await ReplyAsync($"{game.Name}'s uses port {game.Port}");
        }

        [Command("games")]
        [Summary("Returns list of all games in the database")]
        public async Task ListAllGamesAsync()
        {
            List<Domain.Game> Games = Database.Games.ToList();

            if (Games.Count is 0)
            {
                await ReplyAsync(ReplyHandler.Error.NoContent(Context.User));
                return;
            }

            EmbedBuilder builder = new EmbedBuilder().Create(Context.User);

            foreach (Domain.Game game in Games)
            {
                string st = game.IsOnline ? "Online " : "Offline ";
                builder.AddInlineField(game.Name, st + $"on port {game.Port}, version {game.Version}");
            }

            await ReplyAsync(ReplyHandler.Generic, false, builder.Build());
        }
    }
}
