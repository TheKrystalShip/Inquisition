using Discord.Commands;

namespace TheKrystalShip.Inquisition.Core.Modules
{
    public class EmptyResult : RuntimeResult
    {
        public EmptyResult(CommandError? error, string reason) : base(error, reason)
        {
        }

        public EmptyResult() : base(null, null)
        {

        }
    }
}
