using Microsoft.Extensions.DependencyInjection;
using ELKApi.Services.LoggingService;
using ELKApi.Config;
using Microsoft.Extensions.Configuration;

namespace ELKApi.Extensions
{
    public static class ServiceCollectionExtension
    {
        public static void AddServices(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddTransient<ILoggingService, LoggingService>();

            serviceCollection.AddHttpClient();
        }

        public static void AddConfigurations(this IServiceCollection serviceCollection, IConfiguration configuration)
        {
            serviceCollection.Configure<ElasticConfiguration>(options => configuration.GetSection("ElasticConfiguration").Bind(options));
        }
    }
}