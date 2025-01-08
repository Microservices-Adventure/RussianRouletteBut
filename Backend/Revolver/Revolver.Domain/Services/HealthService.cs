using Revolver.Domain.Config;
using Revolver.Domain.Services.Interfaces;

namespace Revolver.Domain.Services;

public class HealthService : IHealthService
{
    public double CooldownTime()
    {
        return double.Max(DateTimeOffset.UtcNow.ToUnixTimeSeconds() - HealthSettings.AppStartAt.AddSeconds(HealthSettings.CooldownTime).ToUnixTimeSeconds(), 0);
    }

    public bool IsLive()
    {
        return true;
    }
}