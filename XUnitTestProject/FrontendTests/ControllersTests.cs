using System.Security.Claims;
using Frontend.Entities.Account.Lib.Exceptions;
using Frontend.Entities.Account.Model;
using Frontend.Entities.ActionLog;
using Frontend.Entities.ErrorViewModel.Model;
using Frontend.Entities.LifeMonitor.Model;
using Frontend.Entities.Profile.Model;
using Frontend.Entities.Revolver.Model;
using Frontend.Pages.Home.Model;
using Frontend.Pages.LifeMonitor.Model;
using Frontend.Pages.LogAction.Model;
using Frontend.Pages.Profile.Model;
using Frontend.Pages.Revolver.Model;

namespace XUnitTestProject.FrontendTests;

using System.Threading.Tasks;
using Frontend.Features.Services.Interfaces;
using Frontend.Pages.Account.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

public class AccountControllerTests
{
    private readonly Mock<IAccountService> _mockAccountService;
    private readonly Mock<ILogger<AccountController>> _mockLogger;
    private readonly AccountController _accountController;

    public AccountControllerTests()
    {
        _mockAccountService = new Mock<IAccountService>();
        _mockLogger = new Mock<ILogger<AccountController>>();
        _accountController = new AccountController(_mockAccountService.Object, _mockLogger.Object)
        {
            ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext()
            }
        };
    }

[Fact]
public async Task Login_ShouldRedirectToLoginWithError_WhenLoginFails()
{
    // Arrange
    var loginModel = new LoginModel 
    { 
        Username = "testuser", 
        Password = "password"  
    };

    _mockAccountService
        .Setup(x => x.Login(loginModel))
        .ThrowsAsync(new RequestLoginException("Invalid credentials"));

    // Act
    var result = await _accountController.Login(loginModel);

    // Assert
    var redirectResult = Assert.IsType<RedirectToActionResult>(result);
    Assert.Equal("Login", redirectResult.ActionName);
    Assert.Equal("Account", redirectResult.ControllerName);
    Assert.Equal("Неправильный логин или пароль", redirectResult.RouteValues["error"]);
}

    [Fact]
    public async Task Register_ShouldRedirectToRegisterSend_WhenRegistrationIsSuccessful()
    {
        // Arrange
        var registerModel = new RegisterModel 
        { 
            Username = "testuser",      
            Email = "test@example.com", 
            Password = "password",      
            ConfirmPassword = "password" 
        };

        _mockAccountService
            .Setup(x => x.Register(registerModel))
            .Returns(Task.CompletedTask);

        // Act
        var result = await _accountController.Register(registerModel);

        // Assert
        var redirectResult = Assert.IsType<RedirectToActionResult>(result);
        Assert.Equal("RegisterSend", redirectResult.ActionName);
        Assert.Equal("Account", redirectResult.ControllerName);
    }

    [Fact]
    public void RegisterSend_ShouldReturnView_WithUsername()
    {
        // Arrange
        var registerModel = new RegisterModel 
        { 
            Username = "testuser",      
            Email = "test@example.com", 
            Password = "password",      
            ConfirmPassword = "password" 
        };

        // Act
        var result = _accountController.RegisterSend(registerModel);

        // Assert
        var viewResult = Assert.IsType<ViewResult>(result);
        Assert.Equal("testuser", viewResult.ViewData["username"]);
    }
}

public class HomeControllerTests
{
    private readonly Mock<ILogger<HomeController>> _mockLogger;
    private readonly HomeController _homeController;

    public HomeControllerTests()
    {
        _mockLogger = new Mock<ILogger<HomeController>>();
        _homeController = new HomeController(_mockLogger.Object)
        {
            ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext()
            }
        };
    }

    [Fact]
    public void Index_ShouldReturnViewResult()
    {
        // Act
        var result = _homeController.Index();

        // Assert
        var viewResult = Assert.IsType<ViewResult>(result);
        Assert.Null(viewResult.ViewName); 
    }

    [Fact]
    public void Privacy_ShouldReturnViewResult()
    {
        // Act
        var result = _homeController.Privacy();

        // Assert
        var viewResult = Assert.IsType<ViewResult>(result);
        Assert.Null(viewResult.ViewName); 
    }

    [Fact]
    public void Error_ShouldReturnViewResult_WithErrorViewModel()
    {
        // Arrange
        var traceIdentifier = "test-trace-id";
        _homeController.HttpContext.TraceIdentifier = traceIdentifier;

        // Act
        var result = _homeController.Error();

        // Assert
        var viewResult = Assert.IsType<ViewResult>(result);
        Assert.Null(viewResult.ViewName); 

        var model = Assert.IsType<ErrorViewModel>(viewResult.Model);
        Assert.Equal(traceIdentifier, model.RequestId);
    }
}

