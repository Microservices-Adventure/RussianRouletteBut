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
    }
}
