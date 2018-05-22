using Discord;
using Discord.Commands;

using Inquisition.Handlers;
using Inquisition.Services;

using System.Linq;
using System.Threading.Tasks;

namespace Inquisition.Modules
{
	public class HelpModule : ModuleBase<SocketCommandContext>
    {
        private static CommandService CommandService;

        public HelpModule(CommandService commandService)
        {
            CommandService = commandService;
        }

        [Command("help")]
        [Summary("List of all available commands.")]
        public async Task Help()
        {
            try
            {
                var embed = EmbedHandler.Create(Context.User);

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
                await Context.User.SendMessageAsync(ReplyHandler.Generic, false, embed.Build());
            }
            catch (System.Exception e)
            {
                ReportHandler.Report(Context, e);
            }
        }
    }
}
