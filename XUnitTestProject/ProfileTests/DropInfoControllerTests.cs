using Microsoft.Extensions.Hosting;

namespace XUnitTestProject.ProfileTests;

using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Profile.Api.Controllers;
using Profile.Api.Models;
using Profile.Api.Services;
using Xunit;

    public class ProfileControllerTests
    {
        private readonly Mock<IDropInfoService> _mockDropInfoService;
        private readonly Mock<IHostApplicationLifetime> _mockAppLifetime;
        private readonly ProfileController _controller;

        public ProfileControllerTests()
        {
            _mockDropInfoService = new Mock<IDropInfoService>();
            _mockAppLifetime = new Mock<IHostApplicationLifetime>();
            _controller = new ProfileController(_mockDropInfoService.Object, _mockAppLifetime.Object);
        }

        [Fact]
        public async Task AddUserProfile_ReturnsBadRequest_WhenRequestIsNull()
        {
            // Arrange
            AddUserProfileRequest request = null;

            // Act
            var result = await _controller.AddUserProfile(request);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("UserProfile data is null.", badRequestResult.Value);
        }

        [Fact]
        public async Task GetUserProfile_ReturnsBadRequest_WhenRequestIsNull()
        {
            // Arrange
            GetUserProfileRequest request = null;

            // Act
            var result = await _controller.GetUserProfile(request);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("Request data is null.", badRequestResult.Value);
        }

        [Fact]
        public async Task AddDropInfoByUsername_ReturnsBadRequest_WhenRequestIsNull()
        {
            // Arrange
            AddDropInfoByUsernameRequest request = null;

            // Act
            var result = await _controller.AddDropInfoByUsername(request);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("DropInfo data is null.", badRequestResult.Value);
        }
    }