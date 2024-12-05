using Authorization.Api.Controllers;
using Microsoft.Extensions.Hosting;

namespace XUnitTestProject.AuthorizationTests;

using System.Threading.Tasks;
using Authorization.Domain.Models;
using Authorization.Domain.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

public class AccountControllerTests
    {
        private readonly Mock<IAccountService> _accountServiceMock;
        private readonly Mock<IHostApplicationLifetime> _lifetimeMock;

        public AccountControllerTests()
        {
            _accountServiceMock = new Mock<IAccountService>();
            _lifetimeMock = new Mock<IHostApplicationLifetime>();
        }

        [Fact]
        public async Task Login_ReturnsOkIfTheLoginIsSuccessful()
        {
            // Arrange
            var loginUserModel = new LoginUserModel("testuser","password");
            var loginResult = new LoginUserResult("testuser", "testuser@example.com", "token");

            _accountServiceMock.Setup(x => x.Login(loginUserModel, It.IsAny<CancellationToken>()))
                .ReturnsAsync(loginResult);

            var controller = new AccountController(_accountServiceMock.Object, _lifetimeMock.Object);

            // Act
            var result = await controller.Login(loginUserModel);

            // Assert
            Assert.IsType<OkObjectResult>(result);
            var okResult = result as OkObjectResult;
            Assert.Equal(loginResult, okResult.Value);
        }
    }