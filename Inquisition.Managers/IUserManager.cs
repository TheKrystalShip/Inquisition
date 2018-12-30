using System.Collections.Generic;
using System.Threading.Tasks;
using Discord.WebSocket;

namespace TheKrystalShip.Inquisition.Managers
{
    public interface IUserManager
    {
        bool AddUser(SocketGuildUser socketGuildUser);
        Task OnClientJoinedGuildAsync(SocketGuild guild);
        Task OnClientReadyAsync(IReadOnlyCollection<SocketGuild> guilds);
        Task OnUserBannedAsync(SocketUser user, SocketGuild guild);
        Task OnUserJoinedAsync(SocketGuildUser user);
        Task OnUserLeftAsync(SocketGuildUser user);
        Task OnUserUnbannedAsync(SocketUser user, SocketGuild guild);
        Task OnUserUpdatedAsync(SocketUser before, SocketUser after);
        void RemoveUser(SocketGuildUser user);
    }
}