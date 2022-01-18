using ELKApi.Dtos;
using ELKApi.Services.LoggingService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace ELKApi.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class LogController : ControllerBase
    {
        private ILoggingService _loggingService;
        public LogController(ILoggingService loggingService)
        {
            _loggingService = loggingService;
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Log(LogDto logDto)
        {
            if (!_loggingService.IsValidLogDto(logDto)) return StatusCode(StatusCodes.Status400BadRequest);

            var isLogged = await _loggingService.Log(logDto);

            if (!isLogged) return StatusCode(StatusCodes.Status500InternalServerError);

            return StatusCode(StatusCodes.Status201Created);
        }
    }
}
