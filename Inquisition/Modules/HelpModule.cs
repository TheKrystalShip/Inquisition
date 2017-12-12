using Discord;
using Discord.Commands;
using Inquisition.Data;
using System.Linq;
using System.Threading.Tasks;

namespace Inquisition.Modules
{
    public class HelpModule : ModuleBase<SocketCommandContext>
    {
        private CommandService _commands;

        public HelpModule(CommandService service)
        {
            _commands = service;
        }

        [Command("help"), Summary("List of all available commands.")]
        public async Task Help()
        {
            var embed = EmbedTemplate.Create(Context.Client.CurrentUser, Context.User);

            embed.Title = "Inquisition Help:";

            foreach (var c in _commands.Commands)
            {
                embed.AddField(c.Module.Aliases.FirstOrDefault() + " " + c.Name, c.Summary ?? "No specific description");
            }
            await Context.User.SendMessageAsync(Message.Info.Generic, false, embed.Build());
        }
    }
}
