using ActionLog.Api.Config;

namespace XUnitTestProject.ActionLogTests;

using System;
using Xunit;


    public class KafkaSettingsTests
    {
        [Fact]
        public void BootstrapServers_ShouldReturnEnvironmentVariable_WhenSet()
        {
            // Arrange
            var expectedBootstrapServers = "localhost:9092";
            Environment.SetEnvironmentVariable("ASPNETCORE_Kafka_Bootstrap_Servers", expectedBootstrapServers);
            var kafkaSettings = new KafkaSettings
            {
                BootstrapServers = "default:9092"
            };

            // Act
            var actualBootstrapServers = kafkaSettings.BootstrapServers;

            // Assert
            Assert.Equal(expectedBootstrapServers, actualBootstrapServers);
        }

        [Fact]
        public void BootstrapServers_ShouldReturnDefaultValue_WhenEnvironmentVariableNotSet()
        {
            // Arrange
            var expectedBootstrapServers = "default:9092";
            Environment.SetEnvironmentVariable("ASPNETCORE_Kafka_Bootstrap_Servers", null);
            var kafkaSettings = new KafkaSettings
            {
                BootstrapServers = expectedBootstrapServers
            };

            // Act
            var actualBootstrapServers = kafkaSettings.BootstrapServers;

            // Assert
            Assert.Equal(expectedBootstrapServers, actualBootstrapServers);
        }
        
        [Fact]
        public void Topic_ShouldReturnSetValue()
        {
            // Arrange
            var expectedTopic = "test-topic";
            var kafkaSettings = new KafkaSettings
            {
                Topic = expectedTopic
            };

            // Act
            var actualTopic = kafkaSettings.Topic;

            // Assert
            Assert.Equal(expectedTopic, actualTopic);
        }

        [Fact]
        public void ConsumerGroupId_ShouldReturnSetValue()
        {
            // Arrange
            var expectedConsumerGroupId = "test-group";
            var kafkaSettings = new KafkaSettings
            {
                ConsumerGroupId = expectedConsumerGroupId
            };

            // Act
            var actualConsumerGroupId = kafkaSettings.ConsumerGroupId;

            // Assert
            Assert.Equal(expectedConsumerGroupId, actualConsumerGroupId);
        }
    }
