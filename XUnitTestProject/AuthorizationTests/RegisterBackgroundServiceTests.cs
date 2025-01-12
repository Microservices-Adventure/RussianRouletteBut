using Authorization.Api.BackgroundServices;
using Microsoft.Extensions.DependencyInjection;

namespace XUnitTestProject.AuthorizationTests;

using Authorization.Domain.Config;
using Authorization.Domain.Services.Interfaces;
using Authorization.Infrastructure.Kafka;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using System;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

    public class RegisterBackgroundServiceTests
    {
        private readonly Mock<IOptions<KafkaSettings>> _mockKafkaOptions;
        private readonly Mock<ILogger<RegisterConsumer>> _mockConsumerLogger;
        private readonly Mock<IServiceScopeFactory> _mockServiceScopeFactory;
        private readonly Mock<IServiceScope> _mockServiceScope;
        private readonly Mock<IAccountService> _mockAccountService;
        private readonly Mock<RegisterConsumer> _mockRegisterConsumer;
        private readonly RegisterBackgroundService _registerBackgroundService;

        public RegisterBackgroundServiceTests()
        {
            _mockKafkaOptions = new Mock<IOptions<KafkaSettings>>();
            _mockConsumerLogger = new Mock<ILogger<RegisterConsumer>>();
            _mockServiceScopeFactory = new Mock<IServiceScopeFactory>();
            _mockServiceScope = new Mock<IServiceScope>();
            _mockAccountService = new Mock<IAccountService>();

            // Мокируем RegisterConsumer
            _mockRegisterConsumer = new Mock<RegisterConsumer>(
                "localhost:9092", // BootstrapServers
                "test-topic",     // Topic
                "test-group",     // ConsumerGroupId
                _mockConsumerLogger.Object,
                _mockAccountService.Object);

            var serviceProvider = new Mock<IServiceProvider>();
            serviceProvider.Setup(x => x.GetService(typeof(IAccountService))).Returns(_mockAccountService.Object);

            _mockServiceScope.Setup(x => x.ServiceProvider).Returns(serviceProvider.Object);
            _mockServiceScopeFactory.Setup(x => x.CreateScope()).Returns(_mockServiceScope.Object);

            _mockKafkaOptions.Setup(x => x.Value).Returns(new KafkaSettings
            {
                BootstrapServers = "localhost:9092",
                Topic = "test-topic",
                ConsumerGroupId = "test-group"
            });

            _registerBackgroundService = new RegisterBackgroundService(
                _mockKafkaOptions.Object,
                _mockConsumerLogger.Object,
                _mockServiceScopeFactory.Object);

            // Внедряем mock-объект RegisterConsumer через рефлексию
            var consumerField = typeof(RegisterBackgroundService)
                .GetField("_registerConsumer", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic);
            consumerField.SetValue(_registerBackgroundService, _mockRegisterConsumer.Object);
        }

        [Fact]
        public async Task ExecuteAsync_ShouldCallConsume()
        {
            // Arrange
            var cancellationTokenSource = new CancellationTokenSource();
            var cancellationToken = cancellationTokenSource.Token;

            _mockRegisterConsumer.Setup(x => x.Consume(cancellationToken)).Returns(Task.CompletedTask);

            // Act
            await _registerBackgroundService.StartAsync(cancellationToken); // Запускаем фоновую службу
            await Task.Delay(100); // Даем время для выполнения
            await _registerBackgroundService.StopAsync(cancellationToken); // Останавливаем фоновую службу

            // Assert
            _mockRegisterConsumer.Verify(x => x.Consume(cancellationToken), Times.Never);
        }

        [Fact]
        public async Task StopAsync_ShouldDisposeConsumerAndScope()
        {
            // Arrange
            var cancellationTokenSource = new CancellationTokenSource();
            var cancellationToken = cancellationTokenSource.Token;

            // Act
            await _registerBackgroundService.StopAsync(cancellationToken);

            // Assert
            _mockRegisterConsumer.Verify(x => x.Dispose(), Times.Once);
            _mockServiceScope.Verify(x => x.Dispose(), Times.Once);
        }
    }
