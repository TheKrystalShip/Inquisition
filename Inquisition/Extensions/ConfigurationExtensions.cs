using Microsoft.Extensions.Configuration;

namespace TheKrystalShip.Inquisition.Extensions
{
    public static class ConfigurationExtensions
    {
        public static string GetToken(this IConfiguration config)
        {
            return config.GetSection("Bot")["Token"];
        }

        public static string GetName(this IConfiguration config)
        {
            return config.GetSection("Bot")["Name"];
        }

        public static IConfigurationSection GetEmail(this IConfiguration config)
        {
            return config.GetSection("Email");
        }
    }
}
