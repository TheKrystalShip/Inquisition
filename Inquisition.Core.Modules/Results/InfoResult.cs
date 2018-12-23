using Discord;
using Discord.Commands;

namespace TheKrystalShip.Inquisition.Core.Modules
{
    public class InfoResult : Result
    {
        // Redirect all constructors to this one
        public InfoResult(CommandError? error, string reason) : base(error, reason)
        {

        }

        public InfoResult(string message) : this(null, message)
        {

        }

        public InfoResult(string message, EmbedBuilder embedBuilder) : this(null, message)
        {
            Embed = embedBuilder;
        }
    }
}
