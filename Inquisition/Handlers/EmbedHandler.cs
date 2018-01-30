using Discord;
using Discord.WebSocket;

using Inquisition.Data.Models;

using System;

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

		public static EmbedBuilder Create(Offer offer)
		{
			Embed = new EmbedBuilder();

			if (offer.ExpireDate != null)
			{
				string expires = string.Format("{0:dd} day(s), {0:hh}h:{0:mm}m:{0:ss}s", offer.ExpireDate.Subtract(DateTime.Now));
				Embed.WithTitle("Expires in: " + expires);
			} else
			{
				Embed.WithTitle("No expiration date set");
			}

			Embed.WithDescription(offer.Url);
			Embed.WithCurrentTimestamp();
			Embed.WithColor(Color.DarkGreen);
			Embed.WithFooter($"{offer.User.Username}", offer.User.AvatarUrl);

			return Embed;
		}
    }
}
