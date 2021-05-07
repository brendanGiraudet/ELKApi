using ELKApi.Enumerations;
using ELKApi.Services.LoggingService;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace ELKApi.Controllers
{
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
        [Route("{type}")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Log([FromBody]string content, string type)
        {
            if (!Enum.TryParse(type, out LogType logtype) || !Enum.IsDefined(typeof(LogType), logtype)) return StatusCode(StatusCodes.Status400BadRequest);

            var isLogged = await _loggingService.Log(content, logtype);

            if (!isLogged) return StatusCode(StatusCodes.Status500InternalServerError);

            return StatusCode(StatusCodes.Status201Created);
        }
    }
}
