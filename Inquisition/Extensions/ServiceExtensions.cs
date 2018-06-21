using Inquisition.Database.Repositories;
using Inquisition.Handlers;
using Inquisition.Services;

using Microsoft.Extensions.DependencyInjection;

namespace Inquisition.Extensions
{
    public static class ServiceExtensions
    {
        public static IServiceCollection AddRepository(this IServiceCollection services)
        {
            services.AddScoped<IRepositoryWrapper, RepositoryWrapper>();
            return services;
        }

        public static IServiceCollection AddHandlers(this IServiceCollection services)
        {
            services.AddSingleton<ConversionHandler>()
                .AddSingleton<EventHandler>()
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
