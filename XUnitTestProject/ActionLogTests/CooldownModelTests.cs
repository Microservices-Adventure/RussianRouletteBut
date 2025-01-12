namespace XUnitTestProject.ActionLogTests;

using ActionLog.Api.Models;
using Xunit;

    public class CooldownModelTests
    {
        [Fact]
        public void CooldownModel_Properties_ShouldBeSetCorrectly()
        {
            // Arrange
            var model = new CooldownModel
            {
                IsCooldown = true,
                SecondsLeft = 10.5
            };

            // Assert
            Assert.True(model.IsCooldown);
            Assert.Equal(10.5, model.SecondsLeft);
        }
    }
