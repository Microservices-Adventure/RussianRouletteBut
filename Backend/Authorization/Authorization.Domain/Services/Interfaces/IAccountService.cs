using Authorization.Domain.Models;

namespace Authorization.Domain.Services.Interfaces;
public interface IAccountService
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="user"></param>
    /// <exception cref="FluentValidation.ValidationException"></exception>
    /// <returns></returns>
    Task<bool> RegisterUser(RegisterUserModel user);
}
