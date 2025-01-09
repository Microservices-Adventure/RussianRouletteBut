using ActionLog.Api.DataAccess.Entity;
using ActionLog.Api.Models;
using ActionLog.Api.Services;
using Confluent.Kafka;
using System.ComponentModel.DataAnnotations;
using System.Text.Json;

namespace ActionLog.Api.Kafka
{
    public class LogConsumer : IDisposable
    {
        private readonly IConsumer<Ignore, string> _consumer;
        private readonly ILogger<LogConsumer> _logger;
        private readonly ILogService _logService;

        public LogConsumer(
            string bootstrapServers,
            string topic,
            string groupId,
            ILogger<LogConsumer> logger,
            ILogService logService)
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
            _logService = logService;
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
                _logger.LogInformation("Log request: {result}", result.Message.Value);
                try
                {
                    var registerResult = await TryLog(result, stoppingToken);

                    if (registerResult)
                    {
                        _logger.LogInformation("Log request successfully processed!");
                    }
                    else
                    {
                        _logger.LogInformation("Log request failed!");
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
            ALog aLog = await _logService.AddLogAsync(userModel, ct);
            return aLog != null;
        }

        public void Dispose()
        {
            _consumer.Close();
        }
    }
}
