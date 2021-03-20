using ELKApi.Config;
using Serilog;
using Serilog.Sinks.Elasticsearch;
using System;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;

namespace ELKApi.Services.LoggingService
{
    public class LoggingService : ILoggingService
    {
        private ILogger _logger;
        private ElasticConfiguration _elasticConfiguration;
        public LoggingService(IOptions<ElasticConfiguration> elasticConfiguration)
        {
            _elasticConfiguration = elasticConfiguration.Value;
            var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

            Log.Logger = new LoggerConfiguration()
                .Enrich.FromLogContext()
                .WriteTo.Elasticsearch(ConfigureElasticSink(_elasticConfiguration.Uri, environment))
                .Enrich.WithProperty("Environment", environment)
                .CreateLogger();
            _logger = Log.Logger;
        }

        private static ElasticsearchSinkOptions ConfigureElasticSink(string elasticUri, string environment)
        {
            return new ElasticsearchSinkOptions(new Uri(elasticUri))
            {
                AutoRegisterTemplate = true,
                IndexFormat = $"{Assembly.GetExecutingAssembly().GetName().Name.ToLower().Replace(".", "-")}-{environment?.ToLower().Replace(".", "-")}-{DateTime.UtcNow:yyyy-MM}"
            };
        }

        public Task<bool> LogInformation(string logContent)
        {
            if (string.IsNullOrWhiteSpace(logContent)) return Task.FromResult(false);
            try
            {
                _logger.Information(logContent);
                return Task.FromResult(true);
            }
            catch (Exception)
            {
                return Task.FromResult(false);
            }
        }
    }
}
