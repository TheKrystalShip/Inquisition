using Discord.WebSocket;
using Inquisition.Database.Commands;
using Inquisition.Database.Models;

namespace Inquisition.Core.Handlers
{
	public class ConversionHandler
    {
		public static User ToUser(SocketGuildUser user)
		{
			return new User
			{
				Username = user.Username,
				Id = user.Id.ToString(),
				Nickname = user.Nickname,
				AvatarUrl = user.GetAvatarUrl(),
				Discriminator = user.Discriminator,
				Guild = ToGuild(user.Guild)
			};
		}

		public static Guild ToGuild(SocketGuild guild)
		{
			return new Guild
			{
				Id = guild.Id.ToString(),
				Name = guild.Name.ToString()
			};
		}

		public static User GrabUserFromDb(SocketGuildUser user)
		{
			User temp = ToUser(user);
			User u = Select.User(temp);
			return u;
		}

		public static User GrabFromDb(SocketUser user)
		{
			User temp = ToUser(user as SocketGuildUser);
			User u = Select.User(temp);
			return u;
		}
	}
}
