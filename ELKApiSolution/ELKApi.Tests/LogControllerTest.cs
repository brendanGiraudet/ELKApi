using ELKApi.Controllers;
using ELKApi.Dtos;
using ELKApi.Services.LoggingService;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
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
        [InlineData("azerzr", "-1", StatusCodes.Status400BadRequest)]
        [InlineData("", "1", StatusCodes.Status400BadRequest)]
        [InlineData("content", "1", StatusCodes.Status201Created)]
        public async Task ShouldHaveTheRightStatusCodeWhenLog(string content, string logLevel, int expectedStatusCode)
        {
            // Arrange 
            LogDto logDto = new()
            {
                Message = content,
                Level = logLevel
            };

            // Act
            var response = await _logController.Log(logDto);

            // Assert
            Assert.IsType<StatusCodeResult>(response);
            var result = response as StatusCodeResult;
            Assert.Equal(expectedStatusCode, result.StatusCode);

        }
        
        [Fact]
        public async Task ShouldHaveInternalServerErrorStatusCodeWhenLog()
        {
            // Arrange
            LogDto logDto = new()
            {
                Message = "content",
                Level = Enumerations.LogLevel.Errors.ToString()
            };
            var serviceMock = new Mock<ILoggingService>();
            serviceMock
                .Setup(s => s.Log(It.IsAny<LogDto>()))
                .Returns(Task.FromResult(false))
                .Verifiable();
            
            serviceMock
                .Setup(s => s.IsValidLogDto(It.IsAny<LogDto>()))
                .Returns(true)
                .Verifiable();
            var controller = new LogController(serviceMock.Object);

            // Act
            var response = await controller.Log(logDto);

            // Assert
            Assert.IsType<StatusCodeResult>(response);
            var result = response as StatusCodeResult;
            Assert.Equal(StatusCodes.Status500InternalServerError, result.StatusCode);

        }
        #endregion
    }
}
