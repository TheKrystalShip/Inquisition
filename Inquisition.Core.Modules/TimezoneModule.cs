using Discord.Commands;

using System.Threading.Tasks;

using TheKrystalShip.Inquisition.Domain;

namespace TheKrystalShip.Inquisition.Core.Modules
{
    public class TimezoneModule : Module
    {
        [Command("timezone")]
        [Summary("Tells you your timezone from the database")]
        public async Task<RuntimeResult> ShowTimezoneAsync()
        {
            if (User.TimezoneOffset is null)
            {
                return new ErrorResult(CommandError.UnmetPrecondition, "Timezone not set");
            }

            return new InfoResult($"Your timezone is set to: {User.TimezoneOffset}");
        }

        [Command("set timezone")]
        [Summary("Set your timezone")]
        public async Task<RuntimeResult> SetTimezoneAsync(int offset)
        {
            User.TimezoneOffset = offset;
            return new InfoResult($"Your timezone is set to: {User.TimezoneOffset}");
        }
    }
}
