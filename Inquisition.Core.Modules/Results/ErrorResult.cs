using Discord;
using Discord.Commands;

namespace TheKrystalShip.Inquisition.Core.Modules
{
    public class ErrorResult : Result
    {
        // Redirect all constructors to this one
        public ErrorResult(CommandError? error, string reason) : base(error, reason)
        {

        }

        public ErrorResult(string message) : this(null, message)
        {

        }

        public ErrorResult(string message, EmbedBuilder embedBuilder) : this(null, message)
        {
            Embed = embedBuilder;
        }
    }
}
