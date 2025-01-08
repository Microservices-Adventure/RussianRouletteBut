namespace ActionLog.Api.Models
{
    public class GetLogsRequest
    {
        public long Page { get; set; } = 1;
        public long Size { get; set; } = 10;
        public string? Username { get; set; }
        public string? Email { get; set; }
        public long? MicroserviceId { get; set; }
        public string? MicroserviceName { get; set; }
        public string? Description { get; set; }
        public string? Status { get; set; }
        public bool? HasError { get; set; }
        public DateTimeOffset? From { get; set; }
        public DateTimeOffset? To { get; set; }
    }
}
