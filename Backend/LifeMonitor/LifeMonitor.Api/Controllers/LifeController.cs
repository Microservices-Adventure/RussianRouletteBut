using LifeMonitor.Api.Models;
using LifeMonitor.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace LifeMonitor.Api.Controllers
{
    [ApiController]
    [Route("api/life")]
    public class LifeController : ControllerBase
    {
        private static Dictionary<string, string> ServiceHostsPorts = new()
        {
            { "localhost", "8082" }, // Authorization
            { "localhost", "8084" }, // Revolver
            { "localhost", "8086" }, // ActionLog
            { "localhost", "8088" } // Profile
        };
        private readonly IMonitorService _monitorService;
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
                return StatusCode(500, "An error occurred while processing your request.");
            }
            

        }

    }
}
