using Discord;
using Discord.Commands;

using Microsoft.EntityFrameworkCore;

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using TheKrystalShip.Inquisition.Extensions;
using TheKrystalShip.Inquisition.Services;

namespace TheKrystalShip.Inquisition.Core.Modules
{
    [Group("game")]
    public class GameModule : Module
    {
        private readonly GameService _gameService;

        public GameModule(GameService gameService)
        {
            _gameService = gameService;
        }

        [Command("start")]
        [Summary("Starts up a game server")]
        public async Task<RuntimeResult> StartGameAsync(string name)
        {
            Domain.Game game = Database.Games
                .FirstOrDefault(x => x.Name.ToLower() == name.ToLower());

            if (game is null)
            {
                return new ErrorResult(CommandError.ObjectNotFound, "Game not found");
            }

            await _gameService.StartServer(game, Context);
            return new EmptyResult();
        }

        [Command("stop")]
        [Summary("Stops a game server")]
        public async Task<RuntimeResult> StopGameAsync(string name)
        {
            Domain.Game game = Database.Games
                .FirstOrDefault(x => x.Name.ToLower() == name.ToLower());

            if (game is null)
            {
                return new ErrorResult(CommandError.ObjectNotFound, "Game not found");
            }

            await _gameService.StopServer(game, Context);
            return new EmptyResult();
        }

        [Command("version")]
        [Summary("Returns a game's version")]
        public async Task<RuntimeResult> GameVersionAsync(string name)
        {
            Domain.Game game = Database.Games
                .FirstOrDefault(x => x.Name.ToLower() == name.ToLower());

            if (game is null)
            {
                return new ErrorResult(CommandError.ObjectNotFound, "Game not found");
            }

            return new InfoResult($"{game.Name}'s version is {game.Version}");
        }

        [Command("port")]
        [Summary("Returns a game's port")]
        public async Task<RuntimeResult> GamePortAsync(string name)
        {
            Domain.Game game = await Database.Games
                .FirstOrDefaultAsync(x => x.Name.ToLower() == name.ToLower());

            if (game is null)
            {
                return new ErrorResult(CommandError.ObjectNotFound, "Game not found");
            }

            return new InfoResult($"{game.Name}'s uses port {game.Port}");
        }

        [Command("games")]
        [Summary("Returns list of all games in the database")]
        public async Task<RuntimeResult> ListAllGamesAsync()
        {
            List<Domain.Game> Games = await Database.Games
                .ToListAsync();

            if (Games.Count is 0)
            {
                return new ErrorResult(CommandError.ObjectNotFound, "No games found in the database");
            }

            EmbedBuilder embedBuilder = new EmbedBuilder().Create(Context.User);

            foreach (Domain.Game game in Games)
            {
                string st = game.IsOnline ? "Online " : "Offline ";
                embedBuilder.AddField(game.Name, st + $"on port {game.Port}, version {game.Version}");
            }
            
            return new InfoResult("Info", embedBuilder);
        }
    }
}
