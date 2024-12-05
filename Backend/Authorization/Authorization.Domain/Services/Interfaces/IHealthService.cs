namespace Authorization.Domain.Services.Interfaces;

public interface IHealthService
{
    double CooldownTime();
    bool IsLive();
}