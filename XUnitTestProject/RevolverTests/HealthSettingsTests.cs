namespace XUnitTestProject.RevolverTests;

using Revolver.Domain.Config;
using System;
using Xunit;

public class HealthSettingsTests
{
    [Fact]
    public void CrashTime_ShouldReturnValueFromEnvironmentVariable()
    {
        // Arrange
        const string crashTimeValue = "10";
        Environment.SetEnvironmentVariable("HealthSettings_CrashTime", crashTimeValue);

        // Act
        var crashTime = HealthSettings.CrashTime;

        // Assert
        Assert.Equal(10, crashTime);

        // Cleanup
        Environment.SetEnvironmentVariable("HealthSettings_CrashTime", null);
    }

    [Fact]
    public void CrashTime_ShouldThrowFormatException_WhenEnvironmentVariableIsInvalid()
    {
        // Arrange
        const string invalidCrashTimeValue = "invalid";
        Environment.SetEnvironmentVariable("HealthSettings_CrashTime", invalidCrashTimeValue);

        // Act & Assert
        Assert.Throws<FormatException>(() => HealthSettings.CrashTime);

        // Cleanup
        Environment.SetEnvironmentVariable("HealthSettings_CrashTime", null);
    }

    [Fact]
    public void CooldownTime_ShouldReturnValueFromEnvironmentVariable()
    {
        // Arrange
        const string cooldownTimeValue = "5";
        Environment.SetEnvironmentVariable("HealthSettings_CooldownTime", cooldownTimeValue);

        try
        {
            // Act
            var cooldownTime = HealthSettings.CooldownTime;

            // Assert
            Assert.Equal(5, cooldownTime);
        }
        finally
        {
            // Cleanup
            Environment.SetEnvironmentVariable("HealthSettings_CooldownTime", null);
        }
    }

    [Fact]
    public void CooldownTime_ShouldThrowFormatException_WhenEnvironmentVariableIsInvalid()
    {
        // Arrange
        const string invalidCooldownTimeValue = "invalid";
        Environment.SetEnvironmentVariable("HealthSettings_CooldownTime", invalidCooldownTimeValue);

        // Act & Assert
        Assert.Throws<FormatException>(() => HealthSettings.CooldownTime);

        // Cleanup
        Environment.SetEnvironmentVariable("HealthSettings_CooldownTime", null);
    }

    [Fact]
    public void AppStartAt_ShouldBeInitializedToCurrentUtcTime()
    {
        // Arrange
        var expectedTime = DateTimeOffset.UtcNow;

        // Act
        var appStartAt = HealthSettings.AppStartAt;

        // Assert
        Assert.True(appStartAt >= expectedTime || appStartAt.AddMilliseconds(100) >= expectedTime);
    }
}