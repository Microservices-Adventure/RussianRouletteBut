using LifeMonitor.Api.Models;
using LifeMonitor.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace LifeMonitor.Api.Controllers
{
    [ApiController]
    [Route("api/life")]
    public class LifeController : ControllerBase
    {
        private readonly IMonitorService _monitorService;
        private static readonly Dictionary<string, string> ServiceHostsPorts = new()
        {
            { "authorization", "8082" }, // Authorization
            { "revolver", "8084" }, // Revolver
            { "actionlog", "8086" }, // ActionLog
            { "profile", "8088" } // Profile
        };
        
        public LifeController(IMonitorService monitorService)
        {
            _monitorService = monitorService;
        }

        [HttpGet("getlifes")]
        public async Task<IActionResult> GetLifes()
        {
            List<LifeServiceModel> lifeServiceModels = [];
            try
            {
                foreach (var serviceHost in ServiceHostsPorts)
                {
                    lifeServiceModels.Add(await _monitorService.GetLife(serviceHost.Key, serviceHost.Value));
                }
                return Ok(lifeServiceModels);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
            

        }

    }
}
