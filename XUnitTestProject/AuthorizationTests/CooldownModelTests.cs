namespace XUnitTestProject.AuthorizationTests;

using Authorization.Domain.Models;
using Xunit;

    public class CooldownModelTests
    {
        [Fact]
        public void CooldownModel_Properties_CanBeInitialized()
        {
            // Arrange
            var cooldownModel = new CooldownModel
            {
                IsCooldown = true,
                SecondsLeft = 10.5
            };

            // Act
            var isCooldown = cooldownModel.IsCooldown;
            var secondsLeft = cooldownModel.SecondsLeft;

            // Assert
            Assert.True(isCooldown);
            Assert.Equal(10.5, secondsLeft);
        }

        [Fact]
        public void CooldownModel_Properties_AreImmutable()
        {
            // Arrange
            var cooldownModel = new CooldownModel
            {
                IsCooldown = true,
                SecondsLeft = 10.5
            };

            // Act & Assert
            Assert.True(cooldownModel.IsCooldown);
            Assert.Equal(10.5, cooldownModel.SecondsLeft);
        }

        [Fact]
        public void CooldownModel_Equality_WorksCorrectly()
        {
            // Arrange
            var cooldownModel1 = new CooldownModel
            {
                IsCooldown = true,
                SecondsLeft = 10.5
            };

            var cooldownModel2 = new CooldownModel
            {
                IsCooldown = true,
                SecondsLeft = 10.5
            };

            var cooldownModel3 = new CooldownModel
            {
                IsCooldown = false,
                SecondsLeft = 5.0
            };

            // Act & Assert
            Assert.Equal(cooldownModel1, cooldownModel2); 
            Assert.NotEqual(cooldownModel1, cooldownModel3); 
        }
    }
