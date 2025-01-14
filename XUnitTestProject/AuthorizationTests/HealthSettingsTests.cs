using Authorization.Domain.Config;

namespace XUnitTestProject.AuthorizationTests;

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
}