using Microsoft.Extensions.DependencyInjection;

using TheKrystalShip.Inquisition.Handlers;
using TheKrystalShip.Inquisition.Managers;
using TheKrystalShip.Inquisition.Services;

namespace TheKrystalShip.Inquisition.Extensions
{
    public static class ServiceExtensions
    {
        public static IServiceCollection AddHandlers(this IServiceCollection services)
        {
            services.AddSingleton<EmbedHandler>()
                .AddSingleton<PrefixHandler>()
                .AddSingleton<ReportHandler>()
                .AddSingleton<ServiceHandler>();

            return services;
        }

        public static IServiceCollection AddServices(this IServiceCollection services)
        {
            services.AddSingleton<ActivityService>()
                .AddSingleton<AudioService>()
                .AddSingleton<GameService>()
                .AddSingleton<ReminderService>();

            return services;
        }

        public static IServiceCollection AddManagers(this IServiceCollection services)
        {
            services.AddSingleton<ChannelManager>()
                .AddSingleton<EventManager>()
                .AddSingleton<GuildManager>()
                .AddSingleton<RoleManager>()
                .AddSingleton<UserManager>();

            return services;
        }
    }
}
