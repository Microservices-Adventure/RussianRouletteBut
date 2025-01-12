namespace XUnitTestProject.ActionLogTests;

using ActionLog.Api.Models;
using Xunit;

    public class GetLogsRequestTests
    {
        [Fact]
        public void GetLogsRequest_Properties_ShouldHaveDefaultValues()
        {
            // Arrange
            var request = new GetLogsRequest();

            // Assert
            Assert.Equal(1, request.Page);
            Assert.Equal(10, request.Size);
            Assert.Null(request.Username);
            Assert.Null(request.Email);
            Assert.Null(request.MicroserviceId);
            Assert.Null(request.MicroserviceName);
            Assert.Null(request.Description);
            Assert.Null(request.Status);
            Assert.Null(request.HasError);
            Assert.Null(request.From);
            Assert.Null(request.To);
        }

        [Fact]
        public void GetLogsRequest_Properties_ShouldBeSetCorrectly()
        {
            // Arrange
            var request = new GetLogsRequest
            {
                Page = 2,
                Size = 20,
                Username = "testuser",
                Email = "test@example.com",
                MicroserviceId = 1,
                MicroserviceName = "TestService",
                Description = "Test description",
                Status = "Success",
                HasError = true,
                From = DateTimeOffset.UtcNow.AddDays(-1),
                To = DateTimeOffset.UtcNow
            };

            // Assert
            Assert.Equal(2, request.Page);
            Assert.Equal(20, request.Size);
            Assert.Equal("testuser", request.Username);
            Assert.Equal("test@example.com", request.Email);
            Assert.Equal(1, request.MicroserviceId);
            Assert.Equal("TestService", request.MicroserviceName);
            Assert.Equal("Test description", request.Description);
            Assert.Equal("Success", request.Status);
            Assert.True(request.HasError);
            Assert.NotNull(request.From);
            Assert.NotNull(request.To);
        }
    }
