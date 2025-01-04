using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ActionLog.Api.DataAccess;
using ActionLog.Api.DataAccess.Entity;
using ActionLog.Api.Models;

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
        public async Task<IActionResult> GetLogs([FromQuery] GetLogsRequest request)
        {
            if (request.Page < 1 || request.Size < 1)
            {
                return BadRequest("Page and size must be greater than 0.");
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