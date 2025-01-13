using Profile.Api.Config;

namespace XUnitTestProject.ProfileTests;

using System;
using Xunit;

    public class KafkaSettingsTests
    {
        [Fact]
        public void BootstrapServers_ShouldReturnEnvironmentVariable_WhenSet()
        {
            // Arrange
            var expectedBootstrapServers = "env_bootstrap_servers";
            Environment.SetEnvironmentVariable("ASPNETCORE_Kafka_Bootstrap_Servers", expectedBootstrapServers);
            var kafkaSettings = new KafkaSettings
            {
                BootstrapServers = "default_bootstrap_servers"
            };

            // Act
            var result = kafkaSettings.BootstrapServers;

            // Assert
            Assert.Equal(expectedBootstrapServers, result);
        }

        [Fact]
        public void BootstrapServers_ShouldReturnInitializedValue_WhenEnvironmentVariableNotSet()
        {
            // Arrange
            var expectedBootstrapServers = "default_bootstrap_servers";
            Environment.SetEnvironmentVariable("ASPNETCORE_Kafka_Bootstrap_Servers", null);
            var kafkaSettings = new KafkaSettings
            {
                BootstrapServers = expectedBootstrapServers
            };

            // Act
            var result = kafkaSettings.BootstrapServers;

            // Assert
            Assert.Equal(expectedBootstrapServers, result);
        }

        [Fact]
        public void Topic_ShouldReturnSetValue()
        {
            // Arrange
            var expectedTopic = "test_topic";
            var kafkaSettings = new KafkaSettings
            {
                Topic = expectedTopic
            };

            // Act
            var result = kafkaSettings.Topic;

            // Assert
            Assert.Equal(expectedTopic, result);
        }

        [Fact]
        public void ConsumerGroupId_ShouldReturnSetValue()
        {
            // Arrange
            var expectedConsumerGroupId = "test_group";
            var kafkaSettings = new KafkaSettings
            {
                ConsumerGroupId = expectedConsumerGroupId
            };

            // Act
            var result = kafkaSettings.ConsumerGroupId;

            // Assert
            Assert.Equal(expectedConsumerGroupId, result);
        }
    }
