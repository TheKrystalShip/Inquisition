using Discord.WebSocket;

using Inquisition.Data.Handlers;
using Inquisition.Data.Models;

using System.Linq;

namespace Inquisition.Handlers
{
	public class ConversionHandler
    {
		private static DbHandler db;

		public static void AddUser(SocketGuildUser socketGuildUser)
		{
			db = new DbHandler();
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

				var state = db.Entry(user.Guild).State;

				db.Users.Add(user);
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
