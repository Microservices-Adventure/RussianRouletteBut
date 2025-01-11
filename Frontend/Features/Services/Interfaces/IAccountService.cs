using Frontend.Entities.Account.Model;

namespace Frontend.Features.Services.Interfaces;

public interface IAccountService
{
    Task<LoginResponse> Login(LoginModel loginModel);
    Task Register(RegisterModel registerModel);
}