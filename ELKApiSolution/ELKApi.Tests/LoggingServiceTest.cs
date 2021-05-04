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
        [InlineData(LogType.Informations)]
        [InlineData(LogType.Errors)]
        public async Task ShouldHaveTrueWhenLogInformationMethodIsCall(LogType logType)
        {
            // Arrange
            var logContent = "elk-api-test Content";

            // Act
            var isLogged = await _loggingService.Log(logContent, logType);

            // Assert
            Assert.True(isLogged);
        }

        [Theory]
        [InlineData(LogType.Informations)]
        [InlineData(LogType.Errors)]
        public async Task ShouldHaveFalseWhenLogWithEmptyLogContent(LogType logType)
        {
            // Arrange
            var logContent = string.Empty;

            // Act
            var isLogged = await _loggingService.Log(logContent, logType);

            // Assert
            Assert.False(isLogged);
        }
        
        [Fact]
        public async Task ShouldHaveFalseWhenLogWithWrongType()
        {
            // Arrange
            var logContent = "elk-api-test Content";
            var logType = -1;

            // Act
            var isLogged = await _loggingService.Log(logContent, (LogType)logType);

            // Assert
            Assert.False(isLogged);
        }
        #endregion
    }
}
