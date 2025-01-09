using ActionLog.Api.Config;
using ActionLog.Api.Kafka;
using ActionLog.Api.Services;
using Microsoft.Extensions.Options;

namespace ActionLog.Api.BackgroundServices
{
    public class LogBackgroundService : BackgroundService
    {
        private readonly LogConsumer _logConsumer;
        private readonly IServiceScope _scope;

        public LogBackgroundService(IOptions<KafkaSettings> kafkaOptions, ILogger<LogConsumer> consumerLogger, IServiceScopeFactory serviceScopeFactory)
        {
            _scope = serviceScopeFactory.CreateScope();
            var logService = _scope.ServiceProvider.GetRequiredService<ILogService>();

            _logConsumer = new LogConsumer(
                kafkaOptions.Value.BootstrapServers,
                kafkaOptions.Value.Topic,
                kafkaOptions.Value.ConsumerGroupId,
                consumerLogger,
                logService);
        }

        public override Task StopAsync(CancellationToken cancellationToken)
        {
            _logConsumer.Dispose();
            _scope.Dispose();
            return base.StopAsync(cancellationToken);
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await _logConsumer.Consume(stoppingToken);
        }
    }
}
