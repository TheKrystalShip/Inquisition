using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Primitives;

using System.Collections.Generic;
using System.IO;

namespace TheKrystalShip.Inquisition.Properties
{
    public static class Settings
    {
        private static IConfiguration _config;

        public static IConfiguration Instance => _config ??
            (_config = new ConfigurationBuilder()
                .SetBasePath(Path.Combine(Directory.GetCurrentDirectory(), "Properties"))
                .AddJsonFile("settings.json", optional: false, reloadOnChange: true)
                .AddJsonFile("secrets.json", optional: false, reloadOnChange: true)
                .Build());

        public static IEnumerable<IConfigurationSection> GetChildren()
        {
            return Instance.GetChildren();
        }

        public static IChangeToken GetReloadToken()
        {
            return Instance.GetReloadToken();
        }

        public static IConfigurationSection GetSection(string key)
        {
            return Instance.GetSection(key);
        }

        public static string GetConnectionString(string name)
        {
            return Instance.GetSection("ConnectionStrings")[name];
        }
    }
}
