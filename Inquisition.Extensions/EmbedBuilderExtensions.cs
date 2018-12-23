using Discord;

namespace TheKrystalShip.Inquisition.Extensions
{
    public static class EmbedBuilderExtensions
    {
        public static EmbedBuilder CreateError(this EmbedBuilder builder, string title, string message)
        {
            builder = CreateBase(title, message);
            builder.WithColor(Color.DarkRed);

            return builder;
        }

        public static EmbedBuilder CreateSuccess(this EmbedBuilder builder, string title, string message)
        {
            builder = CreateBase(title, message);
            builder.WithColor(Color.DarkGreen);

            return builder;
        }

        public static EmbedBuilder CreateInfo(this EmbedBuilder builder, string title, string message)
        {
            builder = CreateBase(title, message);
            builder.WithColor(Color.Teal);

            return builder;
        }

        public static EmbedBuilder CreateDefault(this EmbedBuilder builder, string title, string message)
        {
            builder = CreateBase(title, message);
            builder.WithColor(Color.Gold);

            return builder;
        }

        private static EmbedBuilder CreateBase(string title, string message)
        {
            EmbedBuilder builder = new EmbedBuilder();
            builder.WithCurrentTimestamp();
            builder.WithTitle(title);
            builder.WithDescription(message);

            return builder;
        }
    }
}
