using ELKApi.Config;
using Serilog;
using Serilog.Sinks.Elasticsearch;
using System;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using ELKApi.Enumerations;

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

            Serilog.Log.Logger = new LoggerConfiguration()
                .Enrich.FromLogContext()
                .WriteTo.Elasticsearch(ConfigureElasticSink(_elasticConfiguration.Uri, environment))
                .Enrich.WithProperty("Environment", environment)
                .CreateLogger();
            _logger = Serilog.Log.Logger;
        }

        private static ElasticsearchSinkOptions ConfigureElasticSink(string elasticUri, string environment)
        {
            return new ElasticsearchSinkOptions(new Uri(elasticUri))
            {
                AutoRegisterTemplate = true,
                IndexFormat = $"{Assembly.GetExecutingAssembly().GetName().Name.ToLower().Replace(".", "-")}-{environment?.ToLower().Replace(".", "-")}-{DateTime.UtcNow:yyyy-MM}"
            };
        }

        public Task<bool> Log(string logContent, LogType type)
        {
            if (string.IsNullOrWhiteSpace(logContent)) return Task.FromResult(false);

            try
            {
                switch (type)
                {
                    case LogType.Error:
                        _logger.Error(logContent);
                        break;
                    case LogType.Information:
                        _logger.Information(logContent);
                        break;
                    default:
                        return Task.FromResult(false);
                }
                return Task.FromResult(true);
            }
            catch (Exception)
            {
                return Task.FromResult(false);
            }
        }
    }
}
