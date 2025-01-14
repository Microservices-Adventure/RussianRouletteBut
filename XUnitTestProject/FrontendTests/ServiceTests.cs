using Frontend.App.Config;
using Frontend.Entities.Profile.Model;
using Frontend.Features.KafkaProducers;

namespace XUnitTestProject.FrontendTests;

using System;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Frontend.Entities.ActionLog;
using Frontend.Features.Services;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using Moq.Protected;
using Xunit;

public class LogServiceTests
{
    private readonly Mock<IOptions<KafkaSettings>> _mockKafkaSettings;
    private readonly Mock<ILogger<LogProducer>> _mockLoggerLogProducer;
    private readonly LogService _logService;

    public LogServiceTests()
    {
        _mockKafkaSettings = new Mock<IOptions<KafkaSettings>>();
        _mockLoggerLogProducer = new Mock<ILogger<LogProducer>>();

        _mockKafkaSettings.Setup(x => x.Value).Returns(new KafkaSettings
        {
            BootstrapServers = "localhost:9092",
            LogTopic = "log-topic"
        });

        _logService = new LogService(_mockKafkaSettings.Object, _mockLoggerLogProducer.Object);
    }

    [Fact]
    public async Task GetLogs_ShouldThrowException_WhenRequestFails()
    {
        // Arrange
        var getLogsRequest = new GetLogsRequest
        {
            Page = 1,
            Size = 10,
            Username = "testuser",
            Email = "test@example.com",
            MicroserviceId = 1,
            MicroserviceName = "TestService",
            Description = "Test log",
            Status = "Success",
            HasError = false,
            From = DateTime.UtcNow.AddDays(-1),
            To = DateTime.UtcNow
        };

        var mockHttpMessageHandler = new Mock<HttpMessageHandler>();
        mockHttpMessageHandler.Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>()
            )
            .ReturnsAsync(new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.BadRequest,
                Content = new StringContent("Failed to get logs")
            });

        var httpClient = new HttpClient(mockHttpMessageHandler.Object);
        
        var httpClientField = typeof(LogService).GetField("_httpClient", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        httpClientField.SetValue(_logService, httpClient);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<Exception>(() => _logService.GetLogs(getLogsRequest));
        Assert.Contains("Failed to get logs", exception.Message);
    }
}

public class MonitorServiceTests
{
    [Fact]
    public async Task GetLifes_ShouldReturnEmptyList_WhenRequestFails()
    {
        // Arrange
        var mockHttpMessageHandler = new Mock<HttpMessageHandler>();
        mockHttpMessageHandler.Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>()
            )
            .ReturnsAsync(new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.BadRequest
            });

        var httpClient = new HttpClient(mockHttpMessageHandler.Object);
        var monitorService = new MonitorService();
        var httpClientField = typeof(MonitorService).GetField("_httpClient", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        httpClientField.SetValue(monitorService, httpClient);

        // Act
        var result = await monitorService.GetLifes();

        // Assert
        Assert.NotNull(result);
        Assert.Empty(result);
    }

    [Fact]
    public async Task GetLifes_ShouldReturnEmptyList_WhenJsonDeserializationFails()
    {
        // Arrange
        var mockHttpMessageHandler = new Mock<HttpMessageHandler>();
        mockHttpMessageHandler.Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>()
            )
            .ReturnsAsync(new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent("invalid json")
            });

        var httpClient = new HttpClient(mockHttpMessageHandler.Object);
        var monitorService = new MonitorService();
        var httpClientField = typeof(MonitorService).GetField("_httpClient", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        httpClientField.SetValue(monitorService, httpClient);

        // Act
        var result = await monitorService.GetLifes();

        // Assert
        Assert.NotNull(result);
        Assert.Empty(result);
    }
}

public class ProfileServiceTests
{
    [Fact]
    public async Task GetUserProfile_ShouldThrowException_WhenRequestFails()
    {
        // Arrange
        var request = new GetUserProfileRequest { Username = "testuser" };

        var mockHttpMessageHandler = new Mock<HttpMessageHandler>();
        mockHttpMessageHandler.Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.Is<HttpRequestMessage>(req =>
                    req.RequestUri.ToString().Contains($"Username={Uri.EscapeDataString(request.Username)}")
                ),
                ItExpr.IsAny<CancellationToken>()
            )
            .ReturnsAsync(new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.BadRequest,
                Content = new StringContent("User not found")
            });

        var httpClient = new HttpClient(mockHttpMessageHandler.Object);
        var profileService = new ProfileService();
        var httpClientField = typeof(ProfileService).GetField("_httpClient", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        httpClientField.SetValue(profileService, httpClient);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<Exception>(() => profileService.GetUserProfile(request));
        Assert.Contains("HTTP Request failed", exception.Message);
    }

    [Fact]
    public async Task GetUserProfile_ShouldThrowException_WhenResponseIsInvalid()
    {
        // Arrange
        var request = new GetUserProfileRequest { Username = "testuser" };

        var mockHttpMessageHandler = new Mock<HttpMessageHandler>();
        mockHttpMessageHandler.Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.Is<HttpRequestMessage>(req =>
                    req.RequestUri.ToString().Contains($"Username={Uri.EscapeDataString(request.Username)}")
                ),
                ItExpr.IsAny<CancellationToken>()
            )
            .ReturnsAsync(new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent("invalid json")
            });

        var httpClient = new HttpClient(mockHttpMessageHandler.Object);
        var profileService = new ProfileService();
        var httpClientField = typeof(ProfileService).GetField("_httpClient", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        httpClientField.SetValue(profileService, httpClient);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<Exception>(() => profileService.GetUserProfile(request));
        Assert.Contains("Error while deserializing the HTTP response", exception.Message);
    }

    [Fact]
    public async Task GetUserProfile_ShouldThrowArgumentNullException_WhenRequestIsNull()
    {
        // Arrange
        var profileService = new ProfileService();

        // Act & Assert
        await Assert.ThrowsAsync<ArgumentNullException>(() => profileService.GetUserProfile(null));
    }
}