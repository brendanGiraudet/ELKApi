using System;
using Xunit;

namespace ELKApi.Tests.Extensions
{
    public class DateTimeExtensionsTests
    {
        #region ToTimestamp
        [Fact]
        public void ShouldHaveTheCorrectTimestampWhenGetTimestampsIntoDatetime()
        {
            // Arrange
            var datetime = new DateTime(2021, 10, 08, 07, 54, 58);
            var expectedTimestamp = "1633679698";

            // Act
            var actualTimestamp = ELKApi.Extensions.DateTimeExtensions.ToTimestamp(datetime);

            // Assert
            Assert.Equal(expectedTimestamp, actualTimestamp);

        }
        #endregion
    }
}
