﻿using Discord;
using Discord.Commands;

using Inquisition.Data.Models;
using Inquisition.Handlers;
using Inquisition.Logging;

using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Inquisition.Modules
{
    public class PollModule : ModuleBase<SocketCommandContext>
    {
        private readonly ReportHandler _reportHandler;
        private readonly ILogger<PollModule> _logger;

        public PollModule(
            ReportHandler reportHandler,
            ILogger<PollModule> logger)
        {
            _reportHandler = reportHandler;
            _logger = logger;
        }

		[Command("poll")]
		[Alias("poll:")]
		[Summary("Create a poll")]
		public async Task CreatePollAsync([Remainder] string question = "")
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
				embed.WithTitle(question);
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
                await ReplyAsync(ReplyHandler.Context(Result.Failed));
                _reportHandler.ReportAsync(Context, e);
                _logger.LogError(e);
			}
		}
	}
}
