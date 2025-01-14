using Frontend.Entities.Account.Model;
using Frontend.Entities.ErrorViewModel.Model;
using Frontend.Entities.LifeMonitor.Model;
using Frontend.Entities.Profile.Model;
using Frontend.Entities.Revolver.Model;

namespace XUnitTestProject.FrontendTests;

using Xunit;

public class LoginModelTests
{
    [Fact]
    public void LoginModel_ShouldInitializeWithRequiredProperties()
    {
        // Arrange
        var username = "testUser";
        var password = "testPassword";

        // Act
        var loginModel = new LoginModel
        {
            Username = username,
            Password = password
        };

        // Assert
        Assert.Equal(username, loginModel.Username);
        Assert.Equal(password, loginModel.Password);
    }
}

public class LoginResponseTests
    {
        [Fact]
        public void LoginResponse_ShouldInitializeWithProvidedValues()
        {
            // Arrange
            var username = "testUser";
            var email = "test@example.com";
            var token = "testToken123";

            // Act
            var loginResponse = new LoginResponse(username, email, token);

            // Assert
            Assert.Equal(username, loginResponse.Username);
            Assert.Equal(email, loginResponse.Email);
            Assert.Equal(token, loginResponse.Token);
        }

        [Fact]
        public void LoginResponse_ShouldHaveValueEquality()
        {
            // Arrange
            var username = "testUser";
            var email = "test@example.com";
            var token = "testToken123";

            var loginResponse1 = new LoginResponse(username, email, token);
            var loginResponse2 = new LoginResponse(username, email, token);

            // Act & Assert
            Assert.Equal(loginResponse1, loginResponse2);
            Assert.True(loginResponse1 == loginResponse2);
        }
    }
    
    public class RegisterModelTests
    {
        [Fact]
        public void RegisterModel_ShouldInitializeWithProvidedValues()
        {
            // Arrange
            var username = "testUser";
            var email = "test@example.com";
            var password = "testPassword123";
            var confirmPassword = "testPassword123";

            // Act
            var registerModel = new RegisterModel
            {
                Username = username,
                Email = email,
                Password = password,
                ConfirmPassword = confirmPassword
            };

            // Assert
            Assert.Equal(username, registerModel.Username);
            Assert.Equal(email, registerModel.Email);
            Assert.Equal(password, registerModel.Password);
            Assert.Equal(confirmPassword, registerModel.ConfirmPassword);
        }
    }
    
    public class ErrorViewModelTests
    {
        [Fact]
        public void ErrorViewModel_ShouldInitializeWithDefaultValues()
        {
            // Arrange & Act
            var errorViewModel = new ErrorViewModel();

            // Assert
            Assert.Null(errorViewModel.RequestId);
            Assert.False(errorViewModel.ShowRequestId);
        }

        [Fact]
        public void ErrorViewModel_ShouldSetRequestId()
        {
            // Arrange
            var requestId = "12345";
            var errorViewModel = new ErrorViewModel();

            // Act
            errorViewModel.RequestId = requestId;

            // Assert
            Assert.Equal(requestId, errorViewModel.RequestId);
            Assert.True(errorViewModel.ShowRequestId);
        }

        [Fact]
        public void ErrorViewModel_ShouldSetRequestIdToNull()
        {
            // Arrange
            var errorViewModel = new ErrorViewModel
            {
                RequestId = "12345"
            };

            // Act
            errorViewModel.RequestId = null;

            // Assert
            Assert.Null(errorViewModel.RequestId);
            Assert.False(errorViewModel.ShowRequestId);
        }

        [Fact]
        public void ErrorViewModel_ShouldSetRequestIdToEmptyString()
        {
            // Arrange
            var errorViewModel = new ErrorViewModel
            {
                RequestId = "12345"
            };

            // Act
            errorViewModel.RequestId = string.Empty;

            // Assert
            Assert.Equal(string.Empty, errorViewModel.RequestId);
            Assert.False(errorViewModel.ShowRequestId);
        }

        [Fact]
        public void ErrorViewModel_ShowRequestId_ShouldReturnFalseWhenRequestIdIsNull()
        {
            // Arrange
            var errorViewModel = new ErrorViewModel
            {
                RequestId = null
            };

            // Act & Assert
            Assert.False(errorViewModel.ShowRequestId);
        }

        [Fact]
        public void ErrorViewModel_ShowRequestId_ShouldReturnFalseWhenRequestIdIsEmpty()
        {
            // Arrange
            var errorViewModel = new ErrorViewModel
            {
                RequestId = string.Empty
            };

            // Act & Assert
            Assert.False(errorViewModel.ShowRequestId);
        }

        [Fact]
        public void ErrorViewModel_ShowRequestId_ShouldReturnTrueWhenRequestIdIsNotEmpty()
        {
            // Arrange
            var errorViewModel = new ErrorViewModel
            {
                RequestId = "12345"
            };

            // Act & Assert
            Assert.True(errorViewModel.ShowRequestId);
        }
    }
    
    public class CooldownModelTests
    {
        [Fact]
        public void CooldownModel_ShouldInitializeWithProvidedValues()
        {
            // Arrange
            var isCooldown = true;
            var secondsLeft = 10.5;

            // Act
            var cooldownModel = new CooldownModel
            {
                IsCooldown = isCooldown,
                SecondsLeft = secondsLeft
            };

            // Assert
            Assert.Equal(isCooldown, cooldownModel.IsCooldown);
            Assert.Equal(secondsLeft, cooldownModel.SecondsLeft);
        }

        [Fact]
        public void CooldownModel_ShouldAllowZeroSecondsLeft()
        {
            // Arrange
            var isCooldown = true;
            var secondsLeft = 0.0;

            // Act
            var cooldownModel = new CooldownModel
            {
                IsCooldown = isCooldown,
                SecondsLeft = secondsLeft
            };

            // Assert
            Assert.Equal(isCooldown, cooldownModel.IsCooldown);
            Assert.Equal(secondsLeft, cooldownModel.SecondsLeft);
        }

        [Fact]
        public void CooldownModel_ShouldHaveValueEquality()
        {
            // Arrange
            var isCooldown = true;
            var secondsLeft = 10.5;

            var cooldownModel1 = new CooldownModel
            {
                IsCooldown = isCooldown,
                SecondsLeft = secondsLeft
            };

            var cooldownModel2 = new CooldownModel
            {
                IsCooldown = isCooldown,
                SecondsLeft = secondsLeft
            };

            // Act & Assert
            Assert.Equal(cooldownModel1, cooldownModel2);
            Assert.True(cooldownModel1 == cooldownModel2);
        }
    }
    
    public class LifeServiceModelTests
    {
        [Fact]
        public void LifeServiceModel_ShouldInitializeWithProvidedValues()
        {
            // Arrange
            var serviceName = "TestService";
            var isLife = true;
            var cooldown = new CooldownModel { IsCooldown = false, SecondsLeft = 0 };

            // Act
            var lifeServiceModel = new LifeServiceModel
            {
                ServiceName = serviceName,
                IsLife = isLife,
                Cooldown = cooldown
            };

            // Assert
            Assert.Equal(serviceName, lifeServiceModel.ServiceName);
            Assert.Equal(isLife, lifeServiceModel.IsLife);
            Assert.Equal(cooldown, lifeServiceModel.Cooldown);
        }

        [Fact]
        public void LifeServiceModel_ShouldAllowNullCooldown()
        {
            // Arrange
            var serviceName = "TestService";
            var isLife = true;
            CooldownModel? cooldown = null;

            // Act
            var lifeServiceModel = new LifeServiceModel
            {
                ServiceName = serviceName,
                IsLife = isLife,
                Cooldown = cooldown
            };

            // Assert
            Assert.Equal(serviceName, lifeServiceModel.ServiceName);
            Assert.Equal(isLife, lifeServiceModel.IsLife);
            Assert.Null(lifeServiceModel.Cooldown);
        }

        [Fact]
        public void LifeServiceModel_ShouldHaveValueEquality()
        {
            // Arrange
            var serviceName = "TestService";
            var isLife = true;
            var cooldown = new CooldownModel { IsCooldown = false, SecondsLeft = 0 };

            var lifeServiceModel1 = new LifeServiceModel
            {
                ServiceName = serviceName,
                IsLife = isLife,
                Cooldown = cooldown
            };

            var lifeServiceModel2 = new LifeServiceModel
            {
                ServiceName = serviceName,
                IsLife = isLife,
                Cooldown = cooldown
            };

            // Act & Assert
            Assert.Equal(lifeServiceModel1, lifeServiceModel2);
            Assert.True(lifeServiceModel1 == lifeServiceModel2);
        }
    }
    
    public class AddDropInfoByUsernameRequestTests
    {

        [Fact]
        public void AddDropInfoByUsernameRequest_ShouldSetPropertiesCorrectly()
        {
            // Arrange
            var username = "testUser";
            var serviceName = "TestService";
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
        public void AddDropInfoByUsernameRequest_ShouldAllowCustomMoment()
        {
            // Arrange
            var customMoment = DateTimeOffset.UtcNow.AddDays(-1);

            // Act
            var request = new AddDropInfoByUsernameRequest
            {
                Username = "testUser",
                ServiceName = "TestService",
                Moment = customMoment
            };

            // Assert
            Assert.Equal(customMoment, request.Moment);
        }
    }
    
    public class DropInfoTests
    {
        [Fact]
        public void DropInfo_ShouldInitializeWithDefaultValues()
        {
            // Arrange & Act
            var dropInfo = new DropInfo();

            // Assert
            Assert.Equal(0, dropInfo.Id);
            Assert.Null(dropInfo.ServiceName);
            Assert.Equal(default(DateTimeOffset), dropInfo.Moment);
            Assert.Equal(0, dropInfo.UserProfileId);
        }

        [Fact]
        public void DropInfo_ShouldSetPropertiesCorrectly()
        {
            // Arrange
            var id = 1L;
            var serviceName = "TestService";
            var moment = DateTimeOffset.UtcNow;
            var userProfileId = 123L;

            // Act
            var dropInfo = new DropInfo
            {
                Id = id,
                ServiceName = serviceName,
                Moment = moment,
                UserProfileId = userProfileId
            };

            // Assert
            Assert.Equal(id, dropInfo.Id);
            Assert.Equal(serviceName, dropInfo.ServiceName);
            Assert.Equal(moment, dropInfo.Moment);
            Assert.Equal(userProfileId, dropInfo.UserProfileId);
        }

        [Fact]
        public void DropInfo_ShouldAllowCustomMoment()
        {
            // Arrange
            var customMoment = DateTimeOffset.UtcNow.AddDays(-1);

            // Act
            var dropInfo = new DropInfo
            {
                Id = 1,
                ServiceName = "TestService",
                Moment = customMoment,
                UserProfileId = 123
            };

            // Assert
            Assert.Equal(customMoment, dropInfo.Moment);
        }

        [Fact]
        public void DropInfo_ShouldAllowNegativeUserProfileId()
        {
            // Arrange
            var userProfileId = -123L;

            // Act
            var dropInfo = new DropInfo
            {
                Id = 1,
                ServiceName = "TestService",
                Moment = DateTimeOffset.UtcNow,
                UserProfileId = userProfileId
            };

            // Assert
            Assert.Equal(userProfileId, dropInfo.UserProfileId);
        }
    }
    
    public class GetUserProfileRequestTests
    {

        [Fact]
        public void GetUserProfileRequest_ShouldSetUsernameCorrectly()
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
    }
    
    public class UserProfileResponseTests
    {
        [Fact]
        public void UserProfileResponse_ShouldInitializeWithProvidedValues()
        {
            // Arrange
            var id = 1L;
            var username = "testUser";
            var email = "test@example.com";
            var history = new List<DropInfo>
            {
                new DropInfo { Id = 1, ServiceName = "Service1", Moment = DateTimeOffset.UtcNow, UserProfileId = 1 },
                new DropInfo { Id = 2, ServiceName = "Service2", Moment = DateTimeOffset.UtcNow, UserProfileId = 1 }
            };

            // Act
            var userProfileResponse = new UserProfileResponse
            {
                Id = id,
                Username = username,
                Email = email,
                History = history
            };

            // Assert
            Assert.Equal(id, userProfileResponse.Id);
            Assert.Equal(username, userProfileResponse.Username);
            Assert.Equal(email, userProfileResponse.Email);
            Assert.Equal(history, userProfileResponse.History);
        }

        [Fact]
        public void UserProfileResponse_ShouldAllowEmptyHistory()
        {
            // Arrange
            var id = 1L;
            var username = "testUser";
            var email = "test@example.com";
            var history = new List<DropInfo>();

            // Act
            var userProfileResponse = new UserProfileResponse
            {
                Id = id,
                Username = username,
                Email = email,
                History = history
            };

            // Assert
            Assert.Empty(userProfileResponse.History);
        }

        [Fact]
        public void UserProfileResponse_ShouldHaveValueEquality()
        {
            // Arrange
            var id = 1L;
            var username = "testUser";
            var email = "test@example.com";
            var history = new List<DropInfo>
            {
                new DropInfo { Id = 1, ServiceName = "Service1", Moment = DateTimeOffset.UtcNow, UserProfileId = 1 }
            };

            var userProfileResponse1 = new UserProfileResponse
            {
                Id = id,
                Username = username,
                Email = email,
                History = history
            };

            var userProfileResponse2 = new UserProfileResponse
            {
                Id = id,
                Username = username,
                Email = email,
                History = history
            };

            // Act & Assert
            Assert.Equal(userProfileResponse1, userProfileResponse2);
            Assert.True(userProfileResponse1 == userProfileResponse2);
        }
    }
    
    public class KilledServiceInfoTests
    {
        [Fact]
        public void KilledServiceInfo_ShouldInitializeWithProvidedValues()
        {
            // Arrange
            var serviceName = "TestService";
            var serviceNameRus = "ТестовыйСервис";
            var host = "example.com";
            var dropSeconds = 60;
            var cooldownSeconds = 300;

            // Act
            var killedServiceInfo = new KilledServiceInfo
            {
                ServiceName = serviceName,
                ServiceNameRus = serviceNameRus,
                Host = host,
                DropSeconds = dropSeconds,
                CooldownSeconds = cooldownSeconds
            };

            // Assert
            Assert.Equal(serviceName, killedServiceInfo.ServiceName);
            Assert.Equal(serviceNameRus, killedServiceInfo.ServiceNameRus);
            Assert.Equal(host, killedServiceInfo.Host);
            Assert.Equal(dropSeconds, killedServiceInfo.DropSeconds);
            Assert.Equal(cooldownSeconds, killedServiceInfo.CooldownSeconds);
        }

        [Fact]
        public void KilledServiceInfo_ShouldHaveValueEquality()
        {
            // Arrange
            var serviceName = "TestService";
            var serviceNameRus = "ТестовыйСервис";
            var host = "example.com";
            var dropSeconds = 60;
            var cooldownSeconds = 300;

            var killedServiceInfo1 = new KilledServiceInfo
            {
                ServiceName = serviceName,
                ServiceNameRus = serviceNameRus,
                Host = host,
                DropSeconds = dropSeconds,
                CooldownSeconds = cooldownSeconds
            };

            var killedServiceInfo2 = new KilledServiceInfo
            {
                ServiceName = serviceName,
                ServiceNameRus = serviceNameRus,
                Host = host,
                DropSeconds = dropSeconds,
                CooldownSeconds = cooldownSeconds
            };

            // Act & Assert
            Assert.Equal(killedServiceInfo1, killedServiceInfo2);
            Assert.True(killedServiceInfo1 == killedServiceInfo2);
        }
    }
    
    public class ShootManTests
    {
        [Fact]
        public void ShootMan_ShouldInitializeWithProvidedValues()
        {
            // Arrange
            var username = "testUser";
            var email = "test@example.com";

            // Act
            var shootMan = new ShootMan
            {
                Username = username,
                Email = email
            };

            // Assert
            Assert.Equal(username, shootMan.Username);
            Assert.Equal(email, shootMan.Email);
        }

        [Fact]
        public void ShootMan_ShouldAllowNullEmail()
        {
            // Arrange
            var username = "testUser";
            string? email = null;

            // Act
            var shootMan = new ShootMan
            {
                Username = username,
                Email = email
            };

            // Assert
            Assert.Equal(username, shootMan.Username);
            Assert.Null(shootMan.Email);
        }

        [Fact]
        public void ShootMan_ShouldAllowEmptyEmail()
        {
            // Arrange
            var username = "testUser";
            var email = string.Empty;

            // Act
            var shootMan = new ShootMan
            {
                Username = username,
                Email = email
            };

            // Assert
            Assert.Equal(username, shootMan.Username);
            Assert.Equal(string.Empty, shootMan.Email);
        }

        [Fact]
        public void ShootMan_ShouldHaveValueEquality()
        {
            // Arrange
            var username = "testUser";
            var email = "test@example.com";

            var shootMan1 = new ShootMan
            {
                Username = username,
                Email = email
            };

            var shootMan2 = new ShootMan
            {
                Username = username,
                Email = email
            };

            // Act & Assert
            Assert.Equal(shootMan1, shootMan2);
            Assert.True(shootMan1 == shootMan2);
        }
    }
    
    public class ShootRequestTests
    {
        [Fact]
        public void ShootRequest_ShouldInitializeWithProvidedValues()
        {
            // Arrange
            var bullets = new List<string> { "bullet1", "bullet2", "bullet3" };

            // Act
            var shootRequest = new ShootRequest
            {
                Bullets = bullets
            };

            // Assert
            Assert.Equal(bullets, shootRequest.Bullets);
        }

        [Fact]
        public void ShootRequest_ShouldAllowEmptyBulletsList()
        {
            // Arrange
            var bullets = new List<string>();

            // Act
            var shootRequest = new ShootRequest
            {
                Bullets = bullets
            };

            // Assert
            Assert.Empty(shootRequest.Bullets);
        }
    }