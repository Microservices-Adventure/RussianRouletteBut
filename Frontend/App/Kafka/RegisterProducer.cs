using System.Text.Json;
using Confluent.Kafka;
using Frontend.Entities.Account.Model;

namespace Frontend.App.Kafka;

public class RegisterProducer : IDisposable
{
    private readonly IProducer<long, string> _producer;
    private readonly string _topic;
    private readonly ILogger<RegisterProducer> _logger;

    public RegisterProducer(
        string bootstrapServers, 
        string topic,
        ILogger<RegisterProducer> producerLogger)
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

    public async Task Register(RegisterModel registerModel)
    {
        CancellationTokenSource cancelTokenSource = new CancellationTokenSource(); 
        CancellationToken token = cancelTokenSource.Token;
        
        string registerJson = JsonSerializer.Serialize(registerModel);
        _logger.LogInformation("Produce register request.{newline}Topic: {topic}{newline}Request: {registerJson}", 
            Environment.NewLine, 
            _topic, 
            Environment.NewLine, 
            registerJson);

        try
        {
            await _producer.ProduceAsync(_topic, new Message<long, string>(){ Key = 0, Value = registerJson }, token);
            _logger.LogInformation("Produce register request completed successfully.");
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Produce register request failed.");
            throw;
        }
    }
    
    public void Dispose()
    {
        _producer.Flush();
        _producer.Dispose();
    }
}