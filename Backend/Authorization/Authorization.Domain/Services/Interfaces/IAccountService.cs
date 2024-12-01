using Authorization.Domain.Models;

namespace Authorization.Domain.Services.Interfaces;
public interface IAccountService
{
    Task<LoginUserResult> Login(LoginUserModel loginUserModel, CancellationToken token);
    Task<bool> RegisterUser(RegisterUserModel user, CancellationToken token);
}
