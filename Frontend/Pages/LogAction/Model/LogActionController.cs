using Frontend.Entities.ActionLog;
using Frontend.Features.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Frontend.Pages.LogAction.Model;

public class LogActionController : Controller
{
    private readonly ILogService _logService;

    public LogActionController(ILogService logService)
    {
        _logService = logService;
    }
    
    public async Task<IActionResult> GetLogs(GetLogsRequest request)
    {
        var response = await _logService.GetLogs(request);
        return Ok(response);
    }
}