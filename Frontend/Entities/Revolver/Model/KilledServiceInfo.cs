namespace Frontend.Entities.Revolver.Model;

public record KilledServiceInfo
{
    public required string ServiceName { get; init; }
    public required string ServiceNameRus { get; init; }
    public required string Host { get; init; }
    public required int DropSeconds { get; init; }
    public required int CooldownSeconds { get; init; }
}