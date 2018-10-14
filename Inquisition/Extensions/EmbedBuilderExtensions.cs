using Discord;
using Discord.WebSocket;

using System;
using System.Collections.Generic;
using System.Linq;

using TheKrystalShip.Inquisition.Domain;

namespace TheKrystalShip.Inquisition.Extensions
{
    public static class EmbedBuilderExtensions
    {
        public static EmbedBuilder Create(this EmbedBuilder embed)
        {
            embed.WithCurrentTimestamp();
            embed.WithColor(Color.Gold);

            return embed;
        }

        public static EmbedBuilder Create(this EmbedBuilder embed, Exception e)
        {
            embed.WithColor(Color.DarkRed);
            embed.WithTitle("Critical error ocurred");
            embed.WithDescription("A report has been sent to Heisenberg");
            embed.WithCurrentTimestamp();

            return embed;
        }

        public static EmbedBuilder Create(this EmbedBuilder embed, SocketSelfUser user)
        {
            embed.WithCurrentTimestamp();
            embed.WithColor(Color.Gold);

            return embed;
        }

        public static EmbedBuilder Create(this EmbedBuilder embed, SocketUser user)
        {
            embed.WithCurrentTimestamp();
            embed.WithColor(Color.Gold);
            embed.WithFooter($"{user.Username}", user.GetAvatarUrl());

            return embed;
        }

        public static EmbedBuilder Create(this EmbedBuilder embed, Deal deal)
        {
            if (deal.ExpireDate != null)
            {
                string expires = string.Format("{0:dd} day(s), {0:hh}h:{0:mm}m:{0:ss}s", deal.ExpireDate.Subtract(DateTime.Now));
                embed.WithTitle("Expires in: " + expires);
            }
            else
            {
                embed.WithTitle("No expiration date set");
            }

            embed.WithDescription(deal.Url);
            embed.WithCurrentTimestamp();
            embed.WithColor(Color.DarkGreen);
            embed.WithFooter($"{deal.User.Username}", deal.User.AvatarUrl);

            return embed;
        }

        public static EmbedBuilder Create(this EmbedBuilder embed, Deal deal, IUserMessage msg)
        {
            IEmbed existingEmbed = msg.Embeds.First();

            if (deal.ExpireDate != null)
            {
                string expires = string.Format("{0:dd} day(s), {0:hh}h:{0:mm}m:{0:ss}s", deal.ExpireDate.Subtract(DateTime.Now));
                embed.WithTitle("Expires in: " + expires);
            }
            else
            {
                embed.WithTitle("No expiration date set");
            }

            embed.WithImageUrl(existingEmbed.Image.ToString());
            embed.WithDescription(existingEmbed.Description);
            embed.WithCurrentTimestamp();
            embed.WithColor(Color.DarkGreen);
            embed.WithFooter($"{deal.User.Username}", deal.User.AvatarUrl);

            return embed;
        }

        public static EmbedBuilder Create(this EmbedBuilder embed, List<Deal> dealList)
        {
            embed.WithTitle("Active deals");
            embed.WithCurrentTimestamp();
            embed.WithColor(Color.DarkGreen);

            foreach (Deal offer in dealList)
            {
                string timeRemaining = String.Format("{0:dd} day(s), {0:hh}h:{0:mm}m:{0:ss}s", offer.ExpireDate.Subtract(DateTime.Now));
                embed.AddField(offer.Url, $"Time remaining: {timeRemaining}");
            }

            return embed;
        }

        public static EmbedBuilder Create(this EmbedBuilder embed, SocketTextChannel channel)
        {
            embed.WithTitle(channel.Guild.Name);
            embed.WithColor(Color.Gold);
            embed.WithCurrentTimestamp();
            embed.AddField("Default channel: ", channel.Mention);

            return embed;
        }

        public static EmbedBuilder Create(this EmbedBuilder embed, Activity activity)
        {
            embed.WithColor(Color.Gold);
            embed.WithCurrentTimestamp();
            embed.WithFooter($"{activity.User.Username}", activity.User.AvatarUrl);

            embed.AddField(activity.Name, $"Scheduled for: {activity.DueTime}, Created on: {activity.ScheduledTime}");

            return embed;
        }

        public static EmbedBuilder Create(this EmbedBuilder embed, List<Activity> activityList)
        {
            embed.WithColor(Color.Gold);
            embed.WithCurrentTimestamp();

            foreach (Activity activity in activityList)
            {
                embed.AddField($"{activity.Id} - {activity.Name} {activity.Arguments}", $"Scheduled for: {activity.DueTime}, Created on: {activity.ScheduledTime}");
            }

            return embed;
        }
    }
}
