using Discord;
using Discord.Commands;

namespace TheKrystalShip.Inquisition.Core.Modules
{
    public class WarningResult : RuntimeResult
    {
        public EmbedBuilder EmbedBuilder { get; set; }
        public Embed Embed { get; set; }

        // Redirect all constructors to this one
        public WarningResult(CommandError? error, string message) : base(error, message)
        {

        }

        public WarningResult(string message) : this(null, message)
        {

        }

        public WarningResult(string message, EmbedBuilder embedBuilder) : this(null, message)
        {
            EmbedBuilder = embedBuilder;
        }

        public WarningResult(string message, Embed embed) : this(null, message)
        {
            Embed = embed;
        }

        public WarningResult(EmbedBuilder embedBuilder) : this(null, "Warning")
        {
            EmbedBuilder = embedBuilder;
        }

        public WarningResult(Embed embed) : this(null, "Warning")
        {
            Embed = embed;
        }

        public WarningResult() : this(null, "Warning")
        {

        }
    }
}
