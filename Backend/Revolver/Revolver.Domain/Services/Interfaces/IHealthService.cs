namespace Revolver.Domain.Services.Interfaces;

public interface IHealthService
{
    double CooldownTime();
    bool IsLive();
}