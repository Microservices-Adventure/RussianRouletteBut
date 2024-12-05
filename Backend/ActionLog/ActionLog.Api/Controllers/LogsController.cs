using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ActionLog.Api.DataAccess;
using ActionLog.Api.DataAccess.Entity;

namespace ActionLog.Api.Controllers
{
    [ApiController]
    [Route("api/logs")]
    public class LogsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public LogsController(AppDbContext context)
        {
            _context = context;
        }

        // GET: /api/logs/getlist
        [HttpGet("getlist")]
        public async Task<IActionResult> GetLogs(
            [FromQuery] long page = 1,
            [FromQuery] long size = 10,
            [FromQuery] long? microservice_id = null,
            [FromQuery] string? status = null,
            [FromQuery] bool? exception = null,
            [FromQuery] DateTimeOffset? from = null,
            [FromQuery] DateTimeOffset? to = null)
        {
            if (page < 1 || size < 1)
            {
                return BadRequest("Page and size must be greater than 0.");
            }

            var query = _context.ActionLogs.AsQueryable();

            // Фильтрация
            if (microservice_id.HasValue)
            {
                query = query.Where(log => log.MicroserviceId == microservice_id.Value);
            }

            if (!string.IsNullOrEmpty(status))
            {
                query = query.Where(log => log.Status == status);
            }

            if (exception.HasValue)
            {
                query = query.Where(log => exception.Value ? log.Error != null : log.Error == null);
            }

            if (from.HasValue)
            {
                query = query.Where(log => log.Moment >= from.Value);
            }

            if (to.HasValue)
            {
                query = query.Where(log => log.Moment <= to.Value);
            }

            // Пагинация
            var totalRecords = await query.CountAsync();
            var logs = await query
                .Skip((int)((page - 1) * size))
                .Take((int)size)
                .ToListAsync();

            // Возврат результата с пагинацией
            return Ok(new
            {
                TotalRecords = totalRecords,
                Logs = logs
            });
        }

        // POST: /api/logs/add
        [HttpPost("add")]
        public async Task<IActionResult> AddLog([FromBody] ALog log)
        {
            if (log == null)
            {
                return BadRequest("Log data is null.");
            }

            // Проверка обязательных полей
            if (string.IsNullOrWhiteSpace(log.MicroserviceName) ||
                string.IsNullOrWhiteSpace(log.Description) ||
                string.IsNullOrWhiteSpace(log.Status))
            {
                return BadRequest("MicroserviceName, Description, and Status are required fields.");
            }

            // Исключение Id из входного объекта (на всякий случай)
            log.Id = 0;

            // Добавление записи
            _context.ActionLogs.Add(log);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetLogs), new { id = log.Id }, log);
        }
    }
}