using Discord;
using Discord.Commands;

namespace TheKrystalShip.Inquisition.Core.Modules
{
    public class SuccessResult : Result
    {
        // Redirect all constructors to this one
        public SuccessResult(CommandError? error, string reason) : base(error, reason)
        {

        }

        public SuccessResult(string message) : this(null, message)
        {

        }

        public SuccessResult(string message, EmbedBuilder embedBuilder) : this(null, message)
        {
            Embed = embedBuilder;
        }
    }
}
