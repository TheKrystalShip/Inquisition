using Discord;
using Discord.Commands;

namespace TheKrystalShip.Inquisition.Core.Modules
{
    public class InfoResult : RuntimeResult
    {
        public Embed Embed { get; set; }

        // Redirect all constructors to this one
        public InfoResult(CommandError? error, string message) : base(error, message)
        {

        }

        public InfoResult(string message) : this(null, message)
        {

        }

        public InfoResult(string message, EmbedBuilder embedBuilder) : this(null, message)
        {
            Embed = embedBuilder.Build();
        }

        public InfoResult(string message, Embed embed) : this(null, message)
        {
            Embed = embed;
        }

        public InfoResult(Embed embed) : this(null, "Info")
        {
            Embed = embed;
        }

        public InfoResult() : this(null, "Info")
        {

        }
    }
}
