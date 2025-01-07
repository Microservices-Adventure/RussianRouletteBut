namespace Profile.Api.Models
{
    public class DropInfoResponse
    {
        public long Id { get; set; }
        public string ServiceName { get; set; } = null!;
        public DateTimeOffset Moment { get; set; }
        public long UserProfileId { get; set; }
    }
}
