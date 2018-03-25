using Discord;
using Discord.Commands;
using Discord.WebSocket;

using Inquisition.Data.Models;
using Inquisition.Database;
using Inquisition.Database.Models;
using Inquisition.Handlers;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Inquisition.Modules
{
	public class JokeModule : ModuleBase<SocketCommandContext>
    {
		private DatabaseContext db;

		public JokeModule(DatabaseContext dbHandler) => db = dbHandler;

		[Command("joke")]
		[Alias("joke by")]
		[Summary("Displays a random joke by random user unless user is specified")]
		public async Task ShowJokeAsync(SocketGuildUser user = null)
		{
			try
			{
				Joke joke;

				switch (user)
				{
					case null:
						joke = db.Jokes.OrderBy(x => Guid.NewGuid()).FirstOrDefault();
						break;
					default:
						User temp = db.Users.FirstOrDefault(x => x.Id == user.Id.ToString());
						joke = db.Jokes.Where(x => x.User == temp).OrderBy(x => Guid.NewGuid()).FirstOrDefault();
						break;
				}

				if (joke is null)
				{
					await ReplyAsync("There doesn't seem to be any jokes in the database");
					return;
				}

				EmbedBuilder embed = EmbedHandler.Create(Context.User);
				embed.WithTitle($"{joke.Id} - {joke.Text}");
				embed.WithFooter($"Submitted by {joke.User.Username}", joke.User.AvatarUrl);

				await ReplyAsync(ReplyHandler.Generic, false, embed.Build());
			}
			catch (Exception e)
			{
				ReportHandler.Report(Context, e);
			}
		}

		[Command("jokes")]
		[Alias("jokes by")]
		[Summary("Shows a list of all jokes from all users unless user is specified")]
		public async Task ListJokesAsync(SocketGuildUser user = null)
		{
			try
			{
				List<Joke> Jokes;

				switch (user)
				{
					case null:
						Jokes = db.Jokes.Take(10).ToList();
						break;
					default:
						User temp = db.Users.FirstOrDefault(x => x.Id == user.Id.ToString());
						Jokes = db.Jokes.Where(x => x.User == temp).Take(10).ToList();
						break;
				}

				if (Jokes.Count == 0)
				{
					await ReplyAsync(ReplyHandler.Error.NoContentGeneric);
					return;
				}

				EmbedBuilder embed = EmbedHandler.Create(Context.User);

				foreach (Joke joke in Jokes)
				{
					embed.AddField($"{joke.Id} - {joke.Text}", $"Submitted by {joke.User.Username}");
				}

				await ReplyAsync(ReplyHandler.Generic, false, embed.Build());
			}
			catch (Exception e)
			{
				ReportHandler.Report(Context, e);
			}
		}

		[Command("add joke")]
		[Summary("Adds a new joke")]
		public async Task AddJokeAsync([Remainder] string jokeText)
		{
			try
			{
				User author = db.Users.FirstOrDefault(x => x.Id == Context.User.Id.ToString());

				if (jokeText is null)
				{
					await ReplyAsync(ReplyHandler.Error.Command.Joke);
					return;
				}

				Joke joke = new Joke
				{
					Text = jokeText,
					User = author
				};

				db.Jokes.Add(joke);
				db.SaveChanges();

				await ReplyAsync(ReplyHandler.Context(Result.Successful));
			}
			catch (Exception e)
			{
				await ReplyAsync(ReplyHandler.Context(Result.Failed));
				ReportHandler.Report(Context, e);
			}
		}

		[Command("delete joke")]
		[Alias("remove joke")]
		[Summary("Delete a joke")]
		public async Task RemoveJokeAsync(int id)
		{
			try
			{
				User localUser = db.Users.FirstOrDefault(x => x.Id == Context.User.Id.ToString());
				Joke joke = db.Jokes.Where(x => x.User == localUser && x.Id == id).FirstOrDefault();

				if (joke is null)
				{
					await ReplyAsync(ReplyHandler.Error.NotTheOwner);
					return;
				}

				db.Jokes.Remove(joke);
				db.SaveChanges();

				await ReplyAsync(ReplyHandler.Context(Result.Successful));
			}
			catch (Exception e)
			{
				await ReplyAsync(ReplyHandler.Context(Result.Failed));
				ReportHandler.Report(Context, e);
			}
		}
	}
}
