using System.Text.Json;
using Authorization.Domain.Models;
using Confluent.Kafka;
using Microsoft.Extensions.Logging;

namespace Authorization.Domain.KafkaProducers;

public class ProfileProducer : IDisposable
{
    private readonly IProducer<long, string> _producer;
    private readonly string _topic;
    private readonly ILogger<ProfileProducer> _logger;

    public ProfileProducer(
        string bootstrapServers,
        string topic,
        ILogger<ProfileProducer> producerLogger)
    {
        _topic = topic;
        _logger = producerLogger;
        var builder = new ProducerBuilder<long, string>(new ProducerConfig
        {
            BootstrapServers = bootstrapServers,
            BatchSize = 50000,
            LingerMs = 5
        });

        _producer = builder.Build();
    }

    public async Task AddProfile(AddDropInfoByUsernameRequest addLogModel)
    {
        CancellationTokenSource cancelTokenSource = new CancellationTokenSource();
        CancellationToken token = cancelTokenSource.Token;

        string profileJson = JsonSerializer.Serialize(addLogModel);
        _logger.LogInformation("Produce add profile request.{newline}Topic: {topic}{newline}Add profile: {profileJson}",
            Environment.NewLine,
            _topic,
            Environment.NewLine,
            profileJson);

        try
        {
            await _producer.ProduceAsync(_topic, new Message<long, string>() { Key = 0, Value = profileJson }, token);
            _logger.LogInformation("Produce add profile request completed successfully.");
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Produce add profile request failed.");
            throw;
        }
    }

    public void Dispose()
    {
        _producer.Flush();
        _producer.Dispose();
    }
}