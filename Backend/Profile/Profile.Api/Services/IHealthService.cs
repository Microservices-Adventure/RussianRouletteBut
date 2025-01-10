namespace Profile.Api.Services
{
    public interface IHealthService
    {
        double CooldownTime();
        bool IsLive();
    }
}
