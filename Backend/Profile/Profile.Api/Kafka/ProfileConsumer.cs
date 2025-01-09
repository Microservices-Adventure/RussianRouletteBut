using Confluent.Kafka;
using Profile.Api.Services;
using System.Text.Json;

namespace Profile.Api.Kafka
{
    public class ProfileConsumer
    {
        private readonly IConsumer<Ignore, string> _consumer;
        private readonly ILogger<ProfileConsumer> _logger;
        private readonly IDropInfoService _dropService;

        public ProfileConsumer(
            string bootstrapServers,
            string topic,
            string groupId,
            ILogger<ProfileConsumer> logger,
            IDropInfoService dropService)
        {
            var builder = new ConsumerBuilder<Ignore, string>(
                new ConsumerConfig
                {
                    BootstrapServers = bootstrapServers,
                    GroupId = groupId,
                    AutoOffsetReset = AutoOffsetReset.Earliest,
                    EnableAutoCommit = false,
                });

            _logger = logger;

            _consumer = builder.Build();
            _consumer.Subscribe(topic);
            _dropService = dropService;
        }

        public async Task Consume(CancellationToken stoppingToken)
        {
            if (stoppingToken.IsCancellationRequested)
            {
                return;
            }
            await Task.Yield();

            while (stoppingToken.IsCancellationRequested == false && _consumer.Consume(stoppingToken) is { } result)
            {
                _logger.LogInformation("Drop request: {result}", result.Message.Value);
                try
                {
                    var registerResult = await TryLog(result, stoppingToken);

                    if (registerResult)
                    {
                        _logger.LogInformation("Drop request successfully processed!");
                    }
                    else
                    {
                        _logger.LogInformation("Drop request failed!");
                    }
                }
                catch (Exception e)
                {
                    _logger.LogError(e, "Error");
                }

                _consumer.Commit(result);
            }
        }

        private async Task<bool> TryLog(ConsumeResult<Ignore, string> result, CancellationToken ct)
        {
            AddLogRequest? userModel = JsonSerializer.Deserialize<AddLogRequest>(result.Message.Value);
            if (userModel == null)
            {
                throw new Exception("AddLogRequest is null");
            }
            ALog aLog = await _dropService.AddLogAsync(userModel, ct);
            return aLog != null;
        }

        public void Dispose()
        {
            _consumer.Close();
        }
    }
}
