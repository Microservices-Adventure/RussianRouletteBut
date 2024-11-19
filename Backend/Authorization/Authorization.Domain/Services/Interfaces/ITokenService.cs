using Authorization.Domain.Entities;

namespace Authorization.Domain.Services.Interfaces;
public interface ITokenService
{
    Task<string> CreateToken(User user);
}
