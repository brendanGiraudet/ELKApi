using ELKApi.Dtos;
using ELKApi.Enumerations;
using System;
using System.Threading.Tasks;
using Elastic.Clients.Elasticsearch;

namespace ELKApi.Services.LoggingService
{
    public class LoggingService : ILoggingService
    {
        private readonly ElasticsearchClient _elasticsearchClient;
        public LoggingService(ElasticsearchClient elasticsearchClient)
        {
            _elasticsearchClient = elasticsearchClient;
        }

        public async Task<bool> Log(LogDto logDto)
        {
            if (!IsValidLogDto(logDto)) return false;

            try
            {
                var response = await _elasticsearchClient.IndexAsync(logDto, request => request.Index(logDto.Fields.Application));

                return response.IsValid;
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