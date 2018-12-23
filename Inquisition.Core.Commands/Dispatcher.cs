using Discord;
using Discord.Commands;

using System.Threading.Tasks;

using TheKrystalShip.Inquisition.Core.Modules;
using TheKrystalShip.Inquisition.Extensions;

namespace TheKrystalShip.Inquisition.Core.Commands
{
    public static class Dispatcher
    {
        public static async Task Dispatch(CommandInfo command, ICommandContext context, IResult result)
        {
            if (!result.IsSuccess)
            {
                await context.Channel.SendMessageAsync(string.Empty, false, new EmbedBuilder().CreateError("Error", result.ErrorReason).Build());
                return;
            }

            switch (result)
            {
                case SuccessResult success:
                    Embed successEmbed = new EmbedBuilder().CreateSuccess(title: "Success", message: success.Reason).Build();
                    await context.Channel.SendMessageAsync(string.Empty, false, successEmbed);
                    break;
                case InfoResult info:
                    Embed infoEmbed = new EmbedBuilder().CreateInfo(title: "Info", message: info.Reason).Build();
                    await context.Channel.SendMessageAsync(string.Empty, false, infoEmbed);
                    break;
                case ErrorResult error:
                    Embed errorEmbed = new EmbedBuilder().CreateError(title: error.Error.ToString(), message: error.Reason).Build();
                    await context.Channel.SendMessageAsync(string.Empty, false, errorEmbed);
                    break;
                default:
                    break;
            }
        }
    }
}
