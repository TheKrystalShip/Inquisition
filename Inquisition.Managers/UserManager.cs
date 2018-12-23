using Discord.WebSocket;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using TheKrystalShip.Inquisition.Database;
using TheKrystalShip.Inquisition.Domain;
using TheKrystalShip.Logging;

namespace TheKrystalShip.Inquisition.Managers
{
    public class UserManager
    {
        private readonly IDbContext _dbContext;
        private readonly ILogger<UserManager> _logger;

        public UserManager(IDbContext dbContext)
        {
            _dbContext = dbContext;
            _logger = new Logger<UserManager>();
        }

        public async Task OnClientReadyAsync(IReadOnlyCollection<SocketGuild> guilds)
        {
            _ = Task.Run(() =>
            {
                int guildsAdded = 0;
                int usersAdded = 0;
                int totalUsersAdded = 0;

                _logger.LogInformation($"Starting user registration");

                try
                {
                    foreach (SocketGuild guild in guilds)
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

        public async Task OnClientJoinedGuildAsync(SocketGuild guild)
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
            if (_dbContext.Users.Any(x => x.Id == socketGuildUser.Id))
                return false;

            Guild guild = ToGuild(socketGuildUser.Guild);
            User user = new User
            {
                Username = socketGuildUser.Username,
                Id = socketGuildUser.Id,
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
            if (_dbContext.Users.Any(x => x.Id == user.Id))
            {
                User toRemove = _dbContext.Users.FirstOrDefault(x => x.Id == user.Id);
                _dbContext.Users.Remove(toRemove);
                _dbContext.SaveChanges();
            }
        }

        private Guild ToGuild(SocketGuild socketGuild)
        {
            return _dbContext.Guilds.FirstOrDefault(x => x.Id == socketGuild.Id) ??
                new Guild
                {
                    Name = socketGuild.Name,
                    IconUrl = socketGuild.IconUrl,
                    Id = socketGuild.Id,
                    MemberCount = socketGuild.MemberCount
                };
        }

        public Task OnUserBannedAsync(SocketUser user, SocketGuild guild)
        {
            _logger.LogInformation($"User banned: {user.Username} in {guild.Name}");
            return Task.CompletedTask;
        }

        public Task OnUserJoinedAsync(SocketGuildUser user)
        {
            _logger.LogInformation($"User joined: {user.Username} in {user.Guild.Name}");
            return Task.CompletedTask;
        }

        public Task OnUserLeftAsync(SocketGuildUser user)
        {
            _logger.LogInformation($"User left: {user.Username} in {user.Guild.Name}");
            return Task.CompletedTask;
        }

        public Task OnUserUnbannedAsync(SocketUser user, SocketGuild guild)
        {
            _logger.LogInformation($"User unbanned: {user.Username} in {guild.Name}");
            return Task.CompletedTask;
        }

        public Task OnUserUpdatedAsync(SocketUser before, SocketUser after)
        {
            SocketGuildUser guildUser = before as SocketGuildUser;

            if (guildUser is null)
                return Task.CompletedTask;

            _logger.LogInformation($"User updated: {guildUser.Username} in {guildUser.Guild.Name}");
            return Task.CompletedTask;
        }
    }
}
