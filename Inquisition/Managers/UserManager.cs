using Discord.WebSocket;

using System;
using System.Linq;
using System.Threading.Tasks;

using TheKrystalShip.Inquisition.Database;
using TheKrystalShip.Inquisition.Database.Models;
using TheKrystalShip.Logging;

namespace TheKrystalShip.Inquisition.Managers
{
    public class UserManager
    {
        private readonly DatabaseContext _dbContext;
        private readonly DiscordSocketClient _client;
        private readonly ILogger<UserManager> _logger;

        public UserManager(DatabaseContext dbContext, DiscordSocketClient client, ILogger<UserManager> logger)
        {
            _dbContext = dbContext;
            _client = client;
            _logger = logger;
        }

        public async Task RegisterGuildUsersAsync()
        {
            await Task.Run(() =>
            {
                int guildsAdded = 0;
                int usersAdded = 0;
                int totalUsersAdded = 0;

                _logger.LogInformation($"Starting user registration");

                try
                {
                    foreach (SocketGuild guild in _client.Guilds)
                    {
                        _logger.LogInformation($"Starting user registration for guild: {guild.Name}");

                        foreach (SocketGuildUser user in guild.Users)
                        {
                            if (!user.IsBot)
                            {
                                if (AddUser(user))
                                {
                                    usersAdded++;
                                    totalUsersAdded++;
                                }
                            }
                        }

                        if (usersAdded > 0)
                            _logger.LogInformation($"Added {usersAdded} users from: {guild.Name}");

                        guildsAdded++;
                        usersAdded = 0;
                    }

                    _logger.LogInformation($"Added {guildsAdded} guilds with a total of {totalUsersAdded} new users");

                }
                catch (Exception e)
                {
                    _logger.LogError(e);
                }
            });
        }

        public async Task RegisterNewGuildUsersAsync(SocketGuild guild)
        {
            await Task.Run(() =>
            {
                _logger.LogInformation($"Joined guild: {guild.Name}");
                _logger.LogInformation($"Starting user registration for: {guild.Name}");
                int usersAdded = 0;

                foreach (SocketGuildUser user in guild.Users)
                {
                    if (AddUser(user))
                        usersAdded++;
                }

                _logger.LogInformation($"Added {usersAdded} users from: {guild.Name}");
                _logger.LogInformation($"Finished user registration for: {guild.Name}");
            });
        }

        public bool AddUser(SocketGuildUser socketGuildUser)
        {
            string socketUserId = socketGuildUser.Id.ToString();

            if (_dbContext.Users.Any(x => x.Id == socketUserId))
                return false;

            Guild guild = ToGuild(socketGuildUser.Guild);
            User user = new User
            {
                Username = socketGuildUser.Username,
                Id = socketUserId,
                AvatarUrl = socketGuildUser.GetAvatarUrl(),
                Discriminator = socketGuildUser.Discriminator,
            };

            user.Servers.Add(new Server()
            {
                User = user,
                Guild = guild,
                Nickname = socketGuildUser.Nickname
            });

            _dbContext.Users.Add(user);
            _dbContext.SaveChanges();
            return true;
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

        public Task UserBannedAsync(SocketUser user, SocketGuild guild)
        {
            _logger.LogInformation($"User banned: {user.Username} in {guild.Name}");
            return Task.CompletedTask;
        }

        public Task UserJoinedAsync(SocketGuildUser user)
        {
            _logger.LogInformation($"User joined: {user.Username} in {user.Guild.Name}");
            return Task.CompletedTask;
        }

        public Task UserLeftAsync(SocketGuildUser user)
        {
            _logger.LogInformation($"User left: {user.Username} in {user.Guild.Name}");
            return Task.CompletedTask;
        }

        public Task UserUnbannedAsync(SocketUser user, SocketGuild guild)
        {
            _logger.LogInformation($"User unbanned: {user.Username} in {guild.Name}");
            return Task.CompletedTask;
        }

        public Task UserUpdatedAsync(SocketUser before, SocketUser after)
        {
            SocketGuildUser guildUser = before as SocketGuildUser;
            _logger.LogInformation($"User updated: {guildUser.Username} in {guildUser.Guild.Name}");
            return Task.CompletedTask;
        }
    }
}
