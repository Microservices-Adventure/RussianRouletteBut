namespace XUnitTestProject.RevolverTests;

using Revolver.Domain.Services;
using System;
using Xunit;

public class HealthServiceTests
{
    private readonly HealthService _healthService;

    public HealthServiceTests()
    {
        _healthService = new HealthService();
    }
    
    [Fact]
    public void CooldownTime_WhenAppStartAtIsInFuture_ShouldReturnZero()
    {
        // Arrange
        var appStartAt = DateTimeOffset.UtcNow.AddSeconds(10); 
        var cooldownTime = 5; 

        
        Environment.SetEnvironmentVariable("HealthSettings_CooldownTime", cooldownTime.ToString());
        Environment.SetEnvironmentVariable("HealthSettings_AppStartAt", appStartAt.ToUnixTimeSeconds().ToString());

        // Act
        var result = _healthService.CooldownTime();

        // Assert
        Assert.Equal(0, result); 
        
        Environment.SetEnvironmentVariable("HealthSettings_CooldownTime", null);
        Environment.SetEnvironmentVariable("HealthSettings_AppStartAt", null);
    }

    [Fact]
    public void CooldownTime_WhenCooldownTimeIsZero_ShouldReturnZero()
    {
        // Arrange
        var appStartAt = DateTimeOffset.UtcNow.AddSeconds(-10); 
        var cooldownTime = 0;
        
        Environment.SetEnvironmentVariable("HealthSettings_CooldownTime", cooldownTime.ToString());
        Environment.SetEnvironmentVariable("HealthSettings_AppStartAt", appStartAt.ToUnixTimeSeconds().ToString());

        // Act
        var result = _healthService.CooldownTime();

        // Assert
        Assert.Equal(0, result); 
        
        Environment.SetEnvironmentVariable("HealthSettings_CooldownTime", null);
        Environment.SetEnvironmentVariable("HealthSettings_AppStartAt", null);
    }

    [Fact]
    public void IsLive_ShouldAlwaysReturnTrue()
    {
        // Act
        var result = _healthService.IsLive();

        // Assert
        Assert.True(result);
    }
}