using Microsoft.Extensions.Options;
using Profile.Api.Config;
using Profile.Api.Kafka;
using Profile.Api.Services;

namespace Profile.Api.BackgroundServices
{
    public class ProfileBackgroundService : BackgroundService
    {
        private readonly DropConsumer _profileConsumer;
        private readonly IServiceScope _scope;

        public ProfileBackgroundService(IOptions<KafkaSettings> kafkaOptions, ILogger<DropConsumer> consumerLogger, IServiceScopeFactory serviceScopeFactory)
        {
            _scope = serviceScopeFactory.CreateScope();
            var logService = _scope.ServiceProvider.GetRequiredService<IDropInfoService>();

            _profileConsumer = new DropConsumer(
                kafkaOptions.Value.BootstrapServers,
                kafkaOptions.Value.Topic,
                kafkaOptions.Value.ConsumerGroupId,
                consumerLogger,
                logService);
        }

        public override Task StopAsync(CancellationToken cancellationToken)
        {
            _profileConsumer.Dispose();
            _scope.Dispose();
            return base.StopAsync(cancellationToken);
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await _profileConsumer.Consume(stoppingToken);
        }
    }
}
