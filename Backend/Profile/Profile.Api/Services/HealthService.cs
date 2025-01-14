using Profile.Api.Config;

namespace Profile.Api.Services
{
    public class HealthService : IHealthService
    {
        public double CooldownTime()
        {
            long currentTime = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
            long endTime = HealthSettings.AppStartAt.AddSeconds(HealthSettings.CooldownTime).ToUnixTimeSeconds();
            return double.Max(endTime - currentTime, 0);
        }

        public bool IsLive()
        {
            return true;
        }
    }
}
