using ActionLog.Api.DataAccess.Entity;
using ActionLog.Api.Models;

namespace ActionLog.Api.Services
{
    public interface ILogService
    {
        Task<(int TotalRecords, IEnumerable<ALog> Logs)> GetLogsAsync(GetLogsRequest request);
        Task<ALog> AddLogAsync(AddLogRequest request);
    }
}
