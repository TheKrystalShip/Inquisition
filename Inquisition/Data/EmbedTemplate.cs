using Discord;
using Discord.WebSocket;

namespace Inquisition.Data
{
    public class EmbedTemplate
    {
        private static EmbedBuilder Embed;

        public static EmbedBuilder Create()
        {
            Embed = new EmbedBuilder();
            Embed.WithCurrentTimestamp();
            Embed.WithColor(Color.Gold);

            return Embed;
        }

        public static EmbedBuilder Create(SocketSelfUser user)
        {
            Embed = new EmbedBuilder();
            Embed.WithCurrentTimestamp();
            Embed.WithColor(Color.Gold);
            Embed.WithAuthor(user);

            return Embed;
        }

        public static EmbedBuilder Create(SocketUser user)
        {
            Embed = new EmbedBuilder();
            Embed.WithCurrentTimestamp();
            Embed.WithColor(Color.Gold);
            Embed.WithFooter($"{user}", user.GetAvatarUrl());

            return Embed;
        }

        public static EmbedBuilder Create(SocketSelfUser author, SocketUser user)
        {
            Embed = new EmbedBuilder();
            Embed.WithCurrentTimestamp();
            Embed.WithColor(Color.Gold);
            Embed.WithAuthor(author);
            Embed.WithFooter($"Requested by: {user}", user.GetAvatarUrl());

            return Embed;
        }
    }
}
