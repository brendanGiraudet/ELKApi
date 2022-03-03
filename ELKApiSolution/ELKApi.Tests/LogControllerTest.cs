using ELKApi.Controllers;
using ELKApi.Dtos;
using ELKApi.Services.LoggingService;
using ELKApi.Tests.Utils;
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
        ILoggingService DefaultLoggingService
        {
            get
            {
                var mock = new Mock<ILoggingService>();

                mock.Setup(s => s.IsValidLogDto(It.IsAny<LogDto>()))
                    .Returns(true)
                    .Verifiable();
                
                mock.Setup(s => s.Log(It.IsAny<LogDto>()))
                    .Returns(Task.FromResult(true))
                    .Verifiable();

                return mock.Object;
            }
        }
        LogController CreateLogController() => new LogController(DefaultLoggingService);
        LogController CreateLogController(ILoggingService loggingService) => new LogController(loggingService);

        #region Log
        [Fact]
        public async Task ShouldHaveCreatedStatusCodeWhenLog()
        {
            // Arrange 
            LogDto logDto = FakerUtils.LogDtoFaker.Generate();
            var expectedStatusCode = StatusCodes.Status201Created;
            var logController = CreateLogController();

            // Act
            var response = await logController.Log(logDto);

            // Assert
            Assert.IsType<StatusCodeResult>(response);
            var result = response as StatusCodeResult;
            Assert.Equal(expectedStatusCode, result.StatusCode);

        }
        
        [Fact]
        public async Task ShouldHaveBadRequestStatusCodeWhenLogWithWrongDto()
        {
            // Arrange
            LogDto logDto = FakerUtils.LogDtoFaker.Generate();
            var expectedStatusCode = StatusCodes.Status400BadRequest;
            var serviceMock = new Mock<ILoggingService>();
            
            serviceMock
                .Setup(s => s.IsValidLogDto(It.IsAny<LogDto>()))
                .Returns(false)
                .Verifiable();

            var logController = CreateLogController(serviceMock.Object);

            // Act
            var response = await logController.Log(logDto);

            // Assert
            Assert.IsType<StatusCodeResult>(response);

            var result = response as StatusCodeResult;
            Assert.Equal(expectedStatusCode, result.StatusCode);

        }
        
        [Fact]
        public async Task ShouldHaveInternalServerErrorStatusCodeWhenLogWithLoggingServiceProblem()
        {
            // Arrange
            LogDto logDto = FakerUtils.LogDtoFaker.Generate();
            var expectedStatusCode = StatusCodes.Status500InternalServerError;
            var serviceMock = new Mock<ILoggingService>();
            serviceMock
                .Setup(s => s.Log(It.IsAny<LogDto>()))
                .Returns(Task.FromResult(false))
                .Verifiable();
            
            serviceMock
                .Setup(s => s.IsValidLogDto(It.IsAny<LogDto>()))
                .Returns(true)
                .Verifiable();

            var logController = CreateLogController(serviceMock.Object);

            // Act
            var response = await logController.Log(logDto);

            // Assert
            Assert.IsType<StatusCodeResult>(response);

            var result = response as StatusCodeResult;
            Assert.Equal(expectedStatusCode, result.StatusCode);

        }
        #endregion
    }
}
