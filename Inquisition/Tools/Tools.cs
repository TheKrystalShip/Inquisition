using Discord.Commands;
using Discord.WebSocket;

using TheKrystalShip.Inquisition.Database;
using TheKrystalShip.Inquisition.Handlers;
using TheKrystalShip.Logging;

namespace TheKrystalShip.Inquisition
{
    public class Tools
    {
        public SQLiteContext Database { get; private set; }
        public ILogger<Tools> Logger { get; private set; }
        public ReportHandler Report { get; private set; }
        public CommandService CommandService { get; private set; }
        public DiscordSocketClient Client { get; private set; }
        public PrefixHandler Prefix { get; private set; }

        public Tools(SQLiteContext database, ILogger<Tools> logger, ReportHandler report, CommandService commandService, DiscordSocketClient client, PrefixHandler prefix)
        {
            Database = database;
            Logger = logger;
            Report = report;
            CommandService = commandService;
            Client = client;
            Prefix = prefix;
        }
    }
}
