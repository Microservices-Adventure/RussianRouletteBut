using Confluent.Kafka;
using Profile.Api.DataAccess.Entity;
using Profile.Api.Models;
using Profile.Api.Services;
using System.Text.Json;

namespace Profile.Api.Kafka
{
    public class DropConsumer
    {
        private readonly IConsumer<Ignore, string> _consumer;
        private readonly ILogger<DropConsumer> _logger;
        private readonly IDropInfoService _dropService;

        public DropConsumer(
            string bootstrapServers,
            string topic,
            string groupId,
            ILogger<DropConsumer> logger,
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
                    var registerResult = await DropLog(result, stoppingToken);

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

        private async Task<bool> DropLog(ConsumeResult<Ignore, string> result, CancellationToken ct)
        {
            AddDropInfoByUsernameRequest? userModel = JsonSerializer.Deserialize<AddDropInfoByUsernameRequest>(result.Message.Value);
            if (userModel == null)
            {
                throw new Exception("AddDropInfoByUsernameRequest is null");
            }
            DropInfo drop = await _dropService.AddDropInfoByUsernameAsync(userModel, ct);
            return drop != null;
        }

        public void Dispose()
        {
            _consumer.Close();
        }
    }
}
