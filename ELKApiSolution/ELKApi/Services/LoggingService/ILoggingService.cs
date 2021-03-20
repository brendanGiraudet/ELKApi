using System.Threading.Tasks;

namespace ELKApi.Services.LoggingService
{
    public interface ILoggingService
    {
        Task<bool> LogInformation(string logContent);
    }
}
