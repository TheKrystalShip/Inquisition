using Discord;
using Discord.Commands;

using Inquisition.Handlers;

using System;
using System.Linq;
using System.Threading.Tasks;

using TheKrystalShip.Logging;

namespace Inquisition.Modules
{
    public class HelpModule : ModuleBase<SocketCommandContext>
    {
        private readonly CommandService _commandService;
        private readonly ReportHandler _reportHandler;
        private readonly ILogger<HelpModule> _logger;

        public HelpModule(
            CommandService commandService,
            ReportHandler reportHandler,
            ILogger<HelpModule> logger)
        {
            _commandService = commandService;
            _reportHandler = reportHandler;
            _logger = logger;
        }

        [Command("help")]
        [Summary("List of all available commands.")]
        public async Task Help()
        {
            try
            {
                EmbedBuilder embed = EmbedHandler.Create(Context.User);

                embed.Title = "Inquisition Help:";

                foreach (CommandInfo c in _commandService.Commands)
                {
                    string str = "";
                    foreach (string a in c.Aliases.Skip(1))
                    {
                        if (a != null)
                        {
                            str += a + " | ";
                        }
                    }
                    embed.AddField(c.Module.Aliases.FirstOrDefault() + " " + c.Name, $"Aliases: {str}\n\n{c.Summary ?? "No specific description"}");
                }
                await Context.User.SendMessageAsync(ReplyHandler.Generic, false, embed.Build());
            }
            catch (Exception e)
            {
                _reportHandler.ReportAsync(Context, e);
                _logger.LogError(e);
            }
        }
    }
}
