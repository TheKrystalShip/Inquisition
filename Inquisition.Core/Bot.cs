using Discord;
using Discord.WebSocket;

using System.Threading.Tasks;

using TheKrystalShip.DependencyInjection;
using TheKrystalShip.Inquisition.Core.Commands;
using TheKrystalShip.Inquisition.Managers;

namespace TheKrystalShip.Inquisition.Core
{
    public class Bot : DiscordSocketClient
    {
        public Bot(DiscordSocketConfig config) : base(config)
        {
            CommandHandler commandHandler = new CommandHandler();
            commandHandler.SetClient(this);
        }

        public void ConfigureEvents()
        {
            UserManager userManager = Container.Get<UserManager>();
            RoleManager roleManager = Container.Get<RoleManager>();
            ChannelManager channelManager = Container.Get<ChannelManager>();
            GuildManager guildManager = Container.Get<GuildManager>();

            ChannelCreated += channelManager.OnChannelCreatedAsync;
            ChannelDestroyed += channelManager.OnChannelDestroyedAsync;
            ChannelUpdated += channelManager.OnChannelUpdatedAsync;

            GuildMemberUpdated += guildManager.OnGuildMemberUpdatedAsync;
            GuildUpdated += guildManager.OnGuildUpdatedAsync;

            RoleCreated += roleManager.OnRoleCreatedAsync;
            RoleDeleted += roleManager.OnRoleDeletedAsync;
            RoleUpdated += roleManager.OnRoleUpdatedAsync;

            Ready += userManager.OnClientReadyAsync;
            JoinedGuild += userManager.OnClientJoinedGuildAsync;
            UserBanned += userManager.OnUserBannedAsync;
            UserJoined += userManager.OnUserJoinedAsync;
            UserLeft += userManager.OnUserLeftAsync;
            UserUnbanned += userManager.OnUserUnbannedAsync;
            UserUpdated += userManager.OnUserUpdatedAsync;
        }

        public async Task InitAsync(string token)
        {
            await LoginAsync(TokenType.Bot, token);
            await StartAsync();
            await SetGameAsync("God");
        }
    }
}