public class MonitorControllerTests
{
    private readonly Mock<IMonitorService> _mockMonitorService;
    private readonly MonitorController _monitorController;

    public MonitorControllerTests()
    {
        _mockMonitorService = new Mock<IMonitorService>();
        _monitorController = new MonitorController(_mockMonitorService.Object);
    }

    [Fact]
    public async Task GetLifes_ShouldReturnEmptyList_WhenServiceReturnsEmptyList()
    {
        // Arrange
        var expectedLifes = new List<LifeServiceModel>();

        _mockMonitorService
            .Setup(x => x.GetLifes())
            .ReturnsAsync(expectedLifes);

        // Act
        var result = await _monitorController.GetLifes();

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var returnedLifes = Assert.IsType<List<LifeServiceModel>>(okResult.Value);
        Assert.Empty(returnedLifes);
    }
}

public class RevolverControllerTests
{
    private readonly Mock<IRevolverService> _mockRevolverService;
    private readonly RevolverController _revolverController;

    public RevolverControllerTests()
    {
        _mockRevolverService = new Mock<IRevolverService>();
        _revolverController = new RevolverController(_mockRevolverService.Object)
        {
            ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext()
            }
        };
    }

    [Fact]
    public async Task Shoot_ShouldReturnOkResult_WithKilledServiceInfo()
    {
        // Arrange
        var username = "testuser";
        var email = "test@example.com";
        var expectedResult = new KilledServiceInfo
        {
            ServiceName = "TestService",
            ServiceNameRus = "Тестовый сервис",
            Host = "localhost", 
            DropSeconds = 10,   
            CooldownSeconds = 5 
        };
        
        _mockRevolverService
            .Setup(x => x.Shoot(It.Is<ShootMan>(s => s.Username == username && s.Email == email)))
            .ReturnsAsync(expectedResult);
        var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
        {
            new Claim(ClaimTypes.Name, username),
            new Claim(ClaimTypes.Email, email)
        }, "mock"));

        _revolverController.HttpContext.User = user;

        // Act
        var result = await _revolverController.Shoot();

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var killedServiceInfo = Assert.IsType<KilledServiceInfo>(okResult.Value);
        Assert.Equal(expectedResult.ServiceName, killedServiceInfo.ServiceName);
        Assert.Equal(expectedResult.ServiceNameRus, killedServiceInfo.ServiceNameRus);
        Assert.Equal(expectedResult.Host, killedServiceInfo.Host);
        Assert.Equal(expectedResult.DropSeconds, killedServiceInfo.DropSeconds);
        Assert.Equal(expectedResult.CooldownSeconds, killedServiceInfo.CooldownSeconds);
    }

    [Fact]
    public async Task Shoot_ShouldThrowException_WhenUserIdentityIsNull()
    {
        // Arrange
        _revolverController.HttpContext.User = new ClaimsPrincipal(); 

        // Act & Assert
        await Assert.ThrowsAsync<NullReferenceException>(() => _revolverController.Shoot());
    }
}

public class ProfileControllerTests
{
    private readonly Mock<IProfileService> _mockProfileService;
    private readonly ProfileController _profileController;

    public ProfileControllerTests()
    {
        _mockProfileService = new Mock<IProfileService>();
        _profileController = new ProfileController(_mockProfileService.Object)
        {
            ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext()
            }
        };
    }

    [Fact]
    public async Task Profile_ShouldReturnViewResult()
    {
        // Arrange
        var username = "testuser";
        var expectedProfileResponse = new UserProfileResponse
        {
            Id = 1, 
            Username = username, 
            Email = "test@example.com", 
            History = new List<DropInfo>() 
        };

        _mockProfileService
            .Setup(x => x.GetUserProfile(It.Is<GetUserProfileRequest>(r => r.Username == username)))
            .ReturnsAsync(expectedProfileResponse);

        var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
        {
            new Claim(ClaimTypes.Name, username)
        }, "mock"));

        _profileController.HttpContext.User = user;

        // Act
        var result = await _profileController.Profile();

        // Assert
        var viewResult = Assert.IsType<ViewResult>(result);
        Assert.Null(viewResult.ViewName); 
        Assert.True(viewResult.ViewData.ContainsKey(nameof(UserProfileResponse)));
        var profileResponse = viewResult.ViewData[nameof(UserProfileResponse)] as UserProfileResponse;
        Assert.NotNull(profileResponse);
        Assert.Equal(expectedProfileResponse.Id, profileResponse.Id);
        Assert.Equal(expectedProfileResponse.Username, profileResponse.Username);
        Assert.Equal(expectedProfileResponse.Email, profileResponse.Email);
        Assert.Equal(expectedProfileResponse.History, profileResponse.History);
    }

    [Fact]
    public async Task Profile_ShouldThrowException_WhenUserIdentityIsNull()
    {
        // Arrange
        _profileController.HttpContext.User = new ClaimsPrincipal();

        // Act & Assert
        await Assert.ThrowsAsync<NullReferenceException>(() => _profileController.Profile());
    }
}

