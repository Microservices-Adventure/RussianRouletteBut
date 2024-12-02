using Frontend.Features.Interfaces;
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
        bool result = await _revolverService.Shoot();
        return result ? Ok() : StatusCode(StatusCodes.Status500InternalServerError);
    }
}
