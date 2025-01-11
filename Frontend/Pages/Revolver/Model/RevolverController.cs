using System.Security.Claims;
using Frontend.Entities.Revolver.Model;
using Frontend.Features.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Frontend.Pages.Revolver.Model;

public class RevolverController : Controller
{
    private readonly IRevolverService _revolverService;

    public RevolverController(IRevolverService revolverService)
    {
        _revolverService = revolverService;
    }
    
    [Authorize]
    [HttpPost]
    public async Task<IActionResult> Shoot()
    {
        KilledServiceInfo result = await _revolverService.Shoot(new ShootMan()
        {
            Username = HttpContext.User.Identity!.Name!,
            Email = HttpContext.User.Claims.FirstOrDefault(claim => claim.Type == ClaimTypes.Email)?.Value
        });
        return Ok(result);
    }
}
