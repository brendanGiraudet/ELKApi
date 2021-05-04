using ELKApi.Enumerations;
using System.Threading.Tasks;

namespace ELKApi.Services.LoggingService
{
    public interface ILoggingService
    {
        Task<bool> Log(string logContent, LogType type);
    }
}
