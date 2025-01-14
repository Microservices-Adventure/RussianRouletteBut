using Microsoft.Extensions.Hosting;

namespace XUnitTestProject.ProfileTests;

using Microsoft.AspNetCore.Mvc;
using Moq;
using Profile.Api.Controllers;
using Profile.Api.Models;
using Profile.Api.Services;
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
        public async Task Kill_ReturnsOk_WhenCooldownTimeIsZero()
        {
            // Arrange
            _mockHealthService
                .Setup(service => service.CooldownTime())
                .Returns(0);

            // Act
            var result = _controller.Kill();
            
            await Task.Delay(600); 

            // Assert
            Assert.IsType<OkResult>(result);
            _mockLifetime.Verify(lifetime => lifetime.StopApplication(), Times.Once);
        }

        [Fact]
        public void Kill_ReturnsBadRequest_WhenCooldownTimeIsGreaterThanZero()
        {
            // Arrange
            double cooldownTime = 10.5;
            _mockHealthService
                .Setup(service => service.CooldownTime())
                .Returns(cooldownTime);

            // Act
            var result = _controller.Kill();

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("Cooldown time left: " + cooldownTime, badRequestResult.Value);
            _mockLifetime.Verify(lifetime => lifetime.StopApplication(), Times.Never);
        }

        [Fact]
        public void IsLive_ReturnsOkWithLiveStatus()
        {
            // Arrange
            bool isLive = true;
            _mockHealthService
                .Setup(service => service.IsLive())
                .Returns(isLive);

            // Act
            var result = _controller.IsLive();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(isLive, okResult.Value);
        }

        [Fact]
        public void KillAvailable_ReturnsOkWithCooldownInfo()
        {
            // Arrange
            double cooldownTime = 5.0;
            _mockHealthService
                .Setup(service => service.CooldownTime())
                .Returns(cooldownTime);

            var expectedResponse = new CooldownModel
            {
                IsCooldown = cooldownTime > 0,
                SecondsLeft = cooldownTime
            };

            // Act
            var result = _controller.KillAvailable();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var response = Assert.IsType<CooldownModel>(okResult.Value);
            Assert.Equal(expectedResponse.IsCooldown, response.IsCooldown);
            Assert.Equal(expectedResponse.SecondsLeft, response.SecondsLeft);
        }
    }