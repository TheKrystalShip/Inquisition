using Discord;
using Discord.Commands;

using TheKrystalShip.Inquisition.Extensions;

namespace TheKrystalShip.Inquisition.Core.Modules
{
    public class SuccessResult : RuntimeResult
    {
        public EmbedBuilder EmbedBuilder { get; private set; }
        public Embed Embed { get; set; }

        // Redirect all constructors to this one
        public SuccessResult(CommandError? error, string message) : base(error, message)
        {
            EmbedBuilder = new EmbedBuilder().CreateSuccess("Success", message);
        }

        public SuccessResult(string message) : this(null, message)
        {

        }

        public SuccessResult(string message, EmbedBuilder embedBuilder) : this(null, message)
        {
            EmbedBuilder = embedBuilder;
        }

        public SuccessResult(string message, Embed embed) : this(null, message)
        {
            Embed = embed;
        }

        public SuccessResult(Embed embed) : this(null, "Success")
        {
            Embed = embed;
        }

        public SuccessResult() : this(null, "Success")
        {

        }
    }
}
