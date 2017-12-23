using Discord;
using Discord.Commands;
using System.Linq;
using System.Threading.Tasks;
using Inquisition.Data;

namespace Inquisition.Modules
{
    [Group("help")]
    public class HelpModule : ModuleBase<SocketCommandContext>
    {
        private static CommandService CommandService;

        public static void Create(CommandService commandService)
        {
            CommandService = commandService;
        }

        [Command, Summary("List of all available commands.")]
        public async Task Help()
        {
            var embed = EmbedTemplate.Create(Context.Client.CurrentUser, Context.User);

            embed.Title = "Inquisition Help:";

            foreach (var c in CommandService.Commands)
            {
                string str = "";
                foreach (var a in c.Aliases.Skip(1))
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
