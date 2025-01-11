namespace Frontend.Entities.ActionLog;

public record GetLogsResponse
{
    public required int TotalRecords { get; init; }
    public required IEnumerable<LogModel> Logs { get; init; }
}