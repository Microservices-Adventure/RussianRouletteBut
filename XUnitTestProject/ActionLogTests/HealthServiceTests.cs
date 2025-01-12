using ActionLog.Api.Config;

namespace XUnitTestProject.ActionLogTests;

using ActionLog.Api.Services;
using Xunit;

    public class HealthServiceTests
    {
        [Fact]
        public void IsLive_ShouldReturnTrue()
        {
            // Arrange
            var healthService = new HealthService();

            // Act
            var isLive = healthService.IsLive();

            // Assert
            Assert.True(isLive);
        }
    }

