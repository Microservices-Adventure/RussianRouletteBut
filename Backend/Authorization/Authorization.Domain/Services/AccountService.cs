using Authorization.Domain.Config;
using Authorization.Domain.Entities;
using Authorization.Domain.Kafka;
using Authorization.Domain.Models;
using Authorization.Domain.Services.Interfaces;
using FluentValidation;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Authorization.Domain.Services;

public class AccountService : IAccountService
{
    private readonly UserManager<User> _userManager;
    private readonly SignInManager<User> _signInManager;
    private readonly ITokenService _tokenService;
    private readonly IValidator<RegisterUserModel> _registerUserModelValidator;
    private readonly IValidator<LoginUserModel> _loginUserModelValidator;
    private const string UnauthorizedExceptionMessage = "Username or password is incorrect.";
    private readonly LogProducer _logProducer;

    public AccountService(
        UserManager<User> userManager, 
        SignInManager<User> signInManager, 
        ITokenService tokenService,
        IValidator<RegisterUserModel> registerUserModelValidator,
        IValidator<LoginUserModel> loginUserModelValidator,
        IOptions<KafkaSettings> kafkaSettings,
        ILogger<LogProducer> logProducer)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _tokenService = tokenService;
        _registerUserModelValidator = registerUserModelValidator;
        _loginUserModelValidator = loginUserModelValidator;
        _logProducer = new LogProducer(kafkaSettings.Value.BootstrapServers, kafkaSettings.Value.LogTopic, logProducer);
    }

    public async Task<LoginUserResult> Login(LoginUserModel loginUserModel, CancellationToken ct)
    {
        await _loginUserModelValidator.ValidateAndThrowAsync(loginUserModel, ct);
        
        var user = await _userManager.Users.FirstOrDefaultAsync(u => u.UserName == loginUserModel.Username, ct);

        if (user == null)
        {
            throw new UnauthorizedAccessException(UnauthorizedExceptionMessage);
        }
        
        var result = await _signInManager.CheckPasswordSignInAsync(user, loginUserModel.Password, false);

        if (!result.Succeeded)
        {
            throw new UnauthorizedAccessException(UnauthorizedExceptionMessage);
        }
        
        string token = await _tokenService.CreateToken(user);
        return new LoginUserResult(user.UserName!, user.Email!, token);
    }

    public async Task<bool> RegisterUser(RegisterUserModel userModel, CancellationToken ct)
    {
        await _registerUserModelValidator.ValidateAndThrowAsync(userModel, ct);

        User user = new User
        {
            UserName = userModel.Username,
            Email = userModel.Email
        };
        
        ct.ThrowIfCancellationRequested();
        var createdUser = await _userManager.CreateAsync(user, userModel.Password);

        if (createdUser.Succeeded)
        {
            var roleResult = await _userManager.AddToRoleAsync(user, Roles.User.ToString());
            if (roleResult.Succeeded)
            {
                _logProducer.AddLog(new AddLogRequest()
                {
                    Description = "Пользователь успешно зарегистрировался!",
                    Email = user.Email,
                    Error = null,
                    MicroserviceId = 1,
                    MicroserviceName = "Authorization",
                    Username = user.UserName,
                    Status = "Success"
                });
            }
            return roleResult.Succeeded;
        }
        else
        {
            return false;
        }
    }
}
