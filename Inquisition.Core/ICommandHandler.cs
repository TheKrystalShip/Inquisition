using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;

namespace TheKrystalShip.Inquisition.Core
{
    public interface ICommandHandler
    {
        Task LoadModulesAsync();
        Task OnClientMessageRecievedAsync(SocketMessage socketMessage);
        Task OnLog(LogMessage logMessage);
    }
}