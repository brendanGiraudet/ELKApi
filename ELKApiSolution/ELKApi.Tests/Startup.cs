using ELKApi.Config;
using ELKApi.Services.LoggingService;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ELKApi.Tests
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            var configBuilder = new ConfigurationBuilder();
            configBuilder.AddJsonFile("appsettings.json");
            var config = configBuilder.Build();

            // Config
            services.Configure<ElasticConfiguration>(options => config.GetSection("ElasticConfiguration").Bind(options));

            // Services
            services.AddTransient<ILoggingService, LoggingService>();
            services.AddHttpClient();
        }
    }
}
