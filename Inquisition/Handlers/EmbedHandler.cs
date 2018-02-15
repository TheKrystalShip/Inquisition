using Discord;
using Discord.WebSocket;

using Inquisition.Data.Models;

using System;
using System.Collections.Generic;
using System.Linq;

namespace Inquisition.Handlers
{
	public class EmbedHandler
    {
        private static EmbedBuilder Embed;

        public static EmbedBuilder Create()
        {
            Embed = new EmbedBuilder();
            Embed.WithCurrentTimestamp();
            Embed.WithColor(Color.Gold);

            return Embed;
        }

		public static EmbedBuilder Create(Exception e)
		{
			Embed = new EmbedBuilder();
			Embed.WithColor(Color.DarkRed);
			Embed.WithTitle("Critical error ocurred");
			Embed.WithDescription("A report has been sent to Heisenberg");
			Embed.WithCurrentTimestamp();

			return Embed;
		}

        public static EmbedBuilder Create(SocketSelfUser user)
        {
            Embed = new EmbedBuilder();
            Embed.WithCurrentTimestamp();
            Embed.WithColor(Color.Gold);

            return Embed;
        }

        public static EmbedBuilder Create(SocketUser user)
        {
            Embed = new EmbedBuilder();
            Embed.WithCurrentTimestamp();
            Embed.WithColor(Color.Gold);
            Embed.WithFooter($"{user.Username}", user.GetAvatarUrl());

            return Embed;
        }

		public static EmbedBuilder Create(Deal deal)
		{
			Embed = new EmbedBuilder();

			if (deal.ExpireDate != null)
			{
				string expires = string.Format("{0:dd} day(s), {0:hh}h:{0:mm}m:{0:ss}s", deal.ExpireDate.Subtract(DateTime.Now));
				Embed.WithTitle("Expires in: " + expires);
			} else
			{
				Embed.WithTitle("No expiration date set");
			}

			Embed.WithDescription(deal.Url);
			Embed.WithCurrentTimestamp();
			Embed.WithColor(Color.DarkGreen);
			Embed.WithFooter($"{deal.User.Username}", deal.User.AvatarUrl);

			return Embed;
		}

		public static EmbedBuilder Create(Deal deal, IUserMessage msg)
		{
			Embed = new EmbedBuilder();
			var existingEmbed = msg.Embeds.First();

			if (deal.ExpireDate != null)
			{
				string expires = string.Format("{0:dd} day(s), {0:hh}h:{0:mm}m:{0:ss}s", deal.ExpireDate.Subtract(DateTime.Now));
				Embed.WithTitle("Expires in: " + expires);
			}
			else
			{
				Embed.WithTitle("No expiration date set");
			}

			Embed.WithImageUrl(existingEmbed.Image.ToString());
			Embed.WithDescription(existingEmbed.Description);
			Embed.WithCurrentTimestamp();
			Embed.WithColor(Color.DarkGreen);
			Embed.WithFooter($"{deal.User.Username}", deal.User.AvatarUrl);

			return Embed;
		}

		public static EmbedBuilder Create(List<Deal> dealList)
		{
			Embed = new EmbedBuilder();
			Embed.WithTitle("Active deals");
			Embed.WithCurrentTimestamp();
			Embed.WithColor(Color.DarkGreen);

			foreach (Deal offer in dealList)
			{
				string timeRemaining = String.Format("{0:dd} day(s), {0:hh}h:{0:mm}m:{0:ss}s", offer.ExpireDate.Subtract(DateTime.Now));
				Embed.AddField(offer.Url, $"Time remaining: {timeRemaining}");
			}

			return Embed;
		}

		public static EmbedBuilder Create(SocketTextChannel channel)
		{
			Embed = new EmbedBuilder();
			Embed.WithTitle(channel.Guild.Name);
			Embed.WithColor(Color.Gold);
			Embed.WithCurrentTimestamp();
			Embed.AddField("Default log channel: ", channel.Mention);

			return Embed;
		}
    }
}
