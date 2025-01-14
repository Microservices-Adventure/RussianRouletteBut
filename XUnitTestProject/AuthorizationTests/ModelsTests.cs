namespace XUnitTestProject.AuthorizationTests;

using Authorization.Domain.Models;
using System;
using Xunit;

public class AddDropInfoByUsernameRequestTests
{
    [Fact]
    public void AddDropInfoByUsernameRequest_Properties_ShouldBeSetCorrectly()
    {
        // Arrange
        var request = new AddDropInfoByUsernameRequest
        {
            Username = "testUser",
            ServiceName = "TestService",
            Moment = DateTimeOffset.UtcNow
        };

        // Act & Assert
        Assert.Equal("testUser", request.Username);
        Assert.Equal("TestService", request.ServiceName);
    }
}

public class LoginUserModelTests
{
    [Fact]
    public void LoginUserModel_Properties_ShouldBeSetCorrectly()
    {
        // Arrange
        var loginUserModel = new LoginUserModel("testUser", "testPassword");

        // Act & Assert
        Assert.Equal("testUser", loginUserModel.Username);
        Assert.Equal("testPassword", loginUserModel.Password);
    }

    [Fact]
    public void LoginUserModel_ShouldBeEqual_WhenPropertiesAreSame()
    {
        // Arrange
        var model1 = new LoginUserModel("testUser", "testPassword");
        var model2 = new LoginUserModel("testUser", "testPassword");

        // Act & Assert
        Assert.Equal(model1, model2);
        Assert.True(model1 == model2); // Проверка оператора ==
    }

    [Fact]
    public void LoginUserModel_ShouldNotBeEqual_WhenPropertiesAreDifferent()
    {
        // Arrange
        var model1 = new LoginUserModel("testUser", "testPassword");
        var model2 = new LoginUserModel("anotherUser", "anotherPassword");

        // Act & Assert
        Assert.NotEqual(model1, model2);
        Assert.True(model1 != model2); // Проверка оператора !=
    }

    [Fact]
    public void LoginUserModel_Deconstruct_ShouldReturnCorrectValues()
    {
        // Arrange
        var loginUserModel = new LoginUserModel("testUser", "testPassword");

        // Act
        var (username, password) = loginUserModel;

        // Assert
        Assert.Equal("testUser", username);
        Assert.Equal("testPassword", password);
    }
}

public class LoginUserResultTests
{
    [Fact]
    public void LoginUserResult_Properties_ShouldBeSetCorrectly()
    {
        // Arrange
        var loginUserResult = new LoginUserResult("testUser", "test@example.com", "testToken");

        // Act & Assert
        Assert.Equal("testUser", loginUserResult.Username);
        Assert.Equal("test@example.com", loginUserResult.Email);
        Assert.Equal("testToken", loginUserResult.Token);
    }

    [Fact]
    public void LoginUserResult_ShouldBeEqual_WhenPropertiesAreSame()
    {
        // Arrange
        var result1 = new LoginUserResult("testUser", "test@example.com", "testToken");
        var result2 = new LoginUserResult("testUser", "test@example.com", "testToken");

        // Act & Assert
        Assert.Equal(result1, result2);
        Assert.True(result1 == result2); // Проверка оператора ==
    }

    [Fact]
    public void LoginUserResult_ShouldNotBeEqual_WhenPropertiesAreDifferent()
    {
        // Arrange
        var result1 = new LoginUserResult("testUser", "test@example.com", "testToken");
        var result2 = new LoginUserResult("anotherUser", "another@example.com", "anotherToken");

        // Act & Assert
        Assert.NotEqual(result1, result2);
        Assert.True(result1 != result2); // Проверка оператора !=
    }

    [Fact]
    public void LoginUserResult_Deconstruct_ShouldReturnCorrectValues()
    {
        // Arrange
        var loginUserResult = new LoginUserResult("testUser", "test@example.com", "testToken");

        // Act
        var (username, email, token) = loginUserResult;

        // Assert
        Assert.Equal("testUser", username);
        Assert.Equal("test@example.com", email);
        Assert.Equal("testToken", token);
    }
}

public class RegisterUserModelTests
{
    [Fact]
    public void RegisterUserModel_Properties_ShouldBeSetCorrectly()
    {
        // Arrange
        var registerUserModel = new RegisterUserModel("testUser", "test@example.com", "testPassword", "testPassword");

        // Act & Assert
        Assert.Equal("testUser", registerUserModel.Username);
        Assert.Equal("test@example.com", registerUserModel.Email);
        Assert.Equal("testPassword", registerUserModel.Password);
        Assert.Equal("testPassword", registerUserModel.ConfirmPassword);
    }

    [Fact]
    public void RegisterUserModel_ShouldBeEqual_WhenPropertiesAreSame()
    {
        // Arrange
        var model1 = new RegisterUserModel("testUser", "test@example.com", "testPassword", "testPassword");
        var model2 = new RegisterUserModel("testUser", "test@example.com", "testPassword", "testPassword");

        // Act & Assert
        Assert.Equal(model1, model2);
        Assert.True(model1 == model2); // Проверка оператора ==
    }

    [Fact]
    public void RegisterUserModel_ShouldNotBeEqual_WhenPropertiesAreDifferent()
    {
        // Arrange
        var model1 = new RegisterUserModel("testUser", "test@example.com", "testPassword", "testPassword");
        var model2 = new RegisterUserModel("anotherUser", "another@example.com", "anotherPassword", "anotherPassword");

        // Act & Assert
        Assert.NotEqual(model1, model2);
        Assert.True(model1 != model2); // Проверка оператора !=
    }

    [Fact]
    public void RegisterUserModel_Deconstruct_ShouldReturnCorrectValues()
    {
        // Arrange
        var registerUserModel = new RegisterUserModel("testUser", "test@example.com", "testPassword", "testPassword");

        // Act
        var (username, email, password, confirmPassword) = registerUserModel;

        // Assert
        Assert.Equal("testUser", username);
        Assert.Equal("test@example.com", email);
        Assert.Equal("testPassword", password);
        Assert.Equal("testPassword", confirmPassword);
    }
}