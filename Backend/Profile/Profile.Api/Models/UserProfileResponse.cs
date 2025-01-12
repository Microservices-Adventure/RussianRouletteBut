using Profile.Api.DataAccess.Entity;

namespace Profile.Api.Models
{
    public record UserProfileResponse
    {
        public required long Id { get; init; }
        public required string Username { get; init; } = null!;
        public required string Email { get; init; } = null!;
        public required List<DropInfo> History { get; init; } = [];
    }
}
