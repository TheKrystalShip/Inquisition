using Discord;
using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Text;

namespace Inquisition.Data
{
    public class EmbedTemplate
    {
        public static EmbedBuilder Create(SocketSelfUser author, SocketUser user)
        {
            EmbedBuilder embed = new EmbedBuilder();
            embed = new EmbedBuilder();
            embed.WithCurrentTimestamp();
            embed.WithColor(Color.Blue);
            embed.WithAuthor(author);
            embed.WithFooter($"Requested by: {user}");
            embed.WithThumbnailUrl(user.GetAvatarUrl());

            return embed;
        }
    }
}
