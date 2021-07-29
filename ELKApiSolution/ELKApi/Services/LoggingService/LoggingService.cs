using ELKApi.Config;
using ELKApi.Dtos;
using ELKApi.Enumerations;
using Microsoft.Extensions.Options;
using Serilog;
using Serilog.Sinks.Elasticsearch;
using System;
using System.Threading.Tasks;

namespace ELKApi.Services.LoggingService
{
    public class LoggingService : ILoggingService
    {
        private ElasticConfiguration _elasticConfiguration;
        public LoggingService(IOptions<ElasticConfiguration> elasticConfiguration)
        {
            _elasticConfiguration = elasticConfiguration.Value;
        }

        private ILogger CreateLogger(string applicationName, string environment)
        {
            return new LoggerConfiguration()
            .Enrich.WithProperty("Application", applicationName)
            .Enrich.WithProperty("Environment", environment)
            .MinimumLevel.Debug()
            .WriteTo.Elasticsearch(new ElasticsearchSinkOptions(new Uri(_elasticConfiguration.Uri))
            {
                AutoRegisterTemplate = true,
                AutoRegisterTemplateVersion = AutoRegisterTemplateVersion.ESv6,
                EmitEventFailure = EmitEventFailureHandling.ThrowException | EmitEventFailureHandling.WriteToSelfLog,
                FailureCallback = e => Console.WriteLine("Unable to submit event " + e.MessageTemplate),
            })
            .CreateLogger();
        }

        public Task<bool> Log(LogDto logDto)
        {
            if (!IsValidLogDto(logDto)) return Task.FromResult(false);

            var logger = CreateLogger(logDto.Application, logDto.Environnement);

            try
            {
                Enum.TryParse(logDto.Level, out LogLevel logLevel);
                switch (logLevel)
                {
                    case LogLevel.Errors:
                        logger.Error(logDto.Message);
                        break;
                    case LogLevel.Informations:
                        logger.Information(logDto.Message);
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

        public bool IsValidLogDto(LogDto logDto)
        {
            return logDto != null
            && !string.IsNullOrWhiteSpace(logDto.Message)
            && Enum.TryParse(logDto.Level, out LogLevel logLevel)
            && Enum.IsDefined(typeof(LogLevel), logLevel);
        }
    }
}
