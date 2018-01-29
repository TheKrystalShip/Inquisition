using Discord;
using Discord.WebSocket;

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
    }
}
