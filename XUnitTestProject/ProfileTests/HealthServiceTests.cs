namespace XUnitTestProject.ProfileTests;

using Profile.Api.Services;
using Xunit;

public class HealthServiceTests
    {
        private readonly HealthService _healthService;

        public HealthServiceTests()
        {
            _healthService = new HealthService();
        }

        [Fact]
        public void IsLive_ShouldReturnTrue()
        {
            // Act
            var result = _healthService.IsLive();

            // Assert
            Assert.True(result);
        }
    }