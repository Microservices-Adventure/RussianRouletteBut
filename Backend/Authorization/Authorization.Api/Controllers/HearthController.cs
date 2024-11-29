using Microsoft.AspNetCore.Mvc;

namespace Authorization.Api.Controllers;

[Route("/api/hearth")]
[ApiController]
public class HearthController : ControllerBase
{
    private readonly IHostApplicationLifetime _lifetime;
    private const bool Live = true;
    
    public HearthController(IHostApplicationLifetime lifetime)
    {
        _lifetime = lifetime;
    }
    
    public IActionResult Kill()
    {
        Thread kill = new Thread(() =>
        {
            Thread.Sleep(500);
            _lifetime.StopApplication();
        });
        return Ok();
    }

    public IActionResult IsLive()
    {
        return Ok(Live);
    }
}