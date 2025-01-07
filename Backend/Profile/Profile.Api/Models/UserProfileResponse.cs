namespace Profile.Api.Models
{
    public class UserProfileResponse
    {
        public long Id { get; set; }
        public string Username { get; set; } = null!;
        public string Email { get; set; } = null!;
        public List<DropInfoResponse> History { get; set; } = [];
    }
}
