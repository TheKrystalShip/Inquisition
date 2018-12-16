using Discord;
using Discord.Commands;

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using TheKrystalShip.Inquisition.Core.Modules;
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
        public async Task StartGameAsync(string name)
        {
            Domain.Game game = Database.Games
                .FirstOrDefault(x => x.Name.ToLower() == name.ToLower());

            if (game is null)
            {
                await ReplyAsync("Game not found");
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
                await ReplyAsync("Game not found");
                return;
            }

            await _gameService.StopServer(game, Context);
        }

        [Command("version")]
        [Summary("Returns a game's version")]
        public async Task GameVersionAsync(string name)
        {
            Domain.Game game = Database.Games
                .FirstOrDefault(x => x.Name.ToLower() == name.ToLower());

            if (game is null)
            {
                await ReplyAsync("Game not found");
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
                await ReplyAsync("Game not found");
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
                await ReplyAsync("No games in database");
                return;
            }

            EmbedBuilder builder = new EmbedBuilder().Create(Context.User);

            foreach (Domain.Game game in Games)
            {
                string st = game.IsOnline ? "Online " : "Offline ";
                builder.AddField(game.Name, st + $"on port {game.Port}, version {game.Version}");
            }

            await ReplyAsync(builder);
        }
    }
}
