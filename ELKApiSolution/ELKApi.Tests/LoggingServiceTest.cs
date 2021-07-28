using ELKApi.Dtos;
using ELKApi.Enumerations;
using ELKApi.Services.LoggingService;
using System.Threading.Tasks;
using Xunit;

namespace ELKApi.Tests
{
    public class LoggingServiceTest
    {
        private ILoggingService _loggingService;
        public LoggingServiceTest(ILoggingService loggingService)
        {
            _loggingService = loggingService;
        }

        #region Log
        [Theory]
        [InlineData(LogLevel.Informations)]
        [InlineData(LogLevel.Errors)]
        public async Task ShouldHaveTrueWhenLogWithRightParameters(LogLevel logLevel)
        {
            // Arrange
            LogDto logDto = new()
            {
                Level = logLevel,
                Message = "Unit test"
            };

            // Act
            var isLogged = await _loggingService.Log(logDto);

            // Assert
            Assert.True(isLogged);
        }

        [Theory]
        [InlineData(LogLevel.Informations)]
        [InlineData(LogLevel.Errors)]
        public async Task ShouldHaveFalseWhenLogWithEmptyMessage(LogLevel logLevel)
        {
            // Arrange
            LogDto logDto = new()
            {
                Level = logLevel,
                Message = string.Empty
            };

            // Act
            var isLogged = await _loggingService.Log(logDto);

            // Assert
            Assert.False(isLogged);
        }
        
        [Fact]
        public async Task ShouldHaveFalseWhenLogWithWrongLevel()
        {
            // Arrange
            LogDto logDto = new()
            {
                Level = LogLevel.NA,
                Message = "Unit test"
            };

            // Act
            var isLogged = await _loggingService.Log(logDto);

            // Assert
            Assert.False(isLogged);
        }
        #endregion
    }
}
