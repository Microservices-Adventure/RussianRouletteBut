using System.Text.Json;
using Confluent.Kafka;
using Frontend.Entities.ActionLog;

namespace Frontend.Features.KafkaProducers;

public class LogProducer : IDisposable
{
    private readonly IProducer<long, string> _producer;
    private readonly string _topic;
    private readonly ILogger<LogProducer> _logger;

    public LogProducer(
        string bootstrapServers, 
        string topic,
        ILogger<LogProducer> producerLogger)
    {
        _topic = topic;
        _logger = producerLogger;
        var builder = new ProducerBuilder<long, string>(new ProducerConfig{
            BootstrapServers = bootstrapServers,
            BatchSize = 50000,
            LingerMs = 5
        });

        _producer = builder.Build();
    }

    public async Task AddLog(AddLogRequest addLogModel)
    {
        CancellationTokenSource cancelTokenSource = new CancellationTokenSource(); 
        CancellationToken token = cancelTokenSource.Token;
        
        string logJson = JsonSerializer.Serialize(addLogModel);
        _logger.LogInformation("Produce log request.{newline}Topic: {topic}{newline}Log: {logJson}", 
            Environment.NewLine, 
            _topic, 
            Environment.NewLine, 
            logJson);

        try
        {
            await _producer.ProduceAsync(_topic, new Message<long, string>(){ Key = 0, Value = logJson }, token);
            _logger.LogInformation("Produce log request completed successfully.");
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Produce log request failed.");
            throw;
        }
    }
    
    public void Dispose()
    {
        _producer.Flush();
        _producer.Dispose();
    }
}