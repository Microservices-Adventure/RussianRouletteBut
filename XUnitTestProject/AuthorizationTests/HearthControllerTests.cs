using Authorization.Api.Controllers;
using Microsoft.Extensions.Hosting;

namespace XUnitTestProject.AuthorizationTests;

using Authorization.Domain.Models;
using Authorization.Domain.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

public class HearthControllerTests
    {
        private readonly Mock<IHostApplicationLifetime> _lifetimeMock;
        private readonly Mock<IHealthService> _healthServiceMock;

        public HearthControllerTests()
        {
            _lifetimeMock = new Mock<IHostApplicationLifetime>();
            _healthServiceMock = new Mock<IHealthService>();
        }

        [Fact]
        public void Kill_ReturnsBadRequestIfThereIsRemainingCoolingTime()
        {
            // Arrange
            _healthServiceMock.Setup(x => x.CooldownTime()).Returns(10.0);

            var controller = new HearthController(_lifetimeMock.Object, _healthServiceMock.Object);

            // Act
            var result = controller.Kill();

            // Assert
            Assert.IsType<BadRequestObjectResult>(result);
            var badRequestResult = result as BadRequestObjectResult;
            Assert.Equal("Cooldown time left: 10", badRequestResult.Value);
        }

        [Fact]
        public void IsLive_ReturnsOkWithTheResultTrue()
        {
            // Arrange
            _healthServiceMock.Setup(x => x.IsLive()).Returns(true);

            var controller = new HearthController(_lifetimeMock.Object, _healthServiceMock.Object);

            // Act
            var result = controller.IsLive();

            // Assert
            Assert.IsType<OkObjectResult>(result);
            var okResult = result as OkObjectResult;
            Assert.True((bool)okResult.Value);
        }

        [Fact]
        public void KillAvailable_ReturnsOkWithTheCorrectInformationAboutTheAvailabilityOfTheApplicationShutdown()
        {
            // Arrange
            _healthServiceMock.Setup(x => x.CooldownTime()).Returns(10.0);

            var controller = new HearthController(_lifetimeMock.Object, _healthServiceMock.Object);

            // Act
            var result = controller.KillAvailable();

            // Assert
            Assert.IsType<OkObjectResult>(result);
            var okResult = result as OkObjectResult;
            var response = okResult.Value as CooldownModel;
            Assert.NotNull(response);
            Assert.True(response.IsCooldown);
            Assert.Equal(10.0, response.SecondsLeft);
        }
    }