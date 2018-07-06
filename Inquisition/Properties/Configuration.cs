using Microsoft.Extensions.Configuration;

using System.IO;

namespace Inquisition.Properties
{
    public static class Configuration
    {
        private static IConfiguration _config;
        private static readonly string _configPath = Path.Combine("Properties", "settings.json");

        public static IConfiguration Config
        {
            get => (_config is null) ? _config = new ConfigurationBuilder()
                        .SetBasePath(Directory.GetCurrentDirectory())
                        .AddJsonFile(_configPath, optional: false, reloadOnChange: true)
                        .Build() : _config;
        }

        public static string Get(params string[] index)
        {
            int limit = index.Length - 1;
            IConfigurationSection section = null;

            for (int i = 0; i < limit; i++)
            {
                section = Config.GetSection(index[i]);
            }

            return section[index[limit]];
        }
    }
}
