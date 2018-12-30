using Discord;
using Discord.Commands;

namespace TheKrystalShip.Inquisition.Core.Modules
{
    public class ErrorResult : RuntimeResult
    {
        public Embed Embed { get; set; }

        // Redirect all constructors to this one
        public ErrorResult(CommandError? error, string message) : base(error, message)
        {

        }

        public ErrorResult(string message) : this(CommandError.Unsuccessful, message)
        {

        }

        public ErrorResult(string message, EmbedBuilder embedBuilder) : this(CommandError.Unsuccessful, message)
        {
            Embed = embedBuilder.Build();
        }

        public ErrorResult(string message, Embed embed) : this (CommandError.Unsuccessful, message)
        {
            Embed = embed;
        }

        public ErrorResult() : this(CommandError.Unsuccessful, "Error")
        {

        }
    }
}
