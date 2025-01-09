using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ActionLog.Api.DataAccess;
using ActionLog.Api.DataAccess.Entity;
using ActionLog.Api.Models;
using ActionLog.Api.Services;
using System.Threading;

namespace ActionLog.Api.Controllers
{
    [ApiController]
    [Route("api/logs")]
    public class LogsController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly ILogService _logService;
        private readonly IHostApplicationLifetime _appLifetime;

        public LogsController(AppDbContext context, ILogService logService, IHostApplicationLifetime appLifetime)
        {
            _context = context;
            _logService = logService;
            _appLifetime = appLifetime;
        }

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

        [HttpPost("add")]
        public async Task<IActionResult> AddLog([FromBody] AddLogRequest request)
        {
            var stoppingToken = _appLifetime.ApplicationStopping;

            try
            {
                if (request == null)
                {
                    return BadRequest("Log data is null.");
                }

                var log = await _logService.AddLogAsync(request, stoppingToken);

                return CreatedAtAction(nameof(GetLogs), new { id = log.Id }, log);
            }
            catch (OperationCanceledException)
            {
                return StatusCode(499, "Application is stopping.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }
    }
}