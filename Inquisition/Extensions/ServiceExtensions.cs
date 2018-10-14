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
            services.AddSingleton<PrefixHandler>();
            services.AddSingleton<ReportHandler>();
            services.AddSingleton<ServiceHandler>();

            return services;
        }

        public static IServiceCollection AddServices(this IServiceCollection services)
        {
            services.AddSingleton<ActivityService>();
            services.AddSingleton<AudioService>();
            services.AddSingleton<GameService>();
            services.AddSingleton<ReminderService>();

            return services;
        }

        public static IServiceCollection AddManagers(this IServiceCollection services)
        {
            services.AddSingleton<ChannelManager>();
            services.AddSingleton<EventManager>();
            services.AddSingleton<GuildManager>();
            services.AddSingleton<RoleManager>();
            services.AddSingleton<UserManager>();

            return services;
        }

        public static IServiceCollection AddTools(this IServiceCollection services)
        {
            services.AddSingleton<Tools>();
            services.AddSingleton<Benchmark>();

            return services;
        }
    }
}
