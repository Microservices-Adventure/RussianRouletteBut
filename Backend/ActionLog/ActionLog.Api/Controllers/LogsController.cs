using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ActionLog.Api.DataAccess;
using ActionLog.Api.DataAccess.Entity;
using ActionLog.Api.Models;
using ActionLog.Api.Services;

namespace ActionLog.Api.Controllers
{
    [ApiController]
    [Route("api/logs")]
    public class LogsController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly ILogService _logService;

        public LogsController(AppDbContext context, ILogService logService)
        {
            _context = context;
            _logService = logService;
        }

        // GET: /api/logs/getlist
        [HttpGet("getlist")]
        public async Task<IActionResult> GetLogs([FromQuery] GetLogsRequest request)
        {
            try
            {
                var (totalRecords, logs) = await _logService.GetLogsAsync(request);

                return Ok(new
                {
                    TotalRecords = totalRecords,
                    Logs = logs
                });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }

        // POST: /api/logs/add
        [HttpPost("add")]
        public async Task<IActionResult> AddLog([FromBody] AddLogRequest request)
        {
            try
            {
                if (request == null)
                {
                    return BadRequest("Log data is null.");
                }

                // Добавление лога через сервис
                var log = await _logService.AddLogAsync(request);

                // Возврат результата с кодом 201 (Created) и ссылкой на созданный ресурс
                return CreatedAtAction(nameof(GetLogs), new { id = log.Id }, log);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }
    }
}