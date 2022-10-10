using Elastic.Clients.Elasticsearch;
using Elastic.Transport;
using ELKApi.Dtos;
using ELKApi.Services.LoggingService;
using ELKApi.Tests.Utils;
using Moq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace ELKApi.Tests
{
    public class LoggingServiceTest
    {
        private ILoggingService CreateLoggingService() => new LoggingService(ElasticsearchClient);

        private ElasticsearchClient ElasticsearchClient
        {
            get
            {
                // var mock = new Mock<ElasticsearchClient>();

                // mock.Setup(s => s.IndexAsync(It.IsAny<object>(), It.IsAny<CancellationToken>()))
                //     .Returns(Task.FromResult(FakerUtils.IndexResponseFaker.Generate()))
                //     .Verifiable();


                // return mock.Object;

                return new ElasticsearchClient("cloudid", new ApiKey("apikey"));
            }
        }

        #region Log
        [Fact(Skip = "mock problem")]
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

        [Fact(Skip = "mock problem")]
        public async Task ShouldHaveFalseWhenLogWithWrongDto()
        {
            // Arrange
            LogDto logDto = null;
            var loggingService = CreateLoggingService();

            // Act
            var isLogged = await loggingService.Log(logDto);

            // Assert
            Assert.False(isLogged);
        }
        #endregion
    }
}
