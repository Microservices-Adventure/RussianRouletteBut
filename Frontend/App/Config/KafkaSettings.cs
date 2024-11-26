namespace Frontend.App.Config;

public class KafkaSettings
{
    public string BootstrapServers { get; set; } = null!;
    public string RegisterTopic { get; set; } = null!;
}