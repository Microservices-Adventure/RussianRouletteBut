using System.Text.Json;
using Confluent.Kafka;
using Frontend.Entities.Account.Model;

namespace Frontend.App.Kafka;

public class RegisterProducer : IDisposable
{
    private readonly IProducer<Ignore, string> _producer;
    private readonly string _topic;
    private readonly ILogger<RegisterProducer> _logger;

    public RegisterProducer(
        string bootstrapServers, 
        string topic,
        ILogger<RegisterProducer> producerLogger)
    {
        _topic = topic;
        _logger = producerLogger;
        var builder = new ProducerBuilder<Ignore, string>(new ProducerConfig{
                BootstrapServers = bootstrapServers
            });

        _producer = builder.Build();
    }

    public async Task Register(RegisterModel registerModel)
    {
        string registerJson = JsonSerializer.Serialize(registerModel);
        _logger.LogDebug("Produce register request.{newline}Topic: {topic}Request: {registerJson}", 
            Environment.NewLine, 
            _topic, 
            registerJson);

        try
        {
            await _producer.ProduceAsync(_topic, new Message<Ignore, string>(){ Value = registerJson});
            _logger.LogDebug("Produce register request completed successfully.");
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