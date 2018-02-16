using Discord.WebSocket;

using Inquisition.Data.Handlers;
using Inquisition.Data.Models;

using System.Linq;

namespace Inquisition.Handlers
{
	public class ConversionHandler
    {
		public static void AddUser(SocketGuildUser user)
		{
			DbHandler db = new DbHandler();
			if (!db.Users.Any(x => x.Id == user.Id.ToString()))
			{
				db.Users.Add(new User
					{
						Username = user.Username,
						Id = user.Id.ToString(),
						Nickname = user.Nickname,
						AvatarUrl = user.GetAvatarUrl(),
						Discriminator = user.Discriminator,
						Guild = ToGuild(user.Guild)
					}
				);
				db.SaveChanges();
			}
		}

		private static Guild ToGuild(SocketGuild socketGuild)
		{
			DbHandler db = new DbHandler();
			return db.Guilds.FirstOrDefault(x => x.Id == socketGuild.Id.ToString()) ??
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
