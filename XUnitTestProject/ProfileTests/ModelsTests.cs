using Profile.Api.DataAccess.Entity;

namespace XUnitTestProject.ProfileTests
{

    using Profile.Api.Models;
    using Xunit;

    public class AddDropInfoByUsernameRequestTests
    {
        [Fact]
        public void AddDropInfoByUsernameRequest_ShouldInitializePropertiesCorrectly()
        {
            // Arrange
            var username = "testUser";
            var serviceName = "testService";
            var moment = DateTimeOffset.UtcNow;

            // Act
            var request = new AddDropInfoByUsernameRequest
            {
                Username = username,
                ServiceName = serviceName,
                Moment = moment
            };

            // Assert
            Assert.Equal(username, request.Username);
            Assert.Equal(serviceName, request.ServiceName);
            Assert.Equal(moment, request.Moment);
        }

        [Fact]
        public void AddDropInfoByUsernameRequest_ShouldHaveDefaultMomentValue()
        {
            // Arrange
            var request = new AddDropInfoByUsernameRequest();

            // Act
            var defaultMoment = request.Moment;

            // Assert
            Assert.True(defaultMoment <= DateTimeOffset.UtcNow);
        }

        [Fact]
        public void AddDropInfoByUsernameRequest_ShouldAllowNullUsernameAndServiceName()
        {
            // Arrange
            var request = new AddDropInfoByUsernameRequest
            {
                Username = null,
                ServiceName = null
            };

            // Act & Assert
            Assert.Null(request.Username);
            Assert.Null(request.ServiceName);
        }
    }

    public class AddUserProfileRequestTests
    {
        [Fact]
        public void AddUserProfileRequest_ShouldInitializePropertiesCorrectly()
        {
            // Arrange
            var username = "testUser";
            var email = "test@example.com";

            // Act
            var request = new AddUserProfileRequest
            {
                Username = username,
                Email = email
            };

            // Assert
            Assert.Equal(username, request.Username);
            Assert.Equal(email, request.Email);
        }

        [Fact]
        public void AddUserProfileRequest_ShouldAllowNullUsernameAndEmail()
        {
            // Arrange
            var request = new AddUserProfileRequest
            {
                Username = null,
                Email = null
            };

            // Act & Assert
            Assert.Null(request.Username);
            Assert.Null(request.Email);
        }
    }

    public class CooldownModelTests
    {
        [Fact]
        public void CooldownModel_ShouldInitializePropertiesCorrectly()
        {
            // Arrange
            var isCooldown = true;
            var secondsLeft = 123.45;

            // Act
            var model = new CooldownModel
            {
                IsCooldown = isCooldown,
                SecondsLeft = secondsLeft
            };

            // Assert
            Assert.Equal(isCooldown, model.IsCooldown);
            Assert.Equal(secondsLeft, model.SecondsLeft);
        }

        [Fact]
        public void CooldownModel_ShouldAllowFalseIsCooldown()
        {
            // Arrange
            var isCooldown = false;
            var secondsLeft = 0.0;

            // Act
            var model = new CooldownModel
            {
                IsCooldown = isCooldown,
                SecondsLeft = secondsLeft
            };

            // Assert
            Assert.False(model.IsCooldown);
            Assert.Equal(secondsLeft, model.SecondsLeft);
        }

        [Fact]
        public void CooldownModel_ShouldAllowNegativeSecondsLeft()
        {
            // Arrange
            var isCooldown = true;
            var secondsLeft = -10.0;

            // Act
            var model = new CooldownModel
            {
                IsCooldown = isCooldown,
                SecondsLeft = secondsLeft
            };

            // Assert
            Assert.Equal(isCooldown, model.IsCooldown);
            Assert.Equal(secondsLeft, model.SecondsLeft);
        }
    }

    public class DropInfoResponseTests
    {
        [Fact]
        public void DropInfoResponse_ShouldInitializePropertiesCorrectly()
        {
            // Arrange
            var id = 123L;
            var serviceName = "TestService";
            var moment = DateTimeOffset.UtcNow;
            var userProfileId = 456L;

            // Act
            var response = new DropInfoResponse
            {
                Id = id,
                ServiceName = serviceName,
                Moment = moment,
                UserProfileId = userProfileId
            };

            // Assert
            Assert.Equal(id, response.Id);
            Assert.Equal(serviceName, response.ServiceName);
            Assert.Equal(moment, response.Moment);
            Assert.Equal(userProfileId, response.UserProfileId);
        }

        [Fact]
        public void DropInfoResponse_ShouldAllowNullServiceName()
        {
            // Arrange
            var response = new DropInfoResponse
            {
                Id = 123L,
                ServiceName = null,
                Moment = DateTimeOffset.UtcNow,
                UserProfileId = 456L
            };

            // Act & Assert
            Assert.Null(response.ServiceName);
        }

        [Fact]
        public void DropInfoResponse_ShouldHandleDefaultValues()
        {
            // Arrange
            var response = new DropInfoResponse();

            // Act & Assert
            Assert.Equal(0, response.Id);
            Assert.Null(response.ServiceName);
            Assert.Equal(default(DateTimeOffset), response.Moment);
            Assert.Equal(0, response.UserProfileId);
        }
    }

    public class GetUserProfileRequestTests
    {
        [Fact]
        public void GetUserProfileRequest_ShouldInitializeUsernameCorrectly()
        {
            // Arrange
            var username = "testUser";

            // Act
            var request = new GetUserProfileRequest
            {
                Username = username
            };

            // Assert
            Assert.Equal(username, request.Username);
        }

        [Fact]
        public void GetUserProfileRequest_ShouldAllowNullUsername()
        {
            // Arrange
            var request = new GetUserProfileRequest
            {
                Username = null
            };

            // Act & Assert
            Assert.Null(request.Username);
        }
    }

    public class UserProfileResponseTests
    {
        [Fact]
        public void UserProfileResponse_ShouldInitializePropertiesCorrectly()
        {
            // Arrange
            var id = 123L;
            var username = "testUser";
            var email = "test@example.com";
            var history = new List<DropInfo>
            {
                new DropInfo { Id = 1, ServiceName = "Service1", Moment = DateTimeOffset.UtcNow },
                new DropInfo { Id = 2, ServiceName = "Service2", Moment = DateTimeOffset.UtcNow }
            };

            // Act
            var response = new UserProfileResponse
            {
                Id = id,
                Username = username,
                Email = email,
                History = history
            };

            // Assert
            Assert.Equal(id, response.Id);
            Assert.Equal(username, response.Username);
            Assert.Equal(email, response.Email);
            Assert.Equal(history, response.History);
        }

        [Fact]
        public void UserProfileResponse_ShouldHandleEmptyHistory()
        {
            // Arrange
            var id = 123L;
            var username = "testUser";
            var email = "test@example.com";
            var history = new List<DropInfo>();

            // Act
            var response = new UserProfileResponse
            {
                Id = id,
                Username = username,
                Email = email,
                History = history
            };

            // Assert
            Assert.Equal(id, response.Id);
            Assert.Equal(username, response.Username);
            Assert.Equal(email, response.Email);
            Assert.Empty(response.History);
        }
    }
}