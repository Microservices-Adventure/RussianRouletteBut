using Authorization.Domain.Config;

namespace XUnitTestProject.AuthorizationTests;

using System;
using Xunit;

public class KafkaSettingsTests
{
    [Fact]
    public void BootstrapServers_ReturnsTheValueFromTheEnvironmentVariableIfItIsSet()
    {
        // Arrange
        var expectedBootstrapServers = "kafka:9092";
        Environment.SetEnvironmentVariable("ASPNETCORE_Kafka_Bootstrap_Servers", expectedBootstrapServers);

        var kafkaSettings = new KafkaSettings
        {
            Topic = "test-topic",
            ConsumerGroupId = "test-group"
        };

        // Act
        var bootstrapServers = kafkaSettings.BootstrapServers;

        // Assert
        Assert.Equal(expectedBootstrapServers, bootstrapServers);

        // Cleanup
        Environment.SetEnvironmentVariable("ASPNETCORE_Kafka_Bootstrap_Servers", null);
    }

    [Fact]
    public void BootstrapServers_ReturnsTheDefaultValueIfTheEnvironmentVariableIsNotSet()
    {
        // Arrange
        var expectedBootstrapServers = "default-kafka:9092";
        Environment.SetEnvironmentVariable("ASPNETCORE_Kafka_Bootstrap_Servers", null);

        var kafkaSettings = new KafkaSettings
        {
            BootstrapServers = expectedBootstrapServers,
            Topic = "test-topic",
            ConsumerGroupId = "test-group"
        };

        // Act
        var bootstrapServers = kafkaSettings.BootstrapServers;

        // Assert
        Assert.Equal(expectedBootstrapServers, bootstrapServers);
    }
}