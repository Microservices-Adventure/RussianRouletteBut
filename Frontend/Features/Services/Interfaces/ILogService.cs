using Frontend.Entities.ActionLog;

namespace Frontend.Features.Services.Interfaces;

public interface ILogService
{
    Task SendLog(AddLogRequest request);
    Task<IReadOnlyCollection<LogModel>> GetLogs(GetLogsRequest request);
}