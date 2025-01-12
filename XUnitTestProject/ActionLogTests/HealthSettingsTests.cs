using ActionLog.Api.Config;

namespace XUnitTestProject.ActionLogTests;

using System;
using Xunit;

    public class HealthSettingsTests
    {
        [Fact]
        public void CrashTime_ReturnsCorrectValueFromEnvironmentVariable()
        {
            // Arrange
            var expectedCrashTime = "60";
            Environment.SetEnvironmentVariable("HealthSettings_CrashTime", expectedCrashTime);

            // Act
            var crashTime = HealthSettings.CrashTime;

            // Assert
            Assert.Equal(double.Parse(expectedCrashTime), crashTime);

            // Cleanup
            Environment.SetEnvironmentVariable("HealthSettings_CrashTime", null);
        }

        [Fact]
        public void CrashTime_ThrowsExceptionIfEnvironmentVariableNotSet()
        {
            // Arrange
            Environment.SetEnvironmentVariable("HealthSettings_CrashTime", null);

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => HealthSettings.CrashTime);
        }

        [Fact]
        public void CrashTime_ThrowsExceptionIfEnvironmentVariableIsInvalid()
        {
            // Arrange
            var invalidCrashTime = "invalid";
            Environment.SetEnvironmentVariable("HealthSettings_CrashTime", invalidCrashTime);

            // Act & Assert
            Assert.Throws<FormatException>(() => HealthSettings.CrashTime);

            // Cleanup
            Environment.SetEnvironmentVariable("HealthSettings_CrashTime", null);
        }

        [Fact]
        public void CooldownTime_ReturnsCorrectValueFromEnvironmentVariable()
        {
            // Arrange
            var expectedCooldownTime = "120";
            Environment.SetEnvironmentVariable("HealthSettings_CooldownTime", expectedCooldownTime);

            // Act
            var cooldownTime = HealthSettings.CooldownTime;

            // Assert
            Assert.Equal(double.Parse(expectedCooldownTime), cooldownTime);

            // Cleanup
            Environment.SetEnvironmentVariable("HealthSettings_CooldownTime", null);
        }

        [Fact]
        public void CooldownTime_ThrowsExceptionIfEnvironmentVariableNotSet()
        {
            // Arrange
            Environment.SetEnvironmentVariable("HealthSettings_CooldownTime", null);

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => HealthSettings.CooldownTime);
        }

        [Fact]
        public void CooldownTime_ThrowsExceptionIfEnvironmentVariableIsInvalid()
        {
            // Arrange
            var invalidCooldownTime = "invalid";
            Environment.SetEnvironmentVariable("HealthSettings_CooldownTime", invalidCooldownTime);

            // Act & Assert
            Assert.Throws<FormatException>(() => HealthSettings.CooldownTime);

            // Cleanup
            Environment.SetEnvironmentVariable("HealthSettings_CooldownTime", null);
        }

        [Fact]
        public void AppStartAt_ReturnsCurrentUtcTime()
        {
            // Arrange
            var expectedTime = DateTimeOffset.UtcNow;

            // Act
            var appStartAt = HealthSettings.AppStartAt;

            // Assert
            Assert.True((appStartAt - expectedTime).TotalSeconds < 1); // Проверяем, что время отличается не более чем на 1 секунду
        }
    }
