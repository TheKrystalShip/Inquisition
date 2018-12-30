using Discord;
using Discord.Commands;
using Discord.WebSocket;

using Microsoft.EntityFrameworkCore;

using System.IO;
using System.Threading.Tasks;

using TheKrystalShip.DependencyInjection;
using TheKrystalShip.Inquisition.Core;
using TheKrystalShip.Inquisition.Core.Commands;
using TheKrystalShip.Inquisition.Database;
using TheKrystalShip.Inquisition.Database.SQLite;
using TheKrystalShip.Inquisition.Handlers;
using TheKrystalShip.Inquisition.Managers;
using TheKrystalShip.Inquisition.Services;
using TheKrystalShip.Tools.Configuration;

namespace TheKrystalShip.Inquisition
{
    public class Startup
    {
        public Startup()
        {
            Configuration.SetBasePath(Path.Combine(Directory.GetCurrentDirectory(), "Properties"));
            Configuration.AddFiles("secrets.json", "settings.json");
        }

        public Startup ConfigureDatabase()
        {
            DbContextOptionsBuilder<SQLiteContext> builder = new DbContextOptionsBuilder<SQLiteContext>();
            builder.UseSqlite(Configuration.GetConnectionString("SQLite"));
            SQLiteContext context = new SQLiteContext(builder.Options);

            context.Migrate();

            Container.Add<IDbContext>(context);

            return this;
        }

        public Startup ConfigureServices()
        {
            Container.Add<IPrefixHandler, PrefixHandler>();

            Container.Add<IChannelManager, ChannelManager>();
            Container.Add<IGuildManager, GuildManager>();
            Container.Add<IRoleManager, RoleManager>();
            Container.Add<IUserManager, UserManager>();

            Container.Add<ActivityService>();
            Container.Add<AudioService>();
            Container.Add<GameService>();
            Container.Add<ReminderService>();

            return this;
        }

        public Startup ConfigureClient()
        {
            Bot client = new Bot(new DiscordSocketConfig() {
                LogLevel = LogSeverity.Info,
                DefaultRetryMode = RetryMode.AlwaysRetry,
                ConnectionTimeout = 5000,
                AlwaysDownloadUsers = true,
                HandlerTimeout = 1000,
                LargeThreshold = 250
            });

            CommandHandler commandHandler = new CommandHandler(client, new CommandServiceConfig() {
                CaseSensitiveCommands = false,
                DefaultRunMode = RunMode.Async,
                IgnoreExtraArgs = false,
                LogLevel = LogSeverity.Debug,
                ThrowOnError = true
            });

            client.Log += commandHandler.OnLog;
            client.MessageReceived += commandHandler.OnClientMessageRecievedAsync;
            client.InitAsync(Configuration.Get("Bot:Token")).Wait();

            commandHandler.Log += commandHandler.OnLog;
            commandHandler.CommandExecuted += Dispatcher.Dispatch;

            Container.Add(client);
            Container.Add<ICommandHandler>(commandHandler);

            commandHandler.LoadModulesAsync().Wait();

            return this;
        }

        public Startup ConfigureEvents()
        {
            Bot client = Container.Get<Bot>();

            IUserManager userManager = Container.Get<IUserManager>();
            IRoleManager roleManager = Container.Get<IRoleManager>();
            IChannelManager channelManager = Container.Get<IChannelManager>();
            IGuildManager guildManager = Container.Get<IGuildManager>();

            client.ChannelCreated += channelManager.OnChannelCreatedAsync;
            client.ChannelDestroyed += channelManager.OnChannelDestroyedAsync;
            client.ChannelUpdated += channelManager.OnChannelUpdatedAsync;

            client.GuildMemberUpdated += guildManager.OnGuildMemberUpdatedAsync;
            client.GuildUpdated += guildManager.OnGuildUpdatedAsync;

            client.RoleCreated += roleManager.OnRoleCreatedAsync;
            client.RoleDeleted += roleManager.OnRoleDeletedAsync;
            client.RoleUpdated += roleManager.OnRoleUpdatedAsync;

            client.Ready += () => userManager.OnClientReadyAsync(client.Guilds);
            client.JoinedGuild += userManager.OnClientJoinedGuildAsync;
            client.UserBanned += userManager.OnUserBannedAsync;
            client.UserJoined += userManager.OnUserJoinedAsync;
            client.UserLeft += userManager.OnUserLeftAsync;
            client.UserUnbanned += userManager.OnUserUnbannedAsync;
            client.UserUpdated += userManager.OnUserUpdatedAsync;

            return this;
        }

        public async Task InitAsync()
        {
            await Task.Delay(-1);
        }
    }
}
