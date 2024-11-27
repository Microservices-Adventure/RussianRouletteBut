using Authorization.Domain.Entities;
using Authorization.Domain.Models;
using Authorization.Domain.Services.Interfaces;
using FluentValidation;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Authorization.Domain.Services;

public class AccountService : IAccountService
{
    private readonly UserManager<User> _userManager;
    private readonly SignInManager<User> _signInManager;
    private readonly ITokenService _tokenService;
    private readonly IValidator<RegisterUserModel> _registerUserModelValidator;
    private readonly IValidator<LoginUserModel> _loginUserModelValidator;
    private const string UnauthorizedExceptionMessage = "Username or password is incorrect.";

    public AccountService(
        UserManager<User> userManager, 
        SignInManager<User> signInManager, 
        ITokenService tokenService,
        IValidator<RegisterUserModel> registerUserModelValidator,
        IValidator<LoginUserModel> loginUserModelValidator)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _tokenService = tokenService;
        _registerUserModelValidator = registerUserModelValidator;
        _loginUserModelValidator = loginUserModelValidator;
    }

    public async Task<LoginUserResult> Login(LoginUserModel loginUserModel)
    {
        await _loginUserModelValidator.ValidateAndThrowAsync(loginUserModel);
        
        var user = await _userManager.Users.FirstOrDefaultAsync(u => u.UserName == loginUserModel.Username);

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

    public async Task<bool> RegisterUser(RegisterUserModel userModel)
    {
        await _registerUserModelValidator.ValidateAndThrowAsync(userModel);

        User user = new User
        {
            UserName = userModel.Username,
            Email = userModel.Email
        };

        var createdUser = await _userManager.CreateAsync(user, userModel.Password);

        if (createdUser.Succeeded)
        {
            var roleResult = await _userManager.AddToRoleAsync(user, Roles.User.ToString());
            return roleResult.Succeeded;
        }
        else
        {
            return false;
        }
    }
}
