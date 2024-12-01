namespace Revolver.Domain.Models;

public sealed record TargetServiceModel
{
    public required string ServiceName { get; init; }
    public int DropSeconds { get; init; }
    public required string KillerName { get; init; }
    public long Points { get; init; }
}