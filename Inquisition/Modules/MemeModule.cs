﻿using Discord;
using Discord.Commands;
using Discord.WebSocket;

using Inquisition.Data.Models;
using Inquisition.Database;
using Inquisition.Database.Repositories;
using Inquisition.Handlers;
using Inquisition.Logging;

using System;
using System.Threading.Tasks;

namespace Inquisition.Modules
{
    public class MemeModule : ModuleBase<SocketCommandContext>
    {
		private readonly DatabaseContext _dbContext;
        private readonly ReportHandler _reportHandler;
        private readonly IRepositoryWrapper _repository;
        private readonly ILogger<MemeModule> _logger;

		public MemeModule(
            DatabaseContext dbContext,
            ReportHandler reportHandler,
            IRepositoryWrapper repository,
            ILogger<MemeModule> logger)
        {
            _dbContext = dbContext;
            _reportHandler = reportHandler;
            _repository = repository;
            _logger = logger;
        }

		[Command("meme")]
		[Alias("meme by")]
		[Summary("Displays a random meme by random user unless user is specified")]
		public async Task ShowMemeAsync(SocketGuildUser user = null)
		{
			try
			{

			}
			catch (Exception e)
			{
                await ReplyAsync(ReplyHandler.Context(Result.Failed));
                _reportHandler.ReportAsync(Context, e);
                _logger.LogError(e);
			}
		}

		[Command("meme random")]
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
                await ReplyAsync(ReplyHandler.Context(Result.Failed));
				_reportHandler.ReportAsync(Context, e);
                _logger.LogError(e);
			}
		}

		[Command("memes")]
		[Alias("memes by")]
		[Summary("Shows a list of all memes from all users unless user is specified")]
		public async Task ListMemesAsync(SocketUser user = null)
		{
			try
			{

			}
			catch (Exception e)
			{
                await ReplyAsync(ReplyHandler.Context(Result.Failed));
                _reportHandler.ReportAsync(Context, e);
                _logger.LogError(e);
			}
		}

		[Command("add meme")]
		[Summary("Adds a new meme")]
		public async Task AddMemeAsync([Remainder] string url)
		{
			try
			{
				
			}
			catch (Exception e)
			{
                await ReplyAsync(ReplyHandler.Context(Result.Failed));
                _reportHandler.ReportAsync(Context, e);
                _logger.LogError(e);
			}
		}

		[Command("delete meme")]
		[Alias("remove meme")]
		[Summary("Delete a meme")]
		public async Task RemoveMemeAsync(int id)
		{
			try
			{

			}
			catch (Exception e)
			{
                await ReplyAsync(ReplyHandler.Context(Result.Failed));
                _reportHandler.ReportAsync(Context, e);
                _logger.LogError(e);
			}
		}
	}
}
