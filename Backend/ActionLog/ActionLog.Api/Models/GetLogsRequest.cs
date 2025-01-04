namespace ActionLog.Api.Models
{
    public class GetLogsRequest
    {
        public string? Username { get; set; }
        public string? Email { get; set; }
        public long MicroserviceId { get; set; }
        public string MicroserviceName { get; set; } = null!;
        public string Description { get; set; } = null!;
        public string Status { get; set; } = null!;
        public string? Error { get; set; }
        public DateTimeOffset Moment { get; set; }
    }
}
