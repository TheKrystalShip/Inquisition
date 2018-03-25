using Discord.WebSocket;

using Inquisition.Database.Core;
using Inquisition.Database.Models;

using System.Linq;

namespace Inquisition.Handlers
{
	public class ConversionHandler : BaseHandler
    {
		private static DatabaseContext db;
		public static int UsersAdded = 0;

		static ConversionHandler() => db = new DatabaseContext();

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
