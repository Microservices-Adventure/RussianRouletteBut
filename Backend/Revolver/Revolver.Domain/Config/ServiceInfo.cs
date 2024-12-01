namespace Revolver.Domain.Config;

public sealed class ServiceInfo
{
    public string ServiceName { get; set; } = null!;
    public string ServiceNameRus { get; set; } = null!;
    public string Host { get; set; } = null!;
    public int DropSeconds { get; set; }
    public int CooldownSeconds { get; set; }
}