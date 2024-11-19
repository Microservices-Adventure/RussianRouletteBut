using Authorization.Domain.Entities;
using Authorization.Domain.Models;
using Authorization.Domain.Services.Interfaces;
using FluentValidation;
using Microsoft.AspNetCore.Identity;

namespace Authorization.Domain.Services;

public class AccountService : IAccountService
{
    private readonly UserManager<User> _userManager;
    private readonly IValidator<RegisterUserModel> _userModelValidator; 

    public AccountService(UserManager<User> userManager, IValidator<RegisterUserModel> userModelValidator)
    {
        _userManager = userManager;
        _userModelValidator = userModelValidator;
    }

    public async Task<bool> RegisterUser(RegisterUserModel userModel)
    {
        _userModelValidator.ValidateAndThrow(userModel);

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
