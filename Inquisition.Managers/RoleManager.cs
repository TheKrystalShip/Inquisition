using Discord.WebSocket;

using System.Threading.Tasks;

using TheKrystalShip.Logging;

namespace TheKrystalShip.Inquisition.Managers
{
    public class RoleManager : IRoleManager
    {
        private readonly ILogger<RoleManager> _logger;

        public RoleManager()
        {
            _logger = new Logger<RoleManager>();
        }

        public Task OnRoleCreatedAsync(SocketRole role)
        {
            _logger.LogInformation($"Role created: {role.Name} in {role.Guild.Name}");
            return Task.CompletedTask;
        }

        public Task OnRoleDeletedAsync(SocketRole role)
        {
            _logger.LogInformation($"Role deleted: {role.Name} in {role.Guild.Name}");
            return Task.CompletedTask;
        }

        public Task OnRoleUpdatedAsync(SocketRole before, SocketRole after)
        {
            _logger.LogInformation($"Role updated: {before.Name} in {before.Guild.Name}");
            return Task.CompletedTask;
        }
    }
}
