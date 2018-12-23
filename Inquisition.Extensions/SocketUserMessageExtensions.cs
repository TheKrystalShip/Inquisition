using Discord;
using Discord.Commands;
using Discord.WebSocket;

namespace TheKrystalShip.Inquisition.Extensions
{
    public static class SocketUserMessageExtensions
    {
        public static bool IsValid(this SocketUserMessage message, string prefix, out int argPos)
        {
            bool isValid = false;
            argPos = 0;

            isValid = message.HasStringPrefix(prefix, ref argPos);

            return isValid;
        }

        public static bool IsValid(this SocketUserMessage message, IUser user, out int argPos)
        {
            bool isValid = false;
            argPos = 0;

            isValid = message.HasMentionPrefix(user, ref argPos);

            return isValid;
        }
    }
}
