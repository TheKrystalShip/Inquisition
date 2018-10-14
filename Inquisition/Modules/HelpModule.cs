using Discord;
using Discord.Commands;

using System.Linq;
using System.Threading.Tasks;

using TheKrystalShip.Inquisition.Extensions;
using TheKrystalShip.Inquisition.Handlers;

namespace TheKrystalShip.Inquisition.Modules
{
    public class HelpModule : Module
    {
        private readonly CommandService _commandService;

        public HelpModule(CommandService commandService, Tools tools) : base(tools)
        {
            _commandService = commandService;
        }

        [Command("help")]
        [Summary("List of all available commands.")]
        public async Task Help()
        {
            EmbedBuilder embed = new EmbedBuilder()
                .Create(Context.User);

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
    }
}
