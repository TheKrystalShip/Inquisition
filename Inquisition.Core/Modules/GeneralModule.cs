using Discord;
using Discord.Commands;
using Discord.WebSocket;

using Inquisition.Core.Handlers;
using Inquisition.Core.Services;

using Inquisition.Database.Commands;
using Inquisition.Database.Handlers;
using Inquisition.Database.Models;

using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Inquisition.Core.Modules
{
	public class GaneralModule : ModuleBase<SocketCommandContext>
	{
		[Command("poll", RunMode = RunMode.Async)]
		[Alias("poll:")]
		[Summary("Create a poll")]
		public async Task CreatePollAsync([Remainder] string r = "")
		{
			try
			{
				List<Emoji> reactions = new List<Emoji>
				{
					new Emoji("👍🏻"),
					new Emoji("👎🏻"),
					new Emoji("🤷🏻")
				};

				var messages = await Context.Channel.GetMessagesAsync(1).Flatten();
				await Context.Channel.DeleteMessagesAsync(messages);

				EmbedBuilder embed = EmbedHandler.Create(Context.User);
				embed.WithTitle(r);
				embed.WithFooter($"Asked by {Context.User.Username}", Context.User.GetAvatarUrl() ?? null);

				var msg = await ReplyAsync("", false, embed.Build());

				foreach (Emoji e in reactions)
				{
					await msg.AddReactionAsync(e);
					await Task.Delay(1000);
				}
			}
			catch (Exception e)
			{
				ReportService.Report(Context, e);
			}
		}

		[Command("timezone", RunMode = RunMode.Async)]
		[Summary("Tells you your timezone from the database")]
		public async Task ShowTimezoneAsync()
		{
			try
			{
				User local = ConversionHandler.GrabFromDb(Context.User);

				if (local.TimezoneOffset is null)
				{
					await ReplyAsync(ReplyHandler.Error.TimezoneNotSet);
					return;
				}

				await ReplyAsync(ReplyHandler.Info.UserTimezone(local));
			}
			catch (Exception e)
			{
				ReportService.Report(Context, e);
			}
		}

		[Command("joke", RunMode = RunMode.Async)]
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
						joke = Select.Joke(true);
						break;
					default:
						User temp = ConversionHandler.GrabUserFromDb(user);
						joke = Select.Joke(temp, true);
						break;
				}

				EmbedBuilder embed = EmbedHandler.Create(Context.User);
				embed.WithTitle($"{joke.Id} - {joke.Text}");
				embed.WithFooter($"Submitted by {joke.User.Username}", joke.User.AvatarUrl);

				await ReplyAsync(ReplyHandler.Generic, false, embed.Build());
			}
			catch (Exception e)
			{
				ReportService.Report(Context, e);
			}
		}

		[Command("jokes", RunMode = RunMode.Async)]
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
						Jokes = Select.JokeList(10);
						break;
					default:
						User temp = ConversionHandler.GrabUserFromDb(user);
						Jokes = Select.JokeList(10, temp);
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
				ReportService.Report(Context, e);
			}
		}

		//[Command("meme", RunMode = RunMode.Async)]
		//[Alias("meme by")]
		//[Summary("Displays a random meme by random user unless user is specified")]
		//public async Task ShowMemeAsync(SocketUser user = null)
		//{
		//	try
		//	{
		//		Meme meme;

		//		switch (user)
		//		{
		//			case null:
		//				meme = Select.Meme(true);
		//				break;
		//			default:
		//				User temp = UserHandler.ToUser(user);
		//				meme = Select.Meme(temp, true);
		//				break;
		//		}

		//		if (meme is null)
		//		{
		//			await ReplyAsync(ReplyHandler.Error.NoContentGeneric);
		//			return;
		//		}
				
		//		EmbedBuilder embed = Handlers.EmbedHandler.Create(Context.User);
		//		embed.WithFooter($"Submitted by: {meme.User.Username}#{meme.User.Discriminator}", meme.User.AvatarUrl);
		//		embed.WithImageUrl(meme.Url);
		//		embed.WithTitle($"{meme.Id} - {meme.Url}");

		//		await ReplyAsync(ReplyHandler.Generic, false, embed.Build());
		//	}
		//	catch (Exception e)
		//	{
		//		ReportService.Report(Context, e);
		//	}
		//}

		//[Command("meme random", RunMode = RunMode.Async)]
		//[Alias("random meme")]
		//[Summary("Shows a random meme")]
		//public async Task ShowRandomMemeAsync()
		//{
		//	try
		//	{
		//		Random rn = new Random();
		//		int limit = 33000;

		//		string meme = String.Format("http://images.memes.com/meme/{0}.jpg", rn.Next(limit));

		//		EmbedBuilder embed = EmbedHandler.Create(Context.User);
		//		embed.WithImageUrl(meme);
		//		embed.WithTitle(meme);

		//		await ReplyAsync(ReplyHandler.Generic, false, embed.Build());
		//	}
		//	catch (Exception e)
		//	{
		//		ReportService.Report(Context, e);
		//	}
		//}

		//[Command("memes", RunMode = RunMode.Async)]
		//[Alias("memes by")]
		//[Summary("Shows a list of all memes from all users unless user is specified")]
		//public async Task ListMemesAsync(SocketUser user = null)
		//{
		//	try
		//	{
		//		List<Meme> Memes;

		//		switch (user)
		//		{
		//			case null:
		//				Memes = Select.MemeList(10);
		//				break;
		//			default:
		//				User localUser = UserHandler.SelectOrInsert(user);
		//				Memes = Select.MemeList(10, localUser);
		//				break;
		//		}

		//		if (Memes.Count == 0)
		//		{
		//			await ReplyAsync(ReplyHandler.Error.NoContentGeneric);
		//			return;
		//		}

		//		EmbedBuilder embed = EmbedHandler.Create(Context.User);

		//		foreach (Meme meme in Memes)
		//		{
		//			embed.AddField($"{meme.Id} - {meme.Url}", $"Submitted by {meme.User.Username}");
		//		}

		//		await ReplyAsync(ReplyHandler.Generic, false, embed.Build());
		//	}
		//	catch (Exception e)
		//	{
		//		ReportService.Report(Context, e);
		//	}
		//}

		[Command("reminders", RunMode = RunMode.Async)]
		[Summary("Displays a list with all of your reminders")]
		public async Task ListRemindersAsync()
		{
			try
			{
				User localUser = ConversionHandler.GrabFromDb(Context.User);

				List<Reminder> Reminders = Select.ReminderList(localUser);

				if (Reminders.Count == 0)
				{
					await ReplyAsync(ReplyHandler.Error.NoContentGeneric);
					return;
				}

				EmbedBuilder embed = EmbedHandler.Create(Context.User);

				foreach (Reminder reminder in Reminders)
				{
					embed.AddField($"{reminder.Id} - {reminder.Message ?? "No message"}", $"{reminder.DueDate}");
				}

				await ReplyAsync(ReplyHandler.Generic, false, embed.Build());
			}
			catch (Exception e)
			{
				ReportService.Report(Context, e);
			}
		}

		[Command("alerts", RunMode = RunMode.Async)]
		[Summary("Displays a list of all of your notifications")]
		public async Task ListAlertsAsync()
		{
			try
			{
				User localUser = ConversionHandler.GrabFromDb(Context.User);

				List<Alert> Alerts = Select.AlertList(localUser);

				if (Alerts.Count == 0)
				{
					await ReplyAsync(ReplyHandler.Error.NoContentGeneric);
					return;
				}

				EmbedBuilder embed = EmbedHandler.Create(Context.User);
				string description = "";

				foreach (Alert n in Alerts)
				{
					description += $"**{n.TargetUser.Username}**\n";
				}

				embed.WithDescription(description);

				await ReplyAsync(ReplyHandler.Generic, false, embed.Build());
			}
			catch (Exception e)
			{
				ReportService.Report(Context, e);
			}
		}
	}

	[Group("add")]
	public class AddGeneralModule : ModuleBase<SocketCommandContext>
	{
		[Command("joke", RunMode = RunMode.Async)]
		[Summary("Adds a new joke")]
		public async Task AddJokeAsync([Remainder] string jokeText)
		{
			try
			{
				User author = ConversionHandler.GrabFromDb(Context.User);

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

				Result result;

				using (CrudHandler c = new CrudHandler())
				{
					result = c.Insert(joke);
				}

				await ReplyAsync(ReplyHandler.Context(result));
			}
			catch (Exception e)
			{
				Console.WriteLine(e);
				ReportService.Report(Context, e);
			}
		}

		//[Command("meme", RunMode = RunMode.Async)]
		//[Summary("Adds a new meme")]
		//public async Task AddMemeAsync([Remainder] string url)
		//{
		//	try
		//	{
		//		User localUser = UserHandler.SelectOrInsert(Context.User);

		//		if (url is null)
		//		{
		//			await ReplyAsync(ReplyHandler.Error.Command.Meme);
		//			return;
		//		}

		//		Meme meme = new Meme
		//		{
		//			Url = url,
		//			User = localUser
		//		};

		//		Result result;

		//		using (CrudHandler c = new CrudHandler())
		//		{
		//			result = c.Insert(meme);
		//		}

		//		await ReplyAsync(ReplyHandler.Context(result));
		//	}
		//	catch (Exception e)
		//	{
		//		ReportService.Report(Context, e);
		//	}
		//}

		[Command("reminder", RunMode = RunMode.Async)]
		[Summary("Add a new reminder")]
		public async Task AddReminderAsync(string dueDate, [Remainder] string remainder = "")
		{
			try
			{
				User localUser = ConversionHandler.GrabFromDb(Context.User);

				if (localUser.TimezoneOffset is null)
				{
					await ReplyAsync(ReplyHandler.Error.TimezoneNotSet);
					return;
				}

				DateTimeOffset dueDateUtc;

				try
				{
					dueDateUtc = new DateTimeOffset(DateTime.Parse(dueDate),
						new TimeSpan((int)localUser.TimezoneOffset, 0, 0));
				}
				catch (Exception)
				{
					await ReplyAsync(ReplyHandler.Error.Command.Reminder);
					return;
				}

				Reminder reminder = new Reminder
				{
					DueDate = dueDateUtc,
					Message = remainder,
					User = localUser
				};

				Result result;

				using (CrudHandler c = new CrudHandler())
				{
					result = c.Insert(reminder);
				}

				await ReplyAsync(ReplyHandler.Context(result));
			}
			catch (Exception e)
			{
				ReportService.Report(Context, e);
			}
		}

		[Command("alert", RunMode = RunMode.Async)]
		[Summary("Add a new alert, must specify a target user")]
		public async Task AddAlertAsync(SocketGuildUser targetAlert)
		{
			try
			{
				User author = ConversionHandler.GrabFromDb(Context.User);
				User target = ConversionHandler.GrabUserFromDb(targetAlert);

				Alert alert = new Alert
				{
					User = author,
					TargetUser = target
				};

				Result result;
				using (CrudHandler c = new CrudHandler())
				{
					result = c.Insert(alert);
				}

				await ReplyAsync(ReplyHandler.Context(result));
			}
			catch (Exception e)
			{
				ReportService.Report(Context, e);
			}
		}
	}

	[Group("remove")]
	public class RemoveGeneralModule : ModuleBase<SocketCommandContext>
	{
		[Command("joke")]
		[Summary("Delete a joke")]
		public async Task RemoveJokeAsync(int id)
		{
			try
			{
				User localUser = ConversionHandler.GrabFromDb(Context.User);
				Joke joke = Select.Joke(id, localUser);

				if (joke is null)
				{
					await ReplyAsync(ReplyHandler.Error.NotTheOwner);
					return;
				}

				Result result;

				using (CrudHandler c = new CrudHandler())
				{
					result = c.Delete(joke);
				}

				await ReplyAsync(ReplyHandler.Context(result));
			}
			catch (Exception e)
			{
				ReportService.Report(Context, e);
			}
		}

		//[Command("meme")]
		//[Summary("Delete a meme")]
		//public async Task RemoveMemeAsync(int id)
		//{
		//	try
		//	{
		//		User temp = UserHandler.ToUser(Context.User);
		//		User localUser = Select.User(temp);

		//		Meme meme = Select.Meme(id, localUser);

		//		if (meme is null)
		//		{
		//			await ReplyAsync(ReplyHandler.Error.NotTheOwner);
		//			return;
		//		}

		//		Result result = Delete.Meme(meme);
		//		await ReplyAsync(ReplyHandler.Context(result));
		//	}
		//	catch (Exception e)
		//	{
		//		ReportService.Report(Context, e);
		//	}
		//}

		[Command("reminder", RunMode = RunMode.Async)]
		[Summary("Remove a reminder")]
		public async Task RemoveReminderAsync(int id)
		{
			try
			{
				User localUser = ConversionHandler.GrabFromDb(Context.User);

				Reminder reminder = Select.Reminder(id, localUser);

				Result result;
				using (CrudHandler c = new CrudHandler())
				{
					result = c.Delete(reminder);
				}

				await ReplyAsync(ReplyHandler.Context(result));
			}
			catch (Exception e)
			{
				ReportService.Report(Context, e);
			}
		}

		[Command("alert", RunMode = RunMode.Async)]
		[Summary("Removes an alert, must specify a target user")]
		public async Task RemoveAlertAsync(SocketGuildUser targetUser)
		{
			try
			{
				User author = ConversionHandler.GrabFromDb(Context.User);
				User target = ConversionHandler.GrabUserFromDb(targetUser);

				Alert alert = Select.Alert(author, target);

				if (alert is null)
				{
					await ReplyAsync(ReplyHandler.Error.NotFound.Alert);
					return;
				}

				Result result;
				using (CrudHandler c = new CrudHandler())
				{
					result = c.Delete(alert);
				}

				await ReplyAsync(ReplyHandler.Context(result));
			}
			catch (Exception e)
			{
				ReportService.Report(Context, e);
			}
		}
	}

	[Group("set")]
	public class SetGeneralModule : ModuleBase<SocketCommandContext>
	{
		[Command("timezone", RunMode = RunMode.Async)]
		[Summary("Set your timezone")]
		public async Task SetTimezoneAsync(int offset)
		{
			try
			{
				User localUser = ConversionHandler.GrabFromDb(Context.User);

				localUser.TimezoneOffset = offset;

				Result result;
				using (CrudHandler c = new CrudHandler())
				{
					result = c.Update(localUser);
				}

				await ReplyAsync(ReplyHandler.Info.UserTimezone(localUser));
			}
			catch (Exception e)
			{
				Console.WriteLine(e);
				ReportService.Report(Context, e);
			}
		}
	}
}
