using Microsoft.AspNetCore.Mvc;
using Revolver.Domain.Models;
using Revolver.Domain.Services.Interfaces;

namespace Revolver.Api.Controllers;

[Route("/api/hearth")]
[ApiController]
public class HearthController : ControllerBase
{
    private readonly IHostApplicationLifetime _lifetime;
    private readonly IHealthService _healthService;
    
    public HearthController(IHostApplicationLifetime lifetime, IHealthService healthService)
    {
        _lifetime = lifetime;
        _healthService = healthService;
    }
    
    [HttpPost("kill")]
    public IActionResult Kill()
    {
        
        Console.WriteLine("Kill request");
        double cooldownTime = _healthService.CooldownTime();
        if (cooldownTime > 0)
        {
            Console.WriteLine("Cooldown time left: " + cooldownTime);
            return BadRequest("Cooldown time left: " + cooldownTime);
        }
        Thread kill = new Thread(() =>
        {
            Thread.Sleep(500);
            _lifetime.StopApplication();
        });
        kill.Start();
        return Ok();
    }

    [HttpGet("isLive")]
    public IActionResult IsLive()
    {
        return Ok(_healthService.IsLive());
    }

    [HttpGet("killAvailable")]
    public IActionResult KillAvailable()
    {
        double timeLeft = _healthService.CooldownTime();
        var response = new CooldownModel
        {
            IsCooldown = timeLeft > 0,
            SecondsLeft = timeLeft
        };
        return Ok(response);
    }
}