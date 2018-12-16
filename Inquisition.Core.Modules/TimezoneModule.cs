using Discord.Commands;

using System.Threading.Tasks;

using TheKrystalShip.Inquisition.Domain;

namespace TheKrystalShip.Inquisition.Core.Modules
{
    public class TimezoneModule : Module
    {
        public TimezoneModule()
        {

        }

        [Command("timezone")]
        [Summary("Tells you your timezone from the database")]
        public async Task ShowTimezoneAsync()
        {
            await ReplyAsync($"Your timezone is set to: {User.TimezoneOffset}");
        }

        [Command("set timezone")]
        [Summary("Set your timezone")]
        public async Task SetTimezoneAsync(int offset)
        {
            User.TimezoneOffset = offset;
            await ReplyAsync($"Your timezone is set to: {User.TimezoneOffset}");
        }
    }
}
