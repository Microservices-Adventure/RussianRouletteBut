namespace XUnitTestProject.LifeMonitorTests;

using LifeMonitor.Api.Controllers;
using LifeMonitor.Api.Services;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Threading.Tasks;
using Xunit;

public class LifeControllerTests
{
    private readonly Mock<IMonitorService> _mockMonitorService;
    private readonly LifeController _lifeController;

    public LifeControllerTests()
    {
        _mockMonitorService = new Mock<IMonitorService>();
        _lifeController = new LifeController(_mockMonitorService.Object);
    }

    [Fact]
    public async Task GetLifes_ShouldReturnInternalServerError_WhenExceptionOccurs()
    {
        // Arrange
        _mockMonitorService.Setup(x => x.GetLife(It.IsAny<string>(), It.IsAny<string>()))
            .ThrowsAsync(new Exception("Test exception"));

        // Act
        var result = await _lifeController.GetLifes();

        // Assert
        var objectResult = Assert.IsType<ObjectResult>(result); 
        Assert.Equal(500, objectResult.StatusCode); 
        Assert.Equal("An error occurred while processing your request.", objectResult.Value); 
    }
}