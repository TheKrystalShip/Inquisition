using Discord;
using Discord.Commands;

namespace TheKrystalShip.Inquisition.Core.Modules
{
    public abstract class Result : RuntimeResult
    {
        protected EmbedBuilder Embed;

        protected Result(CommandError? error, string reason) : base(error, reason)
        {

        }
    }
}
