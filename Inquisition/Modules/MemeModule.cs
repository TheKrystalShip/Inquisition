using Discord;
using Discord.Commands;
using Discord.WebSocket;

using Inquisition.Data.Handlers;
using Inquisition.Handlers;
using Inquisition.Services;

using System;
using System.Threading.Tasks;

namespace Inquisition.Modules
{
	public class MemeModule : ModuleBase<SocketCommandContext>
    {
		private DbHandler db;

		public MemeModule(DbHandler dbHandler) => db = dbHandler;

		[Command("meme", RunMode = RunMode.Async)]
		[Alias("meme by")]
		[Summary("Displays a random meme by random user unless user is specified")]
		public async Task ShowMemeAsync(SocketGuildUser user = null)
		{
			try
			{

			}
			catch (Exception e)
			{
				ReportService.Report(Context, e);
			}
		}

		[Command("meme random", RunMode = RunMode.Async)]
		[Alias("random meme")]
		[Summary("Shows a random meme")]
		public async Task ShowRandomMemeAsync()
		{
			try
			{
				Random rn = new Random();
				int limit = 33000;

				string meme = String.Format("http://images.memes.com/meme/{0}.jpg", rn.Next(limit));

				EmbedBuilder embed = EmbedHandler.Create(Context.User);
				embed.WithImageUrl(meme);
				embed.WithTitle(meme);

				await ReplyAsync(ReplyHandler.Generic, false, embed.Build());
			}
			catch (Exception e)
			{
				ReportService.Report(Context, e);
			}
		}

		[Command("memes", RunMode = RunMode.Async)]
		[Alias("memes by")]
		[Summary("Shows a list of all memes from all users unless user is specified")]
		public async Task ListMemesAsync(SocketUser user = null)
		{
			try
			{

			}
			catch (Exception e)
			{
				ReportService.Report(Context, e);
			}
		}

		[Command("add meme", RunMode = RunMode.Async)]
		[Summary("Adds a new meme")]
		public async Task AddMemeAsync([Remainder] string url)
		{
			try
			{
				
			}
			catch (Exception e)
			{
				ReportService.Report(Context, e);
			}
		}

		[Command("delete meme", RunMode = RunMode.Async)]
		[Alias("remove meme")]
		[Summary("Delete a meme")]
		public async Task RemoveMemeAsync(int id)
		{
			try
			{

			}
			catch (Exception e)
			{
				ReportService.Report(Context, e);
			}
		}
	}
}
