using System.Threading.Tasks;

using TheKrystalShip.Inquisition.Core;

namespace TheKrystalShip.Inquisition
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            await BotBuilder
                .UseStartup<Startup>()
                .ConfigureDatabase()
                .ConfigureManagers()
                .ConfigureHandlers()
                .ConfigureServices()
                .ConfigureClient()
                .InitAsync();
        }
    }
}
