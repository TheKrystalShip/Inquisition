using Discord.WebSocket;

using System.Threading.Tasks;

using TheKrystalShip.Logging;

namespace TheKrystalShip.Inquisition.Managers
{
    public class RoleManager
    {
        private readonly ILogger<RoleManager> _logger;

        public RoleManager(ILogger<RoleManager> logger)
        {
            _logger = logger;
        }

        public Task RoleCreatedAsync(SocketRole role)
        {
            _logger.LogInformation($"Role created: {role.Name} in {role.Guild.Name}");
            return Task.CompletedTask;
        }

        public Task RoleDeletedAsync(SocketRole role)
        {
            _logger.LogInformation($"Role deleted: {role.Name} in {role.Guild.Name}");
            return Task.CompletedTask;
        }

        public Task RoleUpdated(SocketRole before, SocketRole after)
        {
            _logger.LogInformation($"Role updated: {before.Name} in {before.Guild.Name}");
            return Task.CompletedTask;
        }
    }
}
