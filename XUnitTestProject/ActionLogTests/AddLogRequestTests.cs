using ActionLog.Api.Models;

namespace XUnitTestProject.ActionLogTests
{

    public class AddLogRequestTests
    {
        [Fact]
        public void AddLogRequest_Properties_ShouldBeSetCorrectly()
        {
            // Arrange
            var request = new AddLogRequest
            {
                Username = "testuser",
                Email = "test@example.com",
                MicroserviceId = 1,
                MicroserviceName = "TestService",
                Description = "Test description",
                Status = "Success",
                Error = "No error",
                Moment = DateTimeOffset.UtcNow
            };

            // Assert
            Assert.Equal("testuser", request.Username);
            Assert.Equal("test@example.com", request.Email);
            Assert.Equal(1, request.MicroserviceId);
            Assert.Equal("TestService", request.MicroserviceName);
            Assert.Equal("Test description", request.Description);
            Assert.Equal("Success", request.Status);
            Assert.Equal("No error", request.Error);
            Assert.Equal(DateTimeOffset.UtcNow.Date, request.Moment.Date); // Проверка даты без времени
        }
    }
}