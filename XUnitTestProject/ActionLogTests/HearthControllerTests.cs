namespace XUnitTestProject.ActionLogTests;

using ActionLog.Api.Controllers;
using ActionLog.Api.Models;
using ActionLog.Api.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using Moq;
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
        public void Kill_ShouldReturnBadRequest_WhenCooldownTimeIsGreaterThanZero()
        {
            // Arrange
            _mockHealthService.Setup(x => x.CooldownTime()).Returns(10);

            // Act
            var result = _controller.Kill();

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("Cooldown time left: 10", badRequestResult.Value);
        }

        [Fact]
        public async Task Kill_ShouldReturnOk_AndStopApplication_WhenCooldownTimeIsZero()
        {
            // Arrange
            _mockHealthService.Setup(x => x.CooldownTime()).Returns(0);

            // Act
            var result = _controller.Kill();

            // Даем время потоку выполниться
            await Task.Delay(600);

            // Assert
            Assert.IsType<OkResult>(result);
            _mockLifetime.Verify(x => x.StopApplication(), Times.Once);
        }
        
        [Fact]
        public void IsLive_ShouldReturnOk_WithIsLiveStatus()
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
        public void KillAvailable_ShouldReturnOk_WithCooldownModel()
        {
            // Arrange
            _mockHealthService.Setup(x => x.CooldownTime()).Returns(5);
            var expectedResponse = new CooldownModel
            {
                IsCooldown = true,
                SecondsLeft = 5
            };

            // Act
            var result = _controller.KillAvailable();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var actualResponse = Assert.IsType<CooldownModel>(okResult.Value);
            Assert.Equal(expectedResponse.IsCooldown, actualResponse.IsCooldown);
            Assert.Equal(expectedResponse.SecondsLeft, actualResponse.SecondsLeft);
        }
    }
