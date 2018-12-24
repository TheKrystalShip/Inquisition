using Discord;

using System;

namespace TheKrystalShip.Inquisition.Tools
{
    public class EmbedFactory
    {
        public static Embed Create(ResultType resultType, Action<EmbedBuilder> action)
        {
            EmbedBuilder embedBuilder = new EmbedBuilder();
            action(embedBuilder);

            embedBuilder.WithCurrentTimestamp();

            switch (resultType)
            {
                case ResultType.Success:
                    embedBuilder.WithColor(Color.DarkGreen);
                    break;
                case ResultType.Info:
                    embedBuilder.WithColor(Color.DarkBlue);
                    break;
                case ResultType.Warning:
                    embedBuilder.WithColor(Color.DarkOrange);
                    break;
                case ResultType.Error:
                    embedBuilder.WithColor(Color.DarkRed);
                    break;
            }

            return embedBuilder.Build();
        }
    }

    public enum ResultType
    {
        Success,
        Info,
        Warning,
        Error
    }
}