public class LogActionControllerTests
{
    private readonly Mock<ILogService> _mockLogService;
    private readonly LogActionController _logActionController;

    public LogActionControllerTests()
    {
        _mockLogService = new Mock<ILogService>();
        _logActionController = new LogActionController(_mockLogService.Object);
    }
    
    [Fact]
public async Task GetLogs_ShouldReturnOkResult_WithGetLogsResponse()
{
    // Arrange
    var request = new GetLogsRequest
    {
        Page = 1,
        Size = 10,
        Username = "testuser",
        Email = "test@example.com",
        MicroserviceId = 1,
        MicroserviceName = "TestService",
        Description = "Test log",
        Status = "Success",
        HasError = false,
        From = DateTime.UtcNow.AddDays(-1),
        To = DateTime.UtcNow
    };

    var expectedResponse = new GetLogsResponse
    {
        TotalRecords = 1,
        Logs = new List<LogModel>
        {
            new LogModel
            {
                Id = 1,
                Description = "Test log",
                Email = "test@example.com",
                Error = null,
                MicroserviceId = 1,
                MicroserviceName = "TestService",
                Status = "Success",
                Username = "testuser",
                Moment = DateTimeOffset.Now
            }
        }
    };
    
    _mockLogService
        .Setup(x => x.GetLogs(request))
        .ReturnsAsync(expectedResponse);

    // Act
    var result = await _logActionController.GetLogs(request);

    // Assert
    var okResult = Assert.IsType<OkObjectResult>(result);
    var returnedResponse = Assert.IsType<GetLogsResponse>(okResult.Value);
    Assert.Equal(expectedResponse.TotalRecords, returnedResponse.TotalRecords);
    Assert.Equal(expectedResponse.Logs.Count(), returnedResponse.Logs.Count());
    Assert.Equal(expectedResponse.Logs.First().Id, returnedResponse.Logs.First().Id);
    Assert.Equal(expectedResponse.Logs.First().Description, returnedResponse.Logs.First().Description);
    Assert.Equal(expectedResponse.Logs.First().Email, returnedResponse.Logs.First().Email);
    Assert.Equal(expectedResponse.Logs.First().MicroserviceId, returnedResponse.Logs.First().MicroserviceId);
    Assert.Equal(expectedResponse.Logs.First().MicroserviceName, returnedResponse.Logs.First().MicroserviceName);
    Assert.Equal(expectedResponse.Logs.First().Status, returnedResponse.Logs.First().Status);
    Assert.Equal(expectedResponse.Logs.First().Username, returnedResponse.Logs.First().Username);
}

[Fact]
public async Task GetLogs_ShouldReturnOkResult_WithEmptyLogs_WhenServiceReturnsEmptyResponse()
{
    // Arrange
    var request = new GetLogsRequest
    {
        Page = 1,
        Size = 10,
        Username = "testuser",
        Email = "test@example.com",
        MicroserviceId = 1,
        MicroserviceName = "TestService",
        Description = "Test log",
        Status = "Success",
        HasError = false,
        From = DateTime.UtcNow.AddDays(-1),
        To = DateTime.UtcNow
    };

    var expectedResponse = new GetLogsResponse
    {
        TotalRecords = 0, 
        Logs = new List<LogModel>() 
    };
    
    _mockLogService
        .Setup(x => x.GetLogs(request))
        .ReturnsAsync(expectedResponse);

    // Act
    var result = await _logActionController.GetLogs(request);

    // Assert
    var okResult = Assert.IsType<OkObjectResult>(result);
    var returnedResponse = Assert.IsType<GetLogsResponse>(okResult.Value);
    Assert.Equal(expectedResponse.TotalRecords, returnedResponse.TotalRecords);
    Assert.Empty(returnedResponse.Logs);
}
}