using ELKApi.Config;
using ELKApi.Dtos;
using ELKApi.Enumerations;
using ELKApi.Services.LoggingService;
using ELKApi.Tests.Utils;
using Microsoft.Extensions.Options;
using Moq;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace ELKApi.Tests
{
    public class LoggingServiceTest
    {
        private ILoggingService CreateLoggingService() => new LoggingService(_elasticConfigurationOptions, _httpClient);
        private ILoggingService CreateLoggingService(HttpClient httpClient) => new LoggingService(_elasticConfigurationOptions, httpClient);
        private IOptions<ElasticConfiguration> _elasticConfigurationOptions;
        private HttpClient _httpClient;
        public LoggingServiceTest(IOptions<ElasticConfiguration> elasticConfigurationOptions)
        {
            _elasticConfigurationOptions = elasticConfigurationOptions;
            var httpClientHandler = new HttpClientHandler();
            httpClientHandler.ServerCertificateCustomValidationCallback = (message, cert, chain, sslPolicyErrors) =>
            {
                return true;
            };
            _httpClient = HttpClientFactory.Create(httpClientHandler);
        }

        #region Log
        [Fact]
        public async Task ShouldHaveTrueWhenLogWithRightParameters()
        {
            // Arrange
            LogDto logDto = FakerUtils.LogDtoFaker.Generate();
            var loggingService = CreateLoggingService();

            // Act
            var isLogged = await loggingService.Log(logDto);

            // Assert
            Assert.True(isLogged);
        }

        [Fact]
        public async Task ShouldHaveFalseWhenLogWithHttpClientProblem()
        {
            // Arrange
            LogDto logDto = FakerUtils.LogDtoFaker.Generate();
            logDto.Fields.Application = string.Empty;
            var httpClientMock = new Mock<HttpClient>();
            httpClientMock.Setup(h => h.PostAsync(It.IsAny<string>(), It.IsAny<HttpContent>()))
                .Returns(Task.FromResult(new HttpResponseMessage
                {
                    StatusCode = System.Net.HttpStatusCode.InternalServerError
                }))
                .Verifiable();
            var loggingService = CreateLoggingService(httpClientMock.Object);

            // Act
            var isLogged = await loggingService.Log(logDto);

            // Assert
            Assert.False(isLogged);
        }
        #endregion
    }
}
