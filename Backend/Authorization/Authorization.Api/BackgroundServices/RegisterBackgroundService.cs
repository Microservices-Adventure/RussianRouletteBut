using Authorization.Api.Config;
using Authorization.Domain.Config;
using Authorization.Domain.Services.Interfaces;
using Authorization.Infrastructure.Kafka;
using Microsoft.Extensions.Options;

namespace Authorization.Api.BackgroundServices;

public class RegisterBackgroundService : BackgroundService
{
    private readonly RegisterConsumer _registerConsumer;

    public RegisterBackgroundService(IOptions<KafkaSettings> kafkaOptions, ILogger<RegisterConsumer> consumerLogger, IAccountService accountService)
    {
        _registerConsumer = new RegisterConsumer(
            kafkaOptions.Value.BootstrapServers,
            kafkaOptions.Value.ConsumerGroupId,
            kafkaOptions.Value.Topic,
            consumerLogger,
            accountService);
    }

    public override Task StopAsync(CancellationToken cancellationToken)
    {
        _registerConsumer.Dispose();
        return base.StopAsync(cancellationToken);
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await _registerConsumer.Consume();
    }
}
