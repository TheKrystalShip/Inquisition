using Discord;
using Discord.Commands;

using System.Threading.Tasks;

using TheKrystalShip.Inquisition.Core.Modules;
using TheKrystalShip.Inquisition.Extensions;

namespace TheKrystalShip.Inquisition.Core.Commands
{
    public static class Dispatcher
    {
        public static async Task Dispatch(Optional<CommandInfo> command, ICommandContext context, IResult result)
        {
            if (!result.IsSuccess)
            {
                await context.Channel.SendMessageAsync(string.Empty, false, new EmbedBuilder().CreateError("Error", result.ErrorReason).Build());
                return;
            }

            switch (result)
            {
                case SuccessResult success:
                    await context.Channel.SendMessageAsync(success?.Reason, false, success.Embed);
                    break;
                case InfoResult info:
                    await context.Channel.SendMessageAsync(info?.Reason, false, info.Embed);
                    break;
                case ErrorResult error:
                    await context
                        .Channel
                        .SendMessageAsync(error?.Reason, false, error.Embed);
                    break;
                default:
                    break;
            }
        }
    }
}
