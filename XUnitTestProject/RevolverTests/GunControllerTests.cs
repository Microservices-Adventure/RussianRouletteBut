using Microsoft.Extensions.Hosting;
using Revolver.Api.Controllers;

namespace XUnitTestProject.RevolverTests;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Moq;
using Revolver.Domain.Config;
using Revolver.Domain.Models;
using Revolver.Domain.Services.Interfaces;
using Xunit;

public class GunControllerTests
    {
        private readonly Mock<IRevolverService> _revolverServiceMock;
        private readonly Mock<IOptionsMonitor<ServicesParameters>> _servicesOptionsMock;
        private readonly Mock<IHostApplicationLifetime> _lifetimeMock;

        public GunControllerTests()
        {
            _revolverServiceMock = new Mock<IRevolverService>();
            _servicesOptionsMock = new Mock<IOptionsMonitor<ServicesParameters>>();
            _lifetimeMock = new Mock<IHostApplicationLifetime>();
        }

        [Fact]
        public void Shoot_ReturnsNotFoundIfServiceNotFound()
        {
            // Arrange
            var request = new ShootRequestModel
            {
                Bullets = new List<string> { "NonExistentService" }
            };

            var servicesParameters = new ServicesParameters
            {
                Services = new List<ServiceInfo>
                {
                    new ServiceInfo { ServiceName = "ExistingService" }
                }
            };

            _servicesOptionsMock.Setup(x => x.CurrentValue).Returns(servicesParameters);

            var controller = new GunController(_revolverServiceMock.Object, _servicesOptionsMock.Object, _lifetimeMock.Object);

            // Act
            var result = controller.Shoot(request);

            // Assert
            Assert.IsType<NotFoundObjectResult>(result);
            var notFoundResult = result as NotFoundObjectResult;
            Assert.Equal("Service not found", notFoundResult.Value);
        }

        [Fact]
        public void Shoot_ProcessesTheRequestCorrectlyAndSelectsTheService()
        {
            // Arrange
            var request = new ShootRequestModel
            {
                Bullets = new List<string> { "Service1", "Service2" }
            };

            var servicesParameters = new ServicesParameters
            {
                Services = new List<ServiceInfo>
                {
                    new ServiceInfo { ServiceName = "Service1" },
                    new ServiceInfo { ServiceName = "Service2" }
                }
            };

            _servicesOptionsMock.Setup(x => x.CurrentValue).Returns(servicesParameters);
            _revolverServiceMock.Setup(x => x.Roll(It.IsAny<List<ServiceInfo>>())).Returns(servicesParameters.Services.First());

            var controller = new GunController(_revolverServiceMock.Object, _servicesOptionsMock.Object, _lifetimeMock.Object);

            // Act
            var result = controller.Shoot(request);

            // Assert
            Assert.IsType<OkResult>(result);
        }

        [Fact]
        public void Shoot_ProcessesTheRequestCorrectlyAndSelectsTheRevolverService()
        {
            // Arrange
            var request = new ShootRequestModel
            {
                Bullets = new List<string> { "Revolver" }
            };

            var servicesParameters = new ServicesParameters
            {
                Services = new List<ServiceInfo>
                {
                    new ServiceInfo { ServiceName = "Revolver" }
                }
            };

            _servicesOptionsMock.Setup(x => x.CurrentValue).Returns(servicesParameters);
            _revolverServiceMock.Setup(x => x.Roll(It.IsAny<List<ServiceInfo>>())).Returns(servicesParameters.Services.First());

            var controller = new GunController(_revolverServiceMock.Object, _servicesOptionsMock.Object, _lifetimeMock.Object);

            // Act
            var result = controller.Shoot(request);

            // Assert
            Assert.IsType<OkResult>(result);
        }
    }