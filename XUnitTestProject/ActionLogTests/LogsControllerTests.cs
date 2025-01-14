using ActionLog.Api.DataAccess.Entity;

namespace XUnitTestProject.ActionLogTests;

using ActionLog.Api.Controllers;
using ActionLog.Api.DataAccess;
using ActionLog.Api.Models;
using ActionLog.Api.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using Moq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

public class LogsControllerTests
{
    private readonly Mock<ILogService> _logServiceMock;
    private readonly Mock<IHostApplicationLifetime> _appLifetimeMock;
    private readonly LogsController _logsController;

    public LogsControllerTests()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: "TestDatabase")
            .Options;
        
        var contextMock = new Mock<AppDbContext>(options);
        
        _logServiceMock = new Mock<ILogService>();
        _appLifetimeMock = new Mock<IHostApplicationLifetime>();
        
        _logsController = new LogsController(contextMock.Object, _logServiceMock.Object, _appLifetimeMock.Object);
    }

    [Fact]
    public async Task GetLogs_ShouldReturnOk_WhenRequestIsValid()
    {
        // Arrange
        var request = new GetLogsRequest
        {
            Page = 1,
            Size = 10
        };

        var logs = new List<ALog>
        {
            new ALog { Id = 1, Username = "user1", Description = "Test log 1" },
            new ALog { Id = 2, Username = "user2", Description = "Test log 2" }
        };

        _logServiceMock
            .Setup(x => x.GetLogsAsync(request))
            .ReturnsAsync((logs.Count, logs));

        // Act
        var result = await _logsController.GetLogs(request);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var response = okResult.Value;
        var totalRecords = (int)response.GetType().GetProperty("TotalRecords").GetValue(response);
        var returnedLogs = (IEnumerable<ALog>)response.GetType().GetProperty("Logs").GetValue(response);

        Assert.Equal(logs.Count, totalRecords);
        Assert.Equal(logs, returnedLogs);
    }

    [Fact]
    public async Task GetLogs_ShouldReturnBadRequest_WhenRequestIsInvalid()
    {
        // Arrange
        var request = new GetLogsRequest
        {
            Page = 0, 
            Size = 10
        };

        _logServiceMock
            .Setup(x => x.GetLogsAsync(request))
            .ThrowsAsync(new ArgumentException("Page and size must be greater than 0."));

        // Act
        var result = await _logsController.GetLogs(request);

        // Assert
        var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
        Assert.Equal("Page and size must be greater than 0.", badRequestResult.Value);
    }

    [Fact]
    public async Task AddLog_ShouldReturnCreated_WhenRequestIsValid()
    {
        // Arrange
        var request = new AddLogRequest
        {
            Username = "testUser",
            Description = "Test log",
            Status = "Success"
        };

        var log = new ALog
        {
            Id = 1,
            Username = request.Username,
            Description = request.Description,
            Status = request.Status
        };

        _logServiceMock
            .Setup(x => x.AddLogAsync(request, It.IsAny<CancellationToken>()))
            .ReturnsAsync(log);

        // Act
        var result = await _logsController.AddLog(request);

        // Assert
        var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(result);
        Assert.Equal(nameof(LogsController.GetLogs), createdAtActionResult.ActionName);
        Assert.Equal(log.Id, (createdAtActionResult.Value as ALog)?.Id);
    }

    [Fact]
    public async Task AddLog_ShouldReturnBadRequest_WhenRequestIsNull()
    {
        // Arrange
        AddLogRequest request = null;

        // Act
        var result = await _logsController.AddLog(request);

        // Assert
        var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
        Assert.Equal("Log data is null.", badRequestResult.Value);
    }

    [Fact]
    public async Task AddLog_ShouldReturn499_WhenOperationIsCanceled()
    {
        // Arrange
        var request = new AddLogRequest
        {
            Username = "testUser",
            Description = "Test log",
            Status = "Success"
        };

        _logServiceMock
            .Setup(x => x.AddLogAsync(request, It.IsAny<CancellationToken>()))
            .ThrowsAsync(new OperationCanceledException());

        // Act
        var result = await _logsController.AddLog(request);

        // Assert
        var objectResult = Assert.IsType<ObjectResult>(result);
        Assert.Equal(499, objectResult.StatusCode); 
        Assert.Equal("Application is stopping.", objectResult.Value);
    }
}