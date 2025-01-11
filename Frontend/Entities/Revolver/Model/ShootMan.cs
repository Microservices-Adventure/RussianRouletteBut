namespace Frontend.Entities.Revolver.Model;

public record ShootMan
{
    public required string Username { get; init; }
    public string? Email { get; init; }
}