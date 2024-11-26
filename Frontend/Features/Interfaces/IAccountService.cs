using Frontend.Entities.Account.Model;

namespace Frontend.Features.Interfaces;

public interface IAccountService
{
    Task Login(LoginModel loginModel);
    Task Register(RegisterModel registerModel);
}