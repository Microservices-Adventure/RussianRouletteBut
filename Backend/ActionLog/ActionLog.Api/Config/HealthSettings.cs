namespace ActionLog.Api.Config;

public class HealthSettings
{
    public static double CrashTime
    {
        get
        {
            var crashTime = Environment.GetEnvironmentVariable("HealthSettings_CrashTime")!; 
            return double.Parse(crashTime);
        }
    }

    public static double CooldownTime
    {
        get
        {
            var cooldownTime = Environment.GetEnvironmentVariable("HealthSettings_CooldownTime")!; 
            return double.Parse(cooldownTime);
        }
    }

    public static readonly DateTimeOffset AppStartAt = DateTimeOffset.UtcNow;
}