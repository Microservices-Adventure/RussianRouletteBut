namespace Frontend.App.Config;

public class KafkaSettings
{
    private string _bootstrapServer = null!;
    public string BootstrapServers { 
        get => Environment.GetEnvironmentVariable("ASPNETCORE_Kafka_Bootstrap_Servers") ?? _bootstrapServer; 
        set => _bootstrapServer = value; 
    }
    public string RegistrationTopic { get; set; } = null!;
}