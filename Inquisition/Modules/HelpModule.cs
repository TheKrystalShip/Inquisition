using Discord;
using Discord.Commands;
using Inquisition.Data;
using System.Linq;
using System.Threading.Tasks;

namespace Inquisition.Modules
{
    [Group("help")]
    public class HelpModule : ModuleBase<SocketCommandContext>
    {
        private static CommandService _commands;

        public static void Create(CommandService commandService)
        {
            _commands = commandService;
        }

        [Command, Summary("List of all available commands.")]
        public async Task Help()
        {
            var embed = EmbedTemplate.Create(Context.Client.CurrentUser, Context.User);

            embed.Title = "Inquisition Help:";

            foreach (var c in _commands.Commands)
            {
                string str = "";
                foreach (var a in c.Aliases)
                {
                    if (a != null)
                    {
                        str += a + " | "; 
                    }
                }
                embed.AddField(c.Module.Aliases.FirstOrDefault() + " " + c.Name, $"Aliases: {str}\n\n{c.Summary ?? "No specific description"}");
            }
            await Context.User.SendMessageAsync(Message.Info.Generic, false, embed.Build());
        }
    }
}
