using Discord;
using Discord.Commands;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using TheKrystalShip.Inquisition.Data.Models;
using TheKrystalShip.Inquisition.Database;
using TheKrystalShip.Inquisition.Handlers;
using TheKrystalShip.Inquisition.Services;
using TheKrystalShip.Logging;

namespace TheKrystalShip.Inquisition.Modules
{
    public class GameModule : ModuleBase<SocketCommandContext>
	{
		private readonly DatabaseContext _dbContext;
        private readonly ReportHandler _reportHandler;
		private readonly GameService _gameService;
        private readonly ILogger<GameModule> _logger;

		public GameModule(
            DatabaseContext dbContext,
            ReportHandler reportHandler,
            GameService gameService,
            ILogger<GameModule> logger)
		{
			_dbContext = dbContext;
            _reportHandler = reportHandler;
			_gameService = gameService;
            _logger = logger;
		}

		[Command("start")]
		[Summary("Starts up a game server")]
		public async Task StartGameAsync(string name)
		{
			try
			{
				Database.Models.Game game = _dbContext.Games.FirstOrDefault(x => x.Name.ToLower() == name.ToLower());

				if (game is null)
				{
					await ReplyAsync(ReplyHandler.Error.NotFound.Game);
					return;
				}

				await _gameService.StartServer(game, Context);
			}
			catch (Exception e)
			{
				_reportHandler.ReportAsync(Context, e);
                _logger.LogError(e);
			}
		}

		[Command("stop")]
		[Summary("Stops a game server")]
		public async Task StopGameAsync(string name)
		{
			try
			{
				Database.Models.Game game = _dbContext.Games.FirstOrDefault(x => x.Name.ToLower() == name.ToLower());
				if (game is null)
				{
					await ReplyAsync(ReplyHandler.Error.NotFound.Game);
					return;
				}

				await _gameService.StopServer(game, Context);
			}
			catch (Exception e)
			{
				_reportHandler.ReportAsync(Context, e);
                _logger.LogError(e);
			}
		}

		[Command("status")]
		[Alias("info")]
		[Summary("Returns if a game server is online")]
		public async Task StatusAsync(string name)
		{
			try
			{
				Database.Models.Game game = _dbContext.Games.FirstOrDefault(x => x.Name.ToLower() == name.ToLower());
				if (game is null)
				{
					await ReplyAsync(ReplyHandler.Error.NotFound.Game);
					return;
				}

				Result result = _gameService.ServerStatus(game);
				await ReplyAsync(ReplyHandler.Context(result));
			}
			catch (Exception e)
			{
				_reportHandler.ReportAsync(Context, e);
                _logger.LogError(e);
			}
		}

		[Command("version")]
		[Summary("Returns a game's version")]
		public async Task GameVersionAsync(string name)
		{
			try
			{
				Database.Models.Game game = _dbContext.Games.FirstOrDefault(x => x.Name.ToLower() == name.ToLower());
				if (game is null)
				{
					await ReplyAsync(ReplyHandler.Error.NotFound.Game);
					return;
				}

				await ReplyAsync($"{game.Name}'s version is {game.Version}");
			}
			catch (Exception e)
			{
				_reportHandler.ReportAsync(Context, e);
                _logger.LogError(e);
			}
		}

		[Command("port")]
		[Summary("Returns a game's port")]
		public async Task GamePortAsync(string name)
		{
			try
			{
				Database.Models.Game game = _dbContext.Games.FirstOrDefault(x => x.Name.ToLower() == name.ToLower());
				if (game is null)
				{
					await ReplyAsync(ReplyHandler.Error.NotFound.Game);
					return;
				}

				await ReplyAsync($"{game.Name}'s uses port {game.Port}");
			}
			catch (Exception e)
			{
				_reportHandler.ReportAsync(Context, e);
                _logger.LogError(e);
			}
		}

		[Command("games")]
		[Summary("Returns list of all games in the database")]
		public async Task ListAllGamesAsync()
		{
			try
			{
				List<Database.Models.Game> Games = _dbContext.Games.ToList();

				if (Games.Count == 0)
				{
					await ReplyAsync(ReplyHandler.Error.NoContent(Context.User));
					return;
				}

				EmbedBuilder builder = EmbedHandler.Create(Context.User);

				foreach (Database.Models.Game game in Games)
				{
					string st = game.IsOnline ? "Online " : "Offline ";
					builder.AddInlineField(game.Name, st + $"on port {game.Port}, version {game.Version}");
				}

				await ReplyAsync(ReplyHandler.Generic, false, builder.Build());
			}
			catch (Exception e)
			{
				await ReplyAsync(ReplyHandler.Error.Generic);
				_reportHandler.ReportAsync(Context, e);
                _logger.LogError(e);
			}
		}
	}
}
