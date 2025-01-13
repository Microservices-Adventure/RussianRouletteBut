namespace XUnitTestProject.ActionLogTests;

using ActionLog.Api.DataAccess;
using ActionLog.Api.DataAccess.Entity;
using ActionLog.Api.Models;
using ActionLog.Api.Services;
using Microsoft.EntityFrameworkCore;
using Moq;
using Xunit;

public class LogServiceTests
{
    private readonly Mock<AppDbContext> _contextMock;
    private readonly LogServices.LogService _logService;

    public LogServiceTests()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: "TestDatabase")
            .Options;

        _contextMock = new Mock<AppDbContext>(options);
        _logService = new LogServices.LogService(_contextMock.Object);
    }

    [Fact]
    public async Task AddLogAsync_ShouldAddLog_WhenRequestIsValid()
    {
        // Arrange
        var request = new AddLogRequest
        {
            Username = "testUser",
            Email = "test@example.com",
            MicroserviceName = "TestService",
            Description = "Test log",
            Status = "Success"
        };

        var mockDbSet = new Mock<DbSet<ALog>>();
        _contextMock.Setup(x => x.ActionLogs).Returns(mockDbSet.Object);

        // Act
        var result = await _logService.AddLogAsync(request, CancellationToken.None);

        // Assert
        mockDbSet.Verify(x => x.Add(It.IsAny<ALog>()), Times.Once);
        _contextMock.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);

        Assert.Equal(request.Username, result.Username);
        Assert.Equal(request.Email, result.Email);
        Assert.Equal(request.MicroserviceName, result.MicroserviceName);
        Assert.Equal(request.Description, result.Description);
        Assert.Equal(request.Status, result.Status);
    }

    [Fact]
    public async Task AddLogAsync_ShouldThrowException_WhenRequestIsNull()
    {
        // Arrange
        AddLogRequest request = null;

        // Act & Assert
        await Assert.ThrowsAsync<ArgumentNullException>(() => _logService.AddLogAsync(request, CancellationToken.None));
    }

    [Fact]
    public async Task AddLogAsync_ShouldThrowException_WhenRequiredFieldsAreMissing()
    {
        // Arrange
        var request = new AddLogRequest
        {
            Username = "testUser",
            Email = "test@example.com",
            MicroserviceName = "", 
            Description = "", 
            Status = "" 
        };

        // Act & Assert
        await Assert.ThrowsAsync<ArgumentException>(() => _logService.AddLogAsync(request, CancellationToken.None));
    }
}