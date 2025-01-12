using Profile.Api.Config;

namespace XUnitTestProject.ProfileTests;

using System;
using Xunit;

public class HealthSettingsTests
{
    [Fact]
    public void CrashTime_ReturnsTheCorrectValueFromTheEnvironmentVariable()
    {
        // Arrange
        var expectedCrashTime = "60";
        Environment.SetEnvironmentVariable("HealthSettings_CrashTime", expectedCrashTime);

        try
        {
            // Act
            var crashTime = HealthSettings.CrashTime;

            // Assert
            Assert.Equal(double.Parse(expectedCrashTime), crashTime);
        }
        finally
        {
            // Cleanup
            Environment.SetEnvironmentVariable("HealthSettings_CrashTime", null);
        }
    }

    [Fact]
    public void CooldownTime_ReturnsTheCorrectValueFromTheEnvironmentVariable()
    {
        // Arrange
        var expectedCooldownTime = "120";
        Environment.SetEnvironmentVariable("HealthSettings_CooldownTime", expectedCooldownTime);

        // Act
        var cooldownTime = HealthSettings.CooldownTime;

        // Assert
        Assert.Equal(double.Parse(expectedCooldownTime), cooldownTime);
    }
}