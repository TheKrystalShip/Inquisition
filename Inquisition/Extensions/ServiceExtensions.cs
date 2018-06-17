using Inquisition.Database.Repositories;
using Inquisition.Handlers;
using Inquisition.Logging;
using Inquisition.Services;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Inquisition.Extensions
{
    public static class ServiceExtensions
    {
        public static IServiceCollection AddRepository(this IServiceCollection services)
        {
            services.AddScoped<IRepositoryWrapper, RepositoryWrapper>();
            return services;
        }

        public static IServiceCollection AddLogger(this IServiceCollection services)
        {
            services.AddOptions();
            services.TryAdd(ServiceDescriptor.Singleton(typeof(ILogger<>), typeof(Logger<>)));
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
                .AddSingleton<Benchmark>()
                .AddSingleton<EventService>()
                .AddSingleton<GameService>()
                .AddSingleton<ReminderService>();

            return services;
        }
    }
}
