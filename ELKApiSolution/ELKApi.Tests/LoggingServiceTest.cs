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
        [Fact]
        public async Task ShouldHaveTrueWhenLogInformationMethodIsCall()
        {
            // Arrange
            var logContent = "elk-api-test Content";

            // Act
            var isLogged = await _loggingService.LogInformation(logContent);

            // Assert
            Assert.True(isLogged);
        }
        [Fact]
        public async Task ShouldHaveFalseWhenLogInformationWithEmptyLogContent()
        {
            // Arrange
            var logContent = string.Empty;

            // Act
            var isLogged = await _loggingService.LogInformation(logContent);

            // Assert
            Assert.False(isLogged);
        }
        #endregion
    }
}
