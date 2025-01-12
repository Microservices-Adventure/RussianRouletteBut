using Microsoft.Extensions.Hosting;

namespace XUnitTestProject.RevolverTests;

using Microsoft.AspNetCore.Mvc;
using Moq;
using Revolver.Api.Controllers;
using Revolver.Domain.Models;
using Revolver.Domain.Services.Interfaces;
using System.Threading;
using Xunit;

public class HearthControllerTests
{
    private readonly Mock<IHostApplicationLifetime> _mockLifetime;
    private readonly Mock<IHealthService> _mockHealthService;
    private readonly HearthController _controller;

    public HearthControllerTests()
    {
        _mockLifetime = new Mock<IHostApplicationLifetime>();
        _mockHealthService = new Mock<IHealthService>();
        _controller = new HearthController(_mockLifetime.Object, _mockHealthService.Object);
    }

    [Fact]
    public void Kill_WhenCooldownTimeIsZero_ShouldStopApplicationAndReturnOk()
    {
        // Arrange
        _mockHealthService.Setup(x => x.CooldownTime()).Returns(0);
        var resetEvent = new ManualResetEventSlim(false);

        _mockLifetime.Setup(x => x.StopApplication())
            .Callback(() => resetEvent.Set()); 

        // Act
        var result = _controller.Kill();
        resetEvent.Wait(TimeSpan.FromSeconds(1)); 

        // Assert
        Assert.IsType<OkResult>(result);
        _mockLifetime.Verify(x => x.StopApplication(), Times.Once);
    }

    [Fact]
    public void Kill_WhenCooldownTimeIsGreaterThanZero_ShouldReturnBadRequest()
    {
        // Arrange
        _mockHealthService.Setup(x => x.CooldownTime()).Returns(10);

        // Act
        var result = _controller.Kill();

        // Assert
        var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
        Assert.Equal("Cooldown time left: 10", badRequestResult.Value);
        _mockLifetime.Verify(x => x.StopApplication(), Times.Never);
    }

    [Fact]
    public void IsLive_ShouldReturnOkWithIsLiveValue()
    {
        // Arrange
        _mockHealthService.Setup(x => x.IsLive()).Returns(true);

        // Act
        var result = _controller.IsLive();

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.True((bool)okResult.Value);
    }

    [Fact]
    public void KillAvailable_WhenCooldownTimeIsZero_ShouldReturnCooldownModelWithFalse()
    {
        // Arrange
        _mockHealthService.Setup(x => x.CooldownTime()).Returns(0);

        // Act
        var result = _controller.KillAvailable();

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var cooldownModel = Assert.IsType<CooldownModel>(okResult.Value);
        Assert.False(cooldownModel.IsCooldown);
        Assert.Equal(0, cooldownModel.SecondsLeft);
    }

    [Fact]
    public void KillAvailable_WhenCooldownTimeIsGreaterThanZero_ShouldReturnCooldownModelWithTrue()
    {
        // Arrange
        _mockHealthService.Setup(x => x.CooldownTime()).Returns(10);

        // Act
        var result = _controller.KillAvailable();

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var cooldownModel = Assert.IsType<CooldownModel>(okResult.Value);
        Assert.True(cooldownModel.IsCooldown);
        Assert.Equal(10, cooldownModel.SecondsLeft);
    }
}