namespace LifeMonitor.Api.Models
{
    public record CooldownModel
    {
        public required bool IsCooldown { get; init; }
        public required double SecondsLeft { get; init; }
    }
}
