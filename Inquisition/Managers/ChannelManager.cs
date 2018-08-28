using Discord.WebSocket;

using System.Threading.Tasks;

using TheKrystalShip.Logging;

namespace TheKrystalShip.Inquisition.Managers
{
    public class ChannelManager
    {
        private readonly ILogger<ChannelManager> _logger;

        public ChannelManager(ILogger<ChannelManager> logger)
        {
            _logger = logger;
        }

        public Task ChannelCreatedAsync(SocketChannel socketChannel)
        {
            SocketGuildChannel channel = socketChannel as SocketGuildChannel;
            _logger.LogInformation($"Channel created: {channel.Name} in {channel.Guild.Name}");
            return Task.CompletedTask;
        }

        public Task ChannelDestroyedAsync(SocketChannel socketChannel)
        {
            SocketGuildChannel channel = socketChannel as SocketGuildChannel;
            _logger.LogInformation($"Channel destroyed: {channel.Name} in {channel.Guild.Name}");
            return Task.CompletedTask;
        }

        public Task ChannelUpdatedAsync(SocketChannel beforeSocketChannel, SocketChannel afterSocketChannel)
        {
            SocketGuildChannel before = beforeSocketChannel as SocketGuildChannel;
            SocketGuildChannel after = afterSocketChannel as SocketGuildChannel;
            _logger.LogInformation($"Channel updated: {before.Name} in {before.Guild.Name}");
            return Task.CompletedTask;
        }
    }
}
