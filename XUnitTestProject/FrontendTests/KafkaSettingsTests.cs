namespace XUnitTestProject.FrontendTests;

using Frontend.App.Config;
using System;
using Xunit;

public class KafkaSettingsTests
{
    [Fact]
    public void KafkaSettings_Properties_ShouldBeSetCorrectly()
    {
        // Arrange
        var kafkaSettings = new KafkaSettings
        {
            BootstrapServers = "localhost:9092",
            RegistrationTopic = "registration-topic",
            LogTopic = "log-topic",
            ProfileTopic = "profile-topic"
        };

        // Act & Assert
        Assert.Equal("localhost:9092", kafkaSettings.BootstrapServers);
        Assert.Equal("registration-topic", kafkaSettings.RegistrationTopic);
        Assert.Equal("log-topic", kafkaSettings.LogTopic);
        Assert.Equal("profile-topic", kafkaSettings.ProfileTopic);
    }

    [Fact]
    public void KafkaSettings_BootstrapServers_ShouldGetValueFromEnvironmentVariable()
    {
        // Arrange
        var kafkaSettings = new KafkaSettings();
        var expectedBootstrapServers = "env-bootstrap-server:9092";
        Environment.SetEnvironmentVariable("ASPNETCORE_Kafka_Bootstrap_Servers", expectedBootstrapServers);

        // Act
        var bootstrapServers = kafkaSettings.BootstrapServers;

        // Assert
        Assert.Equal(expectedBootstrapServers, bootstrapServers);

        // Cleanup
        Environment.SetEnvironmentVariable("ASPNETCORE_Kafka_Bootstrap_Servers", null);
    }

    [Fact]
    public void KafkaSettings_BootstrapServers_ShouldUseDefaultValue_WhenEnvironmentVariableIsNotSet()
    {
        // Arrange
        var kafkaSettings = new KafkaSettings
        {
            BootstrapServers = "default-bootstrap-server:9092"
        };
        
        Environment.SetEnvironmentVariable("ASPNETCORE_Kafka_Bootstrap_Servers", null);

        // Act
        var bootstrapServers = kafkaSettings.BootstrapServers;

        // Assert
        Assert.Equal("default-bootstrap-server:9092", bootstrapServers);
    }
}