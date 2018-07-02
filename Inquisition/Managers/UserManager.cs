using Discord.WebSocket;

using Inquisition.Database;
using Inquisition.Database.Models;
using Inquisition.Logging;

using System.Linq;

namespace Inquisition.Managers
{
    public class UserManager
    {
        private readonly DatabaseContext _dbContext;
        private readonly ILogger<UserManager> _logger;

        public int UsersAdded { get; private set; }

        public UserManager(
            DatabaseContext dbContext,
            ILogger<UserManager> logger)
        {
            UsersAdded = 0;
            _dbContext = dbContext;
            _logger = logger;
        }

        public void Init()
        {
            _logger.LogInformation("Starting user registration...");
        }

        public void Dispose()
        {
            _logger.LogInformation(UsersAdded > 0 ? $"Done, added {UsersAdded} user(s)" : "Done, no new users were added");
        }

		public void AddUser(SocketGuildUser socketGuildUser)
		{
			string socketUserId = socketGuildUser.Id.ToString();
			if (!_dbContext.Users.Any(x => x.Id == socketUserId))
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

				_dbContext.Users.Add(user);
				_dbContext.SaveChanges();
				UsersAdded++;
			}
		}

		public void RemoveUser(SocketGuildUser user)
		{
			string userId = user.Id.ToString();
			if (_dbContext.Users.Any(x => x.Id == userId))
			{
				User toRemove = _dbContext.Users.FirstOrDefault(x => x.Id == userId);
				_dbContext.Users.Remove(toRemove);
				_dbContext.SaveChanges();
			}
		}

		private Guild ToGuild(SocketGuild socketGuild)
		{
			string socketGuildId = socketGuild.Id.ToString();
			return _dbContext.Guilds.FirstOrDefault(x => x.Id == socketGuildId) ??
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
