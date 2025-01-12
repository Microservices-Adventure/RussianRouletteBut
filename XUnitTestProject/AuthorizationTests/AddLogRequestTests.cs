namespace XUnitTestProject.AuthorizationTests;

using Authorization.Domain.Models;
using System;
using Xunit;

    public class AddLogRequestTests
    {
        [Fact]
        public void AddLogRequest_Properties_CanBeSetAndGet()
        {
            // Arrange
            var addLogRequest = new AddLogRequest
            {
                Username = "test-user",
                Email = "test@example.com",
                MicroserviceId = 123,
                MicroserviceName = "TestService",
                Description = "Test description",
                Status = "Success",
                Error = "No error",
                Moment = DateTimeOffset.UtcNow
            };

            // Act
            var username = addLogRequest.Username;
            var email = addLogRequest.Email;
            var microserviceId = addLogRequest.MicroserviceId;
            var microserviceName = addLogRequest.MicroserviceName;
            var description = addLogRequest.Description;
            var status = addLogRequest.Status;
            var error = addLogRequest.Error;
            var moment = addLogRequest.Moment;

            // Assert
            Assert.Equal("test-user", username);
            Assert.Equal("test@example.com", email);
            Assert.Equal(123, microserviceId);
            Assert.Equal("TestService", microserviceName);
            Assert.Equal("Test description", description);
            Assert.Equal("Success", status);
            Assert.Equal("No error", error);
            Assert.IsType<DateTimeOffset>(moment);
        }

        [Fact]
        public void AddLogRequest_Properties_AreNotNullAfterInitialization()
        {
            // Arrange
            var addLogRequest = new AddLogRequest
            {
                MicroserviceName = "TestService",
                Description = "Test description",
                Status = "Success",
                Moment = DateTimeOffset.UtcNow
            };

            // Act & Assert
            Assert.NotNull(addLogRequest.MicroserviceName);
            Assert.NotNull(addLogRequest.Description);
            Assert.NotNull(addLogRequest.Status);
            Assert.NotNull(addLogRequest.Moment);
        }

        [Fact]
        public void AddLogRequest_Properties_CanBeModified()
        {
            // Arrange
            var addLogRequest = new AddLogRequest
            {
                Username = "initial-user",
                Email = "initial@example.com",
                MicroserviceId = 1,
                MicroserviceName = "InitialService",
                Description = "Initial description",
                Status = "Initial status",
                Error = "Initial error",
                Moment = DateTimeOffset.UtcNow
            };

            // Act
            addLogRequest.Username = "modified-user";
            addLogRequest.Email = "modified@example.com";
            addLogRequest.MicroserviceId = 2;
            addLogRequest.MicroserviceName = "ModifiedService";
            addLogRequest.Description = "Modified description";
            addLogRequest.Status = "Modified status";
            addLogRequest.Error = "Modified error";
            addLogRequest.Moment = DateTimeOffset.UtcNow.AddDays(1);

            // Assert
            Assert.Equal("modified-user", addLogRequest.Username);
            Assert.Equal("modified@example.com", addLogRequest.Email);
            Assert.Equal(2, addLogRequest.MicroserviceId);
            Assert.Equal("ModifiedService", addLogRequest.MicroserviceName);
            Assert.Equal("Modified description", addLogRequest.Description);
            Assert.Equal("Modified status", addLogRequest.Status);
            Assert.Equal("Modified error", addLogRequest.Error);
            Assert.IsType<DateTimeOffset>(addLogRequest.Moment);
        }
    }
