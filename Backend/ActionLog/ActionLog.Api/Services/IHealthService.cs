namespace ActionLog.Api.Services;

public interface IHealthService
{
    double CooldownTime();
    bool IsLive();
}