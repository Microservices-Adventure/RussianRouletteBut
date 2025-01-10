namespace LifeMonitor.Api.Models
{
    public record LifeServiceModel
    {
        public required string ServiceName { get; init; }
        public required bool IsLife { get; init; }
        public CooldownModel? Cooldown { get; init; }
    }
}
