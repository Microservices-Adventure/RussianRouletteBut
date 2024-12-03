using Authorization.Domain.Config;
using Authorization.Domain.Services;

namespace XUnitTestProject.AuthorizationTests;

public class HealthServiceTests
{
    [Fact]
    public void HealthService_IsLive()
    {
        // Arrange
        HealthService healthService = new HealthService();
        bool result;

        // Act
        result = healthService.IsLive();

        // Assert
        Assert.True(result);
    }
    
    [Fact]
    public void CooldownTime_CurrentTimeBeforeCooldownEnds()
    {
        // Arrange
        Thread.Sleep(TimeSpan.FromSeconds(10));
        double cooldownTime = 15;
        Environment.SetEnvironmentVariable("HealthSettings_CooldownTime", cooldownTime.ToString());

        var service = new HealthService();

        // Act
        double result = service.CooldownTime();

        // Assert
        double expected = 0;
        Assert.Equal(result, expected);
    }

    [Fact]
    public void CooldownTime_CurrentTimeAfterCooldownEnds()
    {
        // Arrange
        Thread.Sleep(TimeSpan.FromSeconds(10));
        double cooldownTime = 5;
        Environment.SetEnvironmentVariable("HealthSettings_CooldownTime", cooldownTime.ToString());

        var service = new HealthService();

        // Act
        double result = service.CooldownTime();

        // Assert
        double notExpected = 0;
        Assert.NotEqual(result, notExpected);
    }
    
}