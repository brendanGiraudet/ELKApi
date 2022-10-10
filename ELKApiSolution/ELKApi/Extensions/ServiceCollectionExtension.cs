using Microsoft.Extensions.DependencyInjection;
using ELKApi.Services.LoggingService;
using ELKApi.Config;
using Microsoft.Extensions.Configuration;
using Elastic.Clients.Elasticsearch;
using Elastic.Transport;

namespace ELKApi.Extensions
{
    public static class ServiceCollectionExtension
    {
        public static void AddServices(this IServiceCollection serviceCollection, IConfiguration configuration)
        {
            serviceCollection.AddTransient<ILoggingService, LoggingService>();

            serviceCollection.AddHttpClient();

            serviceCollection.AddTransient<ElasticsearchClient>(service => new ElasticsearchClient(configuration["ElasticConfiguration:CloudId"], new ApiKey(configuration["ElasticConfiguration:ApiKey"])));
        }

        public static void AddConfigurations(this IServiceCollection serviceCollection, IConfiguration configuration)
        {
            serviceCollection.Configure<ElasticConfiguration>(options => configuration.GetSection("ElasticConfiguration").Bind(options));
        }
    }
}