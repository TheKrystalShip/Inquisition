using Discord;
using Discord.WebSocket;

using Microsoft.EntityFrameworkCore;

using System.Threading.Tasks;

using TheKrystalShip.DependencyInjection;
using TheKrystalShip.Inquisition.Core;
using TheKrystalShip.Inquisition.Database.SQLite;
using TheKrystalShip.Inquisition.Handlers;
using TheKrystalShip.Inquisition.Managers;
using TheKrystalShip.Inquisition.Services;
using TheKrystalShip.Inquisition.Tools;

namespace TheKrystalShip.Inquisition
{
    public class Startup
    {
        public Startup ConfigureDatabase()
        {
            DbContextOptionsBuilder<SQLiteContext> builder = new DbContextOptionsBuilder<SQLiteContext>();
            builder.UseSqlite(Configuration.GetConnectionString("SQLite"));
            SQLiteContext context = new SQLiteContext(builder.Options);

            Container.Add(context);

            return this;
        }

        public Startup ConfigureHandlers()
        {
            Container.Add<PrefixHandler>();

            return this;
        }

        public Startup ConfigureManagers()
        {
            Container.Add<ChannelManager>();
            Container.Add<GuildManager>();
            Container.Add<RoleManager>();
            Container.Add<UserManager>();

            return this;
        }

        public Startup ConfigureServices()
        {
            Container.Add<ActivityService>();
            Container.Add<AudioService>();
            Container.Add<GameService>();
            Container.Add<ReminderService>();

            return this;
        }

        public Startup ConfigureClient()
        {
            Bot client = new Bot(new DiscordSocketConfig()
                {
                    LogLevel = LogSeverity.Info,
                    DefaultRetryMode = RetryMode.AlwaysRetry
                }
            );

            client.InitAsync(Configuration.Get("Bot:Token")).Wait();

            return this;
        }

        public async Task InitAsync()
        {
            await Task.Delay(-1);
        }
    }
}
