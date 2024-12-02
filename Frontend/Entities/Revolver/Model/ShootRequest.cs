namespace Frontend.Entities.Revolver.Model;

public sealed class ShootRequest
{
    public required List<string> Bullets { get; init; }
}