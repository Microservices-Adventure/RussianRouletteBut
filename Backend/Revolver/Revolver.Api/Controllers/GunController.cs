using Microsoft.AspNetCore.Mvc;

namespace Revolver.Api.Controllers;

[Route("api/gun")]
[ApiController]
public class GunController : ControllerBase
{
    [HttpPost]
    public IActionResult Shoot()
    {
        
        return Ok();
    }
}