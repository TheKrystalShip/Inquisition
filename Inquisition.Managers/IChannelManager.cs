using System.Threading.Tasks;
using Discord.WebSocket;

namespace TheKrystalShip.Inquisition.Managers
{
    public interface IChannelManager
    {
        Task OnChannelCreatedAsync(SocketChannel socketChannel);
        Task OnChannelDestroyedAsync(SocketChannel socketChannel);
        Task OnChannelUpdatedAsync(SocketChannel beforeSocketChannel, SocketChannel afterSocketChannel);
    }
}