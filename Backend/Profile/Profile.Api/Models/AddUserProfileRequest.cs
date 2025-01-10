namespace Profile.Api.Models
{
    public class AddUserProfileRequest
    {
        public string Username { get; set; } = null!;
        public string Email { get; set; } = null!;
    }
}
