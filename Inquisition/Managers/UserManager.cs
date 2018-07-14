using Discord.WebSocket;

using Inquisition.Database;
using Inquisition.Database.Models;
using Inquisition.Logging;

using System.Linq;
using System.Threading.Tasks;

namespace Inquisition.Managers
{
    public class UserManager
    {
        private readonly DatabaseContext _dbContext;
        private readonly ILogger<UserManager> _logger;

        public int UsersAdded { get; private set; }

        public UserManager(DatabaseContext dbContext, ILogger<UserManager> logger)
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
					AvatarUrl = socketGuildUser.GetAvatarUrl(),
					Discriminator = socketGuildUser.Discriminator,
				};

                user.Servers.Add(new Server() {
                    User = user,
                    Guild = guild,
                    Nickname = socketGuildUser.Nickname
                });

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

        public Task UserBanned(SocketUser user, SocketGuild guild)
        {
            _logger.LogInformation($"User banned: {user.Username} in {guild.Name}");
            return Task.CompletedTask;
        }

        public Task UserJoined(SocketGuildUser user)
        {
            _logger.LogInformation($"User joined: {user.Username} in {user.Guild.Name}");
            return Task.CompletedTask;
        }

        public Task UserLeft(SocketGuildUser user)
        {
            _logger.LogInformation($"User left: {user.Username} in {user.Guild.Name}");
            return Task.CompletedTask;
        }

        public Task UserUnbanned(SocketUser user, SocketGuild guild)
        {
            _logger.LogInformation($"User unbanned: {user.Username} in {guild.Name}");
            return Task.CompletedTask;
        }

        public Task UserUpdated(SocketUser before, SocketUser after)
        {
            SocketGuildUser guildUser = before as SocketGuildUser;
            _logger.LogInformation($"User updated: {guildUser.Username} in {guildUser.Guild.Name}");
            return Task.CompletedTask;
        }
	}
}
