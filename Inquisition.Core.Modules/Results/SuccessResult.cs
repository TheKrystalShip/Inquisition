using Discord;
using Discord.Commands;

namespace TheKrystalShip.Inquisition.Core.Modules
{
    public class SuccessResult : RuntimeResult
    {
        public Embed Embed { get; set; }

        // Redirect all constructors to this one
        public SuccessResult(CommandError? error, string message) : base(error, message)
        {

        }

        public SuccessResult(string message) : this(null, message)
        {

        }

        public SuccessResult(string message, EmbedBuilder embedBuilder) : this(null, message)
        {
            Embed = embedBuilder.Build();
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
