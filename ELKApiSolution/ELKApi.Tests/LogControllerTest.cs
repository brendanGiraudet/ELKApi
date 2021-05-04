using ELKApi.Controllers;
using ELKApi.Services.LoggingService;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Xunit;

namespace ELKApi.Tests
{
    public class LogControllerTest
    {
        readonly LogController _logController;
        public LogControllerTest(ILoggingService loggingService)
        {
            _logController = new LogController(loggingService);
        }
        #region Log
        [Theory]
        [InlineData("", "-1", StatusCodes.Status400BadRequest)]
        [InlineData("", "1", StatusCodes.Status500InternalServerError)]
        [InlineData("content", "1", StatusCodes.Status201Created)]
        public async Task ShouldHaveTheRightStatusCodeWhenLog(string content, string logType, int expectedStatusCode)
        {
            // Arrange 

            // Act
            var response = await _logController.Log(content, logType);

            // Assert
            Assert.IsType<StatusCodeResult>(response);
            var result = response as StatusCodeResult;
            Assert.Equal(expectedStatusCode, result.StatusCode);

        }
        #endregion
    }
}
