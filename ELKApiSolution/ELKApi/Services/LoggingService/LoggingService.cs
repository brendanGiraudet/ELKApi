using ELKApi.Config;
using ELKApi.Dtos;
using ELKApi.Enumerations;
using Microsoft.Extensions.Options;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace ELKApi.Services.LoggingService
{
    public class LoggingService : ILoggingService
    {
        private readonly ElasticConfiguration _elasticConfiguration;
        private readonly HttpClient _httpClient;
        public LoggingService(IOptions<ElasticConfiguration> elasticConfiguration, HttpClient httpClient)
        {
            _elasticConfiguration = elasticConfiguration.Value;
            _httpClient = httpClient;
        }

        public async Task<bool> Log(LogDto logDto)
        {
            if (!IsValidLogDto(logDto)) return false;

            try
            {
                var response = await _httpClient.PostAsJsonAsync(_elasticConfiguration.GetLogUrl(logDto.Fields.Application), logDto);

                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }

        public bool IsValidLogDto(LogDto logDto)
        {
            return logDto != null
            && logDto.Fields != null
            && !string.IsNullOrWhiteSpace(logDto.Fields.Message)
            && Enum.TryParse(logDto.Fields.Level, out LogLevel logLevel)
            && Enum.IsDefined(typeof(LogLevel), logLevel)
            && !string.IsNullOrWhiteSpace(logDto.Fields.Application);
        }
    }
}
