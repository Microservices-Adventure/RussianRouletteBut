using Frontend.Entities.LifeMonitor.Model;
using Frontend.Features.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Frontend.Pages.LifeMonitor.Model;

public class MonitorController : Controller
{
    private readonly IMonitorService _monitorService;

    public MonitorController(IMonitorService monitorService)
    {
        _monitorService = monitorService;
    }

    [HttpGet]
    public async Task<IActionResult> GetLifes()
    {
        return Ok((await _monitorService.GetLifes()).ToList());
    }
}