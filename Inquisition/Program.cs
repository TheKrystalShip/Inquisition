using System.Threading.Tasks;

using TheKrystalShip.Inquisition.Extensions;

namespace TheKrystalShip.Inquisition
{
    public class Program
    {
        private static Bot _inquisition;

        public static async Task Main(string[] args)
        {
            await (_inquisition = new Bot())
                .InitAsync()
                .DelayIndefinetly();
        }
    }
}
