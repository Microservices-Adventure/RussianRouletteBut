namespace Profile.Api.BackgroundServices
{
    public class ProfileBackgroundService
    {
        private readonly LogConsumer _profileConsumer;
        private readonly IServiceScope _scope;

        public LogBackgroundService(IOptions<KafkaSettings> kafkaOptions, ILogger<LogConsumer> consumerLogger, IServiceScopeFactory serviceScopeFactory)
        {
            _scope = serviceScopeFactory.CreateScope();
            var logService = _scope.ServiceProvider.GetRequiredService<ILogService>();

            _profileConsumer = new LogConsumer(
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
