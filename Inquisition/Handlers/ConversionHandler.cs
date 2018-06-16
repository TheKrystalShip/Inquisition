using Discord.WebSocket;

using Inquisition.Database;
using Inquisition.Database.Models;

using System;
using System.Linq;

namespace Inquisition.Handlers
{
	public static class ConversionHandler
    {
		private static DatabaseContext db = new DatabaseContext();
		public static int UsersAdded = 0;

		public static void AddUser(SocketGuildUser socketGuildUser)
		{
			string socketUserId = socketGuildUser.Id.ToString();
			if (!db.Users.Any(x => x.Id == socketUserId))
			{
				Guild guild = ToGuild(socketGuildUser.Guild);
				User user = new User
				{
					Username = socketGuildUser.Username,
					Id = socketUserId,
					Nickname = socketGuildUser.Nickname,
					AvatarUrl = socketGuildUser.GetAvatarUrl(),
					Discriminator = socketGuildUser.Discriminator,
					Guild = guild
				};

				db.Users.Add(user);
				db.SaveChanges();
				UsersAdded++;
			}
		}

		public static void RemoveUser(SocketGuildUser user)
		{
			string userId = user.Id.ToString();
			if (db.Users.Any(x => x.Id == userId))
			{
				User toRemove = db.Users.FirstOrDefault(x => x.Id == userId);
				db.Users.Remove(toRemove);
				db.SaveChanges();
			}
		}

		private static Guild ToGuild(SocketGuild socketGuild)
		{
			string socketGuildId = socketGuild.Id.ToString();
			return db.Guilds.FirstOrDefault(x => x.Id == socketGuildId) ??
				new Guild
				{
					Name = socketGuild.Name,
					IconUrl = socketGuild.IconUrl,
					Id = socketGuild.Id.ToString(),
					MemberCount = socketGuild.MemberCount
				};
		}
	}
}
