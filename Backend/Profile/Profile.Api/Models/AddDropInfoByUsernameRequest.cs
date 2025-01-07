namespace Profile.Api.Models
{
    public class AddDropInfoByUsernameRequest
    {
        public string Username { get; set; } = null!;
        public string ServiceName { get; set; } = null!;
        public DateTimeOffset Moment { get; set; } = DateTimeOffset.UtcNow;
    }
}
