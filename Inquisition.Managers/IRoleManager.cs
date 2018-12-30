using System.Threading.Tasks;
using Discord.WebSocket;

namespace TheKrystalShip.Inquisition.Managers
{
    public interface IRoleManager
    {
        Task OnRoleCreatedAsync(SocketRole role);
        Task OnRoleDeletedAsync(SocketRole role);
        Task OnRoleUpdatedAsync(SocketRole before, SocketRole after);
    }
}