namespace Revolver.Domain.Models;

public sealed record ShootRequestModel
{
    public required IReadOnlyList<string> Bullets { get; init; }
}