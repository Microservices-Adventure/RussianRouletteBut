using Authorization.Domain.Config;
using Authorization.Domain.Services.Interfaces;
using Authorization.Infrastructure.Kafka;
using Microsoft.Extensions.Options;

namespace Authorization.Api.BackgroundServices;

public class RegisterBackgroundService : BackgroundService
{
    private readonly RegisterConsumer _registerConsumer;
    private readonly IServiceScope _scope;

    public RegisterBackgroundService(IOptions<KafkaSettings> kafkaOptions, ILogger<RegisterConsumer> consumerLogger, IServiceScopeFactory serviceScopeFactory)
    {
        _scope = serviceScopeFactory.CreateScope();
        var accountService = _scope.ServiceProvider.GetRequiredService<IAccountService>();
        
        _registerConsumer = new RegisterConsumer(
            kafkaOptions.Value.BootstrapServers,
            kafkaOptions.Value.Topic,
            kafkaOptions.Value.ConsumerGroupId,
            consumerLogger,
            accountService);
    }

    public override Task StopAsync(CancellationToken cancellationToken)
    {
        _registerConsumer.Dispose();
        _scope.Dispose();
        return base.StopAsync(cancellationToken);
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await _registerConsumer.Consume(stoppingToken);
    }
}
