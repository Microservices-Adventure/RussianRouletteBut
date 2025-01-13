using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Revolver.Domain.Config;
using Revolver.Domain.Models;
using Revolver.Domain.Services.Interfaces;

namespace Revolver.Api.Controllers;

[Route("api/gun")]
[ApiController]
public class GunController : ControllerBase
{
    private readonly IRevolverService _revolverService; 
    private readonly IOptionsMonitor<ServicesParameters> _servicesOptions;
    private readonly IHostApplicationLifetime _lifetime;
    
    public GunController(
        IRevolverService revolverService, 
        IOptionsMonitor<ServicesParameters> servicesOptions,
        IHostApplicationLifetime lifetime)
    {
        _revolverService = revolverService;
        _servicesOptions = servicesOptions;
        _lifetime = lifetime;
    }
    
    [HttpPost("shoot")]
    public async Task<IActionResult> Shoot(ShootRequestModel request)
    {
        List<ServiceInfo> services = [];
        foreach (string name in request.Bullets)
        {
            ServiceInfo? service = _servicesOptions.CurrentValue.Services
                .SingleOrDefault(s => s.ServiceName == name);
            if (service is null)
            {
                return NotFound("Service not found");
            }
            services.Add(service);
        }
        
        ServiceInfo serviceInfo = _revolverService.Roll(services);
        Console.WriteLine("ServiceName: " + serviceInfo.ServiceName);
        if (serviceInfo.ServiceName == "Revolver")
        {
            KillRevolver();
            return Ok(serviceInfo);
        }

        bool result = await _revolverService.Kill(serviceInfo);
        Console.WriteLine("Result: " + result);
        if (result)
        {
            return Ok(serviceInfo);
        }
        return BadRequest(result);
    }

    private void KillRevolver()
    {
        Thread kill = new Thread(() =>
        {
            Thread.Sleep(500);
            _lifetime.StopApplication();
        });
        kill.Start();
    }
}