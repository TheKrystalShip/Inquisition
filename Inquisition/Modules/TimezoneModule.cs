using Discord.Commands;

using System.Threading.Tasks;

using TheKrystalShip.Inquisition.Domain;
using TheKrystalShip.Inquisition.Handlers;

namespace TheKrystalShip.Inquisition.Modules
{
    public class TimezoneModule : Module
    {
        public TimezoneModule(Tools tools) : base(tools)
        {

        }

        [Command("timezone")]
        [Summary("Tells you your timezone from the database")]
        public async Task ShowTimezoneAsync()
        {
            await ReplyAsync(ReplyHandler.Info.UserTimezone(User));
        }

        [Command("set timezone")]
        [Summary("Set your timezone")]
        public async Task SetTimezoneAsync(int offset)
        {
            User.TimezoneOffset = offset;
            await ReplyAsync(ReplyHandler.Info.UserTimezone(User));
        }
    }
}
