using ELKApi.Dtos;
using System.Threading.Tasks;

namespace ELKApi.Services.LoggingService
{
    public interface ILoggingService
    {
        bool IsValidLogDto(LogDto logDto);
        Task<bool> Log(LogDto logDto);
    }
}
