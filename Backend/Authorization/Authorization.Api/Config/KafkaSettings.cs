namespace Authorization.Api.Config;

public class KafkaSettings
{
    public string BootstrapServers { get; set; } = null!;
    public string Topic { get; set; } = null!;
    public string ConsumerGroupId { get; set; } = null!;
}
