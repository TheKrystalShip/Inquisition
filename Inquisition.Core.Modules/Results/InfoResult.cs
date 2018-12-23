using Discord;
using Discord.Commands;

using TheKrystalShip.Inquisition.Extensions;

namespace TheKrystalShip.Inquisition.Core.Modules
{
    public class InfoResult : RuntimeResult
    {
        public EmbedBuilder EmbedBuilder { get; private set; }
        public Embed Embed { get; set; }

        // Redirect all constructors to this one
        public InfoResult(CommandError? error, string message) : base(error, message)
        {
            EmbedBuilder = new EmbedBuilder().CreateInfo("Info", message);
        }

        public InfoResult(string message) : this(null, message)
        {

        }

        public InfoResult(string message, EmbedBuilder embedBuilder) : this(null, message)
        {
            EmbedBuilder = embedBuilder;
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
