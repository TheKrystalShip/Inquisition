using Discord;
using Discord.Commands;

using Inquisition.Data.Models;
using Inquisition.Database.Core;
using Inquisition.Handlers;
using Inquisition.Services;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Inquisition.Modules
{
	public class GameModule : ModuleBase<SocketCommandContext>
	{
		private GameService GameService;
		private DatabaseContext db;

		public GameModule(GameService gameService, DatabaseContext dbHandler)
		{
			GameService = gameService;
			db = dbHandler;
		}

		[Command("start", RunMode = RunMode.Async)]
		[Summary("Starts up a game server")]
		public async Task StartGameAsync(string name)
		{
			try
			{
				Database.Models.Game game = db.Games.FirstOrDefault(x => x.Name.ToLower() == name.ToLower());

				if (game is null)
				{
					await ReplyAsync(ReplyHandler.Error.NotFound.Game);
					return;
				}

				await GameService.StartServer(game, Context);
			}
			catch (Exception e)
			{
				ReportService.Report(Context, e);
			}
		}

		[Command("stop", RunMode = RunMode.Async)]
		[Summary("Stops a game server")]
		public async Task StopGameAsync(string name)
		{
			try
			{
				Database.Models.Game game = db.Games.FirstOrDefault(x => x.Name.ToLower() == name.ToLower());
				if (game is null)
				{
					await ReplyAsync(ReplyHandler.Error.NotFound.Game);
					return;
				}

				await GameService.StopServer(game, Context);
			}
			catch (Exception e)
			{
				ReportService.Report(Context, e);
			}
		}

		[Command("status", RunMode = RunMode.Async)]
		[Alias("info")]
		[Summary("Returns if a game server is online")]
		public async Task StatusAsync(string name)
		{
			try
			{
				Database.Models.Game game = db.Games.FirstOrDefault(x => x.Name.ToLower() == name.ToLower());
				if (game is null)
				{
					await ReplyAsync(ReplyHandler.Error.NotFound.Game);
					return;
				}

				Result result = GameService.ServerStatus(game, Context);
				await ReplyAsync(ReplyHandler.Context(result));
			}
			catch (Exception e)
			{
				ReportService.Report(Context, e);
			}
		}

		[Command("version", RunMode = RunMode.Async)]
		[Summary("Returns a game's version")]
		public async Task GameVersionAsync(string name)
		{
			try
			{
				Database.Models.Game game = db.Games.FirstOrDefault(x => x.Name.ToLower() == name.ToLower());
				if (game is null)
				{
					await ReplyAsync(ReplyHandler.Error.NotFound.Game);
					return;
				}

				await ReplyAsync($"{game.Name}'s version is {game.Version}");
			}
			catch (Exception e)
			{
				ReportService.Report(Context, e);
			}
		}

		[Command("port", RunMode = RunMode.Async)]
		[Summary("Returns a game's port")]
		public async Task GamePortAsync(string name)
		{
			try
			{
				Database.Models.Game game = db.Games.FirstOrDefault(x => x.Name.ToLower() == name.ToLower());
				if (game is null)
				{
					await ReplyAsync(ReplyHandler.Error.NotFound.Game);
					return;
				}

				await ReplyAsync($"{game.Name}'s uses port {game.Port}");
			}
			catch (Exception e)
			{
				ReportService.Report(Context, e);
			}
		}

		[Command("games", RunMode = RunMode.Async)]
		[Summary("Returns list of all games in the database")]
		public async Task ListAllGamesAsync()
		{
			try
			{
				List<Database.Models.Game> Games = db.Games.ToList();

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
				await ReplyAsync("Exception ocurred: " + e.Message);
				ReportService.Report(Context, e);
			}
		}
	}
}
