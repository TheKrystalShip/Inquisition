using Inquisition.Handlers;
using Inquisition.Managers;
using Inquisition.Services;

using Microsoft.Extensions.DependencyInjection;

namespace Inquisition.Extensions
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
