namespace Authorization.Domain.Config;

public class KafkaSettings
{
    private string _bootstrapServers = null!;
    public string BootstrapServers
    {
        get => Environment.GetEnvironmentVariable("ASPNETCORE_Kafka_Bootstrap_Servers") ?? _bootstrapServers;
        init => _bootstrapServers = value;
    }
    public string Topic { get; set; } = null!;
    public string LogTopic { get; set; } = null!;
    public string ProfileTopic { get; set; } = null!;
    public string ConsumerGroupId { get; set; } = null!;
}
