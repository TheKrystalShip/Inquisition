using Discord;
using Discord.WebSocket;

using System;
using System.Collections.Generic;
using System.Linq;

using TheKrystalShip.Inquisition.Database.Models;

namespace TheKrystalShip.Inquisition.Handlers
{
    public class EmbedHandler
    {
        private static EmbedBuilder _embed;

        public static EmbedBuilder Create()
        {
            _embed = new EmbedBuilder();
            _embed.WithCurrentTimestamp();
            _embed.WithColor(Color.Gold);

            return _embed;
        }

		public static EmbedBuilder Create(Exception e)
		{
			_embed = new EmbedBuilder();
			_embed.WithColor(Color.DarkRed);
			_embed.WithTitle("Critical error ocurred");
			_embed.WithDescription("A report has been sent to Heisenberg");
			_embed.WithCurrentTimestamp();

			return _embed;
		}

        public static EmbedBuilder Create(SocketSelfUser user)
        {
            _embed = new EmbedBuilder();
            _embed.WithCurrentTimestamp();
            _embed.WithColor(Color.Gold);

            return _embed;
        }

        public static EmbedBuilder Create(SocketUser user)
        {
            _embed = new EmbedBuilder();
            _embed.WithCurrentTimestamp();
            _embed.WithColor(Color.Gold);
            _embed.WithFooter($"{user.Username}", user.GetAvatarUrl());

            return _embed;
        }

		public static EmbedBuilder Create(Deal deal)
		{
			_embed = new EmbedBuilder();

			if (deal.ExpireDate != null)
			{
				string expires = string.Format("{0:dd} day(s), {0:hh}h:{0:mm}m:{0:ss}s", deal.ExpireDate.Subtract(DateTime.Now));
				_embed.WithTitle("Expires in: " + expires);
			} else
			{
				_embed.WithTitle("No expiration date set");
			}

			_embed.WithDescription(deal.Url);
			_embed.WithCurrentTimestamp();
			_embed.WithColor(Color.DarkGreen);
			_embed.WithFooter($"{deal.User.Username}", deal.User.AvatarUrl);

			return _embed;
		}

		public static EmbedBuilder Create(Deal deal, IUserMessage msg)
		{
			_embed = new EmbedBuilder();
			var existingEmbed = msg.Embeds.First();

			if (deal.ExpireDate != null)
			{
				string expires = string.Format("{0:dd} day(s), {0:hh}h:{0:mm}m:{0:ss}s", deal.ExpireDate.Subtract(DateTime.Now));
				_embed.WithTitle("Expires in: " + expires);
			}
			else
			{
				_embed.WithTitle("No expiration date set");
			}

			_embed.WithImageUrl(existingEmbed.Image.ToString());
			_embed.WithDescription(existingEmbed.Description);
			_embed.WithCurrentTimestamp();
			_embed.WithColor(Color.DarkGreen);
			_embed.WithFooter($"{deal.User.Username}", deal.User.AvatarUrl);

			return _embed;
		}

		public static EmbedBuilder Create(List<Deal> dealList)
		{
			_embed = new EmbedBuilder();
			_embed.WithTitle("Active deals");
			_embed.WithCurrentTimestamp();
			_embed.WithColor(Color.DarkGreen);

			foreach (Deal offer in dealList)
			{
				string timeRemaining = String.Format("{0:dd} day(s), {0:hh}h:{0:mm}m:{0:ss}s", offer.ExpireDate.Subtract(DateTime.Now));
				_embed.AddField(offer.Url, $"Time remaining: {timeRemaining}");
			}

			return _embed;
		}

		public static EmbedBuilder Create(SocketTextChannel channel)
		{
			_embed = new EmbedBuilder();
			_embed.WithTitle(channel.Guild.Name);
			_embed.WithColor(Color.Gold);
			_embed.WithCurrentTimestamp();
			_embed.AddField("Default channel: ", channel.Mention);

			return _embed;
		}

		public static EmbedBuilder Create(Activity activity)
		{
			_embed = new EmbedBuilder();
			_embed.WithColor(Color.Gold);
			_embed.WithCurrentTimestamp();
			_embed.WithFooter($"{activity.User.Username}", activity.User.AvatarUrl);

			_embed.AddField(activity.Name, $"Scheduled for: {activity.DueTime}, Created on: {activity.ScheduledTime}");

			return _embed;
		}

		public static EmbedBuilder Create(List<Activity> activityList)
		{
			_embed = new EmbedBuilder();
			_embed.WithColor(Color.Gold);
			_embed.WithCurrentTimestamp();

			foreach (Activity activity in activityList)
			{
				_embed.AddField($"{activity.Id} - {activity.Name} {activity.Arguments}", $"Scheduled for: {activity.DueTime}, Created on: {activity.ScheduledTime}");
			}

			return _embed;
		}
	}
}
