namespace XUnitTestProject.LifeMonitorTests;

using LifeMonitor.Api.Models;
using Xunit;

public class CooldownModelTests
{
    [Fact]
    public void CooldownModel_ShouldInitializePropertiesCorrectly()
    {
        // Arrange
        var isCooldown = true;
        var secondsLeft = 123.45;

        // Act
        var model = new CooldownModel
        {
            IsCooldown = isCooldown,
            SecondsLeft = secondsLeft
        };

        // Assert
        Assert.Equal(isCooldown, model.IsCooldown);
        Assert.Equal(secondsLeft, model.SecondsLeft);
    }

    [Fact]
    public void CooldownModel_ShouldAllowFalseIsCooldown()
    {
        // Arrange
        var isCooldown = false;
        var secondsLeft = 0.0;

        // Act
        var model = new CooldownModel
        {
            IsCooldown = isCooldown,
            SecondsLeft = secondsLeft
        };

        // Assert
        Assert.False(model.IsCooldown);
        Assert.Equal(secondsLeft, model.SecondsLeft);
    }

    [Fact]
    public void CooldownModel_ShouldAllowNegativeSecondsLeft()
    {
        // Arrange
        var isCooldown = true;
        var secondsLeft = -10.0;

        // Act
        var model = new CooldownModel
        {
            IsCooldown = isCooldown,
            SecondsLeft = secondsLeft
        };

        // Assert
        Assert.Equal(isCooldown, model.IsCooldown);
        Assert.Equal(secondsLeft, model.SecondsLeft);
    }
}

public class LifeServiceModelTests
{
    [Fact]
    public void LifeServiceModel_ShouldInitializePropertiesCorrectly()
    {
        // Arrange
        var serviceName = "TestService";
        var isLife = true;
        var cooldown = new CooldownModel
        {
            IsCooldown = true,
            SecondsLeft = 123.45
        };

        // Act
        var model = new LifeServiceModel
        {
            ServiceName = serviceName,
            IsLife = isLife,
            Cooldown = cooldown
        };

        // Assert
        Assert.Equal(serviceName, model.ServiceName);
        Assert.Equal(isLife, model.IsLife);
        Assert.Equal(cooldown, model.Cooldown);
    }

    [Fact]
    public void LifeServiceModel_ShouldAllowNullCooldown()
    {
        // Arrange
        var serviceName = "TestService";
        var isLife = false;

        // Act
        var model = new LifeServiceModel
        {
            ServiceName = serviceName,
            IsLife = isLife,
            Cooldown = null
        };

        // Assert
        Assert.Equal(serviceName, model.ServiceName);
        Assert.Equal(isLife, model.IsLife);
        Assert.Null(model.Cooldown);
    }

    [Fact]
    public void LifeServiceModel_ShouldHandleDefaultValues()
    {
        // Arrange
        var serviceName = "TestService";
        var isLife = true;

        // Act
        var model = new LifeServiceModel
        {
            ServiceName = serviceName,
            IsLife = isLife
            // Cooldown is not initialized
        };

        // Assert
        Assert.Equal(serviceName, model.ServiceName);
        Assert.Equal(isLife, model.IsLife);
        Assert.Null(model.Cooldown);
    }
}