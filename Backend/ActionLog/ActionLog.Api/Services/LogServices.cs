using ActionLog.Api.DataAccess;
using ActionLog.Api.DataAccess.Entity;
using ActionLog.Api.Models;
using Microsoft.EntityFrameworkCore;
namespace ActionLog.Api.Services
{
    public class LogServices
    {
        public class LogService : ILogService
        {
            private readonly AppDbContext _context;

            public LogService(AppDbContext context)
            {
                _context = context;
            }

            public async Task<(int TotalRecords, IEnumerable<ALog> Logs)> GetLogsAsync(GetLogsRequest request)
            {
                if (request.Page < 1 || request.Size < 1)
                {
                    throw new ArgumentException("Page and size must be greater than 0.");
                }

                var query = _context.ActionLogs.AsQueryable();

                // Фильтрация по полям модели
                if (!string.IsNullOrEmpty(request.Username))
                {
                    query = query.Where(log => log.Username == request.Username);
                }

                if (!string.IsNullOrEmpty(request.Email))
                {
                    query = query.Where(log => log.Email == request.Email);
                }

                if (request.MicroserviceId.HasValue)
                {
                    query = query.Where(log => log.MicroserviceId == request.MicroserviceId.Value);
                }

                if (!string.IsNullOrEmpty(request.MicroserviceName))
                {
                    query = query.Where(log => log.MicroserviceName == request.MicroserviceName);
                }

                if (!string.IsNullOrEmpty(request.Description))
                {
                    query = query.Where(log => log.Description == request.Description);
                }

                if (!string.IsNullOrEmpty(request.Status))
                {
                    query = query.Where(log => log.Status == request.Status);
                }

                if (request.HasError.HasValue)
                {
                    query = query.Where(log => request.HasError.Value ? log.Error != null : log.Error == null);
                }

                if (request.From.HasValue)
                {
                    query = query.Where(log => log.Moment >= request.From.Value);
                }

                if (request.To.HasValue)
                {
                    query = query.Where(log => log.Moment <= request.To.Value);
                }

                // Пагинация
                var totalRecords = await query.CountAsync();
                var logs = await query
                    .Skip((int)((request.Page - 1) * request.Size))
                    .Take((int)request.Size)
                    .ToListAsync();

                return (totalRecords, logs);
            }

            public async Task<ALog> AddLogAsync(AddLogRequest request, CancellationToken ct)
            {
                if (request == null)
                {
                    throw new ArgumentNullException(nameof(request), "Log data is null.");
                }

                // Проверка обязательных полей
                if (string.IsNullOrWhiteSpace(request.MicroserviceName) ||
                    string.IsNullOrWhiteSpace(request.Description) ||
                    string.IsNullOrWhiteSpace(request.Status))
                {
                    throw new ArgumentException("MicroserviceName, Description, and Status are required fields.");
                }

                // Создание сущности ALog из запроса
                var log = new ALog
                {
                    Username = request.Username,
                    Email = request.Email,
                    MicroserviceId = request.MicroserviceId,
                    MicroserviceName = request.MicroserviceName,
                    Description = request.Description,
                    Status = request.Status,
                    Error = request.Error,
                    Moment = request.Moment == default ? DateTimeOffset.UtcNow : request.Moment // Установка временной метки, если она не передана
                };

                // Добавление записи в базу данных
                _context.ActionLogs.Add(log);
                await _context.SaveChangesAsync(ct);

                return log;
            }
        }


    }
}
