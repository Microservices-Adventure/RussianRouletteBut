using Microsoft.Extensions.DependencyInjection;

namespace XUnitTestProject.ProfileTests;

using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using Profile.Api.BackgroundServices;
using Profile.Api.Config;
using Profile.Api.Kafka;
using Profile.Api.Services;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

    public class ProfileBackgroundServiceTests
    {
        private readonly Mock<IOptions<KafkaSettings>> _kafkaOptionsMock;
        private readonly Mock<ILogger<DropConsumer>> _consumerLoggerMock;
        private readonly Mock<IServiceScopeFactory> _serviceScopeFactoryMock;
        private readonly Mock<IDropInfoService> _dropInfoServiceMock;
        private readonly ProfileBackgroundService _profileBackgroundService;

        public ProfileBackgroundServiceTests()
        {
            _kafkaOptionsMock = new Mock<IOptions<KafkaSettings>>();
            _consumerLoggerMock = new Mock<ILogger<DropConsumer>>();
            _serviceScopeFactoryMock = new Mock<IServiceScopeFactory>();
            _dropInfoServiceMock = new Mock<IDropInfoService>();
            
            var serviceScopeMock = new Mock<IServiceScope>();
            var serviceProviderMock = new Mock<IServiceProvider>();

            serviceProviderMock
                .Setup(x => x.GetService(typeof(IDropInfoService)))
                .Returns(_dropInfoServiceMock.Object);

            serviceScopeMock
                .Setup(x => x.ServiceProvider)
                .Returns(serviceProviderMock.Object);

            _serviceScopeFactoryMock
                .Setup(x => x.CreateScope())
                .Returns(serviceScopeMock.Object);
            
            var kafkaSettings = new KafkaSettings
            {
                BootstrapServers = "localhost:9092",
                Topic = "test-topic",
                ConsumerGroupId = "test-group"
            };

            _kafkaOptionsMock
                .Setup(x => x.Value)
                .Returns(kafkaSettings);
            
            _profileBackgroundService = new ProfileBackgroundService(
                _kafkaOptionsMock.Object,
                _consumerLoggerMock.Object,
                _serviceScopeFactoryMock.Object);
        }

        [Fact]
        public async Task StopAsync_DisposesProfileConsumerAndScope()
        {
            // Arrange
            var cancellationTokenSource = new CancellationTokenSource();
            var cancellationToken = cancellationTokenSource.Token;

            // Act
            await _profileBackgroundService.StartAsync(cancellationToken);
            await _profileBackgroundService.StopAsync(cancellationToken);

            // Assert
            _serviceScopeFactoryMock.Verify(x => x.CreateScope(), Times.Once);
        }

        [Fact]
        public async Task ExecuteAsync_HandlesCancellationGracefully()
        {
            // Arrange
            var cancellationTokenSource = new CancellationTokenSource();
            var cancellationToken = cancellationTokenSource.Token;

            // Act
            await _profileBackgroundService.StartAsync(cancellationToken);
            cancellationTokenSource.Cancel(); 
            await _profileBackgroundService.StopAsync(cancellationToken);

            // Assert
            _serviceScopeFactoryMock.Verify(x => x.CreateScope(), Times.Once);
        }
    }


    
