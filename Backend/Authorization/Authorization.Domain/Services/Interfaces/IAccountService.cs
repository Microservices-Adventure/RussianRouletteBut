using Authorization.Domain.Models;

namespace Authorization.Domain.Services.Interfaces;
public interface IAccountService
{
    Task<LoginUserResult> Login(LoginUserModel loginUserModel);
    Task<bool> RegisterUser(RegisterUserModel user);
}
