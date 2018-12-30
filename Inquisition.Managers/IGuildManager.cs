using System.Threading.Tasks;
using Discord.WebSocket;

namespace TheKrystalShip.Inquisition.Managers
{
    public interface IGuildManager
    {
        Task OnGuildMemberUpdatedAsync(SocketGuildUser before, SocketGuildUser after);
        Task OnGuildUpdatedAsync(SocketGuild before, SocketGuild after);
    }
}