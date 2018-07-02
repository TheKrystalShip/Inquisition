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
            services.AddSingleton<ConversionHandler>()
                .AddSingleton<EventManager>()
                .AddSingleton<EmbedHandler>()
                .AddSingleton<PrefixHandler>()
                .AddSingleton<ReportHandler>()
                .AddSingleton<ServiceHandler>();

            return services;
        }

        public static IServiceCollection AddServices(this IServiceCollection services)
        {
            services.AddSingleton<ActivityService>()
                .AddSingleton<AudioService>()
                .AddSingleton<EventService>()
                .AddSingleton<GameService>()
                .AddSingleton<ReminderService>();

            return services;
        }
    }
}
