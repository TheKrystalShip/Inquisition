using Discord.WebSocket;

using Inquisition.Database;
using Inquisition.Database.Models;

using System;
using System.Linq;

namespace Inquisition.Handlers
{
	public class ConversionHandler : Handler
    {
		private DatabaseContext db;
		public static int UsersAdded = 0;

		public ConversionHandler()
		{
			db = new DatabaseContext();
		}

		public void AddUser(SocketGuildUser socketGuildUser)
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

		public void RemoveUser(SocketGuildUser user)
		{
			string userId = user.Id.ToString();
			if (db.Users.Any(x => x.Id == userId))
			{
				User toRemove = db.Users.FirstOrDefault(x => x.Id == userId);
				db.Users.Remove(toRemove);
				db.SaveChanges();
			}
		}

		private Guild ToGuild(SocketGuild socketGuild)
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

		public override void Dispose()
		{
			throw new NotImplementedException();
		}
	}
}
