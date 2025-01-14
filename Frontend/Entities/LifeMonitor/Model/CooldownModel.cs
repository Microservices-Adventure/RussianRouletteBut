namespace Frontend.Entities.LifeMonitor.Model;

public record CooldownModel
{
    public required bool IsCooldown { get; init; }
    public required double SecondsLeft { get; init; }
}